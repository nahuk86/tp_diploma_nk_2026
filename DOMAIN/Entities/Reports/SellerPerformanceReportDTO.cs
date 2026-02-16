using System;

namespace DOMAIN.Entities.Reports
{
    public class SellerPerformanceReportDTO
    {
        public string SellerName { get; set; }
        public int TotalSales { get; set; }
        public int TotalUnits { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageTicket { get; set; }
        public string TopProduct { get; set; }
        public int TopProductQuantity { get; set; }
    }
}
