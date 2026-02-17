# Guía de Prueba: Cambio de Idioma / Language Switching Test Guide

## 🎯 Objetivo / Objective

Verificar que el cambio de idioma funciona correctamente en toda la aplicación.
Verify that language switching works correctly throughout the application.

---

## ✅ Solución Implementada / Implemented Solution

El problema de cambio de idioma ha sido **RESUELTO** con las siguientes mejoras:
The language switching problem has been **FIXED** with the following improvements:

### 1. Patrón Singleton / Singleton Pattern
- `LocalizationService` ahora usa un patrón singleton con `Lazy<T>`
- `LocalizationService` now uses a singleton pattern with `Lazy<T>`
- Todos los formularios comparten la misma instancia
- All forms share the same instance

### 2. Propagación de Eventos / Event Propagation
- Cuando cambias el idioma, se dispara el evento `LanguageChanged`
- When you change language, the `LanguageChanged` event fires
- El formulario principal notifica a todos los formularios hijos abiertos
- The main form notifies all open child forms

### 3. Actualización Automática / Automatic Refresh
- Los formularios se actualizan automáticamente sin necesidad de cerrarlos
- Forms update automatically without needing to close them

---

## 🧪 Cómo Probar / How to Test

### Paso 1: Iniciar la Aplicación / Start Application
```
1. Compilar y ejecutar el proyecto UI / Build and run the UI project
2. Iniciar sesión / Log in
3. Abrir el formulario principal / Open main form
```

### Paso 2: Abrir Varios Formularios / Open Multiple Forms
Desde el menú, abre varios formularios:
From the menu, open several forms:

```
- Inventario → Productos / Inventory → Products
- Inventario → Almacenes / Inventory → Warehouses  
- Administración → Usuarios / Administration → Users
- Operaciones → Ventas / Operations → Sales
- Operaciones → Consultar Stock / Operations → Query Stock
```

### Paso 3: Verificar Idioma Inicial / Verify Initial Language
Todos los formularios deben mostrar texto en español (idioma por defecto).
All forms should show text in Spanish (default language).

**Elementos a verificar / Elements to verify:**
- ✅ Títulos de formularios / Form titles
- ✅ Etiquetas / Labels
- ✅ Botones / Buttons
- ✅ Encabezados de columnas / Column headers
- ✅ Menús / Menus

### Paso 4: Cambiar a Inglés / Switch to English

1. En el menú principal, selecciona / In the main menu, select:
   ```
   Configuración → Idioma → English
   Settings → Language → English
   ```

2. **Observa / Observe:**
   - ✅ El menú principal debe cambiar a inglés
   - ✅ The main menu should change to English
   - ✅ TODOS los formularios abiertos deben actualizarse inmediatamente
   - ✅ ALL open forms should update immediately
   - ✅ No es necesario cerrar y volver a abrir los formularios
   - ✅ No need to close and reopen forms

### Paso 5: Verificar Cambios / Verify Changes

**Formulario de Productos / Products Form:**
- "Gestión de Productos" → "Products Management"
- "Nuevo" → "New"
- "Editar" → "Edit"
- "Eliminar" → "Delete"
- "SKU", "Nombre", "Precio" → "SKU", "Name", "Price"

**Formulario de Ventas / Sales Form:**
- "Gestión de Ventas" → "Sales Management"
- "Agregar Línea" → "Add Line"
- "Cliente" → "Client"
- "Productos" → "Products"

**Formulario de Consulta de Stock / Stock Query Form:**
- "Consulta de Stock" → "Stock Query"
- "Almacén" → "Warehouse"
- "Cantidad" → "Quantity"
- "Buscar" → "Search"

### Paso 6: Cambiar de Nuevo a Español / Switch Back to Spanish

1. Selecciona / Select:
   ```
   Settings → Language → Español
   Configuración → Idioma → Español
   ```

2. **Verifica que todo vuelva al español / Verify everything returns to Spanish**

---

## 🔍 Qué Verificar / What to Verify

### ✅ Comportamiento Esperado / Expected Behavior

1. **Cambio Inmediato / Immediate Change**
   - El idioma cambia instantáneamente sin reinicios
   - Language changes instantly without restarts

2. **Todos los Formularios / All Forms**
   - TODOS los formularios abiertos se actualizan
   - ALL open forms update
   - No solo el formulario principal
   - Not just the main form

3. **Elementos de UI / UI Elements**
   - Títulos de ventana / Window titles
   - Etiquetas / Labels
   - Botones / Buttons
   - Encabezados de grilla / Grid headers
   - Mensajes de validación / Validation messages

4. **Nuevos Formularios / New Forms**
   - Los formularios abiertos después del cambio ya muestran el nuevo idioma
   - Forms opened after the change already show the new language

---

## ❌ Problemas Conocidos Resueltos / Known Issues Resolved

### ❌ ANTES / BEFORE (Problema):
- Los formularios NO cambiaban de idioma
- Forms did NOT change language
- Era necesario cerrar y volver a abrir
- Had to close and reopen
- Cada formulario tenía su propia instancia de LocalizationService
- Each form had its own LocalizationService instance

### ✅ AHORA / NOW (Solucionado):
- Los formularios cambian automáticamente
- Forms change automatically
- Todos comparten la misma instancia singleton
- All share the same singleton instance
- El evento LanguageChanged notifica a todos
- The LanguageChanged event notifies all

---

## 🔧 Detalles Técnicos / Technical Details

### Implementación / Implementation

**LocalizationService.cs:**
```csharp
private static readonly Lazy<LocalizationService> _instance = 
    new Lazy<LocalizationService>(() => new LocalizationService());

public static LocalizationService Instance => _instance.Value;
```

**Form1.cs:**
```csharp
// Suscripción al evento / Event subscription
_localizationService.LanguageChanged += OnLanguageChanged;

private void OnLanguageChanged(object sender, EventArgs e)
{
    ApplyLocalization();      // Actualiza formulario principal
    RefreshMdiChildren();     // Actualiza formularios hijos
}
```

**Todos los formularios / All forms:**
```csharp
_localizationService = LocalizationService.Instance; // Singleton
```

---

## 📝 Registro de Pruebas / Test Log

### Fecha de Prueba / Test Date: ___________

| Formulario / Form | Español → Inglés | Inglés → Español | Notas / Notes |
|-------------------|------------------|------------------|---------------|
| Main Menu         | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Products          | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Warehouses        | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Clients           | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Sales             | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Stock Query       | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Stock Movement    | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Users             | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |
| Roles             | ⬜ Pasó / Passed | ⬜ Pasó / Passed |               |

**Resultado / Result:** ⬜ EXITOSO / SUCCESSFUL  ⬜ FALLÓ / FAILED

**Probador / Tester:** ___________

---

## 📞 Soporte / Support

Si encuentras algún problema después de seguir esta guía:
If you find any problem after following this guide:

1. Verifica que estés usando la versión correcta del código
   Verify you're using the correct code version
   
2. Asegúrate de que los archivos de traducción existan:
   Make sure translation files exist:
   - `UI/Translations/es.json`
   - `UI/Translations/en.json`

3. Revisa los logs en la carpeta `Logs/`
   Check logs in `Logs/` folder

4. Contacta al equipo de desarrollo
   Contact the development team

---

**Estado / Status:** ✅ IMPLEMENTADO Y LISTO PARA PROBAR / IMPLEMENTED AND READY TO TEST

**Última actualización / Last updated:** 2026-02-17
