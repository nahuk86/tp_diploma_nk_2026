# Plan de Pruebas: Segmentación de Acceso a Reportes

## Objetivo
Verificar que el sistema de control de acceso basado en roles para reportes funciona correctamente según lo especificado.

## Pre-requisitos
- Base de datos actualizada con el permiso Reports.View
- Aplicación compilada con los cambios implementados
- Múltiples usuarios con diferentes roles para pruebas

## Preparación del Entorno de Pruebas

### 1. Configurar Base de Datos
```sql
-- Para instalación nueva:
-- Ejecutar Database/01_CreateSchema.sql
-- Ejecutar Database/02_SeedData.sql

-- Para base de datos existente:
-- Ejecutar Database/04_AddReportsPermission.sql
```

### 2. Verificar Usuarios de Prueba
```sql
-- Verificar que existen usuarios con diferentes roles
SELECT 
    u.Username,
    r.RoleName,
    CASE WHEN EXISTS (
        SELECT 1 FROM RolePermissions rp
        INNER JOIN Permissions p ON rp.PermissionId = p.PermissionId
        WHERE rp.RoleId = r.RoleId AND p.PermissionCode = 'Reports.View'
    ) THEN 'SÍ' ELSE 'NO' END AS [Tiene Reports.View]
FROM Users u
INNER JOIN UserRoles ur ON u.UserId = ur.UserId
INNER JOIN Roles r ON ur.RoleId = r.RoleId
WHERE u.IsActive = 1
ORDER BY r.RoleName, u.Username;
```

## Casos de Prueba

