using System;
using System.Collections.Generic;
using BLL.Services;
using DAO.Contracts;
using DAL.Contracts;
using DOMAIN.Entities;
using Moq;
using NUnit.Framework;
using SERVICES.Interfaces;

namespace StockManager.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para SaleService.
    /// Verifica las validaciones de venta, disponibilidad de stock y cálculo de totales.
    /// </summary>
    [TestFixture]
    public class SaleServiceTests
    {
        private Mock<ISaleRepository> _saleRepoMock;
        private Mock<IClientRepository> _clientRepoMock;
        private Mock<IProductRepository> _productRepoMock;
        private Mock<IStockRepository> _stockRepoMock;
        private Mock<IAuditLogRepository> _auditRepoMock;
        private Mock<ILogService> _logServiceMock;
        private Mock<IErrorHandlerService> _errorHandlerMock;
        private SaleService _saleService;

        [SetUp]
        public void SetUp()
        {
            _saleRepoMock = new Mock<ISaleRepository>();
            _clientRepoMock = new Mock<IClientRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _stockRepoMock = new Mock<IStockRepository>();
            _auditRepoMock = new Mock<IAuditLogRepository>();
            _logServiceMock = new Mock<ILogService>();
            _errorHandlerMock = new Mock<IErrorHandlerService>();

            _saleService = new SaleService(
                _saleRepoMock.Object,
                _clientRepoMock.Object,
                _productRepoMock.Object,
                _stockRepoMock.Object,
                _auditRepoMock.Object,
                _logServiceMock.Object,
                _errorHandlerMock.Object);
        }

        // ─── CreateSale – Validaciones básicas ───────────────────────────────────

        [Test]
        public void CreateSale_ConVentaNull_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => _saleService.CreateSale(null, new List<SaleLine>(), 1));
        }

        [Test]
        public void CreateSale_SinNombreDeVendedor_LanzaArgumentException()
        {
            // Arrange
            var sale = new Sale { SellerName = "", ClientId = 1 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 1, UnitPrice = 10 }
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_SinLineasDeDetalle_LanzaArgumentException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1 };

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => _saleService.CreateSale(sale, new List<SaleLine>(), 1));
        }

        [Test]
        public void CreateSale_ConLineasNull_LanzaArgumentException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1 };

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => _saleService.CreateSale(sale, null, 1));
        }

        [Test]
        public void CreateSale_SinCliente_LanzaArgumentException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = null };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 1, UnitPrice = 10 }
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_ClienteInactivo_LanzaInvalidOperationException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 5 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 1, UnitPrice = 10 }
            };

            _clientRepoMock.Setup(r => r.GetById(5))
                .Returns(new Client { ClientId = 5, IsActive = false });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_ClienteInexistente_LanzaInvalidOperationException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 99 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 1, UnitPrice = 10 }
            };

            _clientRepoMock.Setup(r => r.GetById(99)).Returns((Client)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_ProductoInactivo_LanzaInvalidOperationException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 10, Quantity = 1, UnitPrice = 50 }
            };

            _clientRepoMock.Setup(r => r.GetById(1))
                .Returns(new Client { ClientId = 1, IsActive = true });
            _productRepoMock.Setup(r => r.GetById(10))
                .Returns(new Product { ProductId = 10, IsActive = false, Name = "Producto Inactivo" });
            _stockRepoMock.Setup(r => r.GetByProduct(10))
                .Returns(new List<Stock> { new Stock { ProductId = 10, Quantity = 100 } });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_StockInsuficiente_LanzaInvalidOperationException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 100, UnitPrice = 50 }
            };

            _clientRepoMock.Setup(r => r.GetById(1))
                .Returns(new Client { ClientId = 1, IsActive = true });
            _productRepoMock.Setup(r => r.GetById(1))
                .Returns(new Product { ProductId = 1, IsActive = true, Name = "Producto" });
            // Stock disponible: solo 5 unidades
            _stockRepoMock.Setup(r => r.GetByProduct(1))
                .Returns(new List<Stock>
                {
                    new Stock { ProductId = 1, WarehouseId = 1, Quantity = 5 }
                });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_CantidadNegativaEnLinea_LanzaArgumentException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = -1, UnitPrice = 50 }
            };

            _clientRepoMock.Setup(r => r.GetById(1))
                .Returns(new Client { ClientId = 1, IsActive = true });
            _productRepoMock.Setup(r => r.GetById(1))
                .Returns(new Product { ProductId = 1, IsActive = true, Name = "Producto" });
            _stockRepoMock.Setup(r => r.GetByProduct(1))
                .Returns(new List<Stock>
                {
                    new Stock { ProductId = 1, WarehouseId = 1, Quantity = 100 }
                });

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_PrecioNegativoEnLinea_LanzaArgumentException()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1 };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 1, UnitPrice = -10 }
            };

            _clientRepoMock.Setup(r => r.GetById(1))
                .Returns(new Client { ClientId = 1, IsActive = true });
            _productRepoMock.Setup(r => r.GetById(1))
                .Returns(new Product { ProductId = 1, IsActive = true, Name = "Producto" });
            _stockRepoMock.Setup(r => r.GetByProduct(1))
                .Returns(new List<Stock>
                {
                    new Stock { ProductId = 1, WarehouseId = 1, Quantity = 100 }
                });

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _saleService.CreateSale(sale, lines, 1));
        }

        [Test]
        public void CreateSale_ConDatosValidos_RetornaIdDeVenta()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1, SaleDate = DateTime.Now };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 2, UnitPrice = 50 }
            };

            _clientRepoMock.Setup(r => r.GetById(1))
                .Returns(new Client { ClientId = 1, IsActive = true });
            _productRepoMock.Setup(r => r.GetById(1))
                .Returns(new Product { ProductId = 1, IsActive = true, Name = "Producto", SKU = "SKU-001" });
            _stockRepoMock.Setup(r => r.GetByProduct(1))
                .Returns(new List<Stock>
                {
                    new Stock { StockId = 1, ProductId = 1, WarehouseId = 1, Quantity = 100, WarehouseName = "Almacén Principal" }
                });
            _saleRepoMock.Setup(r => r.CreateWithLines(It.IsAny<Sale>(), It.IsAny<List<SaleLine>>()))
                .Returns(42);

            // Act
            int saleId = _saleService.CreateSale(sale, lines, 1);

            // Assert
            Assert.AreEqual(42, saleId);
        }

        [Test]
        public void CreateSale_ConDatosValidos_DescontaStockDelAlmacen()
        {
            // Arrange
            var sale = new Sale { SellerName = "Vendedor", ClientId = 1, SaleDate = DateTime.Now };
            var lines = new List<SaleLine>
            {
                new SaleLine { ProductId = 1, Quantity = 3, UnitPrice = 50 }
            };

            _clientRepoMock.Setup(r => r.GetById(1))
                .Returns(new Client { ClientId = 1, IsActive = true });
            _productRepoMock.Setup(r => r.GetById(1))
                .Returns(new Product { ProductId = 1, IsActive = true, Name = "Producto", SKU = "SKU-001" });
            _stockRepoMock.Setup(r => r.GetByProduct(1))
                .Returns(new List<Stock>
                {
                    new Stock { StockId = 1, ProductId = 1, WarehouseId = 1, Quantity = 10, WarehouseName = "Almacén" }
                });
            _saleRepoMock.Setup(r => r.CreateWithLines(It.IsAny<Sale>(), It.IsAny<List<SaleLine>>()))
                .Returns(1);

            // Act
            _saleService.CreateSale(sale, lines, 1);

            // Assert – stock reducido de 10 a 7
            _stockRepoMock.Verify(r => r.UpdateStock(1, 1, 7, 1), Times.Once);
        }

        // ─── GetTotalAvailableStock ───────────────────────────────────────────────

        [Test]
        public void GetTotalAvailableStock_SumaStockDeTodosLosAlmacenes()
        {
            // Arrange
            _stockRepoMock.Setup(r => r.GetByProduct(1))
                .Returns(new List<Stock>
                {
                    new Stock { ProductId = 1, WarehouseId = 1, Quantity = 30 },
                    new Stock { ProductId = 1, WarehouseId = 2, Quantity = 20 },
                    new Stock { ProductId = 1, WarehouseId = 3, Quantity = 10 }
                });

            // Act
            int total = _saleService.GetTotalAvailableStock(1);

            // Assert
            Assert.AreEqual(60, total);
        }

        [Test]
        public void GetTotalAvailableStock_SinStockEnNingunAlmacen_RetornaCero()
        {
            // Arrange
            _stockRepoMock.Setup(r => r.GetByProduct(99))
                .Returns(new List<Stock>());

            // Act
            int total = _saleService.GetTotalAvailableStock(99);

            // Assert
            Assert.AreEqual(0, total);
        }
    }
}
