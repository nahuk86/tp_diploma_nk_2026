-- ============================================
-- Script de Corrección: Activar permiso Reports.View
-- Propósito: Asegurar que el permiso Reports.View esté activo y visible
-- Versión: 1.0
-- Fecha: 2026-02-17
-- ============================================

USE StockManagerDB;
GO

PRINT '========================================';
PRINT 'Activando permiso Reports.View...';
PRINT '========================================';
PRINT '';

-- ============================================
-- Verificar que la base de datos existe
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'StockManagerDB')
BEGIN
    PRINT 'ERROR: La base de datos StockManagerDB no existe.';
    PRINT 'Por favor, ejecute primero los scripts:';
    PRINT '  1. Database/01_CreateSchema.sql';
    PRINT '  2. Database/02_SeedData.sql';
    RAISERROR('Base de datos no encontrada.', 16, 1);
    RETURN;
END
GO

-- ============================================
-- Verificar y activar permiso Reports.View
-- ============================================

PRINT 'Verificando permiso Reports.View...';

DECLARE @ReportsViewPermissionId INT;
DECLARE @IsActive BIT;

-- Verificar si el permiso existe
SELECT 
    @ReportsViewPermissionId = PermissionId,
    @IsActive = IsActive
FROM [dbo].[Permissions] 
WHERE [PermissionCode] = 'Reports.View';

IF @ReportsViewPermissionId IS NULL
BEGIN
    -- El permiso no existe, crearlo
    PRINT 'El permiso Reports.View no existe. Creándolo...';
    
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module], [IsActive]) 
    VALUES ('Reports.View', 'View Reports', 'View and generate reports', 'Reports', 1);
    
    SELECT @ReportsViewPermissionId = SCOPE_IDENTITY();
    
    PRINT '  ✓ Permiso Reports.View creado exitosamente (ID: ' + CAST(@ReportsViewPermissionId AS VARCHAR(10)) + ')';
    PRINT '  ✓ Estado: ACTIVO';
END
ELSE
BEGIN
    PRINT 'Permiso Reports.View encontrado (ID: ' + CAST(@ReportsViewPermissionId AS VARCHAR(10)) + ')';
    
    IF @IsActive = 0
    BEGIN
        -- El permiso existe pero está inactivo, activarlo
        PRINT '  ⚠ Estado actual: INACTIVO';
        PRINT '  → Activando permiso...';
        
        UPDATE [dbo].[Permissions]
        SET [IsActive] = 1
        WHERE [PermissionId] = @ReportsViewPermissionId;
        
        PRINT '  ✓ Permiso Reports.View activado exitosamente';
    END
    ELSE
    BEGIN
        PRINT '  ✓ Estado: Ya estaba ACTIVO';
        PRINT '  → No se requiere ninguna acción';
    END
END

PRINT '';

-- ============================================
-- Verificación de estado final
-- ============================================

PRINT '========================================';
PRINT 'Estado Final del Permiso Reports.View:';
PRINT '========================================';

SELECT 
    PermissionId AS [ID],
    PermissionCode AS [Código],
    PermissionName AS [Nombre],
    [Module] AS [Módulo],
    CASE WHEN IsActive = 1 THEN 'ACTIVO ✓' ELSE 'INACTIVO ✗' END AS [Estado],
    [Description] AS [Descripción]
FROM [dbo].[Permissions]
WHERE [PermissionCode] = 'Reports.View';

PRINT '';

-- ============================================
-- Verificar qué roles tienen el permiso asignado
-- ============================================

PRINT '========================================';
PRINT 'Roles con Acceso a Reports.View:';
PRINT '========================================';

IF EXISTS (
    SELECT 1 
    FROM [dbo].[RolePermissions] rp
    INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
    WHERE p.PermissionCode = 'Reports.View'
)
BEGIN
    SELECT 
        r.RoleName AS [Rol],
        r.Description AS [Descripción],
        CASE WHEN r.IsActive = 1 THEN 'ACTIVO ✓' ELSE 'INACTIVO ✗' END AS [Estado del Rol]
    FROM [dbo].[Roles] r
    INNER JOIN [dbo].[RolePermissions] rp ON r.RoleId = rp.RoleId
    INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
    WHERE p.PermissionCode = 'Reports.View'
    ORDER BY r.RoleName;
    
    PRINT '';
    PRINT 'Roles listados arriba tienen el permiso Reports.View asignado.';
END
ELSE
BEGIN
    PRINT 'ADVERTENCIA: Ningún rol tiene el permiso Reports.View asignado.';
    PRINT '';
    PRINT 'Para asignar el permiso a un rol:';
    PRINT '  1. Inicie sesión como Administrator en la aplicación';
    PRINT '  2. Vaya a Administración → Roles';
    PRINT '  3. Seleccione el rol deseado';
    PRINT '  4. Haga clic en "Asignar Permisos"';
    PRINT '  5. Marque la casilla "Reports.View"';
    PRINT '  6. Haga clic en "Guardar"';
    PRINT '';
    PRINT 'Alternativamente, ejecute el script:';
    PRINT '  Database/04_AddReportsPermission.sql';
END

PRINT '';

-- ============================================
-- Resultado final
-- ============================================

PRINT '========================================';
PRINT 'CORRECCIÓN COMPLETADA EXITOSAMENTE';
PRINT '========================================';
PRINT '';
PRINT 'El permiso Reports.View está ahora ACTIVO y visible en el sistema.';
PRINT '';
PRINT 'Próximos pasos:';
PRINT '  1. El permiso Reports.View ahora aparecerá en el formulario';
PRINT '     de gestión de permisos (Administración → Roles → Asignar Permisos)';
PRINT '  2. Los administradores pueden asignar/quitar este permiso a cualquier rol';
PRINT '  3. Los usuarios deben cerrar sesión y volver a iniciar para que';
PRINT '     los cambios surtan efecto';
PRINT '';
PRINT 'Documentación relacionada:';
PRINT '  - REPORTS_ACCESS_QUICK_GUIDE.md - Guía de uso';
PRINT '  - REPORTS_ACCESS_SEGMENTATION.md - Documentación técnica';
PRINT '';

PRINT '========================================';
PRINT 'Fin del script';
PRINT '========================================';
GO
