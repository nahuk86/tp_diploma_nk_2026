# Stock Manager - Sistema de Gestión de Inventario

## Descripción
Sistema de gestión de inventario para accesorios de celulares (fundas, carcasas, protectores de pantalla, parlantes, etc.) desarrollado en .NET Framework 4.8 con WinForms.

## Arquitectura

El sistema está organizado en capas siguiendo principios de arquitectura limpia:

```
┌─────────────────────────────────────────┐
│           UI (WinForms)                 │  ← Presentación
├─────────────────────────────────────────┤
│         BLL (Business Logic)            │  ← Lógica de Negocio
├─────────────────────────────────────────┤
│    SERVICES (Cross-Cutting Concerns)    │  ← Servicios Transversales
├─────────────────────────────────────────┤
│       DAO (Data Access Objects)         │  ← Acceso a Datos
├─────────────────────────────────────────┤
│           DOMAIN (Entities)             │  ← Dominio
└─────────────────────────────────────────┘
```

## Proyectos de la Solución

### 1. DOMAIN
**Propósito**: Contiene las entidades del dominio, enumeraciones y contratos (interfaces).

**Contenido Implementado**:
- **Entidades**:
  - `User`: Usuario del sistema
  - `Role`: Roles para RBAC
  - `Permission`: Permisos granulares
  - `Product`: Productos (accesorios)
  - `Warehouse`: Almacenes
  - `Stock`: Inventario actual por producto/almacén
  - `StockMovement`: Cabecera de movimientos de stock
  - `StockMovementLine`: Líneas de detalle de movimientos
  - `AuditLog`: Registro de auditoría

- **Enums**:
  - `MovementType`: IN, OUT, TRANSFER, ADJUSTMENT
  - `LogLevel`: DEBUG, INFO, WARNING, ERROR, FATAL
  - `AuditAction`: INSERT, UPDATE, DELETE

- **Contratos (Interfaces)**:
  - `IRepository<T>`: Repositorio base genérico
  - `IUserRepository`
  - `IRoleRepository`
  - `IPermissionRepository`
  - `IProductRepository`
  - `IWarehouseRepository`
  - `IStockRepository`
  - `IStockMovementRepository`
  - `IAuditLogRepository`

### 2. SERVICES
**Propósito**: Servicios cross-cutting (logging, autenticación, autorización, localización, manejo de errores).

**Contenido Implementado**:
- **Interfaces**:
  - `ILogService`: Servicio de logging
  - `IAuthenticationService`: Autenticación con hash+salt
  - `IAuthorizationService`: Autorización basada en permisos (RBAC)
  - `ILocalizationService`: Multi-idioma (ES/EN)
  - `IErrorHandlerService`: Manejo centralizado de errores

- **Implementaciones**:
  - `FileLogService`: Logging a archivo con rolling diario
  - `AuthenticationService`: Hash de contraseñas con PBKDF2
  - `AuthorizationService`: Verificación de permisos por usuario
  - `LocalizationService`: Traducciones desde DB o fallback a memoria
  - `ErrorHandlerService`: Mensajes amigables de error
  - `SessionContext`: Contexto de sesión del usuario actual

### 3. DAO (Data Access)
**Propósito**: Acceso a datos utilizando ADO.NET puro (sin Entity Framework).

**Contenido Implementado**:
- **Helpers**:
  - `DatabaseHelper`: Helper para conexiones y operaciones SQL

- **Repositories** (Implementaciones):
  - `UserRepository`: CRUD de usuarios + gestión de roles
  - `RoleRepository`: CRUD de roles + gestión de permisos
  - `PermissionRepository`: CRUD de permisos + consulta por usuario
  - `ProductRepository`: CRUD de productos + búsqueda

**Pendiente de Implementar**:
- `WarehouseRepository`
- `StockRepository`
- `StockMovementRepository`
- `AuditLogRepository`

### 4. BLL (Business Logic Layer)
**Propósito**: Lógica de negocio, validaciones, orquestación.

**Estado**: Implementación mayormente completa.

**Servicios Implementados** ✅:
- `ProductService` ✅: Validaciones de productos (SKU único, precio > 0, etc.)
- `WarehouseService` ✅: Gestión de almacenes (código único, validaciones)
- `UserService` ✅: Validaciones de usuarios (username único, formato email, cambio de contraseña)
- `StockMovementService` ✅: Lógica completa de movimientos con validaciones y actualización automática de stock

**Servicios Pendientes**:
- `RoleService`: Gestión de roles y asignación de permisos

