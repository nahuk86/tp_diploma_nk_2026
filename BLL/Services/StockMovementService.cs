using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class StockMovementService
    {
        private readonly IStockMovementRepository _movementRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IProductRepository _productRepo;
        private readonly IWarehouseRepository _warehouseRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

        public StockMovementService(
            IStockMovementRepository movementRepo,
            IStockRepository stockRepo,
            IProductRepository productRepo,
            IWarehouseRepository warehouseRepo,
            IAuditLogRepository auditRepo,
            ILogService logService)
        {
            _movementRepo = movementRepo ?? throw new ArgumentNullException(nameof(movementRepo));
            _stockRepo = stockRepo ?? throw new ArgumentNullException(nameof(stockRepo));
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _warehouseRepo = warehouseRepo ?? throw new ArgumentNullException(nameof(warehouseRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public List<StockMovement> GetAllMovements()
        {
            try
            {
                return _movementRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all stock movements", ex);
                throw;
            }
        }

        public StockMovement GetMovementById(int movementId)
        {
            try
            {
                return _movementRepo.GetById(movementId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving stock movement {movementId}", ex);
                throw;
            }
        }

        public List<StockMovement> GetMovementsByType(MovementType movementType)
        {
            try
            {
                return _movementRepo.GetByType(movementType);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving movements by type: {movementType}", ex);
                throw;
            }
        }

        public List<StockMovement> GetMovementsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _movementRepo.GetByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving movements by date range", ex);
                throw;
            }
        }

        public List<StockMovementLine> GetMovementLines(int movementId)
        {
            try
            {
                return _movementRepo.GetMovementLines(movementId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving movement lines for movement {movementId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new stock movement with its lines and updates the stock accordingly
        /// </summary>
        public int CreateMovement(StockMovement movement, List<StockMovementLine> lines)
        {
            try
            {
                // Validate movement
                ValidateMovement(movement, lines);

                // Generate movement number
                movement.MovementNumber = _movementRepo.GenerateMovementNumber(movement.MovementType);
                movement.CreatedAt = DateTime.Now;
                movement.CreatedBy = SessionContext.CurrentUserId.Value;

                // Insert movement header
                var movementId = _movementRepo.Insert(movement);

                // Insert lines and update stock
                foreach (var line in lines)
                {
                    line.MovementId = movementId;
                    _movementRepo.InsertLine(line);

                    // Update stock based on movement type
                    UpdateStockForMovement(movement.MovementType, movement.SourceWarehouseId, 
                        movement.DestinationWarehouseId, line.ProductId, line.Quantity);
                }

                // Audit log
                _auditRepo.LogChange("StockMovements", movementId, AuditAction.Insert, null, null,
                    $"Created {movement.MovementType} movement {movement.MovementNumber} with {lines.Count} lines",
                    SessionContext.CurrentUserId);

                _logService.Info($"Stock movement created: {movement.MovementNumber} ({movement.MovementType}) by {SessionContext.CurrentUsername}");

                return movementId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating stock movement", ex);
                throw;
            }
        }

        private void ValidateMovement(StockMovement movement, List<StockMovementLine> lines)
        {
            if (movement == null)
                throw new ArgumentNullException(nameof(movement), "Movement cannot be null.");

            if (lines == null || lines.Count == 0)
                throw new InvalidOperationException("Movement must have at least one line.");

            // Validate movement type specific requirements
            switch (movement.MovementType)
            {
                case MovementType.In:
                    if (!movement.DestinationWarehouseId.HasValue)
                        throw new InvalidOperationException("Destination warehouse is required for IN movements.");
                    
                    // Verify warehouse exists
                    var destWarehouse = _warehouseRepo.GetById(movement.DestinationWarehouseId.Value);
                    if (destWarehouse == null || !destWarehouse.IsActive)
                        throw new InvalidOperationException("Destination warehouse does not exist or is inactive.");
                    break;

                case MovementType.Out:
                    if (!movement.SourceWarehouseId.HasValue)
                        throw new InvalidOperationException("Source warehouse is required for OUT movements.");
                    
                    // Verify warehouse exists
                    var srcWarehouse = _warehouseRepo.GetById(movement.SourceWarehouseId.Value);
                    if (srcWarehouse == null || !srcWarehouse.IsActive)
                        throw new InvalidOperationException("Source warehouse does not exist or is inactive.");
                    
                    // Verify sufficient stock for OUT movements
                    foreach (var line in lines)
                    {
                        var currentStock = _stockRepo.GetCurrentStock(line.ProductId, movement.SourceWarehouseId.Value);
                        if (currentStock < line.Quantity)
                        {
                            var product = _productRepo.GetById(line.ProductId);
                            throw new InvalidOperationException(
                                $"Insufficient stock for product '{product.Name}'. Available: {currentStock}, Required: {line.Quantity}");
                        }
                    }
                    break;

                case MovementType.Transfer:
                    if (!movement.SourceWarehouseId.HasValue || !movement.DestinationWarehouseId.HasValue)
                        throw new InvalidOperationException("Both source and destination warehouses are required for TRANSFER movements.");
                    
                    if (movement.SourceWarehouseId == movement.DestinationWarehouseId)
                        throw new InvalidOperationException("Source and destination warehouses must be different for transfers.");
                    
                    // Verify warehouses exist
                    var srcWh = _warehouseRepo.GetById(movement.SourceWarehouseId.Value);
                    var dstWh = _warehouseRepo.GetById(movement.DestinationWarehouseId.Value);
                    if (srcWh == null || !srcWh.IsActive)
                        throw new InvalidOperationException("Source warehouse does not exist or is inactive.");
                    if (dstWh == null || !dstWh.IsActive)
                        throw new InvalidOperationException("Destination warehouse does not exist or is inactive.");
                    
                    // Verify sufficient stock for transfers
                    foreach (var line in lines)
                    {
                        var currentStock = _stockRepo.GetCurrentStock(line.ProductId, movement.SourceWarehouseId.Value);
                        if (currentStock < line.Quantity)
                        {
                            var product = _productRepo.GetById(line.ProductId);
                            throw new InvalidOperationException(
                                $"Insufficient stock for product '{product.Name}'. Available: {currentStock}, Required: {line.Quantity}");
                        }
                    }
                    break;

                case MovementType.Adjustment:
                    if (!movement.DestinationWarehouseId.HasValue)
                        throw new InvalidOperationException("Warehouse is required for ADJUSTMENT movements.");
                    
                    // Verify warehouse exists
                    var adjWarehouse = _warehouseRepo.GetById(movement.DestinationWarehouseId.Value);
                    if (adjWarehouse == null || !adjWarehouse.IsActive)
                        throw new InvalidOperationException("Warehouse does not exist or is inactive.");
                    
                    if (string.IsNullOrWhiteSpace(movement.Reason))
                        throw new InvalidOperationException("Reason is required for ADJUSTMENT movements.");
                    break;
            }

            // Validate lines
            foreach (var line in lines)
            {
                if (line.Quantity <= 0)
                    throw new InvalidOperationException("Line quantity must be greater than zero.");

                // Verify product exists and is active
                var product = _productRepo.GetById(line.ProductId);
                if (product == null || !product.IsActive)
                    throw new InvalidOperationException($"Product with ID {line.ProductId} does not exist or is inactive.");
            }
        }

        private void UpdateStockForMovement(MovementType movementType, int? sourceWarehouseId, 
            int? destinationWarehouseId, int productId, int quantity)
        {
            var userId = SessionContext.CurrentUserId.Value;

            switch (movementType)
            {
                case MovementType.In:
                    // Add stock to destination warehouse
                    var currentIn = _stockRepo.GetCurrentStock(productId, destinationWarehouseId.Value);
                    _stockRepo.UpdateStock(productId, destinationWarehouseId.Value, currentIn + quantity, userId);
                    break;

                case MovementType.Out:
                    // Remove stock from source warehouse
                    var currentOut = _stockRepo.GetCurrentStock(productId, sourceWarehouseId.Value);
                    _stockRepo.UpdateStock(productId, sourceWarehouseId.Value, currentOut - quantity, userId);
                    break;

                case MovementType.Transfer:
                    // Remove from source
                    var currentSource = _stockRepo.GetCurrentStock(productId, sourceWarehouseId.Value);
                    _stockRepo.UpdateStock(productId, sourceWarehouseId.Value, currentSource - quantity, userId);
                    
                    // Add to destination
                    var currentDest = _stockRepo.GetCurrentStock(productId, destinationWarehouseId.Value);
                    _stockRepo.UpdateStock(productId, destinationWarehouseId.Value, currentDest + quantity, userId);
                    break;

                case MovementType.Adjustment:
                    // Adjust stock by adding the specified quantity
                    // Note: For stock reduction adjustments, use OUT movement type instead
                    var currentAdj = _stockRepo.GetCurrentStock(productId, destinationWarehouseId.Value);
                    _stockRepo.UpdateStock(productId, destinationWarehouseId.Value, currentAdj + quantity, userId);
                    break;
            }
        }

        /// <summary>
        /// Checks if any product prices need confirmation before updating (price decrease scenario)
        /// </summary>
        public List<PriceUpdateInfo> CheckPriceUpdates(MovementType movementType, List<StockMovementLine> lines)
        {
            var priceUpdates = new List<PriceUpdateInfo>();

            // Only check for IN movements
            if (movementType != MovementType.In)
                return priceUpdates;

            foreach (var line in lines)
            {
                if (!line.UnitPrice.HasValue || line.UnitPrice.Value <= 0)
                    continue;

                var product = _productRepo.GetById(line.ProductId);
                if (product == null)
                    continue;

                var currentPrice = product.UnitPrice;
                var newPrice = line.UnitPrice.Value;

                if (newPrice != currentPrice)
                {
                    priceUpdates.Add(new PriceUpdateInfo
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        ProductSKU = product.SKU,
                        CurrentPrice = currentPrice,
                        NewPrice = newPrice,
                        NeedsConfirmation = newPrice < currentPrice
                    });
                }
            }

            return priceUpdates;
        }

        /// <summary>
        /// Updates product prices based on stock movement (for IN movements)
        /// </summary>
        public void UpdateProductPrices(List<StockMovementLine> lines, bool confirmLowerPrices = false)
        {
            foreach (var line in lines)
            {
                if (!line.UnitPrice.HasValue || line.UnitPrice.Value <= 0)
                    continue;

                var product = _productRepo.GetById(line.ProductId);
                if (product == null)
                    continue;

                var currentPrice = product.UnitPrice;
                var newPrice = line.UnitPrice.Value;

                // Skip if no change
                if (newPrice == currentPrice)
                    continue;

                // Update if price is higher (automatic) or if lower price is confirmed
                if (newPrice > currentPrice || (newPrice < currentPrice && confirmLowerPrices))
                {
                    var oldPrice = product.UnitPrice;
                    product.UnitPrice = newPrice;
                    product.UpdatedAt = DateTime.Now;
                    product.UpdatedBy = SessionContext.CurrentUserId;

                    _productRepo.Update(product);

                    // Log the price update
                    _auditRepo.LogChange("Products", product.ProductId, AuditAction.Update,
                        "UnitPrice", oldPrice.ToString("F2"), newPrice.ToString("F2"),
                        SessionContext.CurrentUserId);

                    _logService.Info(
                        $"Product price updated via stock movement: {product.SKU} - {product.Name}, " +
                        $"Price: {oldPrice:C} → {newPrice:C} by {SessionContext.CurrentUsername}");
                }
            }
        }
    }

    /// <summary>
    /// Information about a product price that will be updated
    /// </summary>
    public class PriceUpdateInfo
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal NewPrice { get; set; }
        public bool NeedsConfirmation { get; set; }
    }
}
