-- ============================================
-- Script de Actualización de Permisos
-- Propósito: Habilitar transferencias y entradas de stock para WarehouseOperator
-- Versión: 1.0
-- Fecha: 2026-02-15
-- ============================================

USE StockManagerDB;
GO

PRINT '========================================';
PRINT 'Iniciando actualización de permisos...';
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
-- Agregar permiso Stock.Adjust a WarehouseOperator
-- ============================================

PRINT 'Verificando rol WarehouseOperator...';

DECLARE @OperatorRoleId INT;
DECLARE @AdjustPermissionId INT;

-- Obtener el ID del rol WarehouseOperator
SELECT @OperatorRoleId = RoleId 
FROM [dbo].[Roles] 
WHERE [RoleName] = 'WarehouseOperator';

IF @OperatorRoleId IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el rol WarehouseOperator.';
    PRINT 'Verifique que el script de datos semilla se ejecutó correctamente.';
    RAISERROR('Rol WarehouseOperator no encontrado.', 16, 1);
    RETURN;
END

PRINT 'Rol WarehouseOperator encontrado (ID: ' + CAST(@OperatorRoleId AS VARCHAR(10)) + ')';
PRINT '';

-- Obtener el ID del permiso Stock.Adjust
SELECT @AdjustPermissionId = PermissionId 
FROM [dbo].[Permissions] 
WHERE [PermissionCode] = 'Stock.Adjust';

IF @AdjustPermissionId IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el permiso Stock.Adjust.';
    PRINT 'Verifique que el script de datos semilla se ejecutó correctamente.';
    RAISERROR('Permiso Stock.Adjust no encontrado.', 16, 1);
    RETURN;
END

PRINT 'Permiso Stock.Adjust encontrado (ID: ' + CAST(@AdjustPermissionId AS VARCHAR(10)) + ')';
PRINT '';

-- Verificar si el permiso ya está asignado
IF EXISTS (
    SELECT 1 FROM [dbo].[RolePermissions] 
    WHERE [RoleId] = @OperatorRoleId 
    AND [PermissionId] = @AdjustPermissionId
)
BEGIN
    PRINT 'El permiso Stock.Adjust ya está asignado al rol WarehouseOperator.';
    PRINT 'No se requiere ninguna acción.';
END
ELSE
BEGIN
    -- Agregar el permiso
    PRINT 'Agregando permiso Stock.Adjust al rol WarehouseOperator...';
    
    INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
    VALUES (@OperatorRoleId, @AdjustPermissionId);
    
    PRINT 'Permiso Stock.Adjust agregado exitosamente.';
END

PRINT '';

-- ============================================
-- Verificar permisos actuales de WarehouseOperator
-- ============================================

PRINT '========================================';
PRINT 'Permisos actuales del rol WarehouseOperator:';
PRINT '========================================';

SELECT 
    p.PermissionCode AS [Código de Permiso],
    p.PermissionName AS [Nombre de Permiso],
    p.Description AS [Descripción]
FROM [dbo].[RolePermissions] rp
INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
WHERE rp.RoleId = @OperatorRoleId
ORDER BY p.PermissionCode;

PRINT '';

-- ============================================
-- Verificar permisos de stock específicamente
-- ============================================

PRINT '========================================';
PRINT 'Permisos de Stock para WarehouseOperator:';
PRINT '========================================';

DECLARE @StockViewExists BIT = 0;
DECLARE @StockReceiveExists BIT = 0;
DECLARE @StockIssueExists BIT = 0;
DECLARE @StockTransferExists BIT = 0;
DECLARE @StockAdjustExists BIT = 0;

SELECT @StockViewExists = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
FROM [dbo].[RolePermissions] rp
INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
WHERE rp.RoleId = @OperatorRoleId AND p.PermissionCode = 'Stock.View';

SELECT @StockReceiveExists = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
FROM [dbo].[RolePermissions] rp
INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
WHERE rp.RoleId = @OperatorRoleId AND p.PermissionCode = 'Stock.Receive';

