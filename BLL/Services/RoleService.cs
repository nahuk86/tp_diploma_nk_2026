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

        public RoleService(IRoleRepository roleRepo, IPermissionRepository permissionRepo, IAuditLogRepository auditRepo, ILogService logService)
        {
            _roleRepo = roleRepo ?? throw new ArgumentNullException(nameof(roleRepo));
            _permissionRepo = permissionRepo ?? throw new ArgumentNullException(nameof(permissionRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

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
                role.CreatedBy = SessionContext.CurrentUserId;
                role.IsActive = true;

                // Insert
                var roleId = _roleRepo.Insert(role);

                // Audit log
                _auditRepo.LogChange("Roles", roleId, AuditAction.Insert, null, null, 
                    $"Created role {role.RoleName}", SessionContext.CurrentUserId);

                _logService.Info($"Role created: {role.RoleName} by {SessionContext.CurrentUsername}");

                return roleId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating role: {role?.RoleName}", ex);
                throw;
            }
        }

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
                role.UpdatedBy = SessionContext.CurrentUserId;

                // Update
                _roleRepo.Update(role);

                // Audit log - log each changed field
                LogFieldChange("Roles", role.RoleId, "RoleName", oldRole.RoleName, role.RoleName);
                LogFieldChange("Roles", role.RoleId, "Description", oldRole.Description, role.Description);

                _logService.Info($"Role updated: {role.RoleName} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating role: {role.RoleId}", ex);
                throw;
            }
        }

        public void DeleteRole(int roleId)
        {
            try
            {
                var role = _roleRepo.GetById(roleId);
                if (role == null)
                {
                    throw new InvalidOperationException($"Role with ID {roleId} not found.");
                }

                // Prevent deleting system roles (admin, user)
                if (role.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                    role.RoleName.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Cannot delete system roles (Admin, User).");
                }

                // Soft delete
                _roleRepo.SoftDelete(roleId, SessionContext.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Roles", roleId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.CurrentUserId);

                _logService.Info($"Role deleted (soft): {role.RoleName} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting role: {roleId}", ex);
                throw;
            }
        }

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
                    _roleRepo.AssignPermission(roleId, permissionId, SessionContext.CurrentUserId.Value);
                }

                // Audit log
                _auditRepo.LogChange("RolePermissions", roleId, AuditAction.Update, "Permissions", null, 
                    $"Assigned {permissionIds.Count} permissions", SessionContext.CurrentUserId);

                _logService.Info($"Permissions assigned to role: {role.RoleName} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error assigning permissions to role: {roleId}", ex);
                throw;
            }
        }

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

        private void LogFieldChange(string tableName, int recordId, string fieldName, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                _auditRepo.LogChange(tableName, recordId, AuditAction.Update, fieldName, oldValue, newValue, SessionContext.CurrentUserId);
            }
        }
    }
}
