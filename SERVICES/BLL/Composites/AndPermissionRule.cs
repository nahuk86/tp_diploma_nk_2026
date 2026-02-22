using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Contracts;

namespace SERVICES.BLL.Composites
{
    /// <summary>
    /// Nodo compuesto del patrón Composite: requiere que TODAS las reglas hijas se cumplan (AND lógico).
    /// Permite construir condiciones del estilo "el usuario debe tener permiso A Y permiso B".
    /// </summary>
    public class AndPermissionRule : IPermissionRule
    {
        private readonly IEnumerable<IPermissionRule> _rules;

        /// <summary>
        /// Crea una regla AND que evalúa todas las reglas proporcionadas
        /// </summary>
        /// <param name="rules">Reglas que deben cumplirse todas</param>
        public AndPermissionRule(IEnumerable<IPermissionRule> rules)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        }

        /// <summary>
        /// Crea una regla AND a partir de un conjunto de permisos individuales
        /// </summary>
        public AndPermissionRule(params string[] permissionCodes)
            : this(permissionCodes.Select(code => (IPermissionRule)new SinglePermissionRule(code)))
        {
        }

        /// <inheritdoc/>
        public bool Evaluate(int userId, IPermissionRepository permissionRepository)
        {
            return _rules.All(rule => rule.Evaluate(userId, permissionRepository));
        }
    }
}
