using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        /// <summary>
        /// Obtiene un permiso por su código único
        /// </summary>
        Permission GetByCode(string permissionCode);
        
        /// <summary>
        /// Obtiene todos los permisos de un módulo específico
        /// </summary>
        List<Permission> GetByModule(string module);
        
        /// <summary>
        /// Obtiene la lista de códigos de permisos asignados a un usuario
        /// </summary>
        List<string> GetUserPermissions(int userId);
    }
}
