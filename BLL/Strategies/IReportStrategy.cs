using System;
using System.Collections;

namespace BLL.Strategies
{
    /// <summary>
    /// Parámetros de configuración para la ejecución de reportes.
    /// Encapsula todos los posibles filtros utilizados por las distintas estrategias de reporte.
    /// </summary>
    public class ReportParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Category { get; set; }
        public int? TopN { get; set; }
        public string OrderBy { get; set; }
        public int? ClientId { get; set; }
        public int? ProductId { get; set; }
        public int? WarehouseId { get; set; }
        public string SellerName { get; set; }
        public string MovementType { get; set; }
    }

    /// <summary>
    /// Interfaz del patrón Strategy para la generación de reportes.
    /// Cada estrategia concreta encapsula el algoritmo de generación de un reporte específico,
    /// permitiendo intercambiarlos de forma transparente para el contexto invocador.
    /// </summary>
    public interface IReportStrategy
    {
        /// <summary>
        /// Nombre descriptivo del reporte generado por esta estrategia
        /// </summary>
        string ReportName { get; }

        /// <summary>
        /// Ejecuta la estrategia de reporte con los parámetros dados
        /// </summary>
        /// <param name="parameters">Parámetros de filtrado y configuración del reporte</param>
        /// <returns>Colección de resultados (lista de DTOs tipados)</returns>
        IList Execute(ReportParameters parameters);
    }
}
