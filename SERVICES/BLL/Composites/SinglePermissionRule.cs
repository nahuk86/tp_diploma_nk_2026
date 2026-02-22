using System;
using System.Linq;
using DOMAIN.Contracts;

namespace SERVICES.BLL.Composites
{
    /// <summary>
    /// Nodo hoja del patrón Composite: verifica un único permiso.
    /// </summary>
    public class SinglePermissionRule : IPermissionRule
    {
        private readonly string _permissionCode;

        /// <summary>
        /// Crea una regla para un único código de permiso
        /// </summary>
        /// <param name="permissionCode">Código del permiso a verificar</param>
        public SinglePermissionRule(string permissionCode)
        {
            if (string.IsNullOrWhiteSpace(permissionCode))
                throw new ArgumentNullException(nameof(permissionCode));
            _permissionCode = permissionCode;
        }

        /// <inheritdoc/>
        public bool Evaluate(int userId, IPermissionRepository permissionRepository)
        {
            var permissions = permissionRepository.GetUserPermissions(userId);
            return permissions.Contains(_permissionCode, StringComparer.OrdinalIgnoreCase);
        }
    }
}