### Caso 1: Usuario Administrador
**Objetivo**: Verificar que el administrador tiene acceso a reportes

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 1.1 | Inicio | Login como "admin" | ✅ Login exitoso |
| 1.2 | Navegación | Abrir aplicación | ✅ Menú principal visible |
| 1.3 | Verificar menú | Buscar menú "Reportes" | ✅ Menú "Reportes" visible y habilitado |
| 1.4 | Acceder | Click en "Reportes" | ✅ Formulario de reportes se abre |
| 1.5 | Generar | Generar cualquier reporte | ✅ Reporte se genera correctamente |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 2: Usuario WarehouseManager
**Objetivo**: Verificar que el gerente de almacén tiene acceso a reportes

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 2.1 | Preparación | Crear/Verificar usuario con rol WarehouseManager | ✅ Usuario existe |
| 2.2 | Inicio | Login con usuario WarehouseManager | ✅ Login exitoso |
| 2.3 | Verificar menú | Buscar menú "Reportes" | ✅ Menú "Reportes" visible y habilitado |
| 2.4 | Acceder | Click en "Reportes" | ✅ Formulario de reportes se abre |
| 2.5 | Generar | Generar reporte de stock | ✅ Reporte se genera correctamente |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 3: Usuario Viewer
**Objetivo**: Verificar que el usuario de solo lectura tiene acceso a reportes

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 3.1 | Preparación | Crear/Verificar usuario con rol Viewer | ✅ Usuario existe |
| 3.2 | Inicio | Login con usuario Viewer | ✅ Login exitoso |
| 3.3 | Verificar menú | Buscar menú "Reportes" | ✅ Menú "Reportes" visible y habilitado |
| 3.4 | Acceder | Click en "Reportes" | ✅ Formulario de reportes se abre |
| 3.5 | Generar | Generar cualquier reporte | ✅ Reporte se genera correctamente |
| 3.6 | Verificar restricciones | Intentar modificar datos | ✅ Opciones de modificación deshabilitadas |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 4: Usuario Seller
**Objetivo**: Verificar que el vendedor tiene acceso a reportes

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 4.1 | Preparación | Crear/Verificar usuario con rol Seller | ✅ Usuario existe |
| 4.2 | Inicio | Login con usuario Seller | ✅ Login exitoso |
| 4.3 | Verificar menú | Buscar menú "Reportes" | ✅ Menú "Reportes" visible y habilitado |
| 4.4 | Acceder | Click en "Reportes" | ✅ Formulario de reportes se abre |
| 4.5 | Generar | Generar reporte de ventas | ✅ Reporte se genera correctamente |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 5: Usuario WarehouseOperator (Sin Acceso)
**Objetivo**: Verificar que el operador NO tiene acceso a reportes por defecto

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 5.1 | Preparación | Crear/Verificar usuario con rol WarehouseOperator | ✅ Usuario existe |
| 5.2 | Inicio | Login con usuario WarehouseOperator | ✅ Login exitoso |
| 5.3 | Verificar menú | Buscar menú "Reportes" | ✅ Menú "Reportes" NO visible o deshabilitado |
| 5.4 | Verificar otros accesos | Verificar acceso a stock | ✅ Puede acceder a movimientos de stock |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 6: Asignar Permiso Dinámicamente
**Objetivo**: Verificar que se puede dar acceso a reportes a un rol que no lo tiene

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 6.1 | Preparación | Login como Administrator | ✅ Login exitoso |
| 6.2 | Navegación | Ir a Administración → Roles | ✅ Formulario de roles abierto |
| 6.3 | Selección | Seleccionar rol "WarehouseOperator" | ✅ Rol seleccionado |
| 6.4 | Permisos | Click en "Asignar Permisos" | ✅ Diálogo de permisos abierto |
| 6.5 | Modificar | Marcar checkbox "Reports.View" | ✅ Checkbox marcado |
| 6.6 | Guardar | Click en "Guardar" | ✅ Mensaje de éxito |
| 6.7 | Verificar | Login con usuario WarehouseOperator | ✅ Login exitoso |
| 6.8 | Comprobar | Verificar menú "Reportes" | ✅ Ahora está visible y habilitado |
| 6.9 | Acceder | Click en "Reportes" | ✅ Formulario de reportes se abre |
| 6.10 | Limpiar | Quitar permiso Reports.View del rol | ✅ Permiso removido |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 7: Quitar Permiso Dinámicamente
**Objetivo**: Verificar que se puede quitar acceso a reportes a un rol que lo tiene

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 7.1 | Preparación | Login como Administrator | ✅ Login exitoso |
| 7.2 | Navegación | Ir a Administración → Roles | ✅ Formulario de roles abierto |
| 7.3 | Selección | Seleccionar rol "Seller" | ✅ Rol seleccionado |
| 7.4 | Permisos | Click en "Asignar Permisos" | ✅ Diálogo de permisos abierto |
| 7.5 | Verificar | Confirmar que Reports.View está marcado | ✅ Está marcado |
| 7.6 | Modificar | Desmarcar checkbox "Reports.View" | ✅ Checkbox desmarcado |
| 7.7 | Guardar | Click en "Guardar" | ✅ Mensaje de éxito |
| 7.8 | Verificar | Login con usuario Seller | ✅ Login exitoso |
| 7.9 | Comprobar | Verificar menú "Reportes" | ✅ NO está visible o está deshabilitado |
| 7.10 | Restaurar | Volver a asignar Reports.View a Seller | ✅ Permiso restaurado |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 8: Crear Rol Personalizado
**Objetivo**: Verificar que se puede crear un rol personalizado con acceso a reportes

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 8.1 | Preparación | Login como Administrator | ✅ Login exitoso |
| 8.2 | Navegación | Ir a Administración → Roles | ✅ Formulario de roles abierto |
| 8.3 | Crear | Click en "Nuevo Rol" | ✅ Formulario de nuevo rol |
| 8.4 | Datos | Nombre: "Analista", Descripción: "Solo reportes" | ✅ Datos ingresados |
| 8.5 | Guardar | Click en "Guardar" | ✅ Rol creado |
| 8.6 | Permisos | Click en "Asignar Permisos" | ✅ Diálogo de permisos abierto |
| 8.7 | Seleccionar | Marcar solo "Reports.View" | ✅ Solo Reports.View marcado |
| 8.8 | Guardar | Click en "Guardar" | ✅ Permisos asignados |
| 8.9 | Usuario | Crear usuario de prueba y asignar rol "Analista" | ✅ Usuario creado |
| 8.10 | Verificar | Login con usuario Analista | ✅ Solo menú Reportes visible |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 9: Persistencia después de Logout/Login
**Objetivo**: Verificar que los cambios persisten después de cerrar y abrir sesión

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 9.1 | Preparación | Login con usuario que tiene Reports.View | ✅ Login exitoso |
| 9.2 | Verificar | Confirmar menú Reportes visible | ✅ Visible |
| 9.3 | Cerrar | Logout de la aplicación | ✅ Sesión cerrada |
| 9.4 | Abrir | Login nuevamente con mismo usuario | ✅ Login exitoso |
| 9.5 | Verificar | Confirmar menú Reportes sigue visible | ✅ Sigue visible |

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

