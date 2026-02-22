using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IPermissionRepository _permissionRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

        // System role names that cannot be deleted
        private static readonly string[] SystemRoleNames = { "Admin", "User" };

        /// <summary>
        /// Inicializa el servicio de roles con sus dependencias
        /// </summary>
        /// <param name="roleRepo">Repositorio de roles</param>
        /// <param name="permissionRepo">Repositorio de permisos</param>
        /// <param name="auditRepo">Repositorio de auditoría</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        public RoleService(IRoleRepository roleRepo, IPermissionRepository permissionRepo, IAuditLogRepository auditRepo, ILogService logService)
        {
            _roleRepo = roleRepo ?? throw new ArgumentNullException(nameof(roleRepo));
            _permissionRepo = permissionRepo ?? throw new ArgumentNullException(nameof(permissionRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Obtiene todos los roles del sistema
        /// </summary>
        /// <returns>Lista de todos los roles</returns>
        public List<Role> GetAllRoles()
        {
            try
            {
                return _roleRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all roles", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los roles activos del sistema
        /// </summary>
        /// <returns>Lista de roles activos</returns>
        public List<Role> GetActiveRoles()
        {
            try
            {
                return _roleRepo.GetAllActive();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving active roles", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un rol por su identificador
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        /// <returns>Rol encontrado o null si no existe</returns>
        public Role GetRoleById(int roleId)
        {
            try
            {
                return _roleRepo.GetById(roleId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving role {roleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los permisos asignados a un rol
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        /// <returns>Lista de permisos del rol</returns>
        public List<Permission> GetRolePermissions(int roleId)
        {
            try
            {
                return _roleRepo.GetRolePermissions(roleId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving permissions for role {roleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los permisos disponibles en el sistema
        /// </summary>
        /// <returns>Lista de todos los permisos activos</returns>
        public List<Permission> GetAllPermissions()
        {
            try
            {
                return _permissionRepo.GetAllActive();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all permissions", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo rol en el sistema
        /// </summary>
        /// <param name="role">Datos del rol a crear</param>
        /// <returns>Identificador del rol creado</returns>
        public int CreateRole(Role role)
        {
            try
            {
                // Validations
                ValidateRole(role);

                // Check for duplicate role name
                if (_roleRepo.GetByName(role.RoleName) != null)
                {
                    throw new InvalidOperationException($"Role name '{role.RoleName}' already exists.");
                }

                // Set audit fields
                role.CreatedAt = DateTime.Now;
                role.CreatedBy = SessionContext.Instance.CurrentUserId;
                role.IsActive = true;

                // Insert
                var roleId = _roleRepo.Insert(role);

                // Audit log
                _auditRepo.LogChange("Roles", roleId, AuditAction.Insert, null, null, 
                    $"Created role {role.RoleName}", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Role created: {role.RoleName} by {SessionContext.Instance.CurrentUsername}");

                return roleId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating role: {role?.RoleName}", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un rol existente
        /// </summary>
        /// <param name="role">Datos actualizados del rol</param>
        public void UpdateRole(Role role)
        {
            try
            {
                // Validations
                ValidateRole(role);

                // Get old values for audit
                var oldRole = _roleRepo.GetById(role.RoleId);
                if (oldRole == null)
                {
                    throw new InvalidOperationException($"Role with ID {role.RoleId} not found.");
                }

                // Check for duplicate role name (excluding current role)
                var existingRole = _roleRepo.GetByName(role.RoleName);
                if (existingRole != null && existingRole.RoleId != role.RoleId)
                {
                    throw new InvalidOperationException($"Role name '{role.RoleName}' already exists.");
                }

                // Set audit fields
                role.UpdatedAt = DateTime.Now;
                role.UpdatedBy = SessionContext.Instance.CurrentUserId;

                // Update
                _roleRepo.Update(role);

                // Audit log - log each changed field
                LogFieldChange("Roles", role.RoleId, "RoleName", oldRole.RoleName, role.RoleName);
                LogFieldChange("Roles", role.RoleId, "Description", oldRole.Description, role.Description);

                _logService.Info($"Role updated: {role.RoleName} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating role: {role.RoleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un rol del sistema (borrado lógico)
        /// </summary>
        /// <param name="roleId">Identificador del rol a eliminar</param>
        public void DeleteRole(int roleId)
        {
            try
            {
                var role = _roleRepo.GetById(roleId);
                if (role == null)
                {
                    throw new InvalidOperationException($"Role with ID {roleId} not found.");
                }

                // Prevent deleting system roles
                if (Array.Exists(SystemRoleNames, systemRole => systemRole.Equals(role.RoleName, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"Cannot delete system role '{role.RoleName}'.");
                }

                // Soft delete
                _roleRepo.SoftDelete(roleId, SessionContext.Instance.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Roles", roleId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Role deleted (soft): {role.RoleName} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting role: {roleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Asigna permisos a un rol
        /// </summary>
        /// <param name="roleId">Identificador del rol</param>
        /// <param name="permissionIds">Lista de identificadores de permisos a asignar</param>
        public void AssignPermissions(int roleId, List<int> permissionIds)
        {
            try
            {
                var role = _roleRepo.GetById(roleId);
                if (role == null)
                {
                    throw new InvalidOperationException($"Role with ID {roleId} not found.");
                }

                // Clear existing permissions
                _roleRepo.ClearPermissions(roleId);

                // Assign new permissions
                foreach (var permissionId in permissionIds)
                {
                    _roleRepo.AssignPermission(roleId, permissionId, SessionContext.Instance.CurrentUserId.Value);
                }

                // Audit log
                _auditRepo.LogChange("RolePermissions", roleId, AuditAction.Update, "Permissions", null, 
                    $"Assigned {permissionIds.Count} permissions", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Permissions assigned to role: {role.RoleName} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error assigning permissions to role: {roleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos del rol cumplan con las reglas de negocio
        /// </summary>
        /// <param name="role">Rol a validar</param>
        private void ValidateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role), "Role cannot be null.");

            if (string.IsNullOrWhiteSpace(role.RoleName))
                throw new ArgumentException("Role name is required.", nameof(role.RoleName));

            if (role.RoleName.Length < 2 || role.RoleName.Length > 50)
                throw new ArgumentException("Role name must be between 2 and 50 characters.", nameof(role.RoleName));

            if (!string.IsNullOrWhiteSpace(role.Description) && role.Description.Length > 200)
                throw new ArgumentException("Description cannot exceed 200 characters.", nameof(role.Description));
        }

        /// <summary>
        /// Registra un cambio de campo en la auditoría si el valor ha cambiado
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="recordId">Identificador del registro</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="oldValue">Valor anterior</param>
        /// <param name="newValue">Valor nuevo</param>
        private void LogFieldChange(string tableName, int recordId, string fieldName, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                _auditRepo.LogChange(tableName, recordId, AuditAction.Update, fieldName, oldValue, newValue, SessionContext.Instance.CurrentUserId);
            }
        }
    }
}
