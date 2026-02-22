using System;
using System.Collections.Generic;

namespace SERVICES.Interfaces
{
    public interface ILocalizationService
    {
        /// <summary>
        /// Obtiene una cadena traducida usando el idioma actual
        /// </summary>
        /// <param name="key">Clave de la cadena a traducir</param>
        /// <returns>Cadena traducida o la clave si no se encuentra traducción</returns>
        string GetString(string key);
        
        /// <summary>
        /// Obtiene una cadena traducida en el idioma especificado
        /// </summary>
        /// <param name="key">Clave de la cadena a traducir</param>
        /// <param name="language">Código del idioma (ej: 'es', 'en')</param>
        /// <returns>Cadena traducida o la clave si no se encuentra traducción</returns>
        string GetString(string key, string language);
        
        /// <summary>
        /// Establece el idioma actual de la aplicación
        /// </summary>
        /// <param name="language">Código del idioma a establecer</param>
        void SetLanguage(string language);
        
        /// <summary>
        /// Obtiene el código del idioma actual
        /// </summary>
        string CurrentLanguage { get; }
        
        /// <summary>
        /// Obtiene la lista de idiomas disponibles en la aplicación
        /// </summary>
        List<string> AvailableLanguages { get; }
        
        /// <summary>
        /// Evento que se dispara cuando cambia el idioma de la aplicación
        /// </summary>
        event EventHandler LanguageChanged;
    }
}
