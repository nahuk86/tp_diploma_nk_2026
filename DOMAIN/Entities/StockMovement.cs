using System;
using DOMAIN.Enums;

namespace DOMAIN.Entities
{
    public class StockMovement
    {
        public int MovementId { get; set; }
        public string MovementNumber { get; set; }
        public MovementType MovementType { get; set; }
        public DateTime MovementDate { get; set; }
        public int? SourceWarehouseId { get; set; }
        public int? DestinationWarehouseId { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        
        // Navigation properties
        public string CreatedByUsername { get; set; }
        public string SourceWarehouseName { get; set; }
        public string DestinationWarehouseName { get; set; }
    }
}
