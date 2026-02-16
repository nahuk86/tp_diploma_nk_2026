using System;

namespace DOMAIN.Entities.Reports
{
    public class CategorySalesReportDTO
    {
        public string Category { get; set; }
        public int UnitsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal PercentageOfTotal { get; set; }
    }
}
