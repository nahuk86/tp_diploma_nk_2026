using System.Collections.Generic;
using DOMAIN.Entities;
using DOMAIN.Enums;

namespace DOMAIN.Contracts
{
    public interface IStockMovementRepository
    {
        /// <summary>
        /// Obtiene un movimiento de stock por su identificador
        /// </summary>
        StockMovement GetById(int movementId);
        
        /// <summary>
        /// Obtiene todos los movimientos de stock
        /// </summary>
        List<StockMovement> GetAll();
        
        /// <summary>
        /// Obtiene los movimientos de stock de un tipo específico
        /// </summary>
        List<StockMovement> GetByType(MovementType movementType);
        
        /// <summary>
        /// Obtiene los movimientos de stock de un almacén específico
        /// </summary>
        List<StockMovement> GetByWarehouse(int warehouseId);
        
        /// <summary>
        /// Obtiene los movimientos de stock dentro de un rango de fechas
        /// </summary>
        List<StockMovement> GetByDateRange(System.DateTime startDate, System.DateTime endDate);
        
        /// <summary>
        /// Inserta un nuevo movimiento de stock y retorna su identificador
        /// </summary>
        int Insert(StockMovement movement);
        
        /// <summary>
        /// Genera un número único de movimiento según el tipo
        /// </summary>
        string GenerateMovementNumber(MovementType movementType);
        
        /// <summary>
        /// Obtiene las líneas de detalle de un movimiento de stock
        /// </summary>
        List<StockMovementLine> GetMovementLines(int movementId);
        
        /// <summary>
        /// Inserta una línea de detalle en un movimiento de stock
        /// </summary>
        void InsertLine(StockMovementLine line);
    }
}
