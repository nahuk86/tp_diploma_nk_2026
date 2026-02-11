# Guía de Formularios Implementados

## Descripción General

Se han implementado los formularios principales para soportar las funcionalidades existentes del sistema de gestión de inventario. Estos formularios permiten a los usuarios interactuar con el sistema de manera intuitiva siguiendo el patrón MDI (Multiple Document Interface).

## Formularios Implementados

### 1. MainForm (Form1.cs)

**Descripción**: Formulario principal MDI que actúa como contenedor para todos los demás formularios.

**Características**:
- Sistema de menú completo con las siguientes secciones:
  - **Archivo**: Cerrar Sesión, Salir
  - **Administración**: Usuarios, Roles
  - **Inventario**: Productos, Almacenes
  - **Operaciones**: Movimientos, Consultar Stock
  - **Configuración**: Idioma (Español/English)
  - **Ayuda**: Acerca de...

- Control de permisos basado en roles (RBAC)
- Los elementos del menú se habilitan/deshabilitan según los permisos del usuario
- Soporte multi-idioma (Español/Inglés)
- Barra de estado mostrando información del usuario

**Permisos Requeridos**:
- Para cada opción del menú se verifica el permiso correspondiente
- Ejemplo: `Products.View`, `Warehouses.View`, `Stock.View`

### 2. ProductsForm

**Descripción**: Formulario CRUD completo para la gestión de productos (accesorios de celulares).

**Características**:
- Lista de productos activos en DataGridView
- Búsqueda en tiempo real por SKU, nombre o categoría
- Formulario de detalles con los siguientes campos:
  - SKU (único, requerido)
  - Nombre (requerido)
  - Descripción
  - Categoría (combo box con categorías predefinidas)
  - Precio unitario
  - Stock mínimo
- Operaciones CRUD completas:
  - **Nuevo**: Crear un nuevo producto
  - **Editar**: Modificar un producto existente
  - **Eliminar**: Soft delete del producto
  - **Guardar**: Guardar cambios
  - **Cancelar**: Descartar cambios

**Validaciones**:
- SKU requerido y único
- Nombre requerido
- Categoría requerida
- Precio debe ser número positivo
- Stock mínimo debe ser número entero positivo

**Permisos Requeridos**:
- `Products.View`: Ver productos
- `Products.Create`: Crear productos
- `Products.Edit`: Editar productos
- `Products.Delete`: Eliminar productos

**Categorías Disponibles**:
- Fundas
- Carcasas
- Protectores de Pantalla
- Cables
- Cargadores
- Auriculares
- Parlantes
- Soportes
- Baterías
- Otros

### 3. WarehousesForm

**Descripción**: Formulario CRUD para la gestión de almacenes.

**Características**:
- Lista de almacenes activos en DataGridView
- Formulario de detalles con los siguientes campos:
  - Código (único, requerido, máx. 20 caracteres)
  - Nombre (requerido, máx. 100 caracteres)
  - Dirección (máx. 200 caracteres)
- Operaciones CRUD completas:
  - **Nuevo**: Crear un nuevo almacén
  - **Editar**: Modificar un almacén existente
  - **Eliminar**: Soft delete del almacén
  - **Guardar**: Guardar cambios
  - **Cancelar**: Descartar cambios

**Validaciones**:
- Código requerido y único
- Nombre requerido

**Permisos Requeridos**:
- `Warehouses.View`: Ver almacenes
- `Warehouses.Create`: Crear almacenes
- `Warehouses.Edit`: Editar almacenes
- `Warehouses.Delete`: Eliminar almacenes

### 4. StockQueryForm

**Descripción**: Formulario de consulta de stock actual por almacén.

**Características**:
- Filtro por almacén (combo box)
- Botón "Buscar" para filtrar por almacén seleccionado
- Botón "Mostrar Todo" para ver todo el inventario
- DataGridView con las siguientes columnas:
  - SKU del producto
  - Nombre del producto
  - Nombre del almacén
  - Cantidad en stock
  - Última actualización (fecha y hora)
- Resaltado visual de productos con stock bajo (menor o igual al stock mínimo)
- Barra de estado mostrando cantidad de registros encontrados

**Permisos Requeridos**:
- `Stock.View`: Ver stock

**Funcionalidades Especiales**:
- Los productos con cantidad menor o igual al stock mínimo se resaltan en color rojo claro
- Formato de fecha/hora en español

## Servicios BLL Implementados

### WarehouseService

**Descripción**: Servicio de lógica de negocio para la gestión de almacenes.

**Métodos**:
- `GetAllWarehouses()`: Obtiene todos los almacenes
- `GetActiveWarehouses()`: Obtiene solo almacenes activos
- `GetWarehouseById(int id)`: Obtiene un almacén por ID
- `CreateWarehouse(Warehouse)`: Crea un nuevo almacén con validaciones
- `UpdateWarehouse(Warehouse)`: Actualiza un almacén existente
- `DeleteWarehouse(int id)`: Soft delete de un almacén

