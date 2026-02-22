# Localization - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Localization-related use cases.

---

## UC-01: LoadAllTranslations

### Class Diagram

```mermaid
classDiagram
    class ILocalizationService {
        <<interface>>
        +GetString(key) string
        +SetLanguage(languageCode) void
        +LoadTranslations(languageCode) void
        +GetCurrentLanguage() string
        +GetAvailableLanguages() List~string~
    }

    class LocalizationService {
        -Dictionary~string,string~ _translations
        -string _currentLanguage
        -string _translationsPath
        +GetString(key) string
        +SetLanguage(languageCode) void
        +LoadTranslations(languageCode) void
        +LoadAllTranslations() void
        +GetCurrentLanguage() string
        +GetAvailableLanguages() List~string~
        -ReadJsonFile(path) Dictionary~string,string~
    }

    class LanguageChangedEventArgs {
        +string OldLanguage
        +string NewLanguage
    }

    class AnyForm {
        -ILocalizationService _localizationService
        +ApplyLocalization() void
    }

    AnyForm --> ILocalizationService : uses
    LocalizationService ..|> ILocalizationService : implements
    LocalizationService --> LanguageChangedEventArgs : publishes
    LocalizationService ..> Dictionary : uses
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant APP as Application
    participant LOC as LocalizationService
    participant FS as FileSystem

    Note over APP: Application startup
    APP->>LOC: LoadAllTranslations()
    activate LOC
    LOC->>FS: File.Exists("translations/en.json")
    FS-->>LOC: true
    LOC->>FS: File.ReadAllText("translations/en.json")
    FS-->>LOC: JSON content (English)
    LOC->>LOC: DeserializeJson(content)
    LOC->>LOC: _translations["en"] = englishDict
    LOC->>FS: File.Exists("translations/es.json")
    FS-->>LOC: true
    LOC->>FS: File.ReadAllText("translations/es.json")
    FS-->>LOC: JSON content (Spanish)
    LOC->>LOC: DeserializeJson(content)
    LOC->>LOC: _translations["es"] = spanishDict
    LOC-->>APP: void (all translations loaded)
    deactivate LOC
```

---

## UC-02: LoadDefaultTranslations

### Class Diagram

```mermaid
classDiagram
    class ILocalizationService {
        <<interface>>
        +LoadTranslations(languageCode) void
        +GetString(key) string
    }

    class LocalizationService {
        -Dictionary~string,string~ _activeTranslations
        -string _currentLanguage
        -string _defaultLanguage
        +LoadTranslations(languageCode) void
        +LoadDefaultTranslations() void
        +GetString(key) string
        -ReadJsonFile(path) Dictionary~string,string~
    }

    class AnyForm {
        -ILocalizationService _localizationService
        +InitializeComponent() void
        +ApplyLocalization() void
    }

    AnyForm --> ILocalizationService : uses
    LocalizationService ..|> ILocalizationService : implements
    LocalizationService ..> Dictionary : uses
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant APP as Application
    participant LOC as LocalizationService
    participant FS as FileSystem

    Note over APP: Application startup with no saved preference
    APP->>LOC: LoadDefaultTranslations()
    activate LOC
    Note over LOC: Default language = "es" (Spanish)
    LOC->>FS: File.Exists("translations/es.json")
    FS-->>LOC: true
    LOC->>FS: File.ReadAllText("translations/es.json")
    FS-->>LOC: JSON content
    LOC->>LOC: DeserializeJson(content)
    LOC->>LOC: _activeTranslations = spanishDict
    LOC->>LOC: _currentLanguage = "es"
    LOC-->>APP: void (defaults loaded)
    deactivate LOC
    APP->>APP: CreateForms with localized text
```

---

## UC-03: OnLanguageChanged

### Class Diagram

```mermaid
classDiagram
    class ILocalizationService {
        <<interface>>
        +SetLanguage(languageCode) void
        +GetString(key) string
        +GetCurrentLanguage() string
    }

    class LocalizationService {
        -Dictionary~string,string~ _activeTranslations
        -string _currentLanguage
        +SetLanguage(languageCode) void
        +GetCurrentLanguage() string
        +event LanguageChanged
        -FireLanguageChangedEvent(oldLang, newLang) void
    }

    class LanguageChangedEventArgs {
        +string OldLanguage
        +string NewLanguage
    }

    class MainForm {
        -ILocalizationService _localizationService
        +OnLanguageChanged(sender, e) void
        +ApplyLocalization() void
    }

    class SettingsForm {
        -ILocalizationService _localizationService
        +cmbLanguage_SelectedIndexChanged(sender, e) void
    }

    SettingsForm --> ILocalizationService : uses
    MainForm --> ILocalizationService : subscribes
    LocalizationService ..|> ILocalizationService : implements
    LocalizationService --> LanguageChangedEventArgs : publishes
    MainForm ..> LanguageChangedEventArgs : handles
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SettingsForm
    participant LOC as LocalizationService
    participant MAIN as MainForm
    participant OTHER as OtherOpenForms

    UI->>UI: User selects "English" from language dropdown
    UI->>LOC: SetLanguage("en")
    activate LOC
    LOC->>LOC: LoadTranslations("en")
    LOC->>LOC: _activeTranslations = englishDict
    LOC->>LOC: _currentLanguage = "en"
    LOC->>LOC: FireLanguageChangedEvent("es", "en")
    LOC-->>MAIN: event LanguageChanged(oldLang="es", newLang="en")
    activate MAIN
    MAIN->>LOC: GetString("Menu.Products")
    LOC-->>MAIN: "Products"
    MAIN->>MAIN: ApplyLocalization()
    MAIN-->>MAIN: All labels/buttons updated
    deactivate MAIN
    LOC-->>OTHER: event LanguageChanged (broadcast)
    activate OTHER
    OTHER->>OTHER: ApplyLocalization()
    deactivate OTHER
    LOC-->>UI: void
    deactivate LOC
    UI-->>UI: Show confirmation
```

---

## UC-04: SetLanguage

### Class Diagram

```mermaid
classDiagram
    class ILocalizationService {
        <<interface>>
        +SetLanguage(languageCode) void
        +GetCurrentLanguage() string
        +GetAvailableLanguages() List~string~
    }

    class LocalizationService {
        -Dictionary~string,Dictionary~string,string~~ _allTranslations
        -Dictionary~string,string~ _activeTranslations
        -string _currentLanguage
        +SetLanguage(languageCode) void
        +GetCurrentLanguage() string
        +GetAvailableLanguages() List~string~
        +GetString(key) string
    }

    class AnyForm {
        -ILocalizationService _localizationService
        +ApplyLocalization() void
    }

    AnyForm --> ILocalizationService : uses
    LocalizationService ..|> ILocalizationService : implements
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as AnyForm
    participant LOC as LocalizationService
    participant FS as FileSystem

    UI->>LOC: SetLanguage("en")
    activate LOC
    Note over LOC: Check if "en" is in loaded translations
    alt Translations already loaded
        LOC->>LOC: _activeTranslations = _allTranslations["en"]
        LOC->>LOC: _currentLanguage = "en"
    else Not yet loaded
        LOC->>FS: File.ReadAllText("translations/en.json")
        FS-->>LOC: JSON content
        LOC->>LOC: DeserializeJson(content)
        LOC->>LOC: Cache and set active translations
    end
    LOC-->>UI: void
    deactivate LOC
    UI->>LOC: GetString("Common.Save")
    LOC-->>UI: "Save"
    UI->>UI: ApplyLocalization()
    Note over UI: All UI text updated to new language
```

---
