using System;
using System.Security.Cryptography;
using System.Text;
using DOMAIN.Entities;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DOMAIN.Contracts.IUserRepository _userRepository;
        private readonly ILogService _logService;

        public AuthenticationService(DOMAIN.Contracts.IUserRepository userRepository, ILogService logService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    _logService.Warning($"Authentication attempt with empty credentials for username: {username}");
                    return null;
                }

                var user = _userRepository.GetByUsername(username);
                
                if (user == null)
                {
                    _logService.Warning($"Authentication failed: User '{username}' not found");
                    return null;
                }

                if (!user.IsActive)
                {
                    _logService.Warning($"Authentication failed: User '{username}' is inactive");
                    return null;
                }

                // Check for placeholder password (first run)
                if (user.PasswordHash == "HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP")
                {
                    _logService.Warning($"User '{username}' password not initialized. Please initialize admin password.");
                    return null;
                }

                if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    _logService.Warning($"Authentication failed: Invalid password for user '{username}'");
                    return null;
                }

                // Update last login
                _userRepository.UpdateLastLogin(user.UserId);
                
                _logService.Info($"User '{username}' authenticated successfully");
                return user;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error during authentication for user '{username}'", ex);
                throw;
            }
        }

        public string HashPassword(string password, out string salt)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Hash the password with PBKDF2
            byte[] hash = HashPasswordWithSalt(password, saltBytes);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string hash, string salt)
        {
            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                byte[] hashBytes = HashPasswordWithSalt(password, saltBytes);
                string computedHash = Convert.ToBase64String(hashBytes);
                
                return hash == computedHash;
            }
            catch
            {
                return false;
            }
        }

        public void InitializeAdminPassword(string username, string newPassword)
        {
            try
            {
                var user = _userRepository.GetByUsername(username);
                if (user == null)
                {
                    throw new Exception($"User '{username}' not found");
                }

                string salt;
                string hash = HashPassword(newPassword, out salt);

                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                _userRepository.Update(user);

                _logService.Info($"Password initialized for user '{username}'");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error initializing password for user '{username}'", ex);
                throw;
            }
        }

        private byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return pbkdf2.GetBytes(32);
            }
        }
    }
}
