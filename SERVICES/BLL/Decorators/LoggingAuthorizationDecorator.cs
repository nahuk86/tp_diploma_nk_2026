using System;
using System.Collections.Generic;
using SERVICES.Interfaces;

namespace SERVICES.BLL.Decorators
{
    /// <summary>
    /// Decorador del patrón Decorator para IAuthorizationService.
    /// Agrega logging detallado a cada verificación de permisos sin modificar la implementación base.
    /// Elimina la duplicación de código de logging en múltiples puntos de la aplicación.
    /// </summary>
    public class LoggingAuthorizationDecorator : IAuthorizationService
    {
        private readonly IAuthorizationService _inner;
        private readonly ILogService _logService;

        /// <summary>
        /// Crea el decorador envolviendo la implementación concreta de autorización
        /// </summary>
        /// <param name="inner">Implementación de autorización a decorar</param>
        /// <param name="logService">Servicio de logging</param>
        public LoggingAuthorizationDecorator(IAuthorizationService inner, ILogService logService)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public bool HasPermission(int userId, string permissionCode)
        {
            var result = _inner.HasPermission(userId, permissionCode);
            _logService.Debug(
                $"[AuthZ] HasPermission: userId={userId}, permission='{permissionCode}', result={result}",
                "AuthorizationDecorator");
            return result;
        }

        /// <inheritdoc/>
        public bool HasAnyPermission(int userId, params string[] permissionCodes)
        {
            var result = _inner.HasAnyPermission(userId, permissionCodes);
            _logService.Debug(
                $"[AuthZ] HasAnyPermission: userId={userId}, permissions=[{string.Join(",", permissionCodes)}], result={result}",
                "AuthorizationDecorator");
            return result;
        }

        /// <inheritdoc/>
        public bool HasAllPermissions(int userId, params string[] permissionCodes)
        {
            var result = _inner.HasAllPermissions(userId, permissionCodes);
            _logService.Debug(
                $"[AuthZ] HasAllPermissions: userId={userId}, permissions=[{string.Join(",", permissionCodes)}], result={result}",
                "AuthorizationDecorator");
            return result;
        }

        /// <inheritdoc/>
        public List<string> GetUserPermissions(int userId)
        {
            var result = _inner.GetUserPermissions(userId);
            _logService.Debug(
                $"[AuthZ] GetUserPermissions: userId={userId}, count={result.Count}",
                "AuthorizationDecorator");
            return result;
        }
    }
}
