using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IStockRepository
    {
        Stock GetByProductAndWarehouse(int productId, int warehouseId);
        List<Stock> GetByProduct(int productId);
        List<Stock> GetByWarehouse(int warehouseId);
        List<Stock> GetAll();
        List<Stock> GetLowStock();
        void UpdateStock(int productId, int warehouseId, int quantity, int updatedBy);
        int GetCurrentStock(int productId, int warehouseId);
    }
}
