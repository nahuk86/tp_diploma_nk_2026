using System;
using System.Collections.Generic;
using DOMAIN.Entities.Reports;

namespace DOMAIN.Contracts
{
    public interface IReportRepository
    {
        // Report 1: Top Products
        List<TopProductsReportDTO> GetTopProductsReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null, 
            int? topN = null,
            string orderBy = "units"); // "units" or "revenue"

        // Report 2: Client Purchases
        List<ClientPurchasesReportDTO> GetClientPurchasesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? clientId = null,
            int? topN = null);

        // Report 3: Price Variation
        List<PriceVariationReportDTO> GetPriceVariationReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null);

        // Report 4: Seller Performance
        List<SellerPerformanceReportDTO> GetSellerPerformanceReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string sellerName = null,
            string category = null);

        // Report 5: Category Sales
        List<CategorySalesReportDTO> GetCategorySalesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null);

        // Report 6: Revenue by Date
        List<RevenueByDateReportDTO> GetRevenueByDateReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string movementType = null,
            int? warehouseId = null);

        // Report 7: Client Product Ranking
        List<ClientProductRankingReportDTO> GetClientProductRankingReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null,
            int? topN = null);
    }
}
