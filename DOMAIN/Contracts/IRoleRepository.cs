using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role GetByName(string roleName);
        List<Permission> GetRolePermissions(int roleId);
        void AssignPermission(int roleId, int permissionId, int assignedBy);
        void RemovePermission(int roleId, int permissionId);
        void ClearPermissions(int roleId);
    }
}