### 5. UI (WinForms)
**Propósito**: Interfaz de usuario.

**Estado**: Formularios principales implementados.

**Forms Implementados** ✅:
- `LoginForm` ✅: Autenticación de usuarios
- `AdminPasswordInitForm` ✅: Inicialización de contraseña admin
- `MainForm` (Form1) ✅: MDI Container con menú basado en permisos
- `UsersForm` ✅: ABM de usuarios con cambio de contraseña
- `ProductsForm` ✅: ABM de productos con búsqueda
- `WarehousesForm` ✅: ABM de almacenes
- `StockQueryForm` ✅: Consulta de stock actual con filtros
- `RolesForm` ✅: ABM de roles con asignación de permisos
- `StockMovementForm` ✅: Registro de movimientos (entrada, salida, transferencia, ajuste) con actualización automática de stock

**Forms Pendientes**:
- Ninguno - Todas las funcionalidades principales implementadas

## Base de Datos

### Configuración

El sistema utiliza SQL Server (LocalDB o SQL Express).

**Connection String** (en App.config):
```xml
<connectionStrings>
  <add name="StockManagerDB" 
       connectionString="Server=(localdb)\MSSQLLocalDB;Database=StockManagerDB;Integrated Security=true;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Scripts SQL

Los scripts están en la carpeta `Database/`:

1. **01_CreateSchema.sql**: Crea todas las tablas, índices y relaciones
2. **02_SeedData.sql**: Inserta datos semilla (roles, permisos, productos, almacenes)
3. **03_UpdatePermissions.sql**: (Opcional) Actualiza permisos en bases de datos existentes

**Ejecutar en este orden**:
```sql
-- Para una base de datos nueva:
01_CreateSchema.sql
02_SeedData.sql

-- Para actualizar una base de datos existente:
03_UpdatePermissions.sql
```

> **Nota**: Si ya tiene una base de datos creada con versiones anteriores del seed data, ejecute `03_UpdatePermissions.sql` para habilitar todas las funcionalidades de movimientos de stock para el rol WarehouseOperator.

### Tablas Principales

**Seguridad**:
- `Users`: Usuarios del sistema
- `Roles`: Roles para RBAC
- `Permissions`: Permisos granulares
- `UserRoles`: Relación N:M usuarios-roles
- `RolePermissions`: Relación N:M roles-permisos

**Inventario**:
- `Products`: Productos (accesorios)
- `Warehouses`: Almacenes
- `Stock`: Stock actual por producto/almacén
- `StockMovements`: Cabecera de movimientos
- `StockMovementLines`: Líneas de detalle

**Auditoría**:
- `AuditLog`: Registro de cambios
- `AppLog`: Logs de aplicación
- `Translations`: Traducciones multi-idioma

## Usuario por Defecto

**Importante**: En la primera ejecución, debe inicializar la contraseña del admin.

```
Username: admin
Password: (debe ser configurado en primera ejecución)
```

El sistema detectará automáticamente que el password no está configurado y mostrará un formulario de "Configuración Inicial" donde podrá:
- Configurar la contraseña del administrador
- La contraseña debe tener mínimo 8 caracteres, una mayúscula y un número
- Ejemplo de contraseña válida: `Admin123!`

Una vez configurada la contraseña, podrá iniciar sesión normalmente.

## Funcionalidades Implementadas

### ✅ Completadas

- [x] Modelo de datos SQL con todas las tablas
- [x] Datos semilla (roles, permisos, productos de ejemplo)
- [x] Entidades de dominio
- [x] Contratos/interfaces del repositorio
- [x] Servicio de logging a archivo con rolling diario
- [x] Servicio de autenticación con hash+salt (PBKDF2)
- [x] Servicio de autorización (RBAC)
- [x] Servicio de localización (ES/EN)
- [x] Servicio de manejo de errores
- [x] Repositorios: User, Role, Permission, Product, Warehouse, Stock, StockMovement, AuditLog
- [x] Helper de base de datos
- [x] Servicios BLL: ProductService, WarehouseService, UserService, StockMovementService
- [x] Formulario principal (MainForm) con menú MDI y control de permisos
- [x] LoginForm y AdminPasswordInitForm
- [x] ProductsForm con búsqueda y CRUD completo
- [x] WarehousesForm con CRUD completo
- [x] UsersForm con gestión de usuarios y cambio de contraseña
- [x] RolesForm con gestión de roles y permisos
- [x] StockQueryForm para consultar inventario actual
- [x] StockMovementForm para registrar movimientos con actualización automática de stock

### 🔲 Pendientes

- [ ] Implementar control de permisos en runtime (cambio de idioma)
- [ ] Agregar reportes (PDF, Excel)

## Permisos del Sistema

El sistema define permisos granulares por módulo:

### Users
- `Users.View`
- `Users.Create`
- `Users.Edit`
- `Users.Delete`

### Roles
- `Roles.View`
- `Roles.Create`
- `Roles.Edit`
- `Roles.Delete`
- `Roles.AssignPermissions`

### Products
- `Products.View`
- `Products.Create`
- `Products.Edit`
- `Products.Delete`

### Warehouses
- `Warehouses.View`
- `Warehouses.Create`
- `Warehouses.Edit`
- `Warehouses.Delete`

### Stock
- `Stock.View`
- `Stock.Receive`
- `Stock.Issue`
- `Stock.Transfer`
- `Stock.Adjust`

### Audit
- `Audit.View`

### Reports
- `Reports.View`

## Roles Pre-definidos

1. **Administrator**: Todos los permisos
2. **WarehouseManager**: Gestión completa de stock, productos y almacenes
3. **WarehouseOperator**: Ejecución de movimientos de stock
4. **Viewer**: Solo lectura

## Configuración

### App.config

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <connectionStrings>
    <add name="StockManagerDB" 
         connectionString="Server=(localdb)\MSSQLLocalDB;Database=StockManagerDB;Integrated Security=true;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
  
  <appSettings>
    <add key="LogDirectory" value="Logs" />
    <add key="LogFilePrefix" value="StockManager" />
    <add key="DefaultLanguage" value="es" />
  </appSettings>
</configuration>
```

