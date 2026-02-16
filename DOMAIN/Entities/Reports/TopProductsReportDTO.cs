using System;

namespace DOMAIN.Entities.Reports
{
    public class TopProductsReportDTO
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int UnitsSold { get; set; }
        public decimal Revenue { get; set; }
        public decimal ListPrice { get; set; }
        public decimal AverageSalePrice { get; set; }
        public int Ranking { get; set; }
    }
}
