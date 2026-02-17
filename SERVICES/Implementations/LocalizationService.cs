using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using SERVICES.Interfaces;

namespace SERVICES.Implementations
{
    public class LocalizationService : ILocalizationService
    {
        private static readonly Lazy<LocalizationService> _instance = 
            new Lazy<LocalizationService>(() => new LocalizationService());

        private readonly string _connectionString;
        private Dictionary<string, Dictionary<string, string>> _languageTranslations;
        private string _currentLanguage;
        private string _translationsPath;

        public event EventHandler LanguageChanged;

        /// <summary>
        /// Obtiene la instancia única del servicio de localización (patrón Singleton)
        /// </summary>
        public static LocalizationService Instance => _instance.Value;

        /// <summary>
        /// Constructor privado para implementar el patrón Singleton e inicializar el servicio de localización
        /// </summary>
        private LocalizationService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["StockManagerDB"]?.ConnectionString;
            _currentLanguage = ConfigurationManager.AppSettings["DefaultLanguage"] ?? "es";
            _languageTranslations = new Dictionary<string, Dictionary<string, string>>();
            
            // Determine translations path (UI/Translations or SERVICES/Translations)
            _translationsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translations");
            
            LoadAllTranslations();
        }

        /// <summary>
        /// Obtiene el código del idioma actual
        /// </summary>
        public string CurrentLanguage
        {
            get { return _currentLanguage; }
        }

        /// <summary>
        /// Obtiene la lista de idiomas disponibles en la aplicación
        /// </summary>
        public List<string> AvailableLanguages
        {
            get { return new List<string> { "es", "en" }; }
        }

        /// <summary>
        /// Obtiene una cadena traducida usando el idioma actual
        /// </summary>
        /// <param name="key">Clave de la cadena a traducir</param>
        /// <returns>Cadena traducida o la clave si no se encuentra traducción</returns>
        public string GetString(string key)
        {
            return GetString(key, _currentLanguage);
        }

        /// <summary>
        /// Obtiene una cadena traducida en el idioma especificado
        /// </summary>
        /// <param name="key">Clave de la cadena a traducir</param>
        /// <param name="language">Código del idioma (ej: 'es', 'en')</param>
        /// <returns>Cadena traducida o la clave si no se encuentra traducción</returns>
        public string GetString(string key, string language)
        {
            // Try to get from JSON translations first
            if (_languageTranslations.ContainsKey(language) && 
                _languageTranslations[language].ContainsKey(key))
            {
                return _languageTranslations[language][key];
            }

            // Fallback to Spanish if translation not found
            if (language != "es" && 
                _languageTranslations.ContainsKey("es") && 
                _languageTranslations["es"].ContainsKey(key))
            {
                return _languageTranslations["es"][key];
            }

            return key; // Return the key itself if no translation found
        }

        /// <summary>
        /// Establece el idioma actual de la aplicación
        /// </summary>
        /// <param name="language">Código del idioma a establecer</param>
        public void SetLanguage(string language)
        {
            if (AvailableLanguages.Contains(language) && _currentLanguage != language)
            {
                _currentLanguage = language;
                // Only reload if translations haven't been loaded yet
                if (!_languageTranslations.ContainsKey(language))
                {
                    LoadAllTranslations();
                }
                // Notify subscribers that the language has changed
                OnLanguageChanged();
            }
        }

        /// <summary>
        /// Dispara el evento de cambio de idioma para notificar a los suscriptores
        /// </summary>
        protected virtual void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Carga todas las traducciones desde JSON, base de datos y valores predeterminados
        /// </summary>
        private void LoadAllTranslations()
        {
            _languageTranslations.Clear();
            
            // Load from JSON files first
            LoadTranslationsFromJson();
            
            // Load from database if configured (will override JSON if present)
            LoadTranslationsFromDatabase();
            
            // If no translations loaded, use defaults
            if (_languageTranslations.Count == 0)
            {
                LoadDefaultTranslations();
            }
        }

