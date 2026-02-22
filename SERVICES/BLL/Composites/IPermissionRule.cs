using DOMAIN.Contracts;

namespace SERVICES.BLL.Composites
{
    /// <summary>
    /// Interfaz base del patrón Composite para reglas de permiso.
    /// Permite construir árboles de reglas RBAC combinando condiciones AND/OR.
    /// </summary>
    public interface IPermissionRule
    {
        /// <summary>
        /// Evalúa si la regla de permiso se cumple para el usuario especificado
        /// </summary>
        /// <param name="userId">ID del usuario a evaluar</param>
        /// <param name="permissionRepository">Repositorio para consultar permisos</param>
        /// <returns>True si la regla se cumple, false en caso contrario</returns>
        bool Evaluate(int userId, IPermissionRepository permissionRepository);
    }
}
