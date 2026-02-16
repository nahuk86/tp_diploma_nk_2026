# Implementación de Segmentación de Acceso a Reportes

## Descripción General

Se ha implementado un sistema de control de acceso basado en roles para el módulo de reportes. Los administradores ahora pueden definir qué roles tienen acceso a ver y generar reportes.

## Cambios Implementados

### 1. Nuevo Permiso: Reports.View

Se ha creado un nuevo permiso dedicado para el acceso a reportes:

- **Código**: `Reports.View`
- **Nombre**: View Reports
- **Descripción**: View and generate reports
- **Módulo**: Reports

Este permiso reemplaza el comportamiento anterior donde el acceso a reportes estaba vinculado a los permisos `Sales.View` o `Stock.View`.

### 2. Asignación de Permisos por Rol

#### Roles con Acceso a Reportes por Defecto

Los siguientes roles tienen el permiso `Reports.View` asignado por defecto:

1. **Administrator**
   - Acceso completo a todos los módulos
   - Incluye todos los permisos, incluyendo `Reports.View`

2. **WarehouseManager**
   - Gestión completa de stock, productos y almacenes
   - Acceso a reportes para análisis de inventario

3. **Viewer**
   - Solo lectura en todos los módulos
   - Puede ver reportes sin poder modificar datos

4. **Seller**
   - Gestión de ventas y clientes
   - Acceso a reportes para seguimiento de ventas

#### Roles sin Acceso a Reportes por Defecto

- **WarehouseOperator**: Operador de almacén que ejecuta movimientos de stock
  - No tiene acceso a reportes por defecto
  - Los administradores pueden asignar este permiso si es necesario

### 3. Archivos Modificados

#### Database/02_SeedData.sql
- Agregado el permiso `Reports.View` a la tabla de permisos
- Actualizado las asignaciones de permisos para los roles:
  - WarehouseManager
  - Viewer
  - Seller

#### UI/Form1.cs
- Modificado el método `ConfigureMenuByPermissions()`
- Cambio de lógica de:
  ```csharp
  // Anterior: acceso si tiene Sales.View O Stock.View
  menuReports.Enabled = _authorizationService.HasPermission(userId, "Sales.View") ||
                        _authorizationService.HasPermission(userId, "Stock.View");
  ```
  
  A:
  ```csharp
  // Nuevo: acceso solo con Reports.View
  menuReports.Enabled = _authorizationService.HasPermission(userId, "Reports.View");
  ```

#### README.md
- Agregada documentación del nuevo permiso `Reports.View` en la sección de permisos del sistema

#### Database/04_AddReportsPermission.sql (NUEVO)
- Script de migración para bases de datos existentes
- Agrega el permiso `Reports.View` si no existe
- Asigna el permiso a los roles correspondientes
- Muestra un resumen de roles con acceso a reportes

## Cómo Usar

### Para Instalaciones Nuevas

1. Ejecutar los scripts de base de datos en orden:
   ```sql
   01_CreateSchema.sql
   02_SeedData.sql
   ```

2. El permiso `Reports.View` será creado automáticamente

### Para Bases de Datos Existentes

1. Ejecutar el script de migración:
   ```sql
   Database/04_AddReportsPermission.sql
   ```

2. El script:
   - Verifica si el permiso ya existe
   - Crea el permiso si no existe
   - Asigna el permiso a los roles apropiados
   - Muestra un resumen de la actualización

### Para Administradores

#### Asignar/Quitar Acceso a Reportes

1. Abrir la aplicación e iniciar sesión como administrador
2. Ir a **Administración > Roles**
3. Seleccionar el rol a modificar
4. Hacer clic en **Asignar Permisos**
5. Marcar o desmarcar el permiso `Reports.View`
6. Guardar los cambios

#### Verificar Acceso de un Usuario

1. Los usuarios deben cerrar sesión y volver a iniciar después de cambios en permisos
2. El menú "Reportes" solo estará habilitado si el usuario tiene el permiso `Reports.View`

## Beneficios de la Implementación

1. **Control Granular**: Los administradores pueden controlar específicamente quién tiene acceso a reportes

2. **Seguridad Mejorada**: El acceso a datos sensibles en reportes está mejor protegido

3. **Flexibilidad**: Fácil de asignar/quitar permisos de reportes sin afectar otros módulos

4. **Auditoría**: Todos los cambios de permisos quedan registrados en el log de auditoría

5. **Compatibilidad**: Script de migración incluido para actualizar bases de datos existentes

## Escenarios de Uso

### Escenario 1: Operador de Almacén sin Acceso a Reportes
Un operador solo necesita registrar movimientos de stock, no ver reportes:
- Rol: `WarehouseOperator` (sin `Reports.View`)
- Puede: Registrar entradas, salidas, transferencias
- No puede: Ver o generar reportes

### Escenario 2: Analista de Reportes
Un usuario solo necesita ver reportes sin poder modificar datos:
- Rol: `Viewer` (con `Reports.View`)
- Puede: Ver todos los reportes
- No puede: Modificar productos, stock, ventas

### Escenario 3: Vendedor con Acceso a Reportes
Un vendedor necesita ver reportes de ventas para análisis:
- Rol: `Seller` (con `Reports.View`)
- Puede: Gestionar ventas, clientes, y ver reportes
- No puede: Modificar stock o configuración de almacenes

## Pruebas Recomendadas

1. **Prueba de Usuario sin Permiso**
   - Crear un rol sin `Reports.View`
   - Asignar el rol a un usuario de prueba
   - Verificar que el menú "Reportes" no esté disponible

2. **Prueba de Usuario con Permiso**
   - Crear un rol con `Reports.View`
   - Asignar el rol a un usuario de prueba
   - Verificar que el menú "Reportes" esté disponible y funcional

3. **Prueba de Cambio Dinámico**
   - Quitar el permiso `Reports.View` de un rol
   - Usuario debe cerrar sesión
   - Al volver a iniciar sesión, el menú "Reportes" no debe estar disponible

## Mantenimiento Futuro

### Para Agregar Nuevos Tipos de Reportes

El sistema está diseñado para usar un único permiso `Reports.View` para todos los reportes. Si en el futuro se requiere granularidad adicional, se pueden crear permisos adicionales como:

- `Reports.Sales` - Solo reportes de ventas
- `Reports.Stock` - Solo reportes de inventario
- `Reports.Financial` - Solo reportes financieros

### Consideraciones

- El permiso `Reports.View` da acceso a **todos** los reportes del sistema
- Si se necesita control más granular, se debe extender el sistema de permisos
- Los administradores siempre tienen acceso completo a todos los módulos

## Conclusión

La implementación de segmentación de acceso a reportes proporciona un control más preciso sobre quién puede ver información sensible del negocio. El sistema es flexible, fácil de administrar y mantiene la coherencia con el resto del sistema RBAC implementado en la aplicación.
