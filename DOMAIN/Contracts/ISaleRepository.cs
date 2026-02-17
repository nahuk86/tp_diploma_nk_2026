using DOMAIN.Entities;
using System.Collections.Generic;

namespace DOMAIN.Contracts
{
    public interface ISaleRepository : IRepository<Sale>
    {
        /// <summary>
        /// Obtiene una venta con todas sus líneas de detalle
        /// </summary>
        Sale GetByIdWithLines(int saleId);
        
        /// <summary>
        /// Obtiene todas las ventas con información detallada
        /// </summary>
        List<Sale> GetAllWithDetails();
        
        /// <summary>
        /// Obtiene las ventas realizadas por un vendedor específico
        /// </summary>
        List<Sale> GetBySeller(string sellerName);
        
        /// <summary>
        /// Obtiene las ventas realizadas a un cliente específico
        /// </summary>
        List<Sale> GetByClient(int clientId);
        
        /// <summary>
        /// Obtiene las ventas dentro de un rango de fechas
        /// </summary>
        List<Sale> GetByDateRange(System.DateTime startDate, System.DateTime endDate);
        
        /// <summary>
        /// Crea una venta completa con sus líneas de detalle en una transacción
        /// </summary>
        int CreateWithLines(Sale sale, List<SaleLine> saleLines);
        
        /// <summary>
        /// Elimina todas las líneas de detalle de una venta
        /// </summary>
        void DeleteSaleLines(int saleId);
    }
}
