# Solución: Permisos para Visualizar y Utilizar Reportes

## Problema Identificado

El permiso `Reports.View` está inactivo en la base de datos, lo que impide que aparezca en el formulario de gestión de permisos. Esto hace imposible asignar o remover permisos de reportes a cualquier rol.

## Causa Raíz

El campo `IsActive` del permiso `Reports.View` en la tabla `Permissions` está establecido en `0` (inactivo), y el sistema solo muestra permisos activos (`IsActive = 1`) en el formulario de gestión.

## Solución

Se ha creado un script SQL que:
1. Verifica si el permiso `Reports.View` existe
2. Si existe pero está inactivo, lo activa
3. Si no existe, lo crea como activo
4. Muestra el estado final y los roles que tienen acceso

## Pasos para Aplicar la Solución

### Paso 1: Ejecutar el Script SQL

1. Abrir **SQL Server Management Studio** (SSMS) o su herramienta de SQL preferida
2. Conectarse a la instancia de SQL Server
3. Abrir el archivo: `Database/05_ActivateReportsPermission.sql`
4. Ejecutar el script completo (F5 o botón "Execute")

El script mostrará mensajes indicando:
- Si el permiso fue encontrado y activado
- El estado final del permiso
- Qué roles tienen el permiso asignado

### Paso 2: Verificar en la Base de Datos (Opcional)

Para confirmar que el permiso está activo, ejecute:

```sql
USE StockManagerDB;

SELECT 
    PermissionId,
    PermissionCode,
    PermissionName,
    Module,
    IsActive,
    Description
FROM Permissions
WHERE PermissionCode = 'Reports.View';
```

**Resultado esperado:**
- `IsActive` debe ser `1` (True)
- El registro debe existir

### Paso 3: Verificar en la Aplicación

1. Abrir la aplicación
2. Iniciar sesión como **Administrator**
3. Ir a **Administración → Roles**
4. Seleccionar cualquier rol
5. Hacer clic en **"Asignar Permisos"**
6. **Verificar** que ahora aparece `[Reports] View Reports` en la lista

## Uso Después de la Corrección

### Para Dar Acceso a Reportes a un Rol

1. Iniciar sesión como **Administrator**
2. Ir a **Administración → Roles**
3. Seleccionar el rol (ej: "WarehouseOperator")
4. Hacer clic en **"Asignar Permisos"**
5. Buscar y marcar **[Reports] View Reports**
6. Hacer clic en **"Guardar"**

### Para Quitar Acceso a Reportes de un Rol

1. Iniciar sesión como **Administrator**
2. Ir a **Administración → Roles**
3. Seleccionar el rol
4. Hacer clic en **"Asignar Permisos"**
5. Desmarcar **[Reports] View Reports**
6. Hacer clic en **"Guardar"**

## Verificación Final

### Verificación Técnica (Base de Datos)

```sql
-- Ver estado del permiso Reports.View
SELECT 
    p.PermissionCode,
    p.IsActive,
    COUNT(rp.RolePermissionId) AS [Roles Asignados]
FROM Permissions p
LEFT JOIN RolePermissions rp ON p.PermissionId = rp.PermissionId
WHERE p.PermissionCode = 'Reports.View'
GROUP BY p.PermissionCode, p.IsActive;
```

### Verificación Funcional (Aplicación)

1. ✅ El permiso `Reports.View` aparece en el formulario de permisos
2. ✅ Se puede marcar/desmarcar el permiso para cualquier rol
3. ✅ Los cambios se guardan correctamente
4. ✅ Los usuarios con roles que tienen el permiso pueden ver el menú de Reportes

## Asignación Recomendada por Rol

| Rol | ¿Debe tener Reports.View? | Justificación |
|-----|---------------------------|---------------|
| **Administrator** | ✅ Sí | Acceso completo al sistema |
| **WarehouseManager** | ✅ Sí | Necesita reportes de stock y gestión |
| **Seller** | ✅ Sí | Necesita reportes de ventas |
| **Viewer** | ✅ Sí | Solo lectura, incluyendo reportes |
| **WarehouseOperator** | ❓ Opcional | Depende de las necesidades del negocio |

## Scripts Relacionados

1. `Database/01_CreateSchema.sql` - Creación del esquema
2. `Database/02_SeedData.sql` - Datos iniciales
3. `Database/03_UpdatePermissions.sql` - Actualización de permisos de stock
4. `Database/04_AddReportsPermission.sql` - Asignación de Reports.View a roles
5. **`Database/05_ActivateReportsPermission.sql`** - **(NUEVO)** Activación del permiso Reports.View

## Documentación Adicional

- `REPORTS_ACCESS_QUICK_GUIDE.md` - Guía rápida de gestión de acceso a reportes
- `REPORTS_ACCESS_SEGMENTATION.md` - Documentación técnica detallada
- `COMPLETE_RBAC_SUMMARY.md` - Sistema completo de roles y permisos

## Notas Importantes

1. **Reinicio de Sesión**: Los usuarios deben cerrar sesión y volver a iniciar para que los cambios surtan efecto
2. **Backup**: Se recomienda hacer backup de la base de datos antes de ejecutar cualquier script
3. **Permisos de Ejecución**: Asegúrese de tener permisos de administrador en SQL Server

## Soporte

Si después de aplicar esta solución el problema persiste:

1. Verifique que el script se ejecutó sin errores
2. Confirme que `IsActive = 1` para el permiso Reports.View
3. Reinicie la aplicación
4. Verifique los logs de la aplicación en busca de errores

## Resumen Ejecutivo

**Problema**: No se podía gestionar permisos de reportes desde la interfaz

**Causa**: Permiso `Reports.View` estaba inactivo en la base de datos

**Solución**: Script SQL que activa el permiso

**Resultado**: Ahora los administradores pueden asignar/remover permisos de reportes desde el formulario de gestión de permisos

---

*Documento creado el: 2026-02-17*  
*Versión: 1.0*
