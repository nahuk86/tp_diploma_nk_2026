# Multi-Language Translation System

## Overview
This application supports multiple languages through a flexible translation system that uses JSON files for easy management and modification.

## Supported Languages
- **Spanish (es)** - Default language
- **English (en)**

## Translation Files Location
Translation files are located in: `UI/Translations/`

Each language has its own JSON file:
- `es.json` - Spanish translations
- `en.json` - English translations

## JSON File Structure
Translations are organized hierarchically using JSON objects. The system automatically flattens the structure into dot-notation keys.

Example:
```json
{
  "Common": {
    "Login": "Iniciar Sesión",
    "Username": "Usuario",
    "Password": "Contraseña"
  },
  "Menu": {
    "File": "&Archivo",
    "Users": "&Usuarios"
  }
}
```

This creates the following translation keys:
- `Common.Login`
- `Common.Username`
- `Common.Password`
- `Menu.File`
- `Menu.Users`

## How to Use in Code
```csharp
// Get a translated string
var loginText = _localizationService.GetString("Common.Login");

// Get a translated string with fallback
var buttonText = _localizationService.GetString("Common.Save") ?? "Save";

// Change language dynamically
_localizationService.SetLanguage("en");
```

## Dynamic Language Switching
The application supports changing the language during runtime:
1. Users can select their preferred language from the **Settings > Language** menu
2. The UI automatically refreshes when the language is changed
3. All open windows update to reflect the new language

## Adding New Translations

### To add a new language:
1. Create a new JSON file in `UI/Translations/` (e.g., `fr.json` for French)
2. Copy the structure from `es.json` or `en.json`
3. Translate all strings to the new language
4. Update `AvailableLanguages` property in `LocalizationService.cs`

### To add new translation keys:
1. Add the key-value pair to all language JSON files
2. Use the hierarchical structure to organize related translations
3. Follow the existing naming conventions (e.g., `Category.SubCategory.Key`)

## Translation Priority
The system loads translations in the following order (later sources override earlier ones):
1. **JSON files** - Primary source for translations
2. **Database** - If configured, database translations override JSON
3. **Default/Hardcoded** - Fallback if no JSON or database translations exist

## Fallback Mechanism
If a translation is not found:
1. First, it tries the requested language
2. If not found, it falls back to Spanish (es)
3. If still not found, it returns the key itself

## Best Practices
- Always provide translations for all supported languages
- Use descriptive, hierarchical key names (e.g., `Form.Product.Title`)
- Include keyboard shortcuts in menu items using `&` (e.g., `&File`, `&Save`)
- Test language switching with all forms open to ensure proper refresh
- Keep JSON files properly formatted and validated

## Example Translation Keys

### Common UI Elements
- `Common.Save` - Save button
- `Common.Cancel` - Cancel button
- `Common.Delete` - Delete button
- `Common.Edit` - Edit button
- `Common.New` - New button
- `Common.Search` - Search button
- `Common.Close` - Close button

### Login
- `Login.UsernameRequired` - Username required validation
- `Login.PasswordRequired` - Password required validation
- `Login.InvalidCredentials` - Invalid login message

### Menu Items
- `Menu.File` - File menu
- `Menu.Users` - Users menu item
- `Menu.Products` - Products menu item
- `Menu.Language` - Language menu item

## Notes
- JSON files are loaded once when the `LocalizationService` is created
- Changing language reloads all translation files
- The system is case-sensitive for translation keys
- Database translations (if configured) will override JSON translations
