using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Obtiene un rol por su nombre
        /// </summary>
        Role GetByName(string roleName);
        
        /// <summary>
        /// Obtiene la lista de permisos asignados a un rol
        /// </summary>
        List<Permission> GetRolePermissions(int roleId);
        
        /// <summary>
        /// Asigna un permiso a un rol
        /// </summary>
        void AssignPermission(int roleId, int permissionId, int assignedBy);
        
        /// <summary>
        /// Remueve un permiso de un rol
        /// </summary>
        void RemovePermission(int roleId, int permissionId);
        
        /// <summary>
        /// Elimina todos los permisos de un rol
        /// </summary>
        void ClearPermissions(int roleId);
    }
}
