using System;
using System.Collections.Generic;

namespace DOMAIN.Entities.Reports
{
    public class ClientPurchasesReportDTO
    {
        public int ClientId { get; set; }
        public string ClientFullName { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
        public int PurchaseCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int TotalUnits { get; set; }
        public int DistinctProducts { get; set; }
        public decimal AverageTicket { get; set; }
        public List<ClientProductDetail> ProductDetails { get; set; }
    }

    public class ClientProductDetail
    {
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
