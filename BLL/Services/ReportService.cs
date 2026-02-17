using System;
using System.Collections.Generic;
using DOMAIN.Contracts;
using DOMAIN.Entities.Reports;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class ReportService
    {
        private readonly IReportRepository _reportRepo;
        private readonly ILogService _logService;

        /// <summary>
        /// Inicializa el servicio de reportes con sus dependencias
        /// </summary>
        /// <param name="reportRepo">Repositorio de reportes</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        public ReportService(IReportRepository reportRepo, ILogService logService)
        {
            _reportRepo = reportRepo ?? throw new ArgumentNullException(nameof(reportRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        // Report 1: Top Products
        /// <summary>
        /// Genera el reporte de productos más vendidos
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="category">Categoría para filtrar (opcional)</param>
        /// <param name="topN">Número máximo de productos a retornar (opcional)</param>
        /// <param name="orderBy">Campo de ordenamiento: "units" o "revenue"</param>
        /// <returns>Lista de productos más vendidos con sus estadísticas</returns>
        public List<TopProductsReportDTO> GetTopProductsReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null, 
            int? topN = null,
            string orderBy = "units")
        {
            try
            {
                _logService.Info($"Generating Top Products Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, Category: {category}, TopN: {topN}, OrderBy: {orderBy}");
                return _reportRepo.GetTopProductsReport(startDate, endDate, category, topN, orderBy);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Top Products Report", ex);
                throw;
            }
        }

        // Report 2: Client Purchases
        /// <summary>
        /// Genera el reporte de compras por cliente
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="clientId">Identificador del cliente para filtrar (opcional)</param>
        /// <param name="topN">Número máximo de clientes a retornar (opcional)</param>
        /// <returns>Lista de clientes con sus estadísticas de compras</returns>
        public List<ClientPurchasesReportDTO> GetClientPurchasesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? clientId = null,
            int? topN = null)
        {
            try
            {
                _logService.Info($"Generating Client Purchases Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, ClientId: {clientId}, TopN: {topN}");
                return _reportRepo.GetClientPurchasesReport(startDate, endDate, clientId, topN);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Client Purchases Report", ex);
                throw;
            }
        }

        // Report 3: Price Variation
        /// <summary>
        /// Genera el reporte de variación de precios de productos
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="productId">Identificador del producto para filtrar (opcional)</param>
        /// <param name="category">Categoría para filtrar (opcional)</param>
        /// <returns>Lista de variaciones de precios por producto</returns>
        public List<PriceVariationReportDTO> GetPriceVariationReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null)
        {
            try
            {
                _logService.Info($"Generating Price Variation Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, ProductId: {productId}, Category: {category}");
                return _reportRepo.GetPriceVariationReport(startDate, endDate, productId, category);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Price Variation Report", ex);
                throw;
            }
        }

        // Report 4: Seller Performance
        /// <summary>
        /// Genera el reporte de rendimiento de vendedores
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="sellerName">Nombre del vendedor para filtrar (opcional)</param>
        /// <param name="category">Categoría para filtrar (opcional)</param>
        /// <returns>Lista de vendedores con sus estadísticas de ventas</returns>
        public List<SellerPerformanceReportDTO> GetSellerPerformanceReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string sellerName = null,
            string category = null)
        {
            try
            {
                _logService.Info($"Generating Seller Performance Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, SellerName: {sellerName}, Category: {category}");
                return _reportRepo.GetSellerPerformanceReport(startDate, endDate, sellerName, category);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Seller Performance Report", ex);
                throw;
            }
        }

        // Report 5: Category Sales
        /// <summary>
        /// Genera el reporte de ventas por categoría de productos
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="category">Categoría específica para filtrar (opcional)</param>
        /// <returns>Lista de categorías con sus estadísticas de ventas</returns>
        public List<CategorySalesReportDTO> GetCategorySalesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null)
        {
            try
            {
                _logService.Info($"Generating Category Sales Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, Category: {category}");
                return _reportRepo.GetCategorySalesReport(startDate, endDate, category);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Category Sales Report", ex);
                throw;
            }
        }

        // Report 6: Revenue by Date
        /// <summary>
        /// Genera el reporte de ingresos por fecha
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="movementType">Tipo de movimiento para filtrar (opcional)</param>
        /// <param name="warehouseId">Identificador del almacén para filtrar (opcional)</param>
        /// <returns>Lista de ingresos agrupados por fecha</returns>
        public List<RevenueByDateReportDTO> GetRevenueByDateReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string movementType = null,
            int? warehouseId = null)
        {
            try
            {
                _logService.Info($"Generating Revenue by Date Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, MovementType: {movementType}, WarehouseId: {warehouseId}");
                return _reportRepo.GetRevenueByDateReport(startDate, endDate, movementType, warehouseId);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Revenue by Date Report", ex);
                throw;
            }
        }

        // Report 7: Client Product Ranking
        /// <summary>
        /// Genera el reporte de ranking de clientes por producto
        /// </summary>
        /// <param name="startDate">Fecha de inicio del período</param>
        /// <param name="endDate">Fecha de fin del período</param>
        /// <param name="productId">Identificador del producto para filtrar (opcional)</param>
        /// <param name="category">Categoría para filtrar (opcional)</param>
        /// <param name="topN">Número máximo de clientes a retornar (opcional)</param>
        /// <returns>Lista de clientes rankeados por compras de productos</returns>
        public List<ClientProductRankingReportDTO> GetClientProductRankingReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null,
            int? topN = null)
        {
            try
            {
                _logService.Info($"Generating Client Product Ranking Report. DateRange: {startDate?.ToString("yyyy-MM-dd")} to {endDate?.ToString("yyyy-MM-dd")}, ProductId: {productId}, Category: {category}, TopN: {topN}");
                return _reportRepo.GetClientProductRankingReport(startDate, endDate, productId, category, topN);
            }
            catch (Exception ex)
            {
                _logService.Error("Error generating Client Product Ranking Report", ex);
                throw;
            }
        }
    }
}
