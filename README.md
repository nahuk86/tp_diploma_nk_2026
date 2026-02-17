# Stock Manager - Sistema de Gestión de Inventario y Ventas

## Descripción

Sistema integral de gestión de inventario y ventas para accesorios de celulares (fundas, carcasas, protectores de pantalla, parlantes, etc.) desarrollado en .NET Framework 4.8 con WinForms. 

El sistema incluye gestión completa de productos, almacenes, clientes, ventas, movimientos de stock, usuarios, roles y permisos, con soporte multi-idioma y auditoría completa.

## Arquitectura

El sistema está organizado en capas siguiendo principios de arquitectura limpia (Clean Architecture) y separación de responsabilidades:

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

### Descripción de Capas

**DOMAIN (Dominio)**:
- Capa más interna, sin dependencias
- Define entidades de negocio, enumeraciones e interfaces
- Modelos de datos puros (POCO - Plain Old CLR Objects)
- Contratos de repositorios (interfaces)

**DAO (Data Access Objects)**:
- Implementa el acceso a datos usando ADO.NET puro
- Ejecuta operaciones CRUD contra SQL Server
- Manejo de transacciones y conexiones
- Prevención de SQL Injection con parámetros
- Depende solo de DOMAIN

**SERVICES (Servicios Transversales)**:
- Funcionalidades cross-cutting que se usan en todas las capas
- Autenticación, autorización, logging, localización, manejo de errores
- Sin lógica de negocio específica
- Reutilizables en cualquier parte de la aplicación

**BLL (Business Logic Layer)**:
- Implementa las reglas de negocio
- Validaciones de datos (formato, unicidad, rangos)
- Orquestación de operaciones complejas
- Transacciones que involucran múltiples repositorios
- Depende de DAO, SERVICES y DOMAIN

**UI (User Interface)**:
- Interfaz de usuario con Windows Forms
- 15 formularios para todas las funcionalidades
- MDI Container con menú dinámico según permisos
- Binding de datos con grillas y controles
- Validaciones de entrada del usuario
- Depende de BLL, SERVICES y DOMAIN

### Principios Aplicados

