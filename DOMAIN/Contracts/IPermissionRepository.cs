using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Permission GetByCode(string permissionCode);
        List<Permission> GetByModule(string module);
        List<string> GetUserPermissions(int userId);
    }
}
