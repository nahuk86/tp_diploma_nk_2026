using System;
using System.Collections.Generic;
using System.Linq;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    /// <summary>
    /// Implementación del servicio de diagnóstico que verifica el funcionamiento
    /// correcto de los servicios core del sistema.
    /// </summary>
    public class TestingService : ITestingService
    {
        private readonly IAuthenticationService _authService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogService _logService;

        private const string ServiceVersion = "1.0.0";

        /// <summary>
        /// Inicializa el servicio de diagnóstico con sus dependencias
        /// </summary>
        /// <param name="authService">Servicio de autenticación a diagnosticar</param>
        /// <param name="authorizationService">Servicio de autorización a diagnosticar</param>
        /// <param name="logService">Servicio de logging</param>
        public TestingService(
            IAuthenticationService authService,
            IAuthorizationService authorizationService,
            ILogService logService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Ejecuta todos los diagnósticos disponibles y retorna un reporte consolidado.
        /// Verifica: hash de contraseñas, verificación de contraseñas y disponibilidad de servicios.
        /// </summary>
        /// <returns>Reporte con el resultado de cada diagnóstico</returns>
        public DiagnosticReport RunDiagnostics()
        {
            var report = new DiagnosticReport
            {
                RunAt = DateTime.Now,
                Results = new List<DiagnosticResult>()
            };

            report.Results.Add(TestPasswordHashing());
            report.Results.Add(TestPasswordVerification());
            report.Results.Add(TestAuthorizationServiceAvailability());
            report.Results.Add(TestLogService());

            report.AllPassed = report.Results.All(r => r.Passed);

            _logService.Info(
                $"Diagnósticos ejecutados: {report.Results.Count} | " +
                $"Estado: {(report.AllPassed ? "TODOS PASARON" : "ALGUNOS FALLARON")}");

            return report;
        }

        /// <summary>
        /// Retorna la versión actual del servicio de pruebas
        /// </summary>
        public string GetServiceVersion()
        {
            return ServiceVersion;
        }

        /// <summary>
        /// Verifica que el servicio de autenticación pueda generar hashes de contraseñas
        /// </summary>
        private DiagnosticResult TestPasswordHashing()
        {
            const string testName = "AuthenticationService.HashPassword";
            try
            {
                string salt;
                string hash = _authService.HashPassword("DiagnosticTest@1", out salt);

                bool passed = !string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(salt);
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = passed,
                    Message = passed
                        ? "Generación de hash de contraseña funcionando correctamente"
                        : "El hash o salt generado es vacío"
                };
            }
            catch (Exception ex)
            {
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = false,
                    Message = $"Excepción durante el diagnóstico: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Verifica que el servicio de autenticación pueda validar contraseñas correctamente:
        /// contraseña correcta devuelve true y contraseña incorrecta devuelve false.
        /// </summary>
        private DiagnosticResult TestPasswordVerification()
        {
            const string testName = "AuthenticationService.VerifyPassword";
            try
            {
                string salt;
                string hash = _authService.HashPassword("DiagnosticTest@1", out salt);

                bool correctPasswordVerified = _authService.VerifyPassword("DiagnosticTest@1", hash, salt);
                bool wrongPasswordRejected = !_authService.VerifyPassword("WrongPassword", hash, salt);

                bool passed = correctPasswordVerified && wrongPasswordRejected;
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = passed,
                    Message = passed
                        ? "Verificación de contraseña funcionando correctamente"
                        : "La verificación de contraseña no se comporta como se espera"
                };
            }
            catch (Exception ex)
            {
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = false,
                    Message = $"Excepción durante el diagnóstico: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Verifica que el servicio de autorización esté disponible e inyectado correctamente
        /// </summary>
        private DiagnosticResult TestAuthorizationServiceAvailability()
        {
            const string testName = "AuthorizationService.Disponibilidad";
            try
            {
                bool passed = _authorizationService != null;
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = passed,
                    Message = passed
                        ? "Servicio de autorización disponible e inyectado correctamente"
                        : "El servicio de autorización es null"
                };
            }
            catch (Exception ex)
            {
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = false,
                    Message = $"Excepción durante el diagnóstico: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Verifica que el servicio de logging esté disponible y funcional
        /// </summary>
        private DiagnosticResult TestLogService()
        {
            const string testName = "LogService.Disponibilidad";
            try
            {
                _logService.Info("Diagnóstico: LogService funcionando correctamente");
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = true,
                    Message = "Servicio de logging disponible y funcional"
                };
            }
            catch (Exception ex)
            {
                return new DiagnosticResult
                {
                    ServiceName = testName,
                    Passed = false,
                    Message = $"Excepción durante el diagnóstico: {ex.Message}"
                };
            }
        }
    }
}
