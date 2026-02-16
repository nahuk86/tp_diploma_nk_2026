# Guía Rápida de Gestión de Errores y Traducciones / Quick Guide for Error Handling and Translations

## Para Desarrolladores / For Developers

### 1. Cómo Manejar Errores / How to Handle Errors

#### Opción 1: Solo Logging / Logging Only
```csharp
try
{
    // Tu código / Your code
    productService.Save(product);
}
catch (Exception ex)
{
    _errorHandler.HandleError(ex, "Error saving product");
    // Error se registra en log pero NO se muestra al usuario
    // Error is logged but NOT shown to user
}
```

#### Opción 2: Logging + Mensaje al Usuario / Logging + User Message
```csharp
try
{
    // Tu código / Your code
    productService.Save(product);
}
catch (Exception ex)
{
    // Error se registra Y se muestra mensaje amigable al usuario
    // Error is logged AND user-friendly message is shown
    _errorHandler.ShowError(ex, "Error saving product");
}
```

#### Opción 3: Mensaje Personalizado / Custom Message
```csharp
try
{
    // Tu código / Your code
    productService.Save(product);
}
catch (Exception ex)
{
    _errorHandler.HandleError(ex, "Error saving product");
    
    // Mensaje personalizado / Custom message
    MessageBox.Show(
        _localizationService.GetString("Products.CreateSuccess"),
        _localizationService.GetString("Common.Success"),
        MessageBoxButtons.OK,
        MessageBoxIcon.Information
    );
}
```

---

### 2. Cómo Usar Traducciones / How to Use Translations

#### En Forms / In Forms
```csharp
private void ApplyLocalization()
{
    // Título de la ventana / Window title
    this.Text = _localizationService.GetString("Products.Title") ?? "Gestión de Productos";
    
    // Labels
    lblName.Text = _localizationService.GetString("Products.Name") ?? "Nombre:";
    
    // Botones / Buttons
    btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
}
```

#### Mensajes de Validación / Validation Messages
```csharp
if (string.IsNullOrWhiteSpace(txtSKU.Text))
{
    MessageBox.Show(
        _localizationService.GetString("Products.SKURequired") ?? "El SKU es requerido.",
        _localizationService.GetString("Common.Validation") ?? "Validación",
        MessageBoxButtons.OK,
        MessageBoxIcon.Warning
    );
    return;
}
```

#### Mensajes de Confirmación / Confirmation Messages
```csharp
var result = MessageBox.Show(
    string.Format(
        _localizationService.GetString("Products.ConfirmDelete") ?? "¿Está seguro que desea eliminar el producto '{0}'?",
        product.Name
    ),
    _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question
);

if (result == DialogResult.Yes)
{
    // Eliminar / Delete
}
```

---

### 3. Claves de Traducción Comunes / Common Translation Keys

#### Acciones Comunes / Common Actions
- `Common.Save` - Guardar / Save
- `Common.Cancel` - Cancelar / Cancel
- `Common.Delete` - Eliminar / Delete
- `Common.Edit` - Editar / Edit
- `Common.New` - Nuevo / New
- `Common.Search` - Buscar / Search
- `Common.Close` - Cerrar / Close

#### Diálogos / Dialogs
- `Common.Error` - Error
- `Common.Validation` - Validación / Validation
- `Common.Confirmation` - Confirmación / Confirmation
- `Common.Success` - Éxito / Success

#### Respuestas / Responses
- `Common.Yes` - Sí / Yes
- `Common.No` - No

---

### 4. Errores Estándar / Standard Errors

#### Errores de Carga / Loading Errors
- `Error.LoadingProducts`
- `Error.LoadingClients`
- `Error.LoadingUsers`
- `Error.LoadingRoles`
- `Error.LoadingWarehouses`
- `Error.LoadingStock`

#### Errores de Guardado / Saving Errors
- `Error.SavingProduct`
- `Error.SavingClient`
- `Error.SavingUser`
- `Error.SavingRole`
- `Error.SavingWarehouse`

#### Errores de Eliminación / Deletion Errors
- `Error.DeletingProduct`
- `Error.DeletingClient`
- `Error.DeletingUser`
- `Error.DeletingRole`
- `Error.DeletingWarehouse`

#### Errores de Base de Datos / Database Errors
- `Error.DuplicateEntry` - Registro duplicado
- `Error.ForeignKeyViolation` - Clave foránea
- `Error.DatabaseTimeout` - Timeout
- `Error.DatabaseError` - Error general de BD

#### Errores de Lógica / Logic Errors
- `Error.InvalidOperation` - Operación inválida
- `Error.InvalidData` - Datos inválidos
- `Error.Unauthorized` - Sin permisos
- `Error.Generic` - Error genérico

---

### 5. Estructura de Traducciones por Módulo / Translation Structure by Module

#### Products (Productos)
```
Products.Title - Título de la ventana / Window title
Products.List - "Lista de Productos" / "Products List"
Products.Details - "Detalles del Producto" / "Product Details"
Products.SKU - Campo SKU
Products.Name - Campo Nombre
Products.CreateSuccess - Mensaje de éxito al crear
Products.ConfirmDelete - Confirmación de eliminación
```

