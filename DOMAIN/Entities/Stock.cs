using System;

namespace DOMAIN.Entities
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        
        // Navigation properties (for display purposes)
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public string WarehouseName { get; set; }
    }
}
