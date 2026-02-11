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

        public UserService(IUserRepository userRepo, IAuditLogRepository auditRepo, ILogService logService, IAuthenticationService authService)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

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
                var (hash, salt) = _authService.HashPassword(password);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;

                // Set audit fields
                user.CreatedAt = DateTime.Now;
                user.CreatedBy = SessionContext.CurrentUserId;
                user.IsActive = true;

                // Insert
                var userId = _userRepo.Insert(user);

                // Audit log
                _auditRepo.LogChange("Users", userId, AuditAction.Insert, null, null, 
                    $"Created user {user.Username}", SessionContext.CurrentUserId);

                _logService.Info($"User created: {user.Username} by {SessionContext.CurrentUsername}");

                return userId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating user: {user.Username}", ex);
                throw;
            }
        }

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
                user.UpdatedBy = SessionContext.CurrentUserId;

                // Update
                _userRepo.Update(user);

                // Audit log - log each changed field
                LogFieldChange("Users", user.UserId, "Username", oldUser.Username, user.Username);
                LogFieldChange("Users", user.UserId, "Email", oldUser.Email, user.Email);
                LogFieldChange("Users", user.UserId, "FullName", oldUser.FullName, user.FullName);

                _logService.Info($"User updated: {user.Username} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating user: {user.UserId}", ex);
                throw;
            }
        }

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
                _userRepo.SoftDelete(userId, SessionContext.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Users", userId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.CurrentUserId);

                _logService.Info($"User deleted (soft): {user.Username} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting user: {userId}", ex);
                throw;
            }
        }

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
                var (hash, salt) = _authService.HashPassword(newPassword);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = SessionContext.CurrentUserId;

                _userRepo.Update(user);

                // Audit log
                _auditRepo.LogChange("Users", userId, AuditAction.Update, "Password", "***", "***", SessionContext.CurrentUserId);

                _logService.Info($"Password changed for user: {user.Username} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error changing password for user: {userId}", ex);
                throw;
            }
        }

        public void AssignRolesToUser(int userId, List<int> roleIds)
        {
            try
            {
                _userRepo.AssignRoles(userId, roleIds);

                var user = _userRepo.GetById(userId);
                _auditRepo.LogChange("UserRoles", userId, AuditAction.Update, "Roles", null, 
                    $"Assigned {roleIds.Count} roles", SessionContext.CurrentUserId);

                _logService.Info($"Roles assigned to user: {user.Username} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error assigning roles to user: {userId}", ex);
                throw;
            }
        }

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

        private void LogFieldChange(string tableName, int recordId, string fieldName, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                _auditRepo.LogChange(tableName, recordId, AuditAction.Update, fieldName, oldValue, newValue, SessionContext.CurrentUserId);
            }
        }
    }
}
