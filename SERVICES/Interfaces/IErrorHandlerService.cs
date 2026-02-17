using System;

namespace SERVICES.Interfaces
{
    public interface IErrorHandlerService
    {
        /// <summary>
        /// Procesa y registra una excepción con contexto opcional
        /// </summary>
        /// <param name="ex">Excepción a procesar</param>
        /// <param name="context">Información contextual sobre dónde ocurrió el error (opcional)</param>
        void HandleError(Exception ex, string context = null);
        
        /// <summary>
        /// Convierte una excepción técnica en un mensaje amigable para el usuario
        /// </summary>
        /// <param name="ex">Excepción a traducir</param>
        /// <returns>Mensaje de error comprensible para el usuario</returns>
        string GetFriendlyMessage(Exception ex);
        
        /// <summary>
        /// Procesa una excepción y muestra un mensaje de error al usuario
        /// </summary>
        /// <param name="ex">Excepción a mostrar</param>
        /// <param name="context">Información contextual sobre dónde ocurrió el error (opcional)</param>
        void ShowError(Exception ex, string context = null);
    }
}
