using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UI.Forms;

namespace UI.Factories
{
    /// <summary>
    /// Implementación concreta del patrón Abstract Factory para módulos de la aplicación.
    /// Centraliza la creación de todos los formularios (Forms) de la aplicación, actuando como
    /// punto único de construcción de la capa de presentación (patrón Factory Method: CreateForm).
    /// </summary>
    public class DefaultModuleFactory : IModuleFactory
    {
        private static readonly HashSet<string> _supportedModules = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Products",
            "Warehouses",
            "Clients",
            "Sales",
            "StockMovements",
            "StockQuery",
            "Reports",
            "Users",
            "Roles",
            "UserRoles",
            "RolePermissions",
            "UserManual"
        };

        /// <inheritdoc/>
        public bool CanCreate(string moduleKey)
        {
            return !string.IsNullOrWhiteSpace(moduleKey) && _supportedModules.Contains(moduleKey);
        }

        /// <summary>
        /// Factory Method: crea e inicializa el formulario correspondiente a la clave de módulo.
        /// Cada case es un producto concreto de la fábrica, encapsulando la lógica de construcción.
        /// </summary>
        /// <param name="moduleKey">Clave del módulo a crear</param>
        /// <returns>Formulario inicializado listo para mostrarse</returns>
        /// <exception cref="ArgumentException">Si la clave de módulo no es reconocida</exception>
        public Form CreateForm(string moduleKey)
        {
            if (string.IsNullOrWhiteSpace(moduleKey))
                throw new ArgumentNullException(nameof(moduleKey));

            switch (moduleKey.ToLowerInvariant())
            {
                case "products":
                    return new ProductsForm();
                case "warehouses":
                    return new WarehousesForm();
                case "clients":
                    return new ClientsForm();
                case "sales":
                    return new SalesForm();
                case "stockmovements":
                    return new StockMovementForm();
                case "stockquery":
                    return new StockQueryForm();
                case "reports":
                    return new ReportsForm();
                case "users":
                    return new UsersForm();
                case "roles":
                    return new RolesForm();
                case "userroles":
                    return new UserRolesForm();
                case "rolepermissions":
                    return new RolePermissionsForm();
                case "usermanual":
                    return new UserManualForm();
                default:
                    throw new ArgumentException($"Módulo desconocido: '{moduleKey}'. " +
                        $"Módulos soportados: {string.Join(", ", _supportedModules)}", nameof(moduleKey));
            }
        }
    }
}
