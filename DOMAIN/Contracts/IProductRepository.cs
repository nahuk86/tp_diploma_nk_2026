using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetBySKU(string sku);
        List<Product> Search(string searchTerm);
        List<Product> GetByCategory(string category);
        bool SKUExists(string sku, int? excludeProductId = null);
    }
}