**Validaciones**:
- Código único y requerido
- Nombre requerido
- Longitudes máximas de campos
- Auditoría automática de cambios

## Arquitectura de los Formularios

Todos los formularios siguen un patrón consistente:

### Estructura
```
┌─────────────────────────────────────┐
│         GroupBox: Lista             │
│  ┌───────────────────────────────┐  │
│  │      DataGridView             │  │
│  └───────────────────────────────┘  │
│  [Nuevo] [Editar] [Eliminar]        │
└─────────────────────────────────────┘
┌─────────────────────────────────────┐
│      GroupBox: Detalles             │
│  Campo 1: [____________]            │
│  Campo 2: [____________]            │
│  ...                                │
│              [Guardar] [Cancelar]   │
└─────────────────────────────────────┘
```

### Flujo de Trabajo

1. **Modo Visualización** (inicial):
   - GroupBox de Lista: Habilitado
   - GroupBox de Detalles: Deshabilitado
   - Botones: Nuevo, Editar, Eliminar habilitados

2. **Modo Edición**:
   - GroupBox de Lista: Deshabilitado
   - GroupBox de Detalles: Habilitado
   - Botones: Guardar, Cancelar habilitados

### Inyección de Dependencias

Los formularios utilizan un patrón de inyección manual de dependencias:
- Servicios se crean en el constructor del formulario
- Repositorios se pasan a los servicios
- Servicios transversales (logging, localización) se comparten

## Localización (Multi-idioma)

Todos los formularios soportan español e inglés:
- Etiquetas de campos
- Títulos de formularios
- Mensajes de validación
- Mensajes de confirmación
- Encabezados de columnas

El cambio de idioma se realiza desde el menú principal: **Configuración > Idioma**

## Seguridad

### Control de Permisos
- Cada operación verifica permisos antes de ejecutarse
- Los botones se habilitan/deshabilitan según permisos
- Mensajes de error amigables cuando no hay permisos

### Auditoría
- Todos los cambios se registran en la tabla AuditLog
- Se registra: usuario, fecha, acción, tabla, campo, valores anterior/nuevo
- Los logs también se escriben en archivos

### Soft Delete
- Las eliminaciones son lógicas (IsActive = 0)
- No se pierde información
- Se mantiene integridad referencial

## Uso de los Formularios

### Ejemplo: Crear un Nuevo Producto

1. Desde el menú principal, seleccionar **Inventario > Productos**
2. Hacer clic en el botón **Nuevo**
3. Completar los campos requeridos:
   - SKU: "FUND-001"
   - Nombre: "Funda Silicona iPhone 13"
   - Descripción: "Funda de silicona suave color negro"
   - Categoría: Seleccionar "Fundas"
   - Precio: "15.50"
   - Stock Mínimo: "10"
4. Hacer clic en **Guardar**
5. El producto aparecerá en la lista

### Ejemplo: Consultar Stock

1. Desde el menú principal, seleccionar **Operaciones > Consultar Stock**
2. Seleccionar un almacén del combo box (o dejar "Todos")
3. Hacer clic en **Buscar**
4. Ver los resultados en la grilla
5. Los productos con stock bajo aparecerán resaltados en rojo

## Formularios Pendientes de Implementación

Los siguientes formularios están pendientes y aparecen como "En desarrollo" en el menú:

1. **UsersForm**: Gestión de usuarios del sistema
   - CRUD de usuarios
   - Asignación de roles
   - Cambio de contraseña

2. **RolesForm**: Gestión de roles y permisos
   - CRUD de roles
   - Asignación de permisos a roles

3. **StockMovementForm**: Registro de movimientos de stock
   - Entrada de mercadería
   - Salida de mercadería
   - Transferencias entre almacenes
   - Ajustes de inventario

## Notas Técnicas

### Tecnología
- .NET Framework 4.8
- WinForms
- ADO.NET (sin Entity Framework)
- Patrón MDI
- Arquitectura en capas

### Requisitos
- Windows con .NET Framework 4.8
- SQL Server LocalDB o SQL Express
- Visual Studio 2017 o superior para desarrollo

### Base de Datos
- Todos los datos se almacenan en SQL Server
- Las operaciones usan parámetros SQL (prevención de SQL Injection)
- ProductService y WarehouseService manejan la lógica de negocio
- Los repositorios manejan el acceso a datos

## Próximos Pasos

1. Implementar los formularios pendientes (Users, Roles, StockMovement)
2. Implementar servicios BLL adicionales (UserService, RoleService, StockMovementService)
3. Agregar reportes (PDF, Excel)
4. Implementar dashboard con KPIs
5. Agregar búsqueda avanzada con filtros
6. Implementar validaciones adicionales del lado del servidor

## Soporte

Para preguntas o problemas con los formularios, consultar:
- README.md: Información general del proyecto
- IMPLEMENTATION.md: Detalles de implementación
- Logs en carpeta Logs/: Para depuración
