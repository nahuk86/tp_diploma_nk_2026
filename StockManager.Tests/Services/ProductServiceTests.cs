using System;
using System.Collections.Generic;
using BLL.Services;
using DAO.Contracts;
using DAL.Contracts;
using DOMAIN.Entities;
using Moq;
using NUnit.Framework;
using SERVICES;
using SERVICES.Interfaces;

namespace StockManager.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para ProductService.
    /// Verifica las validaciones y operaciones CRUD de productos.
    /// </summary>
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepoMock;
        private Mock<IAuditLogRepository> _auditRepoMock;
        private Mock<ILogService> _logServiceMock;
        private Mock<IErrorHandlerService> _errorHandlerMock;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _auditRepoMock = new Mock<IAuditLogRepository>();
            _logServiceMock = new Mock<ILogService>();
            _errorHandlerMock = new Mock<IErrorHandlerService>();

            _productService = new ProductService(
                _productRepoMock.Object,
                _auditRepoMock.Object,
                _logServiceMock.Object,
                _errorHandlerMock.Object);
        }

        // ─── CreateProduct – Validaciones ─────────────────────────────────────────

        [Test]
        public void CreateProduct_ConProductoNull_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _productService.CreateProduct(null));
        }

        [Test]
        public void CreateProduct_ConSKUVacio_LanzaArgumentException()
        {
            var product = new Product { SKU = "", Name = "Producto", Category = "Cat", UnitPrice = 10 };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConSKUNulo_LanzaArgumentException()
        {
            var product = new Product { SKU = null, Name = "Producto", Category = "Cat", UnitPrice = 10 };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConSKUDemasiadoLargo_LanzaArgumentException()
        {
            var product = new Product
            {
                SKU = new string('A', 51),
                Name = "Producto",
                Category = "Cat",
                UnitPrice = 10
            };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConNombreVacio_LanzaArgumentException()
        {
            var product = new Product { SKU = "SKU-001", Name = "", Category = "Cat", UnitPrice = 10 };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConCategoriaVacia_LanzaArgumentException()
        {
            var product = new Product { SKU = "SKU-001", Name = "Producto", Category = "", UnitPrice = 10 };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConPrecioNegativo_LanzaArgumentException()
        {
            var product = new Product { SKU = "SKU-001", Name = "Producto", Category = "Cat", UnitPrice = -5 };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConStockMinimoNegativo_LanzaArgumentException()
        {
            var product = new Product
            {
                SKU = "SKU-001",
                Name = "Producto",
                Category = "Cat",
                UnitPrice = 10,
                MinStockLevel = -1
            };
            Assert.Throws<ArgumentException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConSKUDuplicado_LanzaInvalidOperationException()
        {
            // Arrange
            var product = new Product { SKU = "SKU-DUP", Name = "Producto", Category = "Cat", UnitPrice = 10 };
            _productRepoMock.Setup(r => r.SKUExists("SKU-DUP", null)).Returns(true);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _productService.CreateProduct(product));
        }

        [Test]
        public void CreateProduct_ConDatosValidos_RetornaIdDelProducto()
        {
            // Arrange
            var product = new Product { SKU = "SKU-NEW", Name = "Producto Nuevo", Category = "Electrónica", UnitPrice = 100 };
            _productRepoMock.Setup(r => r.SKUExists("SKU-NEW", null)).Returns(false);
            _productRepoMock.Setup(r => r.Insert(It.IsAny<Product>())).Returns(99);

            // Act
            int id = _productService.CreateProduct(product);

            // Assert
            Assert.AreEqual(99, id);
        }

        [Test]
        public void CreateProduct_ConDatosValidos_LlamaInsertUnaVez()
        {
            // Arrange
            var product = new Product { SKU = "SKU-OK", Name = "Producto", Category = "Cat", UnitPrice = 50 };
            _productRepoMock.Setup(r => r.SKUExists("SKU-OK", null)).Returns(false);
            _productRepoMock.Setup(r => r.Insert(It.IsAny<Product>())).Returns(1);

            // Act
            _productService.CreateProduct(product);

            // Assert
            _productRepoMock.Verify(r => r.Insert(It.IsAny<Product>()), Times.Once);
        }

        // ─── GetProductById ───────────────────────────────────────────────────────

        [Test]
        public void GetProductById_ProductoExistente_RetornaProducto()
        {
            // Arrange
            var expected = new Product { ProductId = 5, SKU = "SKU-005", Name = "Test" };
            _productRepoMock.Setup(r => r.GetById(5)).Returns(expected);

            // Act
            var result = _productService.GetProductById(5);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ProductId);
        }

        [Test]
        public void GetProductById_ProductoInexistente_RetornaNull()
        {
            // Arrange
            _productRepoMock.Setup(r => r.GetById(999)).Returns((Product)null);

            // Act
            var result = _productService.GetProductById(999);

            // Assert
            Assert.IsNull(result);
        }

        // ─── SearchProducts ───────────────────────────────────────────────────────

        [Test]
        public void SearchProducts_ConTerminoVacio_LlamaGetAll()
        {
            // Arrange
            var allProducts = new List<Product>
            {
                new Product { ProductId = 1, SKU = "A" },
                new Product { ProductId = 2, SKU = "B" }
            };
            _productRepoMock.Setup(r => r.GetAll()).Returns(allProducts);

            // Act
            var result = _productService.SearchProducts("");

            // Assert
            Assert.AreEqual(2, result.Count);
            _productRepoMock.Verify(r => r.GetAll(), Times.Once);
            _productRepoMock.Verify(r => r.Search(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void SearchProducts_ConTerminoValido_LlamaSearch()
        {
            // Arrange
            var found = new List<Product> { new Product { ProductId = 1, SKU = "LAPTOP-001" } };
            _productRepoMock.Setup(r => r.Search("laptop")).Returns(found);

            // Act
            var result = _productService.SearchProducts("laptop");

            // Assert
            Assert.AreEqual(1, result.Count);
            _productRepoMock.Verify(r => r.Search("laptop"), Times.Once);
        }

        // ─── DeleteProduct ────────────────────────────────────────────────────────

        [Test]
        public void DeleteProduct_ProductoInexistente_LanzaInvalidOperationException()
        {
            // Arrange
            _productRepoMock.Setup(r => r.GetById(999)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _productService.DeleteProduct(999));
        }

        [Test]
        public void DeleteProduct_ProductoExistente_LlamaSoftDelete()
        {
            // Arrange
            var product = new Product { ProductId = 1, SKU = "SKU-001", Name = "Producto", IsActive = true };
            _productRepoMock.Setup(r => r.GetById(1)).Returns(product);

            // Simular sesión activa
            SessionContext.Instance.CurrentUser = new DOMAIN.Entities.User { UserId = 1, Username = "admin" };

            // Act
            _productService.DeleteProduct(1);

            // Assert
            _productRepoMock.Verify(r => r.SoftDelete(1, It.IsAny<int>()), Times.Once);

            // Cleanup
            SessionContext.Instance.Clear();
        }

        // ─── UpdateProduct ────────────────────────────────────────────────────────

        [Test]
        public void UpdateProduct_ProductoInexistente_LanzaInvalidOperationException()
        {
            // Arrange
            var product = new Product { ProductId = 99, SKU = "SKU-099", Name = "Test", Category = "Cat", UnitPrice = 10 };
            _productRepoMock.Setup(r => r.SKUExists("SKU-099", 99)).Returns(false);
            _productRepoMock.Setup(r => r.GetById(99)).Returns((Product)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _productService.UpdateProduct(product));
        }

        [Test]
        public void UpdateProduct_SKUDuplicadoDeOtroProducto_LanzaInvalidOperationException()
        {
            // Arrange
            var product = new Product { ProductId = 1, SKU = "SKU-DUP", Name = "Producto", Category = "Cat", UnitPrice = 10 };
            _productRepoMock.Setup(r => r.SKUExists("SKU-DUP", 1)).Returns(true);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _productService.UpdateProduct(product));
        }
    }
}