#### Clients (Clientes)
```
Clients.Title
Clients.List
Clients.Details
Clients.Nombre - First Name
Clients.Apellido - Last Name
Clients.DNI - ID Number
Clients.CreateSuccess
Clients.ConfirmDelete
```

#### Users (Usuarios)
```
Users.Title
Users.List
Users.Details
Users.Username
Users.FullName
Users.Email
Users.Password
Users.ChangePassword
Users.CreateSuccess
```

---

### 6. Patrón Estándar de Form / Standard Form Pattern

```csharp
public partial class MyForm : Form
{
    private readonly ILogService _logService;
    private readonly ILocalizationService _localizationService;
    private readonly IErrorHandlerService _errorHandler;
    
    public MyForm()
    {
        InitializeComponent();
        
        // Inicializar servicios / Initialize services
        _logService = new FileLogService();
        _localizationService = new LocalizationService();
        _errorHandler = new ErrorHandlerService(_logService, _localizationService);
        
        // Aplicar traducciones / Apply translations
        ApplyLocalization();
        
        // Cargar datos / Load data
        LoadData();
    }
    
    private void ApplyLocalization()
    {
        this.Text = _localizationService.GetString("MyModule.Title") ?? "Default Title";
        // ... más traducciones / more translations
    }
    
    private void LoadData()
    {
        try
        {
            // Cargar datos / Load data
        }
        catch (Exception ex)
        {
            _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingData"));
        }
    }
    
    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Validaciones / Validations
            if (string.IsNullOrWhiteSpace(txtField.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("MyModule.FieldRequired"),
                    _localizationService.GetString("Common.Validation"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            
            // Guardar / Save
            _myService.Save(data);
            
            // Éxito / Success
            MessageBox.Show(
                _localizationService.GetString("MyModule.SaveSuccess"),
                _localizationService.GetString("Common.Success"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (Exception ex)
        {
            _errorHandler.ShowError(ex, _localizationService.GetString("Error.SavingData"));
        }
    }
}
```

---

### 7. Cómo Agregar Nuevas Traducciones / How to Add New Translations

#### Paso 1: Editar es.json
```json
{
  "MyModule": {
    "Title": "Mi Módulo",
    "NewField": "Nuevo Campo",
    "NewFieldRequired": "El nuevo campo es requerido."
  }
}
```

#### Paso 2: Editar en.json
```json
{
  "MyModule": {
    "Title": "My Module",
    "NewField": "New Field",
    "NewFieldRequired": "New field is required."
  }
}
```

#### Paso 3: Usar en el código / Use in code
```csharp
lblNewField.Text = _localizationService.GetString("MyModule.NewField") ?? "Nuevo Campo";
```

---

### 8. Convenciones de Nomenclatura / Naming Conventions

#### Para Módulos / For Modules
- `ModuleName.Title` - Título de la ventana
- `ModuleName.List` - Título de la lista
- `ModuleName.Details` - Título de detalles
- `ModuleName.FieldName` - Nombre del campo
- `ModuleName.FieldNameRequired` - Validación requerida
- `ModuleName.CreateSuccess` - Mensaje de éxito al crear
- `ModuleName.UpdateSuccess` - Mensaje de éxito al actualizar
- `ModuleName.DeleteSuccess` - Mensaje de éxito al eliminar
- `ModuleName.ConfirmDelete` - Confirmación de eliminación
- `ModuleName.SelectItem` - "Por favor seleccione..."

#### Para Errores / For Errors
- `Error.LoadingModule` - Error cargando módulo
- `Error.SavingModule` - Error guardando módulo
- `Error.DeletingModule` - Error eliminando módulo
- `Error.GenericOperation` - Error en operación específica

---

### 9. Validación de JSON / JSON Validation

Antes de commitear, validar los archivos JSON:
Before committing, validate JSON files:

```bash
# En Linux/Mac / On Linux/Mac
python3 -c "import json; f=open('UI/Translations/es.json'); json.load(f); print('es.json OK')"
python3 -c "import json; f=open('UI/Translations/en.json'); json.load(f); print('en.json OK')"

# O usar un validador online / Or use an online validator
# https://jsonlint.com/
```

---

### 10. Checklist antes de Commit / Pre-Commit Checklist

- [ ] Todas las traducciones tienen clave en es.json y en.json
- [ ] No hay claves duplicadas
- [ ] JSON es válido (sin errores de sintaxis)
- [ ] Las traducciones mantienen el mismo número de placeholders ({0}, {1})
- [ ] Los mensajes de error usan claves de Error.*
- [ ] Los mensajes comunes usan claves de Common.*
- [ ] Cada módulo tiene su propia categoría
- [ ] Se usa el fallback operator (??) con texto por defecto

---

## Recursos Adicionales / Additional Resources

- [ERROR_HANDLING_IMPLEMENTATION.md](ERROR_HANDLING_IMPLEMENTATION.md) - Documentación completa
- [MULTILANG_IMPLEMENTATION.md](MULTILANG_IMPLEMENTATION.md) - Implementación multiidioma
- [UI/Translations/README.md](UI/Translations/README.md) - Sistema de traducciones

---

**Última actualización / Last updated:** 2026-02-16
