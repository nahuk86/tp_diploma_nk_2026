using System;
using System.Configuration;
using System.IO;
using DOMAIN.Enums;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    public class FileLogService : ILogService
    {
        private static readonly object _lockObject = new object();
        private readonly string _logDirectory;
        private readonly string _logFilePrefix;

        public FileLogService()
        {
            _logDirectory = ConfigurationManager.AppSettings["LogDirectory"] ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            _logFilePrefix = ConfigurationManager.AppSettings["LogFilePrefix"] ?? "StockManager";
            
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        /// <summary>
        /// Registra un mensaje de depuración para propósitos de desarrollo
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        public void Debug(string message, string logger = null)
        {
            Log(LogLevel.Debug, message, null, logger);
        }

        /// <summary>
        /// Registra un mensaje informativo sobre el funcionamiento normal de la aplicación
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        public void Info(string message, string logger = null)
        {
            Log(LogLevel.Info, message, null, logger);
        }

        /// <summary>
        /// Registra una advertencia sobre situaciones que requieren atención pero no son errores
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        public void Warning(string message, string logger = null)
        {
            Log(LogLevel.Warning, message, null, logger);
        }

        /// <summary>
        /// Registra un error que afecta el funcionamiento de la aplicación
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        public void Error(string message, Exception exception = null, string logger = null)
        {
            Log(LogLevel.Error, message, exception, logger);
        }

        /// <summary>
        /// Registra un error crítico que puede causar la terminación de la aplicación
        /// </summary>
        /// <param name="message">Mensaje de error crítico</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        public void Fatal(string message, Exception exception = null, string logger = null)
        {
            Log(LogLevel.Fatal, message, exception, logger);
        }

        /// <summary>
        /// Registra un mensaje con un nivel de severidad específico
        /// </summary>
        /// <param name="level">Nivel de severidad del log</param>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        public void Log(LogLevel level, string message, Exception exception = null, string logger = null)
        {
            try
            {
                var logEntry = FormatLogEntry(level, message, exception, logger);
                var logFilePath = GetLogFilePath();

                lock (_lockObject)
                {
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch
            {
                // Fail silently to avoid crashing the application due to logging errors
            }
        }

        /// <summary>
        /// Formatea una entrada de log con marca de tiempo, nivel, contexto y detalles de excepción
        /// </summary>
        /// <param name="level">Nivel de severidad del log</param>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        /// <returns>Cadena formateada para escribir en el archivo de log</returns>
        private string FormatLogEntry(LogLevel level, string message, Exception exception, string logger)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var loggerName = logger ?? "Application";
            var username = SessionContext.CurrentUser?.Username ?? "SYSTEM";
            var machineName = Environment.MachineName;

            var entry = $"[{timestamp}] [{level.ToString().ToUpper()}] [{loggerName}] [{username}@{machineName}] {message}";

            if (exception != null)
            {
                entry += Environment.NewLine + $"Exception: {exception.GetType().Name}: {exception.Message}";
                entry += Environment.NewLine + $"StackTrace: {exception.StackTrace}";
                
                if (exception.InnerException != null)
                {
                    entry += Environment.NewLine + $"InnerException: {exception.InnerException.GetType().Name}: {exception.InnerException.Message}";
                }
            }

            return entry;
        }

        /// <summary>
        /// Obtiene la ruta del archivo de log basado en la fecha actual
        /// </summary>
        /// <returns>Ruta completa del archivo de log</returns>
        private string GetLogFilePath()
        {
            var fileName = $"{_logFilePrefix}_{DateTime.Now:yyyyMMdd}.log";
            return Path.Combine(_logDirectory, fileName);
        }

        /// <summary>
        /// Elimina archivos de log antiguos según el período de retención especificado
        /// </summary>
        /// <param name="daysToKeep">Número de días a mantener los archivos de log (por defecto 30)</param>
        public void CleanOldLogs(int daysToKeep = 30)
        {
            try
            {
                var files = Directory.GetFiles(_logDirectory, $"{_logFilePrefix}_*.log");
                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch
            {
                // Fail silently
            }
        }
    }
}
