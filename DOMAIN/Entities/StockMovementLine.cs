namespace DOMAIN.Entities
{
    public class StockMovementLine
    {
        public int LineId { get; set; }
        public int MovementId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        
        // Navigation properties
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
    }
}
