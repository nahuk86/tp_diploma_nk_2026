using System;
using System.Collections.Generic;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

        /// <summary>
        /// Inicializa el servicio de productos con sus dependencias
        /// </summary>
        /// <param name="productRepo">Repositorio de productos</param>
        /// <param name="auditRepo">Repositorio de auditoría</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        public ProductService(IProductRepository productRepo, IAuditLogRepository auditRepo, ILogService logService)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Obtiene todos los productos del sistema
        /// </summary>
        /// <returns>Lista de todos los productos</returns>
        public List<Product> GetAllProducts()
        {
            try
            {
                return _productRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all products", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los productos activos del sistema
        /// </summary>
        /// <returns>Lista de productos activos</returns>
        public List<Product> GetActiveProducts()
        {
            try
            {
                return _productRepo.GetAllActive();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving active products", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un producto por su identificador
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Producto encontrado o null si no existe</returns>
        public Product GetProductById(int productId)
        {
            try
            {
                return _productRepo.GetById(productId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving product {productId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Busca productos por término de búsqueda
        /// </summary>
        /// <param name="searchTerm">Término a buscar en nombre, SKU o descripción</param>
        /// <returns>Lista de productos que coinciden con la búsqueda</returns>
        public List<Product> SearchProducts(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return GetAllProducts();

                return _productRepo.Search(searchTerm);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error searching products with term: {searchTerm}", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene los productos de una categoría específica
        /// </summary>
        /// <param name="category">Nombre de la categoría</param>
        /// <returns>Lista de productos de la categoría</returns>
        public List<Product> GetProductsByCategory(string category)
        {
            try
            {
                return _productRepo.GetByCategory(category);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving products by category: {category}", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema
        /// </summary>
        /// <param name="product">Datos del producto a crear</param>
        /// <returns>Identificador del producto creado</returns>
        public int CreateProduct(Product product)
        {
            try
            {
                // Validations
                ValidateProduct(product);

                // Check for duplicate SKU
                if (_productRepo.SKUExists(product.SKU))
                {
                    throw new InvalidOperationException($"SKU '{product.SKU}' already exists. Please use a unique SKU.");
                }

                // Set audit fields
                product.CreatedAt = DateTime.Now;
                product.CreatedBy = SessionContext.Instance.CurrentUserId;
                product.IsActive = true;

                // Insert
                var productId = _productRepo.Insert(product);

                // Audit log
                _auditRepo.LogChange("Products", productId, AuditAction.Insert, null, null, 
                    $"Created product {product.SKU} - {product.Name}", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Product created: {product.SKU} - {product.Name} by {SessionContext.Instance.CurrentUsername}");

                return productId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating product: {product.SKU}", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un producto existente
        /// </summary>
        /// <param name="product">Datos actualizados del producto</param>
        public void UpdateProduct(Product product)
        {
            try
            {
                // Validations
                ValidateProduct(product);

                // Check for duplicate SKU (excluding current product)
                if (_productRepo.SKUExists(product.SKU, product.ProductId))
                {
                    throw new InvalidOperationException($"SKU '{product.SKU}' already exists. Please use a unique SKU.");
                }

                // Get old values for audit
                var oldProduct = _productRepo.GetById(product.ProductId);
                if (oldProduct == null)
                {
                    throw new InvalidOperationException($"Product with ID {product.ProductId} not found.");
                }

                // Set audit fields
                product.UpdatedAt = DateTime.Now;
                product.UpdatedBy = SessionContext.Instance.CurrentUserId;

                // Update
                _productRepo.Update(product);

                // Audit log - log each changed field
                LogFieldChange("Products", product.ProductId, "Name", oldProduct.Name, product.Name);
                LogFieldChange("Products", product.ProductId, "Description", oldProduct.Description, product.Description);
                LogFieldChange("Products", product.ProductId, "Category", oldProduct.Category, product.Category);
                LogFieldChange("Products", product.ProductId, "UnitPrice", oldProduct.UnitPrice.ToString(), product.UnitPrice.ToString());
                LogFieldChange("Products", product.ProductId, "MinStockLevel", oldProduct.MinStockLevel.ToString(), product.MinStockLevel.ToString());

                _logService.Info($"Product updated: {product.SKU} - {product.Name} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating product: {product.ProductId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un producto del sistema (borrado lógico)
        /// </summary>
        /// <param name="productId">Identificador del producto a eliminar</param>
        public void DeleteProduct(int productId)
        {
            try
            {
                var product = _productRepo.GetById(productId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {productId} not found.");
                }

                // Soft delete
                _productRepo.SoftDelete(productId, SessionContext.Instance.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Products", productId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Product deleted (soft): {product.SKU} - {product.Name} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting product: {productId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos del producto cumplan con las reglas de negocio
        /// </summary>
        /// <param name="product">Producto a validar</param>
        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (string.IsNullOrWhiteSpace(product.SKU))
                throw new ArgumentException("SKU is required.", nameof(product.SKU));

            if (product.SKU.Length > 50)
                throw new ArgumentException("SKU must be 50 characters or less.", nameof(product.SKU));

            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Name is required.", nameof(product.Name));

            if (product.Name.Length > 100)
                throw new ArgumentException("Name must be 100 characters or less.", nameof(product.Name));

            if (string.IsNullOrWhiteSpace(product.Category))
                throw new ArgumentException("Category is required.", nameof(product.Category));

            if (product.UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative.", nameof(product.UnitPrice));

            if (product.MinStockLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative.", nameof(product.MinStockLevel));
        }

        /// <summary>
        /// Registra un cambio de campo en la auditoría si el valor ha cambiado
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="recordId">Identificador del registro</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="oldValue">Valor anterior</param>
        /// <param name="newValue">Valor nuevo</param>
        private void LogFieldChange(string tableName, int recordId, string fieldName, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                _auditRepo.LogChange(tableName, recordId, AuditAction.Update, fieldName, oldValue, newValue, SessionContext.Instance.CurrentUserId);
            }
        }
    }
}
