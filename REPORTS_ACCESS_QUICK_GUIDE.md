# Guía Rápida: Gestión de Acceso a Reportes

## Para Administradores

Esta guía explica cómo gestionar qué usuarios pueden acceder al módulo de reportes.

## Contexto

A partir de esta actualización, el acceso a reportes está controlado por el permiso **Reports.View**. Los administradores pueden asignar o quitar este permiso a los roles según sea necesario.

## Roles y Acceso por Defecto

### ✅ Roles CON acceso a reportes:
- **Administrator** - Acceso completo al sistema
- **WarehouseManager** - Gestión de almacén + reportes
- **Viewer** - Solo lectura en todo el sistema
- **Seller** - Ventas + reportes

### ❌ Roles SIN acceso a reportes:
- **WarehouseOperator** - Solo operaciones de stock

## Cómo Dar Acceso a Reportes

### Opción 1: Asignar el Permiso a un Rol Existente

1. Iniciar sesión como **Administrator**
2. Ir a **Administración → Roles**
3. Seleccionar el rol (por ejemplo, "WarehouseOperator")
4. Hacer clic en **Asignar Permisos**
5. En la lista de permisos, buscar **Reports.View**
6. ✅ Marcar la casilla **Reports.View**
7. Hacer clic en **Guardar**

**Resultado**: Todos los usuarios con ese rol ahora tienen acceso a reportes.

### Opción 2: Crear un Nuevo Rol con Acceso a Reportes

1. Iniciar sesión como **Administrator**
2. Ir a **Administración → Roles**
3. Hacer clic en **Nuevo Rol**
4. Completar:
   - **Nombre**: Por ejemplo, "Analista de Reportes"
   - **Descripción**: "Usuario con acceso solo a reportes"
5. Hacer clic en **Guardar**
6. Con el nuevo rol seleccionado, hacer clic en **Asignar Permisos**
7. Marcar los permisos necesarios:
   - ✅ **Reports.View** (obligatorio para reportes)
   - ✅ Otros permisos según necesidad
8. Hacer clic en **Guardar**
9. Asignar el rol a los usuarios correspondientes

## Cómo Quitar Acceso a Reportes

1. Iniciar sesión como **Administrator**
2. Ir a **Administración → Roles**
3. Seleccionar el rol
4. Hacer clic en **Asignar Permisos**
5. ❌ Desmarcar la casilla **Reports.View**
6. Hacer clic en **Guardar**

**Importante**: Los usuarios afectados deben cerrar sesión y volver a iniciar para que el cambio surta efecto.

## Verificar Acceso de un Usuario

### Desde la Interfaz de Usuario:
1. El usuario inicia sesión
2. Si tiene acceso a reportes:
   - ✅ El menú **Reportes** estará visible y habilitado
3. Si NO tiene acceso:
   - ❌ El menú **Reportes** no estará disponible

### Desde la Base de Datos (para verificación técnica):

```sql
-- Ver qué roles tienen acceso a reportes
SELECT 
    r.RoleName,
    p.PermissionName,
    p.PermissionCode
FROM Roles r
INNER JOIN RolePermissions rp ON r.RoleId = rp.RoleId
INNER JOIN Permissions p ON rp.PermissionId = p.PermissionId
WHERE p.PermissionCode = 'Reports.View'
ORDER BY r.RoleName;

-- Ver si un usuario específico tiene acceso a reportes
SELECT 
    u.Username,
    r.RoleName,
    p.PermissionCode
FROM Users u
INNER JOIN UserRoles ur ON u.UserId = ur.UserId
INNER JOIN Roles r ON ur.RoleId = r.RoleId
INNER JOIN RolePermissions rp ON r.RoleId = rp.RoleId
INNER JOIN Permissions p ON rp.PermissionId = p.PermissionId
WHERE u.Username = 'nombre_usuario'
  AND p.PermissionCode = 'Reports.View';
```

## Casos de Uso Comunes

### Caso 1: Vendedor que necesita ver reportes de ventas
**Solución**: Ya tiene acceso por defecto (rol Seller incluye Reports.View)

### Caso 2: Operador de almacén que necesita ver reportes de stock
**Solución**: Asignar permiso Reports.View al rol WarehouseOperator

### Caso 3: Usuario externo que solo debe ver reportes
**Solución**: Asignar rol Viewer (tiene Reports.View pero sin permisos de modificación)

### Caso 4: Gerente que necesita reportes pero no debe operar el sistema
**Solución**: Crear un rol personalizado con:
- Reports.View ✅
- Sin permisos de modificación en otros módulos

## Migración de Bases de Datos Existentes

Si está actualizando desde una versión anterior:

1. Ejecutar el script de migración:
   ```
   Database/04_AddReportsPermission.sql
   ```

2. El script automáticamente:
   - ✅ Crea el permiso Reports.View
   - ✅ Lo asigna a los roles correspondientes
   - ✅ Muestra un resumen de cambios

3. Los usuarios deben reiniciar sesión

## Preguntas Frecuentes

### ¿Qué pasa con usuarios que tenían acceso a reportes antes?
- Los usuarios con roles Administrator, WarehouseManager, Viewer o Seller mantienen su acceso
- Los usuarios con WarehouseOperator pierden el acceso (pueden recuperarlo si se les asigna el permiso)

### ¿Puedo dar acceso solo a ciertos reportes?
- Actualmente, Reports.View da acceso a TODOS los reportes
- Para control más granular, se requiere extender el sistema de permisos

### ¿Los cambios son inmediatos?
- Los cambios en permisos requieren que el usuario cierre sesión y vuelva a iniciar

### ¿Se pueden auditar los cambios de permisos?
- Sí, todos los cambios quedan registrados en la tabla AuditLogs

## Soporte Técnico

Para más información, consulte:
- `REPORTS_ACCESS_SEGMENTATION.md` - Documentación técnica completa
- `README.md` - Lista completa de permisos del sistema
- `COMPLETE_RBAC_SUMMARY.md` - Documentación del sistema RBAC

## Resumen de Comandos Rápidos

```
✅ Dar acceso:    Roles → Seleccionar → Asignar Permisos → Marcar Reports.View → Guardar
❌ Quitar acceso: Roles → Seleccionar → Asignar Permisos → Desmarcar Reports.View → Guardar
👥 Asignar rol:   Usuarios → Seleccionar → Asignar Roles → Marcar rol → Guardar
🔍 Verificar:     Usuario inicia sesión → Menu Reportes debe estar visible/oculto
```
