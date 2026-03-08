using System;
using Moq;
using NUnit.Framework;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace StockManager.Tests.Services
{
    /// <summary>
    /// Pruebas unitarias para TestingService.
    /// Verifica el comportamiento del servicio de diagnóstico y sus resultados.
    /// </summary>
    [TestFixture]
    public class TestingServiceTests
    {
        private Mock<IAuthenticationService> _authServiceMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Mock<ILogService> _logServiceMock;
        private TestingService _testingService;

        [SetUp]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthenticationService>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _logServiceMock = new Mock<ILogService>();

            _testingService = new TestingService(
                _authServiceMock.Object,
                _authorizationServiceMock.Object,
                _logServiceMock.Object);
        }

        // ─── Constructor ──────────────────────────────────────────────────────────

        [Test]
        public void Constructor_ConAuthServiceNull_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TestingService(null, _authorizationServiceMock.Object, _logServiceMock.Object));
        }

        [Test]
        public void Constructor_ConAuthorizationServiceNull_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TestingService(_authServiceMock.Object, null, _logServiceMock.Object));
        }

        [Test]
        public void Constructor_ConLogServiceNull_LanzaArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TestingService(_authServiceMock.Object, _authorizationServiceMock.Object, null));
        }

        // ─── GetServiceVersion ────────────────────────────────────────────────────

        [Test]
        public void GetServiceVersion_RetornaVersionNoVacia()
        {
            // Act
            string version = _testingService.GetServiceVersion();

            // Assert
            Assert.IsNotNull(version);
            Assert.IsNotEmpty(version);
        }

        [Test]
        public void GetServiceVersion_RetornaFormatoDeVersionValido()
        {
            // Act
            string version = _testingService.GetServiceVersion();

            // Assert – el formato debe ser N.N.N
            var parts = version.Split('.');
            Assert.AreEqual(3, parts.Length, "La versión debe tener formato major.minor.patch");
        }

        // ─── RunDiagnostics ───────────────────────────────────────────────────────

        [Test]
        public void RunDiagnostics_RetornaReporteNoNull()
        {
            // Arrange
            ConfigurarHashPasswordMock();

            // Act
            var report = _testingService.RunDiagnostics();

            // Assert
            Assert.IsNotNull(report);
        }

        [Test]
        public void RunDiagnostics_RetornaFechaDeEjecucionReciente()
        {
            // Arrange
            ConfigurarHashPasswordMock();
            var antes = DateTime.Now.AddSeconds(-1);

            // Act
            var report = _testingService.RunDiagnostics();
            var despues = DateTime.Now.AddSeconds(1);

            // Assert
            Assert.That(report.RunAt, Is.InRange(antes, despues));
        }

        [Test]
        public void RunDiagnostics_ConServiciosFuncionando_TodosPasan()
        {
            // Arrange – todos los servicios se comportan correctamente
            ConfigurarHashPasswordMock();

            // Act
            var report = _testingService.RunDiagnostics();

            // Assert
            Assert.IsTrue(report.AllPassed, "Todos los diagnósticos deben pasar");
            foreach (var result in report.Results)
            {
                Assert.IsTrue(result.Passed, $"El diagnóstico '{result.ServiceName}' debe pasar: {result.Message}");
            }
        }

        [Test]
        public void RunDiagnostics_RetornaCuatroResultados()
        {
            // Arrange
            ConfigurarHashPasswordMock();

            // Act
            var report = _testingService.RunDiagnostics();

            // Assert – se esperan 4 diagnósticos: hash, verify, autorización, log
            Assert.AreEqual(4, report.Results.Count);
        }

        [Test]
        public void RunDiagnostics_TodosLosResultadosTienenNombreNoVacio()
        {
            // Arrange
            ConfigurarHashPasswordMock();

            // Act
            var report = _testingService.RunDiagnostics();

            // Assert
            foreach (var result in report.Results)
            {
                Assert.IsNotEmpty(result.ServiceName,
                    "Cada resultado debe tener un nombre de servicio descriptivo");
                Assert.IsNotEmpty(result.Message,
                    "Cada resultado debe incluir un mensaje informativo");
            }
        }

        [Test]
        public void RunDiagnostics_CuandoHashPasswordFalla_DiagnosticoFalla()
        {
            // Arrange – HashPassword lanza excepción
            _authServiceMock
                .Setup(a => a.HashPassword(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Throws(new Exception("Error simulado en HashPassword"));

            // Act
            var report = _testingService.RunDiagnostics();

            // Assert
            Assert.IsFalse(report.AllPassed, "Si HashPassword falla, el reporte no debe ser exitoso");

            var hashResult = report.Results.Find(r => r.ServiceName.Contains("HashPassword"));
            Assert.IsNotNull(hashResult, "Debe existir un resultado para el diagnóstico de HashPassword");
            Assert.IsFalse(hashResult.Passed);
        }

        [Test]
        public void RunDiagnostics_RegistraLogDeResultado()
        {
            // Arrange
            ConfigurarHashPasswordMock();

            // Act
            _testingService.RunDiagnostics();

            // Assert – debe haberse registrado un log del resultado
            _logServiceMock.Verify(
                l => l.Info(It.IsAny<string>()),
                Times.AtLeastOnce,
                "El servicio de diagnóstico debe registrar el resultado en el log");
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Configura el mock de autenticación para que HashPassword y VerifyPassword
        /// funcionen de forma coherente durante las pruebas de RunDiagnostics.
        /// </summary>
        private void ConfigurarHashPasswordMock()
        {
            _authServiceMock
                .Setup(a => a.HashPassword(It.IsAny<string>(), out It.Ref<string>.IsAny))
                .Returns((string password, out string salt) =>
                {
                    salt = Convert.ToBase64String(new byte[32]);
                    return Convert.ToBase64String(new byte[32]);
                });

            _authServiceMock
                .Setup(a => a.VerifyPassword(
                    It.Is<string>(p => p == "DiagnosticTest@1"),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(true);

            _authServiceMock
                .Setup(a => a.VerifyPassword(
                    It.Is<string>(p => p != "DiagnosticTest@1"),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(false);
        }
    }
}
