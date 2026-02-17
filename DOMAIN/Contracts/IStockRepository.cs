using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IStockRepository
    {
        /// <summary>
        /// Obtiene el registro de stock para un producto en un almacén específico
        /// </summary>
        Stock GetByProductAndWarehouse(int productId, int warehouseId);
        
        /// <summary>
        /// Obtiene todos los registros de stock de un producto en todos los almacenes
        /// </summary>
        List<Stock> GetByProduct(int productId);
        
        /// <summary>
        /// Obtiene todos los registros de stock de un almacén específico
        /// </summary>
        List<Stock> GetByWarehouse(int warehouseId);
        
        /// <summary>
        /// Obtiene todos los registros de stock del sistema
        /// </summary>
        List<Stock> GetAll();
        
        /// <summary>
        /// Obtiene los productos con stock bajo el nivel mínimo
        /// </summary>
        List<Stock> GetLowStock();
        
        /// <summary>
        /// Actualiza la cantidad de stock de un producto en un almacén
        /// </summary>
        void UpdateStock(int productId, int warehouseId, int quantity, int updatedBy);
        
        /// <summary>
        /// Obtiene la cantidad actual de stock de un producto en un almacén
        /// </summary>
        int GetCurrentStock(int productId, int warehouseId);
    }
}
