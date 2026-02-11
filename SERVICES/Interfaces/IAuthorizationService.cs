using System.Collections.Generic;

namespace SERVICES.Interfaces
{
    public interface IAuthorizationService
    {
        bool HasPermission(int userId, string permissionCode);
        bool HasAnyPermission(int userId, params string[] permissionCodes);
        bool HasAllPermissions(int userId, params string[] permissionCodes);
        List<string> GetUserPermissions(int userId);
    }
}
