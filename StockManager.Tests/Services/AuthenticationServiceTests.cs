using System;
using DAL.Contracts;
using DOMAIN.Entities;
using Moq;
using NUnit.Framework;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace StockManager.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para AuthenticationService.
    /// Verifica las funcionalidades de hash de contraseñas, verificación y autenticación.
    /// </summary>
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ILogService> _logServiceMock;
        private AuthenticationService _authService;

        [SetUp]
        public void SetUp()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _logServiceMock = new Mock<ILogService>();
            _authService = new AuthenticationService(_userRepoMock.Object, _logServiceMock.Object);
        }

        // ─── HashPassword ─────────────────────────────────────────────────────────

        [Test]
        public void HashPassword_ConContrasenaValida_RetornaHashYSaltNoVacios()
        {
            // Act
            string salt;
            string hash = _authService.HashPassword("MiClave@123", out salt);

            // Assert
            Assert.IsNotNull(hash, "El hash no debe ser null");
            Assert.IsNotEmpty(hash, "El hash no debe estar vacío");
            Assert.IsNotNull(salt, "El salt no debe ser null");
            Assert.IsNotEmpty(salt, "El salt no debe estar vacío");
        }

        [Test]
        public void HashPassword_MismaContrasena_ProduceDiferentesHashesConDiferentesSalts()
        {
            // Act
            string salt1, salt2;
            string hash1 = _authService.HashPassword("MismaContrasena@1", out salt1);
            string hash2 = _authService.HashPassword("MismaContrasena@1", out salt2);

            // Assert
            Assert.AreNotEqual(salt1, salt2, "Dos llamadas deben producir salts distintos");
            Assert.AreNotEqual(hash1, hash2, "Dos llamadas con el mismo password deben dar hashes distintos por el salt distinto");
        }

        [Test]
        public void HashPassword_RetornaStringBase64Valido()
        {
            // Act
            string salt;
            string hash = _authService.HashPassword("Test@1234", out salt);

            // Assert – debe poder decodificarse como Base64 sin excepción
            Assert.DoesNotThrow(() => Convert.FromBase64String(hash), "El hash debe ser Base64 válido");
            Assert.DoesNotThrow(() => Convert.FromBase64String(salt), "El salt debe ser Base64 válido");
        }

        // ─── VerifyPassword ───────────────────────────────────────────────────────

        [Test]
        public void VerifyPassword_ConContrasenaCorrecta_RetornaTrue()
        {
            // Arrange
            string salt;
            string hash = _authService.HashPassword("ClaveCorrecta@1", out salt);

            // Act
            bool result = _authService.VerifyPassword("ClaveCorrecta@1", hash, salt);

            // Assert
            Assert.IsTrue(result, "La contraseña correcta debe verificarse como válida");
        }

        [Test]
        public void VerifyPassword_ConContrasenaIncorrecta_RetornaFalse()
        {
            // Arrange
            string salt;
            string hash = _authService.HashPassword("ClaveCorrecta@1", out salt);

            // Act
            bool result = _authService.VerifyPassword("ClaveIncorrecta@1", hash, salt);

            // Assert
            Assert.IsFalse(result, "Una contraseña incorrecta no debe verificarse como válida");
        }

        [Test]
        public void VerifyPassword_ConHashCorrupto_RetornaFalse()
        {
            // Act
            bool result = _authService.VerifyPassword("Cualquier@1", "HashCorrupto!!!", "SaltInvalido!!!");

            // Assert
            Assert.IsFalse(result, "Un hash/salt inválido debe retornar false sin lanzar excepción");
        }

        [Test]
        public void VerifyPassword_ConContrasenaDiferenteAlHash_RetornaFalse()
        {
            // Arrange
            string salt;
            string hash = _authService.HashPassword("Contrasena@1", out salt);

            // Act – intentar con otra contraseña usando el mismo salt
            bool result = _authService.VerifyPassword("Contrasena@2", hash, salt);

            // Assert
            Assert.IsFalse(result);
        }

        // ─── Authenticate ─────────────────────────────────────────────────────────

        [Test]
        public void Authenticate_ConUsernameVacio_RetornaNull()
        {
            // Act
            var result = _authService.Authenticate("", "Clave@123");

            // Assert
            Assert.IsNull(result, "Username vacío debe retornar null");
        }

        [Test]
        public void Authenticate_ConPasswordVacio_RetornaNull()
        {
            // Act
            var result = _authService.Authenticate("usuario", "");

            // Assert
            Assert.IsNull(result, "Password vacío debe retornar null");
        }

        [Test]
        public void Authenticate_ConUsernameYPasswordNulos_RetornaNull()
        {
            // Act
            var result = _authService.Authenticate(null, null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Authenticate_ConUsuarioInexistente_RetornaNull()
        {
            // Arrange
            _userRepoMock.Setup(r => r.GetByUsername("noexiste")).Returns((User)null);

            // Act
            var result = _authService.Authenticate("noexiste", "Clave@123");

            // Assert
            Assert.IsNull(result, "Usuario inexistente debe retornar null");
        }

        [Test]
        public void Authenticate_ConUsuarioInactivo_RetornaNull()
        {
            // Arrange
            var inactiveUser = new User
            {
                UserId = 1,
                Username = "usuarioinactivo",
                IsActive = false,
                PasswordHash = "hash",
                PasswordSalt = "salt"
            };
            _userRepoMock.Setup(r => r.GetByUsername("usuarioinactivo")).Returns(inactiveUser);

            // Act
            var result = _authService.Authenticate("usuarioinactivo", "Clave@123");

            // Assert
            Assert.IsNull(result, "Usuario inactivo debe retornar null");
        }

        [Test]
        public void Authenticate_ConPasswordPlaceholder_RetornaNull()
        {
            // Arrange
            var userWithPlaceholder = new User
            {
                UserId = 1,
                Username = "admin",
                IsActive = true,
                PasswordHash = "HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP",
                PasswordSalt = "salt"
            };
            _userRepoMock.Setup(r => r.GetByUsername("admin")).Returns(userWithPlaceholder);

            // Act
            var result = _authService.Authenticate("admin", "Clave@123");

            // Assert
            Assert.IsNull(result, "Contraseña placeholder no inicializada debe retornar null");
        }

        [Test]
        public void Authenticate_ConPasswordIncorrecta_RetornaNull()
        {
            // Arrange
            string salt;
            string hash = _authService.HashPassword("ClaveReal@123", out salt);

            var user = new User
            {
                UserId = 1,
                Username = "usuario",
                IsActive = true,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            _userRepoMock.Setup(r => r.GetByUsername("usuario")).Returns(user);

            // Act
            var result = _authService.Authenticate("usuario", "ClaveIncorrecta@123");

            // Assert
            Assert.IsNull(result, "Contraseña incorrecta debe retornar null");
        }

        [Test]
        public void Authenticate_ConCredencialesValidas_RetornaUsuario()
        {
            // Arrange
            string salt;
            string hash = _authService.HashPassword("ClaveReal@123", out salt);

            var user = new User
            {
                UserId = 1,
                Username = "usuario",
                IsActive = true,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            _userRepoMock.Setup(r => r.GetByUsername("usuario")).Returns(user);
            _userRepoMock.Setup(r => r.UpdateLastLogin(1));

            // Act
            var result = _authService.Authenticate("usuario", "ClaveReal@123");

            // Assert
            Assert.IsNotNull(result, "Credenciales válidas deben retornar el usuario");
            Assert.AreEqual("usuario", result.Username);
        }

        [Test]
        public void Authenticate_ConCredencialesValidas_ActualizaUltimoLogin()
        {
            // Arrange
            string salt;
            string hash = _authService.HashPassword("ClaveReal@123", out salt);

            var user = new User
            {
                UserId = 42,
                Username = "usuario",
                IsActive = true,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            _userRepoMock.Setup(r => r.GetByUsername("usuario")).Returns(user);

            // Act
            _authService.Authenticate("usuario", "ClaveReal@123");

            // Assert – verifica que se llamó a UpdateLastLogin
            _userRepoMock.Verify(r => r.UpdateLastLogin(42), Times.Once);
        }
    }
}
