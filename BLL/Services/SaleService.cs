using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class SaleService
    {
        /// <summary>
        /// Semáforo estático que garantiza que solo un hilo a la vez pueda
        /// ejecutar la sección crítica de creación de venta y descuento de stock,
        /// evitando condiciones de carrera entre usuarios concurrentes.
        /// Su tiempo de vida es el de la aplicación, por lo que no requiere disposición explícita.
        /// </summary>
        private static readonly SemaphoreSlim _saleLock = new SemaphoreSlim(1, 1);

        private readonly ISaleRepository _saleRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IProductRepository _productRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

        /// <summary>
        /// Inicializa el servicio de ventas con sus dependencias
        /// </summary>
        /// <param name="saleRepo">Repositorio de ventas</param>
        /// <param name="clientRepo">Repositorio de clientes</param>
        /// <param name="productRepo">Repositorio de productos</param>
        /// <param name="stockRepo">Repositorio de inventario</param>
        /// <param name="auditRepo">Repositorio de auditoría</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        public SaleService(
            ISaleRepository saleRepo,
            IClientRepository clientRepo,
            IProductRepository productRepo,
            IStockRepository stockRepo,
            IAuditLogRepository auditRepo,
            ILogService logService)
        {
            _saleRepo = saleRepo ?? throw new ArgumentNullException(nameof(saleRepo));
            _clientRepo = clientRepo ?? throw new ArgumentNullException(nameof(clientRepo));
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _stockRepo = stockRepo ?? throw new ArgumentNullException(nameof(stockRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Obtiene todas las ventas del sistema
        /// </summary>
        /// <returns>Lista de todas las ventas</returns>
        public List<Sale> GetAllSales()
        {
            try
            {
                return _saleRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all sales", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todas las ventas con sus detalles completos
        /// </summary>
        /// <returns>Lista de ventas con información de cliente y vendedor</returns>
        public List<Sale> GetAllSalesWithDetails()
        {
            try
            {
                return _saleRepo.GetAllWithDetails();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all sales with details", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una venta por su identificador
        /// </summary>
        /// <param name="saleId">Identificador de la venta</param>
        /// <returns>Venta encontrada o null si no existe</returns>
        public Sale GetSaleById(int saleId)
        {
            try
            {
                return _saleRepo.GetById(saleId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving sale {saleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene una venta con sus líneas de detalle
        /// </summary>
        /// <param name="saleId">Identificador de la venta</param>
        /// <returns>Venta con sus líneas de detalle</returns>
        public Sale GetSaleByIdWithLines(int saleId)
        {
            try
            {
                return _saleRepo.GetByIdWithLines(saleId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving sale {saleId} with lines", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las ventas realizadas por un vendedor específico
        /// </summary>
        /// <param name="sellerName">Nombre del vendedor</param>
        /// <returns>Lista de ventas del vendedor</returns>
        public List<Sale> GetSalesBySeller(string sellerName)
        {
            try
            {
                return _saleRepo.GetBySeller(sellerName);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving sales by seller: {sellerName}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las ventas realizadas a un cliente específico
        /// </summary>
        /// <param name="clientId">Identificador del cliente</param>
        /// <returns>Lista de ventas del cliente</returns>
        public List<Sale> GetSalesByClient(int clientId)
        {
            try
            {
                return _saleRepo.GetByClient(clientId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving sales for client {clientId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene las ventas realizadas en un rango de fechas
        /// </summary>
        /// <param name="startDate">Fecha de inicio</param>
        /// <param name="endDate">Fecha de fin</param>
        /// <returns>Lista de ventas en el rango de fechas</returns>
        public List<Sale> GetSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _saleRepo.GetByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving sales for date range {startDate} to {endDate}", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea una nueva venta con sus líneas de detalle y descuenta el inventario.
        /// El acceso a la sección crítica de validación de stock y creación de la venta
        /// está protegido por un semáforo para evitar condiciones de carrera entre hilos.
        /// </summary>
        /// <param name="sale">Datos de la venta</param>
        /// <param name="saleLines">Líneas de detalle de la venta</param>
        /// <param name="currentUserId">Identificador del usuario que registra la venta</param>
        /// <returns>Identificador de la venta creada</returns>
        public int CreateSale(Sale sale, List<SaleLine> saleLines, int currentUserId)
        {
            bool lockAcquired = false;
            try
            {
                _saleLock.Wait();
                lockAcquired = true;

                // Validate sale (includes stock availability check)
                ValidateSale(sale, saleLines);

                // Set audit fields
                sale.CreatedBy = currentUserId;
                sale.CreatedAt = DateTime.Now;

                // Generate sale number
                sale.SaleNumber = GenerateSaleNumber();

                // Calculate total
                sale.TotalAmount = saleLines.Sum(l => l.LineTotal);

                // Create sale with lines
                int saleId = _saleRepo.CreateWithLines(sale, saleLines);

                // Deduct inventory from warehouses
                DeductInventoryForSale(saleLines, currentUserId, sale.SaleNumber);

                // Log audit
                _auditRepo.LogChange("Sales", saleId, AuditAction.Insert, null, null, 
                    $"Sale {sale.SaleNumber} created with {saleLines.Count} items, Total: {sale.TotalAmount:C}", 
                    currentUserId);

                _logService.Info($"Sale {sale.SaleNumber} created successfully by user {currentUserId}");
                
                return saleId;
            }
            catch (Exception ex)
            {
                _logService.Error("Error creating sale", ex);
                throw;
            }
            finally
            {
                if (lockAcquired)
                    _saleLock.Release();
            }
        }

        /// <summary>
        /// Actualiza los datos de una venta existente
        /// </summary>
        /// <param name="sale">Datos actualizados de la venta</param>
        /// <param name="currentUserId">Identificador del usuario que actualiza</param>
        public void UpdateSale(Sale sale, int currentUserId)
        {
            try
            {
                var existingSale = _saleRepo.GetById(sale.SaleId);
                if (existingSale == null)
                    throw new InvalidOperationException($"Sale with ID {sale.SaleId} not found");

                // Validate
                if (string.IsNullOrWhiteSpace(sale.SellerName))
                    throw new ArgumentException("Seller name is required");

                // Set audit fields
                sale.UpdatedBy = currentUserId;
                sale.UpdatedAt = DateTime.Now;

                _saleRepo.Update(sale);

                // Log audit
                _auditRepo.LogChange("Sales", sale.SaleId, AuditAction.Update, null,
                    $"Seller: {existingSale.SellerName}, Total: {existingSale.TotalAmount:C}",
                    $"Seller: {sale.SellerName}, Total: {sale.TotalAmount:C}",
                    currentUserId);

                _logService.Info($"Sale {sale.SaleNumber} updated by user {currentUserId}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating sale {sale.SaleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina una venta del sistema
        /// </summary>
        /// <param name="saleId">Identificador de la venta a eliminar</param>
        /// <param name="currentUserId">Identificador del usuario que elimina</param>
        public void DeleteSale(int saleId, int currentUserId)
        {
            try
            {
                var sale = _saleRepo.GetById(saleId);
                if (sale == null)
                    throw new InvalidOperationException($"Sale with ID {saleId} not found");

                _saleRepo.Delete(saleId);

                // Log audit
                _auditRepo.LogChange("Sales", saleId, AuditAction.Delete, null,
                    $"Sale {sale.SaleNumber} - Total: {sale.TotalAmount:C}",
                    "Deleted (soft delete)",
                    currentUserId);

                _logService.Info($"Sale {sale.SaleNumber} deleted by user {currentUserId}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting sale {saleId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el stock disponible de un producto por almacén
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Diccionario con el stock disponible por almacén</returns>
        public Dictionary<int, int> GetAvailableStockByWarehouse(int productId)
        {
            try
            {
                var stockRecords = _stockRepo.GetByProduct(productId);
                var result = new Dictionary<int, int>();
                
                foreach (var stock in stockRecords)
                {
                    result[stock.WarehouseId] = stock.Quantity;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving stock for product {productId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el stock total disponible de un producto en todos los almacenes
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Cantidad total disponible</returns>
        public int GetTotalAvailableStock(int productId)
        {
            try
            {
                var stockRecords = _stockRepo.GetByProduct(productId);
                return stockRecords.Sum(s => s.Quantity);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving total stock for product {productId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos de la venta y sus líneas cumplan con las reglas de negocio
        /// </summary>
        /// <param name="sale">Venta a validar</param>
        /// <param name="saleLines">Líneas de detalle de la venta</param>
        private void ValidateSale(Sale sale, List<SaleLine> saleLines)
        {
            if (sale == null)
                throw new ArgumentNullException(nameof(sale));

            if (string.IsNullOrWhiteSpace(sale.SellerName))
                throw new ArgumentException("Seller name is required");

            if (saleLines == null || saleLines.Count == 0)
                throw new ArgumentException("Sale must have at least one line item");

            // Validate client is required
            if (!sale.ClientId.HasValue)
                throw new ArgumentException("Se requiere seleccionar un cliente para la venta");

            // Validate client exists and is active
            var client = _clientRepo.GetById(sale.ClientId.Value);
            if (client == null || !client.IsActive)
                throw new InvalidOperationException($"Client {sale.ClientId} not found or inactive");

            // Group sale lines by product to validate total quantities
            var productQuantities = saleLines
                .GroupBy(l => l.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(l => l.Quantity));

            // Validate aggregated quantities against stock for each unique product
            foreach (var productGroup in productQuantities)
            {
                var productId = productGroup.Key;
                var totalQuantity = productGroup.Value;

                var product = _productRepo.GetById(productId);
                if (product == null || !product.IsActive)
                    throw new InvalidOperationException($"Product {productId} not found or inactive");

                var availableStock = GetTotalAvailableStock(productId);
                if (totalQuantity > availableStock)
                {
                    throw new InvalidOperationException(
                        $"Cantidad insuficiente para el producto '{product.Name}'. " +
                        $"Total solicitado: {totalQuantity}, Disponible: {availableStock}");
                }
            }

            // Validate individual sale lines
            foreach (var line in saleLines)
            {
                if (line.Quantity <= 0)
                    throw new ArgumentException($"Quantity must be positive for product {line.ProductId}");

                if (line.UnitPrice < 0)
                    throw new ArgumentException($"Unit price cannot be negative for product {line.ProductId}");

                // Calculate line total
                line.LineTotal = line.Quantity * line.UnitPrice;
            }
        }

        /// <summary>
        /// Deducts inventory from warehouses when a sale is created.
        /// Uses FIFO approach - deducts from warehouses in order until quantity is satisfied.
        /// </summary>
        private void DeductInventoryForSale(List<SaleLine> saleLines, int currentUserId, string saleNumber)
        {
            // Group lines by product to handle duplicates correctly
            var productQuantities = saleLines
                .GroupBy(l => l.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(l => l.Quantity));

            foreach (var productGroup in productQuantities)
            {
                var productId = productGroup.Key;
                var totalQuantity = productGroup.Value;
                var remainingToDeduct = totalQuantity;

                // Get all warehouses with stock for this product (ordered by warehouse name for consistency)
                var stockRecords = _stockRepo.GetByProduct(productId)
                    .Where(s => s.Quantity > 0)
                    .OrderBy(s => s.WarehouseName)
                    .ToList();

                if (!stockRecords.Any())
                {
                    var product = _productRepo.GetById(productId);
                    throw new InvalidOperationException(
                        $"No hay stock disponible en ningún almacén para el producto '{product.Name}'");
                }

                // Deduct from warehouses until the full quantity is satisfied
                foreach (var stock in stockRecords)
                {
                    if (remainingToDeduct <= 0)
                        break;

                    var quantityToDeduct = Math.Min(remainingToDeduct, stock.Quantity);
                    var newQuantity = stock.Quantity - quantityToDeduct;

                    // Update stock in the warehouse
                    _stockRepo.UpdateStock(productId, stock.WarehouseId, newQuantity, currentUserId);

                    // Log the deduction for audit trail
                    _auditRepo.LogChange("Stock", stock.StockId, AuditAction.Update,
                        "Quantity",
                        stock.Quantity.ToString(),
                        newQuantity.ToString(),
                        currentUserId);

                    _logService.Info(
                        $"Sale {saleNumber}: Deducted {quantityToDeduct} units of product {productId} " +
                        $"from warehouse {stock.WarehouseName} (ID: {stock.WarehouseId}). " +
                        $"Stock reduced from {stock.Quantity} to {newQuantity}");

                    remainingToDeduct -= quantityToDeduct;
                }

                // This should never happen because we validated stock availability earlier
                if (remainingToDeduct > 0)
                {
                    var product = _productRepo.GetById(productId);
                    throw new InvalidOperationException(
                        $"No se pudo descontar la cantidad completa para el producto '{product.Name}'. " +
                        $"Faltaron {remainingToDeduct} unidades.");
                }
            }
        }

        /// <summary>
        /// Genera un número único para la venta
        /// </summary>
        /// <returns>Número de venta generado</returns>
        private string GenerateSaleNumber()
        {
            return "S-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}
