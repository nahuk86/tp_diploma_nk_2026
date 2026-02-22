# Flujo de Inicialización de Contraseña del Administrador

## Problema Original

Al iniciar la aplicación por primera vez, el usuario admin se crea con un hash de contraseña placeholder (`HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP`), lo que impedía el inicio de sesión sin una forma de configurar la contraseña real.

## Solución Implementada

### Flujo de Primera Ejecución

```
┌─────────────────────────────────────────────────────────────────┐
│  1. Usuario inicia la aplicación (UI.exe)                      │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       v
┌─────────────────────────────────────────────────────────────────┐
│  2. Program.cs verifica si admin tiene password placeholder    │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       v
                ┌──────┴──────┐
                │  ¿Password   │
                │ placeholder? │
                └──────┬──────┘
                       │
        ┌──────────────┴──────────────┐
        │                             │
        v NO                          v SÍ
┌───────────────┐            ┌────────────────────┐
│ Mostrar       │            │ Mostrar            │
│ LoginForm     │            │ AdminPasswordInit  │
│ directamente  │            │ Form               │
└───────────────┘            └────────┬───────────┘
                                      │
                                      v
                             ┌─────────────────────┐
                             │ Usuario ingresa:    │
                             │ - Contraseña        │
                             │ - Confirma password │
                             └────────┬────────────┘
                                      │
                                      v
                             ┌─────────────────────┐
                             │ Validaciones:       │
                             │ ✓ Mínimo 8 chars    │
                             │ ✓ Una mayúscula     │
                             │ ✓ Un número         │
                             │ ✓ Passwords match   │
                             └────────┬────────────┘
                                      │
                      ┌───────────────┴───────────────┐
                      │                               │
                      v VÁLIDO                        v INVÁLIDO
         ┌────────────────────────┐         ┌────────────────┐
         │ InitializeAdminPassword│         │ Mostrar error  │
         │ - Genera salt          │         │ Pedir de nuevo │
         │ - Hash PBKDF2          │         └────────────────┘
         │ - Actualiza DB         │
         └────────┬───────────────┘
                  │
                  v
         ┌────────────────────────┐
         │ Mostrar LoginForm      │
         │ Usuario puede iniciar  │
         │ sesión con admin/pass  │
         └────────────────────────┘
```

## Componentes Creados

### 1. AdminPasswordInitForm.cs
**Ubicación**: `UI/Forms/AdminPasswordInitForm.cs`

**Responsabilidades**:
- Mostrar interfaz para configurar contraseña
- Validar requisitos de contraseña
- Llamar a `AuthenticationService.InitializeAdminPassword()`
- Manejar errores y mostrar mensajes apropiados

**Validaciones**:
```csharp
✓ Password no vacío
✓ Mínimo 8 caracteres
✓ Al menos una mayúscula (regex: [A-Z])
✓ Al menos un número (regex: [0-9])
✓ Password == ConfirmPassword
```

### 2. AdminPasswordInitForm.Designer.cs
**Ubicación**: `UI/Forms/AdminPasswordInitForm.Designer.cs`

**Controles UI**:
- `lblTitle`: Título "Configuración Inicial"
- `lblMessage`: Mensaje de bienvenida
- `lblPassword`: Label "Contraseña:"
- `txtPassword`: TextBox para password (PasswordChar='*')
- `lblConfirmPassword`: Label "Confirmar Contraseña:"
- `txtConfirmPassword`: TextBox para confirmar (PasswordChar='*')
- `lblRequirements`: Lista de requisitos de contraseña
- `btnInitialize`: Botón "Configurar"
- `btnCancel`: Botón "Cancelar"

### 3. Modificaciones a Program.cs

**Ubicación**: `UI/Program.cs`

**Cambios**:
```csharp
// ANTES:
Application.Run(new LoginForm(...));

// DESPUÉS:
var adminUser = userRepository.GetByUsername("admin");
if (adminUser != null && 
    adminUser.PasswordHash == "HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP")
{
    var initForm = new AdminPasswordInitForm(...);
    if (initForm.ShowDialog() != DialogResult.OK)
        return; // Usuario canceló, salir
}
Application.Run(new LoginForm(...));
```

**Beneficios**:
- Detección automática de password no inicializado
- Flujo guiado para el usuario
- Manejo de cancelación
- Mejor manejo de errores

### 4. Actualización de UI.csproj

**Cambios**:
```xml
<Compile Include="Forms\AdminPasswordInitForm.cs">
  <SubType>Form</SubType>
</Compile>
<Compile Include="Forms\AdminPasswordInitForm.Designer.cs">
  <DependentUpon>AdminPasswordInitForm.cs</DependentUpon>
</Compile>
```

## Seguridad

### Hash de Contraseñas
- **Algoritmo**: PBKDF2 (RFC 2898)
- **Iteraciones**: 10,000
- **Salt**: 32 bytes aleatorios por usuario
- **Hash**: 32 bytes

