using System;
using DOMAIN.Enums;

namespace SERVICES.Interfaces
{
    public interface ILogService
    {
        /// <summary>
        /// Registra un mensaje de depuración para propósitos de desarrollo
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        void Debug(string message, string logger = null);
        
        /// <summary>
        /// Registra un mensaje informativo sobre el funcionamiento normal de la aplicación
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        void Info(string message, string logger = null);
        
        /// <summary>
        /// Registra una advertencia sobre situaciones que requieren atención pero no son errores
        /// </summary>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        void Warning(string message, string logger = null);
        
        /// <summary>
        /// Registra un error que afecta el funcionamiento de la aplicación
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        void Error(string message, Exception exception = null, string logger = null);
        
        /// <summary>
        /// Registra un error crítico que puede causar la terminación de la aplicación
        /// </summary>
        /// <param name="message">Mensaje de error crítico</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        void Fatal(string message, Exception exception = null, string logger = null);
        
        /// <summary>
        /// Registra un mensaje con un nivel de severidad específico
        /// </summary>
        /// <param name="level">Nivel de severidad del log</param>
        /// <param name="message">Mensaje a registrar</param>
        /// <param name="exception">Excepción asociada (opcional)</param>
        /// <param name="logger">Nombre del logger (opcional)</param>
        void Log(LogLevel level, string message, Exception exception = null, string logger = null);
    }
}
