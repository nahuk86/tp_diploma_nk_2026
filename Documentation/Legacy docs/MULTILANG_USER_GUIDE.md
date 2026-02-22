# Guía de Uso del Sistema Multi-Idioma / Multi-Language System Usage Guide

## Español

### ¿Qué cambió?
El sistema ahora soporta cambio dinámico de idioma durante la ejecución de la aplicación, sin necesidad de reiniciar.

### ¿Cómo cambiar el idioma?
1. Abra la aplicación
2. Vaya al menú **Configuración** → **Idioma**
3. Seleccione el idioma deseado (Español o English)
4. ¡Todas las ventanas abiertas se actualizarán automáticamente!

### Idiomas Disponibles
- **Español** (por defecto)
- **English**

### Para Traductores
Las traducciones se encuentran en archivos JSON fáciles de editar:
- `UI/Translations/es.json` - Traducciones al español
- `UI/Translations/en.json` - Traducciones al inglés

**Importante:** Los archivos JSON deben mantener la misma estructura jerárquica en todos los idiomas.

### Para Agregar Nuevas Traducciones
1. Abra ambos archivos (`es.json` y `en.json`)
2. Agregue la nueva clave en la categoría apropiada
3. Proporcione la traducción en cada idioma
4. Guarde los archivos

Ejemplo:
```json
{
  "Common": {
    "Welcome": "Bienvenido"  // en es.json
  }
}

{
  "Common": {
    "Welcome": "Welcome"     // en en.json
  }
}
```

---

## English

### What changed?
The system now supports dynamic language switching during application runtime, without needing to restart.

### How to change the language?
1. Open the application
2. Go to the **Settings** → **Language** menu
3. Select the desired language (Español or English)
4. All open windows will update automatically!

### Available Languages
- **Spanish** (default)
- **English**

### For Translators
Translations are stored in easy-to-edit JSON files:
- `UI/Translations/es.json` - Spanish translations
- `UI/Translations/en.json` - English translations

**Important:** JSON files must maintain the same hierarchical structure across all languages.

### To Add New Translations
1. Open both files (`es.json` and `en.json`)
2. Add the new key in the appropriate category
3. Provide the translation in each language
4. Save the files

Example:
```json
{
  "Common": {
    "Welcome": "Bienvenido"  // in es.json
  }
}

{
  "Common": {
    "Welcome": "Welcome"     // in en.json
  }
}
```

---

## Características Técnicas / Technical Features

### Cambio Dinámico / Dynamic Switching
- ✅ Cambio de idioma en tiempo de ejecución / Runtime language switching
- ✅ Actualización automática de todas las ventanas / Automatic update of all windows
- ✅ Sin reinicio requerido / No restart required

### Almacenamiento / Storage
- ✅ Archivos JSON separados por idioma / Separate JSON files per language
- ✅ Fácil de editar sin conocimientos técnicos / Easy to edit without technical knowledge
- ✅ Control de versiones amigable / Version control friendly

### Compatibilidad / Compatibility
- ✅ Compatible con traducciones de base de datos / Compatible with database translations
- ✅ Sistema de respaldo si no hay traducciones / Fallback system if translations missing
- ✅ Sin dependencias externas / No external dependencies

### Rendimiento / Performance
- ✅ Carga optimizada (solo una vez por idioma) / Optimized loading (once per language)
- ✅ Cambio instantáneo de idioma / Instant language switching
- ✅ Sin impacto en el rendimiento / No performance impact

---

## Solución de Problemas / Troubleshooting

### El idioma no cambia
1. Verifique que los archivos JSON existan en `bin/Debug/Translations/` o `bin/Release/Translations/`
2. Compruebe que los archivos tengan el formato JSON correcto
3. Revise los logs de la aplicación para errores

### Traducciones faltantes
1. Abra el archivo JSON del idioma correspondiente
2. Agregue la clave de traducción faltante
3. Guarde y reinicie la aplicación (solo la primera vez)

### Language doesn't change
1. Verify that JSON files exist in `bin/Debug/Translations/` or `bin/Release/Translations/`
2. Check that files have correct JSON format
3. Review application logs for errors

### Missing translations
1. Open the JSON file for the corresponding language
2. Add the missing translation key
3. Save and restart the application (first time only)

---

## Contacto / Contact

Para más información, consulte la documentación técnica en `MULTILANG_IMPLEMENTATION.md`

For more information, see the technical documentation in `MULTILANG_IMPLEMENTATION.md`
