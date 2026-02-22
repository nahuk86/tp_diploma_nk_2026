using System.Windows.Forms;

namespace UI.Factories
{
    /// <summary>
    /// Interfaz del patrón Abstract Factory para la creación de módulos de la aplicación.
    /// Define el contrato para fábricas que crean formularios (Form) con sus servicios configurados.
    /// El método CreateForm actúa como Factory Method para la creación concreta de cada módulo.
    /// </summary>
    public interface IModuleFactory
    {
        /// <summary>
        /// Factory Method: crea e inicializa el formulario asociado a la clave de módulo dada.
        /// Centraliza la creación de Forms con sus dependencias (servicios, repositorios).
        /// </summary>
        /// <param name="moduleKey">Clave del módulo (ej: "Products", "Sales", "Reports")</param>
        /// <returns>Formulario inicializado listo para mostrarse</returns>
        Form CreateForm(string moduleKey);

        /// <summary>
        /// Indica si esta fábrica puede crear un formulario para la clave de módulo dada
        /// </summary>
        /// <param name="moduleKey">Clave del módulo a verificar</param>
        /// <returns>True si el módulo es soportado por esta fábrica</returns>
        bool CanCreate(string moduleKey);
    }
}