        /// <summary>
        /// Carga las traducciones desde archivos JSON ubicados en el directorio de traducciones
        /// </summary>
        private void LoadTranslationsFromJson()
        {
            try
            {
                if (!Directory.Exists(_translationsPath))
                {
                    return;
                }

                foreach (var language in AvailableLanguages)
                {
                    var jsonFile = Path.Combine(_translationsPath, $"{language}.json");
                    if (File.Exists(jsonFile))
                    {
                        var jsonContent = File.ReadAllText(jsonFile);
                        var translations = ParseJsonTranslations(jsonContent);
                        _languageTranslations[language] = translations;
                    }
                }
            }
            catch (IOException ex)
            {
                // Log file I/O errors but continue with other sources
                System.Diagnostics.Debug.WriteLine($"Error loading JSON translations: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                // Log permission errors but continue with other sources
                System.Diagnostics.Debug.WriteLine($"Access denied loading JSON translations: {ex.Message}");
            }
        }

        /// <summary>
        /// Analiza el contenido JSON y convierte las traducciones en un diccionario clave-valor
        /// </summary>
        /// <param name="jsonContent">Contenido del archivo JSON</param>
        /// <returns>Diccionario con las traducciones parseadas</returns>
        private Dictionary<string, string> ParseJsonTranslations(string jsonContent)
        {
            var translations = new Dictionary<string, string>();
            
            // Simple line-by-line parser for our specific two-level nested structure
            var lines = jsonContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string currentCategory = null;
            
            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                
                // Skip opening and closing braces
                if (line == "{" || line == "}" || string.IsNullOrEmpty(line))
                    continue;
                
                // Check if this is a category (key followed by opening brace)
                if (line.Contains("{") && line.Contains("\""))
                {
                    var keyStart = line.IndexOf('"') + 1;
                    var keyEnd = line.IndexOf('"', keyStart);
                    if (keyEnd > keyStart)
                    {
                        currentCategory = line.Substring(keyStart, keyEnd - keyStart);
                    }
                    continue;
                }
                
                // Check if this is a closing brace (end of category)
                if (line.Contains("}"))
                {
                    currentCategory = null;
                    continue;
                }
                
                // Parse key-value pair
                if (line.Contains(":") && line.Contains("\""))
                {
                    // Find the key
                    var keyStart = line.IndexOf('"') + 1;
                    var keyEnd = line.IndexOf('"', keyStart);
                    if (keyEnd <= keyStart)
                        continue;
                    
                    var key = line.Substring(keyStart, keyEnd - keyStart);
                    
                    // Find the value (handle potential commas at end)
                    var colonIndex = line.IndexOf(':', keyEnd);
                    var valueStart = line.IndexOf('"', colonIndex) + 1;
                    
                    // Find the closing quote, handling escaped quotes
                    var valueEnd = valueStart;
                    while (valueEnd < line.Length)
                    {
                        valueEnd = line.IndexOf('"', valueEnd);
                        if (valueEnd == -1)
                            break;
                        
                        // Count consecutive backslashes before this quote
                        int backslashCount = 0;
                        int checkPos = valueEnd - 1;
                        while (checkPos >= valueStart && line[checkPos] == '\\')
                        {
                            backslashCount++;
                            checkPos--;
                        }
                        
                        // If there's an odd number of backslashes, the quote is escaped
                        if (backslashCount % 2 == 1)
                        {
                            valueEnd++; // Skip escaped quote
                            continue;
                        }
                        break; // Found unescaped closing quote
                    }
                    
                    if (valueEnd <= valueStart || valueEnd == -1)
                        continue;
                    
                    var value = line.Substring(valueStart, valueEnd - valueStart);
                    // Unescape the value
                    value = value.Replace("\\\"", "\"").Replace("\\\\", "\\");
                    
                    // Build full key
                    var fullKey = currentCategory != null ? $"{currentCategory}.{key}" : key;
                    translations[fullKey] = value;
                }
            }
            
            return translations;
        }

        /// <summary>
        /// Carga las traducciones desde la base de datos si está configurada
        /// </summary>
        private void LoadTranslationsFromDatabase()
        {
            try
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
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
                            var key = reader["ResourceKey"].ToString();
                            var language = reader["Language"].ToString();
                            var value = reader["ResourceValue"].ToString();
                            
                            if (!_languageTranslations.ContainsKey(language))
                            {
                                _languageTranslations[language] = new Dictionary<string, string>();
                            }
                            
                            _languageTranslations[language][key] = value;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log database errors but continue with JSON translations
                System.Diagnostics.Debug.WriteLine($"Database error loading translations: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Log connection errors but continue with JSON translations
                System.Diagnostics.Debug.WriteLine($"Connection error loading translations: {ex.Message}");
            }
        }