### Almacenamiento
```sql
-- En la tabla Users:
PasswordHash: Base64(PBKDF2(password, salt, 10000))
PasswordSalt: Base64(random 32 bytes)
```

### Validación
```csharp
// El mismo salt se usa para verificar:
byte[] computedHash = PBKDF2(inputPassword, storedSalt, 10000);
return computedHash == storedHash;
```

## Casos de Uso

### Caso 1: Primera Instalación
1. Usuario ejecuta la aplicación
2. Ve formulario "Configuración Inicial"
3. Ingresa contraseña: `Admin123!`
4. Confirma contraseña: `Admin123!`
5. Hace clic en "Configurar"
6. Ve mensaje de éxito
7. Ve formulario de Login
8. Inicia sesión con admin/Admin123!

### Caso 2: Usuario Cancela Inicialización
1. Usuario ejecuta la aplicación
2. Ve formulario "Configuración Inicial"
3. Hace clic en "Cancelar"
4. La aplicación se cierra
5. Próxima vez que inicie, verá el mismo formulario

### Caso 3: Contraseña No Cumple Requisitos
1. Usuario ingresa: `admin` (muy corta)
2. Sistema muestra: "La contraseña debe tener al menos 8 caracteres"
3. Usuario ingresa: `adminadmin` (sin mayúscula ni número)
4. Sistema muestra: "La contraseña debe contener al menos una letra mayúscula"
5. Usuario ingresa: `Admin` (sin número)
6. Sistema muestra: "La contraseña debe contener al menos un número"
7. Usuario ingresa: `Admin123!` ✓
8. Configuración exitosa

### Caso 4: Contraseñas No Coinciden
1. Usuario ingresa password: `Admin123!`
2. Usuario ingresa confirm: `Admin321!`
3. Sistema muestra: "Las contraseñas no coinciden"
4. Campo de confirmación se limpia
5. Usuario debe ingresar nuevamente

## Reinicialización Manual

Si un administrador necesita reinicializar la contraseña del admin:

```sql
USE StockManagerDB;
GO

UPDATE Users 
SET PasswordHash = 'HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
    PasswordSalt = 'SALT_PLACEHOLDER_WILL_BE_GENERATED_BY_APP'
WHERE Username = 'admin';
GO
```

La próxima vez que se inicie la aplicación, volverá a mostrar el formulario de inicialización.

## Localización

El formulario soporta múltiples idiomas a través de `ILocalizationService`:

**Claves utilizadas**:
- `AdminInit.Title`: Título del formulario
- `AdminInit.Header`: Encabezado
- `AdminInit.Message`: Mensaje de bienvenida
- `AdminInit.Requirements`: Lista de requisitos
- `AdminInit.PasswordRequired`: Error password vacío
- `AdminInit.PasswordTooShort`: Error password corto
- `AdminInit.PasswordNeedsUppercase`: Error falta mayúscula
- `AdminInit.PasswordNeedsNumber`: Error falta número
- `AdminInit.PasswordsDoNotMatch`: Error passwords no coinciden
- `AdminInit.Success`: Mensaje de éxito
- `AdminInit.Error`: Error genérico
- `Common.Password`: Label "Contraseña"
- `Common.Cancel`: Botón "Cancelar"
- `Common.Validation`: Título diálogo validación

**Fallbacks**: Si no hay traducción, usa texto en español por defecto.

## Testing Manual

Para probar la funcionalidad:

1. **Configurar base de datos** (ver SETUP.md)
2. **Ejecutar scripts SQL** para crear tablas y datos semilla
3. **Compilar solución** en Visual Studio
4. **Ejecutar UI.exe**
5. **Verificar que aparece** AdminPasswordInitForm
6. **Probar validaciones**:
   - Password vacío
   - Password corto (<8)
   - Password sin mayúscula
   - Password sin número
   - Passwords no coinciden
7. **Configurar password válido** (ej: Admin123!)
8. **Verificar mensaje de éxito**
9. **Verificar que aparece** LoginForm
10. **Iniciar sesión** con admin/Admin123!

## Archivos Modificados

```
UI/
├── Forms/
│   ├── AdminPasswordInitForm.cs         [NUEVO]
│   └── AdminPasswordInitForm.Designer.cs [NUEVO]
├── Program.cs                            [MODIFICADO]
└── UI.csproj                             [MODIFICADO]

README.md                                 [MODIFICADO]
SETUP.md                                  [MODIFICADO]
```

## Próximos Pasos

La funcionalidad está completa. El usuario ahora puede:

✅ Iniciar la aplicación por primera vez
✅ Ver formulario de configuración automáticamente
✅ Configurar contraseña del admin con validaciones
✅ Iniciar sesión con las credenciales configuradas
✅ Reinicializar password manualmente si es necesario

**No se requieren cambios adicionales** para resolver el problema planteado.
