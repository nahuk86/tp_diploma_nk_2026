using DOMAIN.Entities;
using System.Collections.Generic;

namespace DOMAIN.Contracts
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Sale GetByIdWithLines(int saleId);
        List<Sale> GetAllWithDetails();
        List<Sale> GetBySeller(string sellerName);
        List<Sale> GetByClient(int clientId);
        List<Sale> GetByDateRange(System.DateTime startDate, System.DateTime endDate);
        int CreateWithLines(Sale sale, List<SaleLine> saleLines);
        void DeleteSaleLines(int saleId);
    }
}
