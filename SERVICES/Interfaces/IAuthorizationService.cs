using System.Collections.Generic;

namespace SERVICES.Interfaces
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="permissionCode">Código del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso, false en caso contrario</returns>
        bool HasPermission(int userId, string permissionCode);
        
        /// <summary>
        /// Verifica si un usuario tiene al menos uno de los permisos especificados
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="permissionCodes">Códigos de permisos a verificar</param>
        /// <returns>True si el usuario tiene al menos uno de los permisos, false en caso contrario</returns>
        bool HasAnyPermission(int userId, params string[] permissionCodes);
        
        /// <summary>
        /// Verifica si un usuario tiene todos los permisos especificados
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="permissionCodes">Códigos de permisos a verificar</param>
        /// <returns>True si el usuario tiene todos los permisos, false en caso contrario</returns>
        bool HasAllPermissions(int userId, params string[] permissionCodes);
        
        /// <summary>
        /// Obtiene la lista de todos los permisos asignados a un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de códigos de permisos del usuario</returns>
        List<string> GetUserPermissions(int userId);
    }
}
