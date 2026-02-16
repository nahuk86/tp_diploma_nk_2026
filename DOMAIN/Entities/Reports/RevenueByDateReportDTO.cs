using System;

namespace DOMAIN.Entities.Reports
{
    public class RevenueByDateReportDTO
    {
        public DateTime ReportDate { get; set; }
        public decimal SalesRevenue { get; set; }
        public int StockInMovements { get; set; }
        public int StockInUnits { get; set; }
        public int StockOutMovements { get; set; }
        public int StockOutUnits { get; set; }
    }
}
