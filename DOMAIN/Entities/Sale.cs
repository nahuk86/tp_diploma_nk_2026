using System;
using System.Collections.Generic;

namespace DOMAIN.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public int? ClientId { get; set; }
        public string SellerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        
        // Navigation properties
        public List<SaleLine> SaleLines { get; set; }
    }
}