SELECT @StockIssueExists = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
FROM [dbo].[RolePermissions] rp
INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
WHERE rp.RoleId = @OperatorRoleId AND p.PermissionCode = 'Stock.Issue';

SELECT @StockTransferExists = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
FROM [dbo].[RolePermissions] rp
INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
WHERE rp.RoleId = @OperatorRoleId AND p.PermissionCode = 'Stock.Transfer';

SELECT @StockAdjustExists = CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END
FROM [dbo].[RolePermissions] rp
INNER JOIN [dbo].[Permissions] p ON rp.PermissionId = p.PermissionId
WHERE rp.RoleId = @OperatorRoleId AND p.PermissionCode = 'Stock.Adjust';

PRINT 'Stock.View (Consultar): ' + CASE WHEN @StockViewExists = 1 THEN 'SÍ ✓' ELSE 'NO ✗' END;
PRINT 'Stock.Receive (Entradas): ' + CASE WHEN @StockReceiveExists = 1 THEN 'SÍ ✓' ELSE 'NO ✗' END;
PRINT 'Stock.Issue (Salidas): ' + CASE WHEN @StockIssueExists = 1 THEN 'SÍ ✓' ELSE 'NO ✗' END;
PRINT 'Stock.Transfer (Transferencias): ' + CASE WHEN @StockTransferExists = 1 THEN 'SÍ ✓' ELSE 'NO ✗' END;
PRINT 'Stock.Adjust (Ajustes): ' + CASE WHEN @StockAdjustExists = 1 THEN 'SÍ ✓' ELSE 'NO ✗' END;

PRINT '';

-- ============================================
-- Verificación final
-- ============================================

IF @StockViewExists = 1 AND 
   @StockReceiveExists = 1 AND 
   @StockIssueExists = 1 AND 
   @StockTransferExists = 1 AND 
   @StockAdjustExists = 1
BEGIN
    PRINT '========================================';
    PRINT 'ACTUALIZACIÓN COMPLETADA EXITOSAMENTE';
    PRINT '========================================';
    PRINT '';
    PRINT 'El rol WarehouseOperator ahora tiene todos los permisos de stock:';
    PRINT '  ✓ Consultar inventario (Stock.View)';
    PRINT '  ✓ Realizar entradas (Stock.Receive)';
    PRINT '  ✓ Realizar salidas (Stock.Issue)';
    PRINT '  ✓ Realizar transferencias (Stock.Transfer)';
    PRINT '  ✓ Realizar ajustes (Stock.Adjust)';
    PRINT '';
    PRINT 'Próximos pasos:';
    PRINT '  1. Reinicie la aplicación para que los cambios surtan efecto';
    PRINT '  2. Los usuarios con rol WarehouseOperator deben cerrar sesión';
    PRINT '     y volver a iniciar sesión para obtener los nuevos permisos';
    PRINT '  3. Pruebe las funcionalidades de transferencias y entradas';
    PRINT '';
    PRINT 'Para más información, consulte el archivo ACTIVATION_GUIDE_ES.md';
END
ELSE
BEGIN
    PRINT '========================================';
    PRINT 'ADVERTENCIA: Faltan algunos permisos';
    PRINT '========================================';
    PRINT '';
    PRINT 'El rol WarehouseOperator no tiene todos los permisos de stock.';
    PRINT 'Verifique la ejecución del script 02_SeedData.sql';
    PRINT '';
    
    IF @StockViewExists = 0 PRINT '  ✗ Falta: Stock.View';
    IF @StockReceiveExists = 0 PRINT '  ✗ Falta: Stock.Receive';
    IF @StockIssueExists = 0 PRINT '  ✗ Falta: Stock.Issue';
    IF @StockTransferExists = 0 PRINT '  ✗ Falta: Stock.Transfer';
    IF @StockAdjustExists = 0 PRINT '  ✗ Falta: Stock.Adjust';
END

PRINT '';
PRINT '========================================';
PRINT 'Fin del script';
PRINT '========================================';
GO