        /// <summary>
        /// Carga traducciones predeterminadas en español e inglés si no se encuentran otras fuentes
        /// </summary>
        private void LoadDefaultTranslations()
        {
            // Initialize dictionaries for each language
            _languageTranslations["es"] = new Dictionary<string, string>();
            _languageTranslations["en"] = new Dictionary<string, string>();
            
            // Common translations
            _languageTranslations["es"]["Common.Login"] = "Iniciar Sesión";
            _languageTranslations["en"]["Common.Login"] = "Login";
            _languageTranslations["es"]["Common.Username"] = "Usuario";
            _languageTranslations["en"]["Common.Username"] = "Username";
            _languageTranslations["es"]["Common.Password"] = "Contraseña";
            _languageTranslations["en"]["Common.Password"] = "Password";
            _languageTranslations["es"]["Common.Save"] = "Guardar";
            _languageTranslations["en"]["Common.Save"] = "Save";
            _languageTranslations["es"]["Common.Cancel"] = "Cancelar";
            _languageTranslations["en"]["Common.Cancel"] = "Cancel";
            _languageTranslations["es"]["Common.Delete"] = "Eliminar";
            _languageTranslations["en"]["Common.Delete"] = "Delete";
            _languageTranslations["es"]["Common.Edit"] = "Editar";
            _languageTranslations["en"]["Common.Edit"] = "Edit";
            _languageTranslations["es"]["Common.New"] = "Nuevo";
            _languageTranslations["en"]["Common.New"] = "New";
            _languageTranslations["es"]["Common.Search"] = "Buscar";
            _languageTranslations["en"]["Common.Search"] = "Search";
            _languageTranslations["es"]["Common.Close"] = "Cerrar";
            _languageTranslations["en"]["Common.Close"] = "Close";
            _languageTranslations["es"]["Common.Yes"] = "Sí";
            _languageTranslations["en"]["Common.Yes"] = "Yes";
            _languageTranslations["es"]["Common.No"] = "No";
            _languageTranslations["en"]["Common.No"] = "No";
            _languageTranslations["es"]["Common.Error"] = "Error";
            _languageTranslations["en"]["Common.Error"] = "Error";
            _languageTranslations["es"]["Common.Validation"] = "Validación";
            _languageTranslations["en"]["Common.Validation"] = "Validation";
            
            // Login translations
            _languageTranslations["es"]["Login.UsernameRequired"] = "Por favor ingrese su usuario.";
            _languageTranslations["en"]["Login.UsernameRequired"] = "Please enter your username.";
            _languageTranslations["es"]["Login.PasswordRequired"] = "Por favor ingrese su contraseña.";
            _languageTranslations["en"]["Login.PasswordRequired"] = "Please enter your password.";
            _languageTranslations["es"]["Login.InvalidCredentials"] = "Usuario o contraseña incorrectos.";
            _languageTranslations["en"]["Login.InvalidCredentials"] = "Invalid username or password.";
            _languageTranslations["es"]["Login.AuthError"] = "Error de Autenticación";
            _languageTranslations["en"]["Login.AuthError"] = "Authentication Error";
            _languageTranslations["es"]["Login.Error"] = "Error al iniciar sesión";
            _languageTranslations["en"]["Login.Error"] = "Login error";
            
            // Menu translations
            _languageTranslations["es"]["Menu.Users"] = "Usuarios";
            _languageTranslations["en"]["Menu.Users"] = "Users";
            _languageTranslations["es"]["Menu.Roles"] = "Roles";
            _languageTranslations["en"]["Menu.Roles"] = "Roles";
            _languageTranslations["es"]["Menu.Products"] = "Productos";
            _languageTranslations["en"]["Menu.Products"] = "Products";
            _languageTranslations["es"]["Menu.Warehouses"] = "Almacenes";
            _languageTranslations["en"]["Menu.Warehouses"] = "Warehouses";
            _languageTranslations["es"]["Menu.Stock"] = "Inventario";
            _languageTranslations["en"]["Menu.Stock"] = "Stock";
            _languageTranslations["es"]["Menu.StockMovements"] = "Movimientos de Stock";
            _languageTranslations["en"]["Menu.StockMovements"] = "Stock Movements";
        }
    }
}