## Cómo Ejecutar

### Requisitos Previos

- .NET Framework 4.8
- SQL Server LocalDB o SQL Express
- Visual Studio 2017 o superior (recomendado 2022)

### Pasos

1. **Crear la base de datos**:
   - Abrir SQL Server Management Studio
   - Conectar a `(localdb)\MSSQLLocalDB` o su instancia SQL
   - Ejecutar `Database/01_CreateSchema.sql`
   - Ejecutar `Database/02_SeedData.sql`

2. **Configurar Connection String**:
   - Editar `UI/App.config`
   - Ajustar el connection string según su configuración

3. **Compilar la solución**:
   - Abrir `tp_diploma_nk_2026.sln` en Visual Studio
   - Compilar (Build > Build Solution)

4. **Ejecutar la aplicación**:
   - Establecer `UI` como proyecto de inicio
   - Presionar F5 o clic en "Start"

5. **Primera ejecución**:
   - El sistema detectará que debe inicializar la contraseña admin
   - Seguir las instrucciones en pantalla

## Decisiones Arquitectónicas

### ¿Por qué ADO.NET y no Entity Framework?

El requisito específico era usar ADO.NET puro para tener control total sobre:
- Queries SQL explícitos
- Transacciones manuales
- Optimización de rendimiento
- Sin overhead de ORM

### ¿Por qué Soft Delete?

- Mantener historial completo
- Cumplir requisitos de auditoría
- Permitir recuperación de datos
- No romper relaciones existentes

### Logging

- **Archivo**: Rolling diario automático
- **SQL** (opcional): Tabla AppLog para consultas
- **Niveles**: DEBUG, INFO, WARNING, ERROR, FATAL
- **Información**: Timestamp, usuario, máquina, excepción completa

### Seguridad

- **Password Hashing**: PBKDF2 con 10,000 iteraciones
- **Salt**: Aleatorio de 32 bytes por usuario
- **Permisos**: Granulares y por módulo
- **RBAC**: Roles asignables a usuarios
- **Soft Delete**: No exposición de datos eliminados

## Próximos Pasos

Para completar el sistema:

1. **Implementar repositorios faltantes**:
   - WarehouseRepository
   - StockRepository
   - StockMovementRepository
   - AuditLogRepository

2. **Crear capa BLL**:
   - Servicios de negocio con validaciones
   - Orquestación de transacciones
   - Reglas de negocio complejas

3. **Desarrollar UI**:
   - LoginForm funcional
   - MainForm con menú según permisos
   - Forms CRUD para cada entidad
   - Implementar multi-idioma en UI
   - Binding con datos

4. **Testing**:
   - Probar flujos completos
   - Validar transacciones
   - Verificar permisos

## Contacto y Soporte

Para preguntas o issues, consultar con el equipo de desarrollo.

## Licencia

Proyecto académico - Universidad/Institución
