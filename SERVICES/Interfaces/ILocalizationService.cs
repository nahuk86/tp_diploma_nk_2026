using System.Collections.Generic;

namespace SERVICES.Interfaces
{
    public interface ILocalizationService
    {
        string GetString(string key);
        string GetString(string key, string language);
        void SetLanguage(string language);
        string CurrentLanguage { get; }
        List<string> AvailableLanguages { get; }
    }
}
