using DOMAIN.Entities;

namespace SERVICES.Interfaces
{
    public interface IAuthenticationService
    {
        User Authenticate(string username, string password);
        string HashPassword(string password, out string salt);
        bool VerifyPassword(string password, string hash, string salt);
        void InitializeAdminPassword(string username, string newPassword);
    }
}
