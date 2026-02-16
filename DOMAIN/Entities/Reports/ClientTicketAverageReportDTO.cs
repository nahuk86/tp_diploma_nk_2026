using System;

namespace DOMAIN.Entities.Reports
{
    public class ClientTicketAverageReportDTO
    {
        public int ClientId { get; set; }
        public string ClientFullName { get; set; }
        public string DNI { get; set; }
        public int PurchaseCount { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageTicket { get; set; }
        public decimal MinTicket { get; set; }
        public decimal MaxTicket { get; set; }
        public decimal StdDeviation { get; set; }
    }
}
