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
    public class UserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// Inicializa el servicio de usuarios con sus dependencias
        /// </summary>
        /// <param name="userRepo">Repositorio de usuarios</param>
        /// <param name="auditRepo">Repositorio de auditoría</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        /// <param name="authService">Servicio de autenticación</param>
        public UserService(IUserRepository userRepo, IAuditLogRepository auditRepo, ILogService logService, IAuthenticationService authService)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Obtiene todos los usuarios del sistema
        /// </summary>
        /// <returns>Lista de todos los usuarios</returns>
        public List<User> GetAllUsers()
        {
            try
            {
                return _userRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all users", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los usuarios activos del sistema
        /// </summary>
        /// <returns>Lista de usuarios activos</returns>
        public List<User> GetActiveUsers()
        {
            try
            {
                return _userRepo.GetAllActive();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving active users", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su identificador
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Usuario encontrado o null si no existe</returns>
        public User GetUserById(int userId)
        {
            try
            {
                return _userRepo.GetById(userId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving user {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema con su contraseña
        /// </summary>
        /// <param name="user">Datos del usuario a crear</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Identificador del usuario creado</returns>
        public int CreateUser(User user, string password)
        {
            try
            {
                // Validations
                ValidateUser(user);
                ValidatePassword(password);

                // Check for duplicate username
                if (_userRepo.GetByUsername(user.Username) != null)
                {
                    throw new InvalidOperationException($"Username '{user.Username}' already exists.");
                }

                // Check for duplicate email
                if (!string.IsNullOrWhiteSpace(user.Email) && _userRepo.GetByEmail(user.Email) != null)
                {
                    throw new InvalidOperationException($"Email '{user.Email}' already exists.");
                }

                // Hash password
                string salt;
                var hash = _authService.HashPassword(password, out salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                // Set audit fields
                user.CreatedAt = DateTime.Now;
                user.CreatedBy = SessionContext.Instance.CurrentUserId;
                user.IsActive = true;

                // Insert
                var userId = _userRepo.Insert(user);

                // Audit log
                _auditRepo.LogChange("Users", userId, AuditAction.Insert, null, null, 
                    $"Created user {user.Username}", SessionContext.Instance.CurrentUserId);

                _logService.Info($"User created: {user.Username} by {SessionContext.Instance.CurrentUsername}");

                return userId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating user: {user.Username}", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente
        /// </summary>
        /// <param name="user">Datos actualizados del usuario</param>
        public void UpdateUser(User user)
        {
            try
            {
                // Validations
                ValidateUser(user);

                // Get old values for audit
                var oldUser = _userRepo.GetById(user.UserId);
                if (oldUser == null)
                {
                    throw new InvalidOperationException($"User with ID {user.UserId} not found.");
                }

                // Check for duplicate username (excluding current user)
                var existingUser = _userRepo.GetByUsername(user.Username);
                if (existingUser != null && existingUser.UserId != user.UserId)
                {
                    throw new InvalidOperationException($"Username '{user.Username}' already exists.");
                }

                // Check for duplicate email (excluding current user)
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    existingUser = _userRepo.GetByEmail(user.Email);
                    if (existingUser != null && existingUser.UserId != user.UserId)
                    {
                        throw new InvalidOperationException($"Email '{user.Email}' already exists.");
                    }
                }

                // Keep existing password hash and salt (don't update password here)
                user.PasswordHash = oldUser.PasswordHash;
                user.PasswordSalt = oldUser.PasswordSalt;

                // Set audit fields
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = SessionContext.Instance.CurrentUserId;

                // Update
                _userRepo.Update(user);

                // Audit log - log each changed field
                LogFieldChange("Users", user.UserId, "Username", oldUser.Username, user.Username);
                LogFieldChange("Users", user.UserId, "Email", oldUser.Email, user.Email);
                LogFieldChange("Users", user.UserId, "FullName", oldUser.FullName, user.FullName);

                _logService.Info($"User updated: {user.Username} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating user: {user.UserId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un usuario del sistema (borrado lógico)
        /// </summary>
        /// <param name="userId">Identificador del usuario a eliminar</param>
        public void DeleteUser(int userId)
        {
            try
            {
                var user = _userRepo.GetById(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found.");
                }

                // Prevent deleting admin user
                if (user.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Cannot delete the admin user.");
                }

                // Soft delete
                _userRepo.SoftDelete(userId, SessionContext.Instance.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Users", userId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.Instance.CurrentUserId);

                _logService.Info($"User deleted (soft): {user.Username} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting user: {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="newPassword">Nueva contraseña</param>
        public void ChangePassword(int userId, string newPassword)
        {
            try
            {
                ValidatePassword(newPassword);

                var user = _userRepo.GetById(userId);
                if (user == null)
                {
                    throw new InvalidOperationException($"User with ID {userId} not found.");
                }

                // Hash new password
                string salt;
                var hash = _authService.HashPassword(newPassword, out salt);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = SessionContext.Instance.CurrentUserId;

                _userRepo.Update(user);

                // Audit log
                _auditRepo.LogChange("Users", userId, AuditAction.Update, "Password", "***", "***", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Password changed for user: {user.Username} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error changing password for user: {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Asigna roles a un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <param name="roleIds">Lista de identificadores de roles a asignar</param>
        public void AssignRolesToUser(int userId, List<int> roleIds)
        {
            try
            {
                _userRepo.AssignRoles(userId, roleIds);

                var user = _userRepo.GetById(userId);
                _auditRepo.LogChange("UserRoles", userId, AuditAction.Update, "Roles", null, 
                    $"Assigned {roleIds.Count} roles", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Roles assigned to user: {user.Username} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error assigning roles to user: {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los roles asignados a un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Lista de roles del usuario</returns>
        public List<Role> GetUserRoles(int userId)
        {
            try
            {
                return _userRepo.GetUserRoles(userId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving roles for user: {userId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos del usuario cumplan con las reglas de negocio
        /// </summary>
        /// <param name="user">Usuario a validar</param>
        private void ValidateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required.", nameof(user.Username));

            if (user.Username.Length < 3 || user.Username.Length > 50)
                throw new ArgumentException("Username must be between 3 and 50 characters.", nameof(user.Username));

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                if (!IsValidEmail(user.Email))
                    throw new ArgumentException("Invalid email format.", nameof(user.Email));
            }
        }

        /// <summary>
        /// Valida que la contraseña cumpla con los requisitos de seguridad
        /// </summary>
        /// <param name="password">Contraseña a validar</param>
        private void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsDigit))
                throw new ArgumentException("Password must contain at least one number.");
        }

        /// <summary>
        /// Valida el formato de una dirección de correo electrónico
        /// </summary>
        /// <param name="email">Correo electrónico a validar</param>
        /// <returns>True si el formato es válido, False en caso contrario</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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
