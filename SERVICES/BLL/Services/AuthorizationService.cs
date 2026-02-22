using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Contracts;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogService _logService;

        public AuthorizationService(IPermissionRepository permissionRepository, ILogService logService)
        {
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="permissionCode">Código del permiso a verificar</param>
        /// <returns>True si el usuario tiene el permiso, false en caso contrario</returns>
        public bool HasPermission(int userId, string permissionCode)
        {
            try
            {
                var userPermissions = _permissionRepository.GetUserPermissions(userId);
                return userPermissions.Contains(permissionCode);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error checking permission '{permissionCode}' for user {userId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene al menos uno de los permisos especificados
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="permissionCodes">Códigos de permisos a verificar</param>
        /// <returns>True si el usuario tiene al menos uno de los permisos, false en caso contrario</returns>
        public bool HasAnyPermission(int userId, params string[] permissionCodes)
        {
            try
            {
                var userPermissions = _permissionRepository.GetUserPermissions(userId);
                return permissionCodes.Any(p => userPermissions.Contains(p));
            }
            catch (Exception ex)
            {
                _logService.Error($"Error checking any permissions for user {userId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Verifica si un usuario tiene todos los permisos especificados
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="permissionCodes">Códigos de permisos a verificar</param>
        /// <returns>True si el usuario tiene todos los permisos, false en caso contrario</returns>
        public bool HasAllPermissions(int userId, params string[] permissionCodes)
        {
            try
            {
                var userPermissions = _permissionRepository.GetUserPermissions(userId);
                return permissionCodes.All(p => userPermissions.Contains(p));
            }
            catch (Exception ex)
            {
                _logService.Error($"Error checking all permissions for user {userId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Obtiene la lista de todos los permisos asignados a un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de códigos de permisos del usuario</returns>
        public List<string> GetUserPermissions(int userId)
        {
            try
            {
                return _permissionRepository.GetUserPermissions(userId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving permissions for user {userId}", ex);
                return new List<string>();
            }
        }
    }
}
