using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Contracts;

namespace SERVICES.BLL.Composites
{
    /// <summary>
    /// Nodo compuesto del patrón Composite: requiere que AL MENOS UNA regla hija se cumpla (OR lógico).
    /// Permite construir condiciones del estilo "el usuario debe tener permiso A O permiso B".
    /// </summary>
    public class OrPermissionRule : IPermissionRule
    {
        private readonly IEnumerable<IPermissionRule> _rules;

        /// <summary>
        /// Crea una regla OR que evalúa todas las reglas proporcionadas
        /// </summary>
        /// <param name="rules">Reglas de las que al menos una debe cumplirse</param>
        public OrPermissionRule(IEnumerable<IPermissionRule> rules)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        }

        /// <summary>
        /// Crea una regla OR a partir de un conjunto de permisos individuales
        /// </summary>
        public OrPermissionRule(params string[] permissionCodes)
            : this(permissionCodes.Select(code => (IPermissionRule)new SinglePermissionRule(code)))
        {
        }

        /// <inheritdoc/>
        public bool Evaluate(int userId, IPermissionRepository permissionRepository)
        {
            return _rules.Any(rule => rule.Evaluate(userId, permissionRepository));
        }
    }
}