### Caso 10: Verificación en Base de Datos
**Objetivo**: Verificar que los cambios se reflejan correctamente en la base de datos

| ID | Paso | Acción | Resultado Esperado |
|----|------|--------|-------------------|
| 10.1 | Consulta | Ejecutar query de verificación de permisos | ✅ Query ejecutado |
| 10.2 | Verificar | Roles con Reports.View coinciden con expectativa | ✅ Coinciden |
| 10.3 | Auditoría | Revisar AuditLogs para cambios de permisos | ✅ Cambios registrados |

```sql
-- Query de verificación
SELECT 
    r.RoleName,
    COUNT(CASE WHEN p.PermissionCode = 'Reports.View' THEN 1 END) AS [Tiene Reports.View]
FROM Roles r
LEFT JOIN RolePermissions rp ON r.RoleId = rp.RoleId
LEFT JOIN Permissions p ON rp.PermissionId = p.PermissionId
WHERE r.IsActive = 1
GROUP BY r.RoleName
ORDER BY r.RoleName;
```

**Estado**: ⬜ No ejecutado | ✅ Pasó | ❌ Falló

---

## Pruebas de Regresión

### Verificar que Otros Módulos NO Fueron Afectados

| Módulo | Verificación | Estado |
|--------|-------------|--------|
| Usuarios | Gestión de usuarios funciona normal | ⬜ |
| Roles | Gestión de roles funciona normal | ⬜ |
| Productos | Gestión de productos funciona normal | ⬜ |
| Almacenes | Gestión de almacenes funciona normal | ⬜ |
| Stock | Movimientos de stock funcionan normal | ⬜ |
| Ventas | Gestión de ventas funciona normal | ⬜ |
| Auditoría | Logs de auditoría funcionan normal | ⬜ |

## Pruebas de Seguridad

| ID | Prueba | Resultado Esperado | Estado |
|----|--------|-------------------|--------|
| S1 | SQL Injection en Reports.View | ✅ Protegido | ⬜ |
| S2 | Bypass de autorización | ✅ Acceso denegado | ⬜ |
| S3 | Escalación de privilegios | ✅ No es posible | ⬜ |
| S4 | Auditoría de cambios | ✅ Todos registrados | ⬜ |

## Criterios de Aceptación

Para que la implementación sea aceptada, TODOS los siguientes criterios deben cumplirse:

- [ ] Todos los casos de prueba principales (1-10) pasan exitosamente
- [ ] Todas las pruebas de regresión pasan
- [ ] Todas las pruebas de seguridad pasan
- [ ] No se introducen bugs en módulos existentes
- [ ] La documentación está completa y es correcta
- [ ] El script de migración funciona en bases de datos existentes

## Registro de Ejecución

| Fecha | Ejecutor | Casos Ejecutados | Casos Pasados | Casos Fallados | Notas |
|-------|----------|------------------|---------------|----------------|-------|
| | | | | | |
| | | | | | |
| | | | | | |

## Reporte de Bugs

| ID | Fecha | Caso | Descripción | Severidad | Estado |
|----|-------|------|-------------|-----------|--------|
| | | | | | |
| | | | | | |

## Conclusión

Una vez completadas todas las pruebas, llenar la siguiente sección:

**Estado General**: ⬜ Pendiente | ✅ Aprobado | ❌ Rechazado

**Fecha de Aprobación**: _____________

**Aprobado por**: _____________

**Notas finales**:
```
[Espacio para comentarios del QA]
```
