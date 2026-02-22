using System;
using System.Collections;
using DOMAIN.Contracts;
using SERVICES.Interfaces;

namespace BLL.Strategies
{
    /// <summary>
    /// Estrategia concreta para el reporte de productos más vendidos (Strategy pattern).
    /// </summary>
    public class TopProductsReportStrategy : IReportStrategy
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogService _logService;

        public string ReportName => "Top Productos Vendidos";

        public TopProductsReportStrategy(IReportRepository reportRepository, ILogService logService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public IList Execute(ReportParameters parameters)
        {
            _logService.Info($"[Strategy] Ejecutando {ReportName}. Parámetros: " +
                $"Desde={parameters.StartDate:yyyy-MM-dd}, Hasta={parameters.EndDate:yyyy-MM-dd}, " +
                $"Categoría={parameters.Category}, Top={parameters.TopN}, OrdenarPor={parameters.OrderBy}");

            return _reportRepository.GetTopProductsReport(
                parameters.StartDate,
                parameters.EndDate,
                parameters.Category,
                parameters.TopN,
                parameters.OrderBy ?? "units");
        }
    }

    /// <summary>
    /// Estrategia concreta para el reporte de compras por cliente (Strategy pattern).
    /// </summary>
    public class ClientPurchasesReportStrategy : IReportStrategy
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogService _logService;

        public string ReportName => "Compras por Cliente";

        public ClientPurchasesReportStrategy(IReportRepository reportRepository, ILogService logService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public IList Execute(ReportParameters parameters)
        {
            _logService.Info($"[Strategy] Ejecutando {ReportName}. Parámetros: " +
                $"Desde={parameters.StartDate:yyyy-MM-dd}, Hasta={parameters.EndDate:yyyy-MM-dd}, " +
                $"ClienteId={parameters.ClientId}, Top={parameters.TopN}");

            return _reportRepository.GetClientPurchasesReport(
                parameters.StartDate,
                parameters.EndDate,
                parameters.ClientId,
                parameters.TopN);
        }
    }

    /// <summary>
    /// Estrategia concreta para el reporte de variación de precios (Strategy pattern).
    /// </summary>
    public class PriceVariationReportStrategy : IReportStrategy
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogService _logService;

        public string ReportName => "Variación de Precios";

        public PriceVariationReportStrategy(IReportRepository reportRepository, ILogService logService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public IList Execute(ReportParameters parameters)
        {
            _logService.Info($"[Strategy] Ejecutando {ReportName}. Parámetros: " +
                $"Desde={parameters.StartDate:yyyy-MM-dd}, Hasta={parameters.EndDate:yyyy-MM-dd}, " +
                $"ProductoId={parameters.ProductId}, Categoría={parameters.Category}");

            return _reportRepository.GetPriceVariationReport(
                parameters.StartDate,
                parameters.EndDate,
                parameters.ProductId,
                parameters.Category);
        }
    }

    /// <summary>
    /// Estrategia concreta para el reporte de rendimiento de vendedores (Strategy pattern).
    /// </summary>
    public class SellerPerformanceReportStrategy : IReportStrategy
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogService _logService;

        public string ReportName => "Rendimiento de Vendedores";

        public SellerPerformanceReportStrategy(IReportRepository reportRepository, ILogService logService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public IList Execute(ReportParameters parameters)
        {
            _logService.Info($"[Strategy] Ejecutando {ReportName}. Parámetros: " +
                $"Desde={parameters.StartDate:yyyy-MM-dd}, Hasta={parameters.EndDate:yyyy-MM-dd}, " +
                $"Vendedor={parameters.SellerName}, Categoría={parameters.Category}");

            return _reportRepository.GetSellerPerformanceReport(
                parameters.StartDate,
                parameters.EndDate,
                parameters.SellerName,
                parameters.Category);
        }
    }

    /// <summary>
    /// Estrategia concreta para el reporte de ventas por categoría (Strategy pattern).
    /// </summary>
    public class CategorySalesReportStrategy : IReportStrategy
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogService _logService;

        public string ReportName => "Ventas por Categoría";

        public CategorySalesReportStrategy(IReportRepository reportRepository, ILogService logService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public IList Execute(ReportParameters parameters)
        {
            _logService.Info($"[Strategy] Ejecutando {ReportName}. Parámetros: " +
                $"Desde={parameters.StartDate:yyyy-MM-dd}, Hasta={parameters.EndDate:yyyy-MM-dd}, " +
                $"Categoría={parameters.Category}");

            return _reportRepository.GetCategorySalesReport(
                parameters.StartDate,
                parameters.EndDate,
                parameters.Category);
        }
    }

    /// <summary>
    /// Estrategia concreta para el reporte de ranking de clientes por producto (Strategy pattern).
    /// </summary>
    public class ClientProductRankingReportStrategy : IReportStrategy
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogService _logService;

        public string ReportName => "Ranking de Clientes por Producto";

        public ClientProductRankingReportStrategy(IReportRepository reportRepository, ILogService logService)
        {
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <inheritdoc/>
        public IList Execute(ReportParameters parameters)
        {
            _logService.Info($"[Strategy] Ejecutando {ReportName}. Parámetros: " +
                $"Desde={parameters.StartDate:yyyy-MM-dd}, Hasta={parameters.EndDate:yyyy-MM-dd}, " +
                $"ProductoId={parameters.ProductId}, Categoría={parameters.Category}, Top={parameters.TopN}");

            return _reportRepository.GetClientProductRankingReport(
                parameters.StartDate,
                parameters.EndDate,
                parameters.ProductId,
                parameters.Category,
                parameters.TopN);
        }
    }
}