- **Separation of Concerns**: Cada capa tiene una responsabilidad específica
- **Dependency Inversion**: Las capas superiores no conocen detalles de implementación de las inferiores
- **Single Responsibility**: Cada clase tiene una única razón para cambiar
- **DRY (Don't Repeat Yourself)**: Código reutilizable en SERVICES y helpers
- **SOLID**: Principios de diseño orientado a objetos aplicados consistentemente

## Proyectos de la Solución

### 1. DOMAIN
**Propósito**: Contiene las entidades del dominio, enumeraciones y contratos (interfaces).

**Contenido Implementado**:
- **Entidades** (12):
  - `User`: Usuario del sistema
  - `Role`: Roles para RBAC
  - `Permission`: Permisos granulares
  - `Product`: Productos (accesorios)
  - `Warehouse`: Almacenes
  - `Stock`: Inventario actual por producto/almacén
  - `StockMovement`: Cabecera de movimientos de stock
  - `StockMovementLine`: Líneas de detalle de movimientos
  - `Client`: Clientes del negocio
  - `Sale`: Ventas realizadas
  - `SaleLine`: Líneas de detalle de ventas
  - `AuditLog`: Registro de auditoría

- **Enums** (3):
  - `MovementType`: IN (entrada), OUT (salida), TRANSFER (transferencia), ADJUSTMENT (ajuste)
  - `LogLevel`: DEBUG, INFO, WARNING, ERROR, FATAL
  - `AuditAction`: INSERT, UPDATE, DELETE

- **Contratos (Interfaces)** (11):
  - `IRepository<T>`: Repositorio base genérico
  - `IUserRepository`
  - `IRoleRepository`
  - `IPermissionRepository`
  - `IProductRepository`
  - `IWarehouseRepository`
  - `IStockRepository`
  - `IStockMovementRepository`
  - `IClientRepository`
  - `ISaleRepository`
  - `IAuditLogRepository`
  - `IReportRepository`

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

- **Repositories** (11 implementados):
  - `UserRepository`: CRUD de usuarios + gestión de roles
  - `RoleRepository`: CRUD de roles + gestión de permisos
  - `PermissionRepository`: CRUD de permisos + consulta por usuario
  - `ProductRepository`: CRUD de productos + búsqueda
  - `WarehouseRepository`: CRUD de almacenes + código único
  - `StockRepository`: Consulta y actualización de stock
  - `StockMovementRepository`: Registro de movimientos con transacciones
  - `ClientRepository`: CRUD de clientes + búsqueda
  - `SaleRepository`: Registro de ventas con líneas de detalle
  - `AuditLogRepository`: Registro de auditoría
  - `ReportRepository`: Consultas para reportes

### 4. BLL (Business Logic Layer)
**Propósito**: Lógica de negocio, validaciones, orquestación.

**Servicios Implementados** (8):
- `ProductService` ✅: Validaciones de productos (SKU único, precio > 0, etc.)
- `WarehouseService` ✅: Gestión de almacenes (código único, validaciones)
- `UserService` ✅: Validaciones de usuarios (username único, formato email, cambio de contraseña)
- `RoleService` ✅: Gestión de roles y asignación de permisos
- `StockMovementService` ✅: Lógica completa de movimientos con validaciones y actualización automática de stock
- `ClientService` ✅: Validaciones de clientes (email único, teléfono)
- `SaleService` ✅: Registro de ventas con validación de stock y cálculo de totales
- `ReportService` ✅: Generación de reportes con control de acceso

### 5. UI (WinForms)
**Propósito**: Interfaz de usuario.

**Forms Implementados** (15):
- `LoginForm` ✅: Autenticación de usuarios
- `AdminPasswordInitForm` ✅: Inicialización de contraseña admin
- `MainForm` (Form1) ✅: MDI Container con menú basado en permisos
- `UsersForm` ✅: ABM de usuarios con cambio de contraseña
- `UserRolesForm` ✅: Asignación de roles a usuarios
- `RolesForm` ✅: ABM de roles
- `RolePermissionsForm` ✅: Asignación de permisos a roles
- `ProductsForm` ✅: ABM de productos con búsqueda
- `WarehousesForm` ✅: ABM de almacenes
- `ClientsForm` ✅: ABM de clientes
- `StockQueryForm` ✅: Consulta de stock actual con filtros
- `StockMovementForm` ✅: Registro de movimientos (entrada, salida, transferencia, ajuste)
- `SalesForm` ✅: Registro de ventas con líneas de detalle
- `ReportsForm` ✅: Visualización de reportes con filtros
- `UserManualForm` ✅: Manual de usuario integrado

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

El sistema utiliza 15 tablas organizadas en grupos funcionales:

#### Seguridad y Control de Acceso
- **Users**: Usuarios del sistema
  - Campos principales: UserId, Username, PasswordHash, PasswordSalt, FullName, Email, IsActive
  - Autenticación con PBKDF2 + salt único por usuario
  - Soft delete con campo IsActive
  
- **Roles**: Roles para RBAC
  - Campos principales: RoleId, RoleName, Description, IsActive
  - 5 roles predefinidos: Administrator, WarehouseManager, WarehouseOperator, Viewer, Seller
  
- **Permissions**: Permisos granulares
  - Campos principales: PermissionId, PermissionCode, PermissionName, Module, Description
  - 31 permisos organizados en 8 módulos
  
- **UserRoles**: Relación N:M usuarios-roles
  - Permite asignación de múltiples roles a un usuario
  - Auditoría: AssignedAt, AssignedBy
  
- **RolePermissions**: Relación N:M roles-permisos
  - Define qué permisos tiene cada rol
  - Auditoría: AssignedAt, AssignedBy

#### Gestión de Productos e Inventario
- **Products**: Catálogo de productos
  - Campos principales: ProductId, SKU (único), Name, Description, Category, UnitPrice, MinStockLevel
  - Validación de SKU único
  - Alertas de stock mínimo
  
- **Warehouses**: Almacenes físicos
  - Campos principales: WarehouseId, Code (único), Name, Address, IsActive
  - Gestión de múltiples ubicaciones
  
- **Stock**: Inventario actual por producto/almacén
  - Campos principales: StockId, ProductId, WarehouseId, CurrentQuantity
  - Actualización automática con cada movimiento
  - Índice único: (ProductId, WarehouseId)
  
- **StockMovements**: Cabecera de movimientos de stock
  - Campos principales: MovementId, MovementNumber (auto-generado), MovementType, MovementDate
  - Tipos: IN (entrada), OUT (salida), TRANSFER (transferencia), ADJUSTMENT (ajuste)
  - Relación con almacenes origen y destino
  
- **StockMovementLines**: Líneas de detalle de movimientos
  - Campos principales: LineId, MovementId, ProductId, Quantity
  - Trazabilidad completa de cada movimiento

#### Gestión Comercial
- **Clients**: Clientes del negocio
  - Campos principales: ClientId, FirstName, LastName, Email, Phone, Address, IsActive
  - Información de contacto completa
  - Historial de compras
  
- **Sales**: Registro de ventas
  - Campos principales: SaleId, SaleNumber, ClientId, SellerId, SaleDate, TotalAmount
  - Relación con cliente y vendedor
  - Cálculo automático de totales
  
- **SaleLines**: Líneas de detalle de ventas
  - Campos principales: SaleLineId, SaleId, ProductId, Quantity, UnitPrice, Subtotal
  - Integración con stock (descuento automático)

#### Auditoría y Logs
- **AuditLog**: Registro de cambios en datos
  - Campos principales: AuditId, TableName, RecordId, Action (INSERT/UPDATE/DELETE), OldValues, NewValues
  - Registro automático de quién, qué y cuándo
  - Preservación de valores anteriores y nuevos
  
- **AppLog**: Logs de aplicación
  - Campos principales: LogId, LogLevel, Message, Exception, Username, MachineName
  - Niveles: DEBUG, INFO, WARNING, ERROR, FATAL
  - Rolling diario automático
  
- **Translations**: Traducciones multi-idioma
  - Campos principales: TranslationId, LanguageCode, TranslationKey, TranslationValue
  - Soporte para ES/EN
  - Fallback a valores en memoria

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

## Funcionalidades del Sistema

### Gestión de Inventario
- ✅ **Productos**: ABM (Alta, Baja, Modificación) completo con validación de SKU único, búsqueda por nombre/SKU/categoría
- ✅ **Almacenes**: ABM de almacenes con código único y direcciones
- ✅ **Stock**: Consulta de inventario actual por producto y almacén, alertas de stock bajo
- ✅ **Movimientos de Stock**: 
  - Entrada (IN): Recepción de mercadería
  - Salida (OUT): Despacho de productos
  - Transferencia (TRANSFER): Entre almacenes
  - Ajuste (ADJUSTMENT): Correcciones de inventario
  - Actualización automática de stock en tiempo real
  - Trazabilidad completa con número de movimiento y líneas de detalle

### Gestión de Ventas
- ✅ **Clientes**: ABM de clientes con información de contacto
- ✅ **Ventas**: Registro de ventas con líneas de detalle, cálculo automático de totales
- ✅ **Integración con Stock**: Descuento automático de stock al registrar ventas
- ✅ **Tracking de vendedores**: Asignación de ventas a usuarios vendedores

### Gestión de Usuarios y Seguridad
- ✅ **Usuarios**: ABM de usuarios con validación de username único y email
- ✅ **Roles**: Sistema RBAC (Role-Based Access Control) con 5 roles predefinidos
- ✅ **Permisos**: 31 permisos granulares organizados por módulos
- ✅ **Autenticación**: Hash seguro de contraseñas con PBKDF2 (10,000 iteraciones) + salt único
- ✅ **Autorización**: Control de acceso basado en permisos en toda la UI y backend
- ✅ **Asignación de Roles**: Interfaz para asignar múltiples roles a usuarios
- ✅ **Gestión de Permisos**: Interfaz para asignar permisos a roles

### Reportes y Consultas
- ✅ **Reportes de Ventas**: Ingresos por fecha, productos más vendidos, ranking de clientes
- ✅ **Reportes de Inventario**: Stock actual, movimientos por período
- ✅ **Reportes de Desempeño**: Rendimiento por vendedor, análisis por categoría
- ✅ **Filtros Avanzados**: Por fecha, almacén, producto, categoría, cliente
- ✅ **Control de Acceso**: Segmentación de reportes según rol del usuario

### Características Transversales
- ✅ **Multi-idioma**: Soporte para Español e Inglés con cambio en tiempo real
- ✅ **Auditoría**: Registro completo de todas las operaciones (quién, qué, cuándo)
- ✅ **Logging**: Sistema de logs con niveles (DEBUG, INFO, WARNING, ERROR, FATAL) y rolling diario
- ✅ **Manejo de Errores**: Mensajes amigables al usuario con logging detallado para soporte
- ✅ **Soft Delete**: Eliminación lógica que preserva integridad referencial
- ✅ **Validaciones**: Validaciones de negocio en todas las operaciones
- ✅ **MDI Interface**: Interfaz multi-documento con menú dinámico según permisos

### Estado de Implementación

**Estado de Completitud**: 95% ✅

**Backend (100%)**:
- [x] Modelo de datos SQL con 15 tablas
- [x] Datos semilla (admin, 5 roles, 31 permisos, productos, almacenes)
- [x] 12 Entidades de dominio con enumeraciones
- [x] 11 Contratos/interfaces del repositorio
- [x] 6 Servicios transversales (SERVICES)
- [x] 11 Repositorios completos (DAO)
- [x] 8 Servicios de negocio (BLL)

**Frontend (100%)**:
- [x] LoginForm con autenticación
- [x] AdminPasswordInitForm para configuración inicial
- [x] MainForm (MDI) con menú basado en permisos
- [x] ProductsForm con CRUD completo
- [x] WarehousesForm con CRUD completo
- [x] ClientsForm con CRUD completo
- [x] UsersForm con gestión y cambio de contraseña
- [x] UserRolesForm para asignar roles
- [x] RolesForm con gestión de roles
- [x] RolePermissionsForm para asignar permisos
- [x] StockQueryForm para consultas de inventario
- [x] StockMovementForm para registrar movimientos
- [x] SalesForm para registrar ventas
- [x] ReportsForm con múltiples reportes
- [x] UserManualForm con guía de usuario

**Pendientes**:
- [ ] Exportación a PDF/Excel desde reportes
- [ ] Dashboard con KPIs en tiempo real
- [ ] Notificaciones de stock bajo

## Permisos del Sistema

El sistema implementa 31 permisos granulares organizados en 8 módulos funcionales. Cada permiso sigue el formato `Módulo.Acción`.

### Módulo: Users (Usuarios)
Gestión de usuarios del sistema:
- **Users.View**: Ver lista de usuarios y sus detalles
- **Users.Create**: Crear nuevos usuarios en el sistema
- **Users.Edit**: Editar información de usuarios existentes (incluye cambio de contraseña)
- **Users.Delete**: Eliminar usuarios (soft delete)

### Módulo: Roles (Roles y Permisos)
Gestión del sistema RBAC:
- **Roles.View**: Ver lista de roles y sus detalles
- **Roles.Create**: Crear nuevos roles personalizados
- **Roles.Edit**: Editar nombre y descripción de roles
- **Roles.Delete**: Eliminar roles (soft delete, protección para roles del sistema)
- **Roles.AssignPermissions**: Asignar/remover permisos de roles (gestión de accesos)

### Módulo: Products (Productos)
Gestión del catálogo de productos:
- **Products.View**: Ver catálogo de productos y búsqueda
- **Products.Create**: Agregar nuevos productos al catálogo
- **Products.Edit**: Modificar información de productos (precio, descripción, etc.)
- **Products.Delete**: Eliminar productos (soft delete)

### Módulo: Warehouses (Almacenes)
Gestión de almacenes físicos:
- **Warehouses.View**: Ver lista de almacenes y ubicaciones
- **Warehouses.Create**: Crear nuevos almacenes
- **Warehouses.Edit**: Modificar información de almacenes
- **Warehouses.Delete**: Eliminar almacenes (soft delete)

### Módulo: Clients (Clientes)
Gestión de clientes:
- **Clients.View**: Ver lista de clientes y su información
- **Clients.Create**: Registrar nuevos clientes
- **Clients.Edit**: Modificar información de clientes
- **Clients.Delete**: Eliminar clientes (soft delete)

### Módulo: Stock (Inventario)
Operaciones de inventario:
- **Stock.View**: Consultar niveles de stock actual y movimientos históricos
- **Stock.Receive**: Registrar entradas de mercadería (recepciones)
- **Stock.Issue**: Registrar salidas de mercadería (despachos)
- **Stock.Transfer**: Transferir stock entre almacenes
- **Stock.Adjust**: Realizar ajustes de inventario (correcciones)

### Módulo: Sales (Ventas)
Gestión de ventas:
- **Sales.View**: Ver historial de ventas y detalles
- **Sales.Create**: Registrar nuevas ventas
- **Sales.Edit**: Modificar ventas existentes
- **Sales.Delete**: Anular/eliminar ventas (soft delete)

### Módulo: Audit (Auditoría)
Acceso a registros de auditoría:
- **Audit.View**: Ver logs de auditoría y cambios en el sistema

### Módulo: Reports (Reportes)
Acceso a reportes e informes:
- **Reports.View**: Ver y generar reportes del sistema (ventas, inventario, desempeño)

## Roles Pre-definidos

El sistema incluye 5 roles predefinidos con permisos específicos para diferentes tipos de usuarios:

### 1. Administrator (Administrador)
**Descripción**: Acceso completo al sistema con todos los permisos.

**Permisos**: TODOS (31 permisos)
- Gestión completa de usuarios, roles y permisos
- Gestión de productos, almacenes y clientes
- Todas las operaciones de stock
- Registro y gestión de ventas
- Acceso a auditoría y reportes

**Uso típico**: 
- Gerente general o propietario del negocio
- Administrador de sistemas
- Personal de IT

**Nota**: Este rol no puede ser eliminado y al menos un usuario debe tener este rol.

---

### 2. WarehouseManager (Gerente de Almacén)
**Descripción**: Gestión completa de inventario, productos y almacenes con capacidad de supervisión.

**Permisos** (21 permisos):
- **Products**: View, Create, Edit, Delete
- **Warehouses**: View, Create, Edit, Delete
- **Clients**: View, Create, Edit, Delete
- **Stock**: View, Receive, Issue, Transfer, Adjust (todas las operaciones)
- **Audit**: View
- **Reports**: View

**No tiene acceso a**:
- Gestión de usuarios y roles
- Gestión de ventas

**Uso típico**:
- Jefe de almacén
- Supervisor de inventario
- Encargado de compras

---

### 3. WarehouseOperator (Operador de Almacén)
**Descripción**: Ejecución de movimientos de stock y consultas básicas, sin capacidad de gestión.

**Permisos** (7 permisos):
- **Products**: View (solo consulta)
- **Warehouses**: View (solo consulta)
- **Stock**: View, Receive, Issue, Transfer, Adjust (operaciones de stock)

**No tiene acceso a**:
- Creación/modificación de productos o almacenes
- Gestión de clientes
- Gestión de usuarios o roles
- Ventas
- Auditoría o reportes completos

**Uso típico**:
- Operario de almacén
- Personal de recepción/despacho
- Repositor

---

### 4. Seller (Vendedor)
**Descripción**: Enfocado en operaciones de venta y gestión de clientes.

**Permisos** (10 permisos):
- **Products**: View (consulta de catálogo)
- **Clients**: View, Create, Edit (gestión de clientes)
- **Stock**: View (consulta de disponibilidad)
- **Sales**: View, Create, Edit (operaciones de venta)
- **Reports**: View (reportes de ventas)

**No tiene acceso a**:
- Gestión de productos o almacenes
- Operaciones de stock (movimientos)
- Gestión de usuarios o roles
- Auditoría

**Uso típico**:
- Vendedor de mostrador
- Vendedor de campo
- Ejecutivo de ventas

---

### 5. Viewer (Visualizador)
**Descripción**: Acceso de solo lectura para consultas y reportes, sin capacidad de modificar datos.

**Permisos** (6 permisos):
- **Products**: View
- **Warehouses**: View
- **Clients**: View
- **Stock**: View
- **Audit**: View
- **Reports**: View

**No tiene acceso a**:
- Ninguna operación de creación, modificación o eliminación
- Operaciones de stock
- Ventas

**Uso típico**:
- Personal de auditoría
- Consultores externos
- Analistas de datos
- Gerencia que solo requiere visibilidad

---

## Asignación de Roles

### Características de la Asignación
- **Múltiples roles**: Un usuario puede tener varios roles simultáneamente
- **Permisos acumulativos**: Los permisos se suman de todos los roles asignados
- **Asignación dinámica**: Los cambios en roles/permisos se reflejan inmediatamente
- **Auditoría completa**: Toda asignación/remoción de roles se registra

### Matriz de Permisos por Rol

| Permiso | Administrator | WarehouseManager | WarehouseOperator | Seller | Viewer |
|---------|--------------|------------------|-------------------|--------|--------|
| **Users.*** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Roles.*** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Products.View** | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Products.Create/Edit/Delete** | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Warehouses.View** | ✅ | ✅ | ✅ | ❌ | ✅ |
| **Warehouses.Create/Edit/Delete** | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Clients.View** | ✅ | ✅ | ❌ | ✅ | ✅ |
| **Clients.Create/Edit** | ✅ | ✅ | ❌ | ✅ | ❌ |
| **Clients.Delete** | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Stock.View** | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Stock.Receive/Issue/Transfer/Adjust** | ✅ | ✅ | ✅ | ❌ | ❌ |
| **Sales.View** | ✅ | ❌ | ❌ | ✅ | ❌ |
| **Sales.Create/Edit** | ✅ | ❌ | ❌ | ✅ | ❌ |
| **Sales.Delete** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Audit.View** | ✅ | ✅ | ❌ | ❌ | ✅ |
| **Reports.View** | ✅ | ✅ | ❌ | ✅ | ✅ |

### Ejemplos de Asignación

**Ejemplo 1: Supervisor de Almacén con Ventas**
- Roles asignados: `WarehouseManager` + `Seller`
- Permisos resultantes: Gestión completa de almacén + capacidad de venta

**Ejemplo 2: Vendedor con Acceso a Auditoría**
- Roles asignados: `Seller` + `Viewer`
- Permisos resultantes: Ventas + lectura de auditoría y reportes extendidos

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

## Próximos Pasos (Opcional)

El sistema está completo y funcional. Las siguientes mejoras son opcionales:

### Mejoras Sugeridas

1. **Reportes Avanzados**:
   - Exportación a PDF y Excel
   - Gráficos y visualizaciones
   - Dashboard interactivo con KPIs

2. **Notificaciones**:
   - Alertas de stock bajo por email
   - Notificaciones de ventas importantes
   - Recordatorios de tareas pendientes

3. **Integraciones**:
   - API REST para integraciones externas
   - Sincronización con sistemas de facturación
   - Importación/exportación masiva de datos

4. **Seguridad Avanzada**:
   - Autenticación de dos factores (2FA)
   - Timeout de sesión automático
   - Bloqueo de cuenta tras intentos fallidos
   - Historial de cambios de contraseña

5. **Optimizaciones**:
   - Caché de consultas frecuentes
   - Paginación en todas las grillas
   - Búsqueda incremental con autocompletado
   - Índices adicionales en base de datos

## Solución de Problemas / Troubleshooting

### Error de Compilación: DLL Bloqueados
Si experimenta errores durante la compilación donde los archivos DLL no pueden ser copiados:
```
error MSB3027: No se pudo copiar "...\BLL.dll" en "bin\Debug\BLL.dll"
error MSB3021: El proceso no puede obtener acceso al archivo
```

**Solución**: La aplicación UI.exe está en ejecución y debe cerrarse antes de recompilar.

📖 **Ver guía completa**: [BUILD_TROUBLESHOOTING.md](BUILD_TROUBLESHOOTING.md)

**Solución rápida**:
1. Presione `Shift + F5` en Visual Studio para detener la depuración
2. O cierre manualmente todas las ventanas de la aplicación
3. Si persiste, finalice el proceso UI.exe desde el Administrador de Tareas

### Otros Problemas Comunes

Para más información sobre problemas específicos, consulte:
- [ERROR_HANDLING_QUICK_GUIDE.md](ERROR_HANDLING_QUICK_GUIDE.md) - Manejo de errores
- [MULTILANG_USER_GUIDE.md](MULTILANG_USER_GUIDE.md) - Configuración multiidioma
- [QUICK_START_ES.md](QUICK_START_ES.md) - Guía de inicio rápido

## Contacto y Soporte

Para preguntas o issues, consultar con el equipo de desarrollo.

## Licencia

Proyecto académico - Universidad/Institución
