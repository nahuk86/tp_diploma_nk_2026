using System.Collections.Generic;
using DOMAIN.Entities;
using DOMAIN.Enums;

namespace DOMAIN.Contracts
{
    public interface IStockMovementRepository
    {
        StockMovement GetById(int movementId);
        List<StockMovement> GetAll();
        List<StockMovement> GetByType(MovementType movementType);
        List<StockMovement> GetByWarehouse(int warehouseId);
        List<StockMovement> GetByDateRange(System.DateTime startDate, System.DateTime endDate);
        int Insert(StockMovement movement);
        string GenerateMovementNumber(MovementType movementType);
        
        // Line items
        List<StockMovementLine> GetMovementLines(int movementId);
        void InsertLine(StockMovementLine line);
    }
}
