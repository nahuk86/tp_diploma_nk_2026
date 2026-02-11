using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        List<User> Search(string searchTerm);
        List<Role> GetUserRoles(int userId);
        void AssignRole(int userId, int roleId, int assignedBy);
        void RemoveRole(int userId, int roleId);
        void UpdateLastLogin(int userId);
    }
}
