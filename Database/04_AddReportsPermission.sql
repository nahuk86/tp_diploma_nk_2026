-- ============================================
-- Script de Actualización: Reports.View Permission
-- Propósito: Agregar permiso de acceso a reportes segmentado por roles
-- Versión: 1.0
-- Fecha: 2026-02-16
-- ============================================

USE StockManagerDB;
GO

PRINT '========================================';
PRINT 'Agregando permiso Reports.View...';
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
-- Agregar permiso Reports.View
-- ============================================

PRINT 'Verificando permiso Reports.View...';

DECLARE @ReportsViewPermissionId INT;

-- Verificar si el permiso ya existe
SELECT @ReportsViewPermissionId = PermissionId 
FROM [dbo].[Permissions] 
WHERE [PermissionCode] = 'Reports.View';

IF @ReportsViewPermissionId IS NULL
BEGIN
    -- Agregar el permiso
    PRINT 'Creando permiso Reports.View...';
    
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Reports.View', 'View Reports', 'View and generate reports', 'Reports');
    
    SELECT @ReportsViewPermissionId = SCOPE_IDENTITY();
    
    PRINT 'Permiso Reports.View creado exitosamente (ID: ' + CAST(@ReportsViewPermissionId AS VARCHAR(10)) + ')';
END
ELSE
BEGIN
    PRINT 'El permiso Reports.View ya existe (ID: ' + CAST(@ReportsViewPermissionId AS VARCHAR(10)) + ')';
END

PRINT '';

-- ============================================
-- Asignar Reports.View a Administrator
-- ============================================

PRINT 'Asignando Reports.View al rol Administrator...';

DECLARE @AdminRoleId INT;
SELECT @AdminRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator';

IF @AdminRoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM [dbo].[RolePermissions] 
        WHERE [RoleId] = @AdminRoleId AND [PermissionId] = @ReportsViewPermissionId
    )
    BEGIN
        INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
        VALUES (@AdminRoleId, @ReportsViewPermissionId);
        PRINT '  ✓ Reports.View asignado a Administrator';
    END
    ELSE
    BEGIN
        PRINT '  - Reports.View ya estaba asignado a Administrator';
    END
END
ELSE
BEGIN
    PRINT '  ✗ Rol Administrator no encontrado';
END

-- ============================================
-- Asignar Reports.View a WarehouseManager
-- ============================================

PRINT 'Asignando Reports.View al rol WarehouseManager...';

DECLARE @ManagerRoleId INT;
SELECT @ManagerRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'WarehouseManager';

IF @ManagerRoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM [dbo].[RolePermissions] 
        WHERE [RoleId] = @ManagerRoleId AND [PermissionId] = @ReportsViewPermissionId
    )
    BEGIN
        INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
        VALUES (@ManagerRoleId, @ReportsViewPermissionId);
        PRINT '  ✓ Reports.View asignado a WarehouseManager';
    END
    ELSE
    BEGIN
        PRINT '  - Reports.View ya estaba asignado a WarehouseManager';
    END
END
ELSE
BEGIN
    PRINT '  ✗ Rol WarehouseManager no encontrado';
END

-- ============================================
-- Asignar Reports.View a Viewer
-- ============================================

PRINT 'Asignando Reports.View al rol Viewer...';

DECLARE @ViewerRoleId INT;
SELECT @ViewerRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'Viewer';

IF @ViewerRoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM [dbo].[RolePermissions] 
        WHERE [RoleId] = @ViewerRoleId AND [PermissionId] = @ReportsViewPermissionId
    )
    BEGIN
        INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
        VALUES (@ViewerRoleId, @ReportsViewPermissionId);
        PRINT '  ✓ Reports.View asignado a Viewer';
    END
    ELSE
    BEGIN
        PRINT '  - Reports.View ya estaba asignado a Viewer';
    END
END
ELSE
BEGIN
    PRINT '  ✗ Rol Viewer no encontrado';
END

-- ============================================
-- Asignar Reports.View a Seller
-- ============================================

PRINT 'Asignando Reports.View al rol Seller...';

DECLARE @SellerRoleId INT;
SELECT @SellerRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'Seller';

IF @SellerRoleId IS NOT NULL
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM [dbo].[RolePermissions] 
        WHERE [RoleId] = @SellerRoleId AND [PermissionId] = @ReportsViewPermissionId
    )
    BEGIN
        INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
        VALUES (@SellerRoleId, @ReportsViewPermissionId);
        PRINT '  ✓ Reports.View asignado a Seller';
    END
    ELSE
    BEGIN
        PRINT '  - Reports.View ya estaba asignado a Seller';
    END
END
ELSE
BEGIN
    PRINT '  ✗ Rol Seller no encontrado';
END

PRINT '';

-- ============================================
-- Verificación final
-- ============================================

PRINT '========================================';
PRINT 'Resumen de Roles con Reports.View:';
PRINT '========================================';

SELECT 
    r.RoleName AS [Rol],
    CASE WHEN rp.RolePermissionId IS NOT NULL THEN 'SÍ ✓' ELSE 'NO ✗' END AS [Tiene Acceso a Reportes]
FROM [dbo].[Roles] r
LEFT JOIN [dbo].[RolePermissions] rp ON r.RoleId = rp.RoleId
LEFT JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId AND p.PermissionCode = 'Reports.View'
WHERE r.IsActive = 1
ORDER BY r.RoleName;

PRINT '';
PRINT '========================================';
PRINT 'ACTUALIZACIÓN COMPLETADA EXITOSAMENTE';
PRINT '========================================';
PRINT '';
PRINT 'El permiso Reports.View ha sido agregado al sistema.';
PRINT '';
PRINT 'Roles con acceso a reportes:';
PRINT '  ✓ Administrator (acceso completo)';
PRINT '  ✓ WarehouseManager (gestión y reportes)';
PRINT '  ✓ Viewer (solo lectura incluyendo reportes)';
PRINT '  ✓ Seller (ventas y reportes)';
PRINT '';
PRINT 'WarehouseOperator NO tiene acceso a reportes por defecto.';
PRINT 'Los administradores pueden asignar este permiso según necesidad.';
PRINT '';
PRINT 'Próximos pasos:';
PRINT '  1. Los usuarios deben cerrar sesión y volver a iniciar';
PRINT '  2. Los administradores pueden asignar/quitar Reports.View a roles';
PRINT '     desde la interfaz de gestión de roles';
PRINT '  3. El acceso a reportes ahora está controlado por el permiso Reports.View';
PRINT '';

PRINT '========================================';
PRINT 'Fin del script';
PRINT '========================================';
GO
