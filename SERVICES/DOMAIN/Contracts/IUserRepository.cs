using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Obtiene un usuario por su nombre de usuario
        /// </summary>
        User GetByUsername(string username);
        
        /// <summary>
        /// Obtiene un usuario por su correo electrónico
        /// </summary>
        User GetByEmail(string email);
        
        /// <summary>
        /// Busca usuarios por término de búsqueda en nombre, username o email
        /// </summary>
        List<User> Search(string searchTerm);
        
        /// <summary>
        /// Obtiene la lista de roles asignados a un usuario
        /// </summary>
        List<Role> GetUserRoles(int userId);
        
        /// <summary>
        /// Asigna un rol a un usuario
        /// </summary>
        void AssignRole(int userId, int roleId, int assignedBy);
        
        /// <summary>
        /// Asigna múltiples roles a un usuario de forma transaccional
        /// </summary>
        void AssignRoles(int userId, List<int> roleIds);
        
        /// <summary>
        /// Remueve un rol de un usuario
        /// </summary>
        void RemoveRole(int userId, int roleId);
        
        /// <summary>
        /// Actualiza la fecha y hora del último inicio de sesión del usuario
        /// </summary>
        void UpdateLastLogin(int userId);
    }
}
