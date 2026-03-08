using System;
using System.Collections.Generic;
using DAL.Contracts;
using Moq;
using NUnit.Framework;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace StockManager.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para AuthorizationService.
    /// Verifica la comprobación de permisos RBAC: individual, cualquiera y todos.
    /// </summary>
    [TestFixture]
    public class AuthorizationServiceTests
    {
        private Mock<IPermissionRepository> _permissionRepoMock;
        private Mock<ILogService> _logServiceMock;
        private AuthorizationService _authorizationService;

        [SetUp]
        public void SetUp()
        {
            _permissionRepoMock = new Mock<IPermissionRepository>();
            _logServiceMock = new Mock<ILogService>();
            _authorizationService = new AuthorizationService(
                _permissionRepoMock.Object,
                _logServiceMock.Object);
        }

        // ─── HasPermission ────────────────────────────────────────────────────────

        [Test]
        public void HasPermission_UsuarioTienePermiso_RetornaTrue()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "USERS_VIEW", "USERS_CREATE" });

            // Act
            bool result = _authorizationService.HasPermission(1, "USERS_VIEW");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasPermission_UsuarioNoTienePermiso_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "USERS_VIEW" });

            // Act
            bool result = _authorizationService.HasPermission(1, "USERS_DELETE");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasPermission_UsuarioSinPermisos_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(5))
                .Returns(new List<string>());

            // Act
            bool result = _authorizationService.HasPermission(5, "PRODUCTS_VIEW");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasPermission_RepositorioLanzaExcepcion_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(It.IsAny<int>()))
                .Throws(new Exception("Error de base de datos"));

            // Act – no debe propagar la excepción
            bool result = _authorizationService.HasPermission(1, "USERS_VIEW");

            // Assert
            Assert.IsFalse(result, "En caso de error, debe retornar false en lugar de lanzar excepción");
        }

        // ─── HasAnyPermission ─────────────────────────────────────────────────────

        [Test]
        public void HasAnyPermission_UsuarioTieneAlMenoUno_RetornaTrue()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "PRODUCTS_VIEW" });

            // Act
            bool result = _authorizationService.HasAnyPermission(1, "USERS_VIEW", "PRODUCTS_VIEW");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasAnyPermission_UsuarioNoTieneNinguno_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "PRODUCTS_VIEW" });

            // Act
            bool result = _authorizationService.HasAnyPermission(1, "USERS_VIEW", "USERS_DELETE");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasAnyPermission_UsuarioTieneTodos_RetornaTrue()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "USERS_VIEW", "USERS_DELETE", "PRODUCTS_VIEW" });

            // Act
            bool result = _authorizationService.HasAnyPermission(1, "USERS_VIEW", "USERS_DELETE");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasAnyPermission_RepositorioLanzaExcepcion_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(It.IsAny<int>()))
                .Throws(new Exception("Error de base de datos"));

            // Act
            bool result = _authorizationService.HasAnyPermission(1, "USERS_VIEW");

            // Assert
            Assert.IsFalse(result);
        }

        // ─── HasAllPermissions ────────────────────────────────────────────────────

        [Test]
        public void HasAllPermissions_UsuarioTieneTodosLosPermisos_RetornaTrue()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "USERS_VIEW", "USERS_CREATE", "USERS_DELETE" });

            // Act
            bool result = _authorizationService.HasAllPermissions(1, "USERS_VIEW", "USERS_CREATE");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasAllPermissions_UsuarioFaltaUnPermiso_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string> { "USERS_VIEW" });

            // Act
            bool result = _authorizationService.HasAllPermissions(1, "USERS_VIEW", "USERS_DELETE");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasAllPermissions_UsuarioSinNingunPermiso_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(new List<string>());

            // Act
            bool result = _authorizationService.HasAllPermissions(1, "USERS_VIEW", "USERS_CREATE");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasAllPermissions_RepositorioLanzaExcepcion_RetornaFalse()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(It.IsAny<int>()))
                .Throws(new Exception("Error de base de datos"));

            // Act
            bool result = _authorizationService.HasAllPermissions(1, "USERS_VIEW");

            // Assert
            Assert.IsFalse(result);
        }

        // ─── GetUserPermissions ───────────────────────────────────────────────────

        [Test]
        public void GetUserPermissions_RetornaListaDePermisos()
        {
            // Arrange
            var expectedPermissions = new List<string> { "USERS_VIEW", "PRODUCTS_VIEW", "STOCK_VIEW" };
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(1))
                .Returns(expectedPermissions);

            // Act
            var result = _authorizationService.GetUserPermissions(1);

            // Assert
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEquivalent(expectedPermissions, result);
        }

        [Test]
        public void GetUserPermissions_RepositorioLanzaExcepcion_RetornaListaVacia()
        {
            // Arrange
            _permissionRepoMock
                .Setup(r => r.GetUserPermissions(It.IsAny<int>()))
                .Throws(new Exception("Error de base de datos"));

            // Act
            var result = _authorizationService.GetUserPermissions(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
