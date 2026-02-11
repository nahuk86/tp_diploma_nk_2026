using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    public class LocalizationService : ILocalizationService
    {
        private readonly string _connectionString;
        private Dictionary<string, string> _translations;
        private string _currentLanguage;

        public LocalizationService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockManagerDB"]?.ConnectionString;
            _currentLanguage = ConfigurationManager.AppSettings["DefaultLanguage"] ?? "es";
            _translations = new Dictionary<string, string>();
            LoadTranslations();
        }

        public string CurrentLanguage
        {
            get { return _currentLanguage; }
        }

        public List<string> AvailableLanguages
        {
            get { return new List<string> { "es", "en" }; }
        }

        public string GetString(string key)
        {
            return GetString(key, _currentLanguage);
        }

        public string GetString(string key, string language)
        {
            var lookupKey = $"{key}|{language}";
            
            if (_translations.ContainsKey(lookupKey))
            {
                return _translations[lookupKey];
            }

            // Fallback to Spanish if translation not found
            var fallbackKey = $"{key}|es";
            if (language != "es" && _translations.ContainsKey(fallbackKey))
            {
                return _translations[fallbackKey];
            }

            return key; // Return the key itself if no translation found
        }

        public void SetLanguage(string language)
        {
            if (AvailableLanguages.Contains(language))
            {
                _currentLanguage = language;
            }
        }

        private void LoadTranslations()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    // Use default translations if database not configured
                    LoadDefaultTranslations();
                    return;
                }

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT ResourceKey, Language, ResourceValue FROM Translations", connection);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var key = $"{reader["ResourceKey"]}|{reader["Language"]}";
                            _translations[key] = reader["ResourceValue"].ToString();
                        }
                    }
                }
            }
            catch
            {
                // If database access fails, use default translations
                LoadDefaultTranslations();
            }
        }

        private void LoadDefaultTranslations()
        {
            // Common translations
            _translations["Common.Login|es"] = "Iniciar Sesión";
            _translations["Common.Login|en"] = "Login";
            _translations["Common.Username|es"] = "Usuario";
            _translations["Common.Username|en"] = "Username";
            _translations["Common.Password|es"] = "Contraseña";
            _translations["Common.Password|en"] = "Password";
            _translations["Common.Save|es"] = "Guardar";
            _translations["Common.Save|en"] = "Save";
            _translations["Common.Cancel|es"] = "Cancelar";
            _translations["Common.Cancel|en"] = "Cancel";
            _translations["Common.Delete|es"] = "Eliminar";
            _translations["Common.Delete|en"] = "Delete";
            _translations["Common.Edit|es"] = "Editar";
            _translations["Common.Edit|en"] = "Edit";
            _translations["Common.New|es"] = "Nuevo";
            _translations["Common.New|en"] = "New";
            _translations["Common.Search|es"] = "Buscar";
            _translations["Common.Search|en"] = "Search";
            _translations["Common.Close|es"] = "Cerrar";
            _translations["Common.Close|en"] = "Close";
            _translations["Common.Yes|es"] = "Sí";
            _translations["Common.Yes|en"] = "Yes";
            _translations["Common.No|es"] = "No";
            _translations["Common.No|en"] = "No";
            
            // Menu translations
            _translations["Menu.Users|es"] = "Usuarios";
            _translations["Menu.Users|en"] = "Users";
            _translations["Menu.Roles|es"] = "Roles";
            _translations["Menu.Roles|en"] = "Roles";
            _translations["Menu.Products|es"] = "Productos";
            _translations["Menu.Products|en"] = "Products";
            _translations["Menu.Warehouses|es"] = "Almacenes";
            _translations["Menu.Warehouses|en"] = "Warehouses";
            _translations["Menu.Stock|es"] = "Inventario";
            _translations["Menu.Stock|en"] = "Stock";
            _translations["Menu.StockMovements|es"] = "Movimientos de Stock";
            _translations["Menu.StockMovements|en"] = "Stock Movements";
        }
    }
}
