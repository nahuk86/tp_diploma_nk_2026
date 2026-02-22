using System;
using System.Collections;

namespace BLL.Strategies
{
    /// <summary>
    /// Contexto del patrón Strategy para reportes.
    /// Mantiene una referencia a la estrategia activa y delega la ejecución a ella,
    /// permitiendo cambiar el algoritmo de generación de reportes en tiempo de ejecución.
    /// </summary>
    public class ReportContext
    {
        private IReportStrategy _strategy;

        /// <summary>
        /// Crea el contexto con una estrategia inicial
        /// </summary>
        /// <param name="strategy">Estrategia de reporte a utilizar</param>
        public ReportContext(IReportStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Nombre del reporte de la estrategia activa
        /// </summary>
        public string CurrentReportName => _strategy.ReportName;

        /// <summary>
        /// Cambia la estrategia de reporte en tiempo de ejecución
        /// </summary>
        /// <param name="strategy">Nueva estrategia a utilizar</param>
        public void SetStrategy(IReportStrategy strategy)
        {
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Ejecuta el reporte delegando a la estrategia activa
        /// </summary>
        /// <param name="parameters">Parámetros del reporte</param>
        /// <returns>Resultados del reporte generado por la estrategia activa</returns>
        public IList ExecuteReport(ReportParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return _strategy.Execute(parameters);
        }
    }
}
