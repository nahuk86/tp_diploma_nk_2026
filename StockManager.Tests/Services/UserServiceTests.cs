using System;
using System.Collections.Generic;
using BLL.Services;
using DAL.Contracts;
using DOMAIN.Entities;
using Moq;
using NUnit.Framework;
using SERVICES;
using SERVICES.Interfaces;

namespace StockManager.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para UserService.
    /// Verifica validaciones de usuario, password y reglas de negocio (ej: no eliminar admin).
    /// </summary>
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IAuditLogRepository> _auditRepoMock;
        private Mock<ILogService> _logServiceMock;
        private Mock<IAuthenticationService> _authServiceMock;
        private Mock<IErrorHandlerService> _errorHandlerMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _auditRepoMock = new Mock<IAuditLogRepository>();
            _logServiceMock = new Mock<ILogService>();
            _authServiceMock = new Mock<IAuthenticationService>();
            _errorHandlerMock = new Mock<IErrorHandlerService>();

            _userService = new UserService(
                _userRepoMock.Object,
                _auditRepoMock.Object,
                _logServiceMock.Object,
                _authServiceMock.Object,
                _errorHandlerMock.Object);
        }

        // ─── CreateUser – Validaciones de datos de usuario ────────────────────────

        [Test]
        public void CreateUser_ConUsuarioNull_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _userService.CreateUser(null, "Password@1"));
        }

        [Test]
        public void CreateUser_ConUsernameVacio_LanzaArgumentException()
        {
            var user = new User { Username = "" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "Password@1"));
        }

        [Test]
        public void CreateUser_ConUsernameDeMenosDeTresCaracteres_LanzaArgumentException()
        {
            var user = new User { Username = "ab" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "Password@1"));
        }

        [Test]
        public void CreateUser_ConUsernameMayorA50Caracteres_LanzaArgumentException()
        {
            var user = new User { Username = new string('a', 51) };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "Password@1"));
        }

        [Test]
        public void CreateUser_ConEmailInvalido_LanzaArgumentException()
        {
            var user = new User { Username = "usuario", Email = "correo-invalido" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "Password@1"));
        }

        [Test]
        public void CreateUser_ConEmailValido_NuncaLanzaExcepcionPorEmail()
        {
            // Arrange
            var user = new User { Username = "usuario", Email = "correo@valido.com" };
            _userRepoMock.Setup(r => r.GetByUsername("usuario")).Returns((User)null);
            _userRepoMock.Setup(r => r.GetByEmail("correo@valido.com")).Returns((User)null);
            _authServiceMock.Setup(a => a.HashPassword(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns((string p, out string s) => { s = "salt"; return "hash"; });
            _userRepoMock.Setup(r => r.Insert(It.IsAny<User>())).Returns(1);

            // Act & Assert – no debe lanzar excepción por formato de email
            Assert.DoesNotThrow(() => _userService.CreateUser(user, "Password@1"));
        }

        // ─── CreateUser – Validaciones de password ────────────────────────────────

        [Test]
        public void CreateUser_ConPasswordVacio_LanzaArgumentException()
        {
            var user = new User { Username = "usuario" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, ""));
        }

        [Test]
        public void CreateUser_ConPasswordMenorA8Caracteres_LanzaArgumentException()
        {
            var user = new User { Username = "usuario" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "Pass@1"));
        }

        [Test]
        public void CreateUser_ConPasswordSinMayuscula_LanzaArgumentException()
        {
            var user = new User { Username = "usuario" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "password@123"));
        }

        [Test]
        public void CreateUser_ConPasswordSinNumero_LanzaArgumentException()
        {
            var user = new User { Username = "usuario" };
            Assert.Throws<ArgumentException>(() => _userService.CreateUser(user, "PasswordSinNumero@"));
        }

        // ─── CreateUser – Reglas de negocio ───────────────────────────────────────

        [Test]
        public void CreateUser_ConUsernameDuplicado_LanzaInvalidOperationException()
        {
            // Arrange
            var user = new User { Username = "existente" };
            _userRepoMock.Setup(r => r.GetByUsername("existente"))
                .Returns(new User { UserId = 5, Username = "existente" });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.CreateUser(user, "Password@1"));
        }

        [Test]
        public void CreateUser_ConEmailDuplicado_LanzaInvalidOperationException()
        {
            // Arrange
            var user = new User { Username = "nuevo", Email = "duplicado@test.com" };
            _userRepoMock.Setup(r => r.GetByUsername("nuevo")).Returns((User)null);
            _userRepoMock.Setup(r => r.GetByEmail("duplicado@test.com"))
                .Returns(new User { UserId = 10, Email = "duplicado@test.com" });

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.CreateUser(user, "Password@1"));
        }

        [Test]
        public void CreateUser_ConDatosValidos_RetornaIdDelUsuario()
        {
            // Arrange
            var user = new User { Username = "nuevo", Email = "nuevo@test.com" };
            _userRepoMock.Setup(r => r.GetByUsername("nuevo")).Returns((User)null);
            _userRepoMock.Setup(r => r.GetByEmail("nuevo@test.com")).Returns((User)null);
            _authServiceMock.Setup(a => a.HashPassword(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns((string p, out string s) => { s = "salt"; return "hash"; });
            _userRepoMock.Setup(r => r.Insert(It.IsAny<User>())).Returns(77);

            // Act
            int id = _userService.CreateUser(user, "Password@1");

            // Assert
            Assert.AreEqual(77, id);
        }

        // ─── DeleteUser ───────────────────────────────────────────────────────────

        [Test]
        public void DeleteUser_UsuarioInexistente_LanzaInvalidOperationException()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetById(999)).Returns((User)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.DeleteUser(999));
        }

        [Test]
        public void DeleteUser_UsuarioAdmin_LanzaInvalidOperationException()
        {
            // Arrange – el usuario 'admin' no debe poder eliminarse
            var adminUser = new User { UserId = 1, Username = "admin", IsActive = true };
            _userRepoMock.Setup(r => r.GetById(1)).Returns(adminUser);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.DeleteUser(1));
        }

        [Test]
        public void DeleteUser_UsuarioAdminConDistintoCasing_LanzaInvalidOperationException()
        {
            // Arrange – la protección debe ser case-insensitive
            var adminUser = new User { UserId = 1, Username = "ADMIN", IsActive = true };
            _userRepoMock.Setup(r => r.GetById(1)).Returns(adminUser);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.DeleteUser(1));
        }

        [Test]
        public void DeleteUser_UsuarioNoAdmin_LlamaSoftDelete()
        {
            // Arrange
            var user = new User { UserId = 5, Username = "operador", IsActive = true };
            _userRepoMock.Setup(r => r.GetById(5)).Returns(user);

            // Simular sesión activa
            SessionContext.Instance.CurrentUser = new User { UserId = 1, Username = "admin" };

            // Act
            _userService.DeleteUser(5);

            // Assert
            _userRepoMock.Verify(r => r.SoftDelete(5, It.IsAny<int>()), Times.Once);

            // Cleanup
            SessionContext.Instance.Clear();
        }

        // ─── GetAllUsers ──────────────────────────────────────────────────────────

        [Test]
        public void GetAllUsers_RetornaListaDeUsuarios()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, Username = "admin" },
                new User { UserId = 2, Username = "operador" }
            };
            _userRepoMock.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        // ─── ChangePassword ───────────────────────────────────────────────────────

        [Test]
        public void ChangePassword_PasswordNuevoInvalido_LanzaArgumentException()
        {
            // Act & Assert – contraseña sin mayúsculas
            Assert.Throws<ArgumentException>(() => _userService.ChangePassword(1, "sinmayuscula1"));
        }

        [Test]
        public void ChangePassword_UsuarioInexistente_LanzaInvalidOperationException()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetById(999)).Returns((User)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userService.ChangePassword(999, "NuevaClave@1"));
        }
    }
}
