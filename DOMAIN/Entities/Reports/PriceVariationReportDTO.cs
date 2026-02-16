using System;

namespace DOMAIN.Entities.Reports
{
    public class PriceVariationReportDTO
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal ListPrice { get; set; }
        public decimal MinSalePrice { get; set; }
        public decimal MaxSalePrice { get; set; }
        public decimal AverageSalePrice { get; set; }
        public decimal AbsoluteVariation { get; set; }
        public decimal PercentageVariation { get; set; }
    }
}
