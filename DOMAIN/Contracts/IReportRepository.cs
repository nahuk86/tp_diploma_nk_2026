using System;
using System.Collections.Generic;
using DOMAIN.Entities.Reports;

namespace DOMAIN.Contracts
{
    public interface IReportRepository
    {
        /// <summary>
        /// Genera el reporte de productos más vendidos con filtros opcionales
        /// </summary>
        List<TopProductsReportDTO> GetTopProductsReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null, 
            int? topN = null,
            string orderBy = "units"); // "units" or "revenue"

        /// <summary>
        /// Genera el reporte de compras realizadas por clientes
        /// </summary>
        List<ClientPurchasesReportDTO> GetClientPurchasesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? clientId = null,
            int? topN = null);

        /// <summary>
        /// Genera el reporte de variación de precios de productos
        /// </summary>
        List<PriceVariationReportDTO> GetPriceVariationReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null);

        /// <summary>
        /// Genera el reporte de rendimiento de vendedores
        /// </summary>
        List<SellerPerformanceReportDTO> GetSellerPerformanceReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string sellerName = null,
            string category = null);

        /// <summary>
        /// Genera el reporte de ventas por categoría de productos
        /// </summary>
        List<CategorySalesReportDTO> GetCategorySalesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null);

        /// <summary>
        /// Genera el reporte de ingresos y movimientos de stock por fecha
        /// </summary>
        List<RevenueByDateReportDTO> GetRevenueByDateReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string movementType = null,
            int? warehouseId = null);

        /// <summary>
        /// Genera el reporte de ranking de clientes por producto comprado
        /// </summary>
        List<ClientProductRankingReportDTO> GetClientProductRankingReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null,
            int? topN = null);
    }
}
