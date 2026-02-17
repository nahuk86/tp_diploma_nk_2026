using System.Collections.Generic;
using DOMAIN.Entities;

namespace DOMAIN.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Obtiene un producto por su código SKU
        /// </summary>
        Product GetBySKU(string sku);
        
        /// <summary>
        /// Busca productos por término de búsqueda en nombre, descripción o SKU
        /// </summary>
        List<Product> Search(string searchTerm);
        
        /// <summary>
        /// Obtiene todos los productos de una categoría específica
        /// </summary>
        List<Product> GetByCategory(string category);
        
        /// <summary>
        /// Verifica si un SKU ya existe en el sistema, con opción de excluir un producto específico
        /// </summary>
        bool SKUExists(string sku, int? excludeProductId = null);
    }
}
