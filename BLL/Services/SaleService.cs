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
    public class SaleService
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IProductRepository _productRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

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

        public int CreateSale(Sale sale, List<SaleLine> saleLines, int currentUserId)
        {
            try
            {
                // Validate sale
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
        }

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

        private void ValidateSale(Sale sale, List<SaleLine> saleLines)
        {
            if (sale == null)
                throw new ArgumentNullException(nameof(sale));

            if (string.IsNullOrWhiteSpace(sale.SellerName))
                throw new ArgumentException("Seller name is required");

            if (saleLines == null || saleLines.Count == 0)
                throw new ArgumentException("Sale must have at least one line item");

            // Validate client if specified
            if (sale.ClientId.HasValue)
            {
                var client = _clientRepo.GetById(sale.ClientId.Value);
                if (client == null || !client.IsActive)
                    throw new InvalidOperationException($"Client {sale.ClientId} not found or inactive");
            }

            // Validate sale lines
            foreach (var line in saleLines)
            {
                if (line.Quantity <= 0)
                    throw new ArgumentException($"Quantity must be positive for product {line.ProductId}");

                if (line.UnitPrice < 0)
                    throw new ArgumentException($"Unit price cannot be negative for product {line.ProductId}");

                // Verify product exists
                var product = _productRepo.GetById(line.ProductId);
                if (product == null || !product.IsActive)
                    throw new InvalidOperationException($"Product {line.ProductId} not found or inactive");

                // Calculate line total
                line.LineTotal = line.Quantity * line.UnitPrice;
            }
        }

        private string GenerateSaleNumber()
        {
            return "S-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}
