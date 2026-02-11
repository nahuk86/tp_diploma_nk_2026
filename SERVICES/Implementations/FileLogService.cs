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

        public void Debug(string message, string logger = null)
        {
            Log(LogLevel.Debug, message, null, logger);
        }

        public void Info(string message, string logger = null)
        {
            Log(LogLevel.Info, message, null, logger);
        }

        public void Warning(string message, string logger = null)
        {
            Log(LogLevel.Warning, message, null, logger);
        }

        public void Error(string message, Exception exception = null, string logger = null)
        {
            Log(LogLevel.Error, message, exception, logger);
        }

        public void Fatal(string message, Exception exception = null, string logger = null)
        {
            Log(LogLevel.Fatal, message, exception, logger);
        }

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

        private string GetLogFilePath()
        {
            var fileName = $"{_logFilePrefix}_{DateTime.Now:yyyyMMdd}.log";
            return Path.Combine(_logDirectory, fileName);
        }

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
