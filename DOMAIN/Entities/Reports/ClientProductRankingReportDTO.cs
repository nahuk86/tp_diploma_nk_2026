using System;

namespace DOMAIN.Entities.Reports
{
    public class ClientProductRankingReportDTO
    {
        public int ClientId { get; set; }
        public string ClientFullName { get; set; }
        public string DNI { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public int UnitsPurchased { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal PercentageOfProductSales { get; set; }
    }
}
