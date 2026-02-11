using System;
using DOMAIN.Enums;

namespace SERVICES.Interfaces
{
    public interface ILogService
    {
        void Debug(string message, string logger = null);
        void Info(string message, string logger = null);
        void Warning(string message, string logger = null);
        void Error(string message, Exception exception = null, string logger = null);
        void Fatal(string message, Exception exception = null, string logger = null);
        void Log(LogLevel level, string message, Exception exception = null, string logger = null);
    }
}
