using System;
using System.Collections.Generic;

namespace SERVICES.Interfaces
{
    /// <summary>
    /// Servicio de diagnóstico para verificar el funcionamiento correcto
    /// de los servicios core del sistema en tiempo de ejecución.
    /// </summary>
    public interface ITestingService
    {
        /// <summary>
        /// Ejecuta un conjunto de diagnósticos sobre los servicios core y retorna un reporte
        /// </summary>
        /// <returns>Reporte con los resultados de cada diagnóstico</returns>
        DiagnosticReport RunDiagnostics();

        /// <summary>
        /// Retorna la versión del servicio de pruebas
        /// </summary>
        string GetServiceVersion();
    }

    /// <summary>
    /// Reporte consolidado con los resultados de todos los diagnósticos ejecutados
    /// </summary>
    public class DiagnosticReport
    {
        /// <summary>
        /// Indica si todos los diagnósticos pasaron exitosamente
        /// </summary>
        public bool AllPassed { get; set; }

        /// <summary>
        /// Lista detallada de resultados por cada diagnóstico ejecutado
        /// </summary>
        public List<DiagnosticResult> Results { get; set; }

        /// <summary>
        /// Fecha y hora en que se ejecutaron los diagnósticos
        /// </summary>
        public DateTime RunAt { get; set; }
    }

    /// <summary>
    /// Resultado individual de un diagnóstico sobre un servicio específico
    /// </summary>
    public class DiagnosticResult
    {
        /// <summary>
        /// Nombre del servicio o funcionalidad evaluada
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Indica si el diagnóstico pasó exitosamente
        /// </summary>
        public bool Passed { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado (éxito o detalle del error)
        /// </summary>
        public string Message { get; set; }
    }
}
