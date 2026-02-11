-- ============================================
-- Stock Management System - Seed Data
-- Default admin user, roles, and permissions
-- ============================================

USE StockManagerDB;
GO

-- ============================================
-- SEED PERMISSIONS
-- ============================================

PRINT 'Inserting Permissions...';

-- User Management Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Users.View')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Users.View', 'View Users', 'View user list and details', 'Users');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Users.Create')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Users.Create', 'Create Users', 'Create new users', 'Users');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Users.Edit')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Users.Edit', 'Edit Users', 'Edit existing users', 'Users');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Users.Delete')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Users.Delete', 'Delete Users', 'Delete users (soft delete)', 'Users');

-- Role Management Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Roles.View')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Roles.View', 'View Roles', 'View role list and details', 'Roles');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Roles.Create')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Roles.Create', 'Create Roles', 'Create new roles', 'Roles');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Roles.Edit')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Roles.Edit', 'Edit Roles', 'Edit existing roles', 'Roles');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Roles.Delete')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Roles.Delete', 'Delete Roles', 'Delete roles (soft delete)', 'Roles');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Roles.AssignPermissions')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Roles.AssignPermissions', 'Assign Permissions', 'Assign permissions to roles', 'Roles');

-- Product Management Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Products.View')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Products.View', 'View Products', 'View product list and details', 'Products');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Products.Create')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Products.Create', 'Create Products', 'Create new products', 'Products');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Products.Edit')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Products.Edit', 'Edit Products', 'Edit existing products', 'Products');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Products.Delete')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Products.Delete', 'Delete Products', 'Delete products (soft delete)', 'Products');

-- Warehouse Management Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Warehouses.View')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Warehouses.View', 'View Warehouses', 'View warehouse list and details', 'Warehouses');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Warehouses.Create')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Warehouses.Create', 'Create Warehouses', 'Create new warehouses', 'Warehouses');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Warehouses.Edit')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Warehouses.Edit', 'Edit Warehouses', 'Edit existing warehouses', 'Warehouses');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Warehouses.Delete')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Warehouses.Delete', 'Delete Warehouses', 'Delete warehouses (soft delete)', 'Warehouses');

-- Stock Management Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Stock.View')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Stock.View', 'View Stock', 'View stock levels and movements', 'Stock');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Stock.Receive')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Stock.Receive', 'Receive Stock', 'Register stock receipts/incoming', 'Stock');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Stock.Issue')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Stock.Issue', 'Issue Stock', 'Register stock issues/outgoing', 'Stock');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Stock.Transfer')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Stock.Transfer', 'Transfer Stock', 'Transfer stock between warehouses', 'Stock');

IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Stock.Adjust')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Stock.Adjust', 'Adjust Stock', 'Make stock adjustments', 'Stock');

-- Audit Permissions
IF NOT EXISTS (SELECT 1 FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Audit.View')
    INSERT INTO [dbo].[Permissions] ([PermissionCode], [PermissionName], [Description], [Module]) 
    VALUES ('Audit.View', 'View Audit Logs', 'View audit logs and history', 'Audit');

PRINT 'Permissions inserted successfully.';
GO

-- ============================================
-- SEED ROLES
-- ============================================

PRINT 'Inserting Roles...';

-- Administrator Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator')
    INSERT INTO [dbo].[Roles] ([RoleName], [Description]) 
    VALUES ('Administrator', 'Full system access with all permissions');

-- Warehouse Manager Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'WarehouseManager')
    INSERT INTO [dbo].[Roles] ([RoleName], [Description]) 
    VALUES ('WarehouseManager', 'Manage stock, products, and warehouses');

-- Warehouse Operator Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'WarehouseOperator')
    INSERT INTO [dbo].[Roles] ([RoleName], [Description]) 
    VALUES ('WarehouseOperator', 'Execute stock movements and view stock');

-- Viewer Role
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE [RoleName] = 'Viewer')
    INSERT INTO [dbo].[Roles] ([RoleName], [Description]) 
    VALUES ('Viewer', 'Read-only access to view data');

PRINT 'Roles inserted successfully.';
GO

-- ============================================
-- ASSIGN ALL PERMISSIONS TO ADMINISTRATOR
-- ============================================

PRINT 'Assigning permissions to Administrator role...';

DECLARE @AdminRoleId INT;
SELECT @AdminRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator';

INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT @AdminRoleId, [PermissionId]
FROM [dbo].[Permissions]
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[RolePermissions] 
    WHERE [RoleId] = @AdminRoleId AND [PermissionId] = [Permissions].[PermissionId]
);

PRINT 'Permissions assigned to Administrator role.';
GO

-- ============================================
-- ASSIGN PERMISSIONS TO WAREHOUSE MANAGER
-- ============================================

PRINT 'Assigning permissions to WarehouseManager role...';

DECLARE @ManagerRoleId INT;
SELECT @ManagerRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'WarehouseManager';

INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT @ManagerRoleId, [PermissionId]
FROM [dbo].[Permissions]
WHERE [PermissionCode] IN (
    'Products.View', 'Products.Create', 'Products.Edit', 'Products.Delete',
    'Warehouses.View', 'Warehouses.Create', 'Warehouses.Edit', 'Warehouses.Delete',
    'Stock.View', 'Stock.Receive', 'Stock.Issue', 'Stock.Transfer', 'Stock.Adjust',
    'Audit.View'
)
AND NOT EXISTS (
    SELECT 1 FROM [dbo].[RolePermissions] 
    WHERE [RoleId] = @ManagerRoleId AND [PermissionId] = [Permissions].[PermissionId]
);

PRINT 'Permissions assigned to WarehouseManager role.';
GO

-- ============================================
-- ASSIGN PERMISSIONS TO WAREHOUSE OPERATOR
-- ============================================

PRINT 'Assigning permissions to WarehouseOperator role...';

DECLARE @OperatorRoleId INT;
SELECT @OperatorRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'WarehouseOperator';

INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT @OperatorRoleId, [PermissionId]
FROM [dbo].[Permissions]
WHERE [PermissionCode] IN (
    'Products.View',
    'Warehouses.View',
    'Stock.View', 'Stock.Receive', 'Stock.Issue', 'Stock.Transfer'
)
AND NOT EXISTS (
    SELECT 1 FROM [dbo].[RolePermissions] 
    WHERE [RoleId] = @OperatorRoleId AND [PermissionId] = [Permissions].[PermissionId]
);

PRINT 'Permissions assigned to WarehouseOperator role.';
GO

-- ============================================
-- ASSIGN PERMISSIONS TO VIEWER
-- ============================================

PRINT 'Assigning permissions to Viewer role...';

DECLARE @ViewerRoleId INT;
SELECT @ViewerRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'Viewer';

INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
SELECT @ViewerRoleId, [PermissionId]
FROM [dbo].[Permissions]
WHERE [PermissionCode] IN (
    'Products.View',
    'Warehouses.View',
    'Stock.View',
    'Audit.View'
)
AND NOT EXISTS (
    SELECT 1 FROM [dbo].[RolePermissions] 
    WHERE [RoleId] = @ViewerRoleId AND [PermissionId] = [Permissions].[PermissionId]
);

PRINT 'Permissions assigned to Viewer role.';
GO

-- ============================================
-- CREATE DEFAULT ADMIN USER
-- Password: Admin123! 
-- Hash and Salt are pre-computed (in real implementation, use proper password hashing)
-- ============================================

PRINT 'Creating default admin user...';

-- NOTE: These values are examples. In production, implement proper password hashing in C#
-- For demo purposes: Password = "Admin123!"
-- In C# code, use proper PBKDF2 or BCrypt hashing
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [dbo].[Users] 
    ([Username], [PasswordHash], [PasswordSalt], [FullName], [Email], [IsActive]) 
    VALUES 
    ('admin', 
     'HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP', 
     'SALT_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
     'System Administrator', 
     'admin@stockmanager.com', 
     1);
     
    DECLARE @AdminUserId INT = SCOPE_IDENTITY();
    DECLARE @AdminRoleIdForUser INT;
    SELECT @AdminRoleIdForUser = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator';
    
    INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId], [AssignedBy])
    VALUES (@AdminUserId, @AdminRoleIdForUser, @AdminUserId);
    
    PRINT 'Admin user created with username: admin';
    PRINT 'IMPORTANT: The password hash needs to be initialized on first run of the application.';
END
ELSE
BEGIN
    PRINT 'Admin user already exists.';
END
GO

-- ============================================
-- SEED SAMPLE WAREHOUSES
-- ============================================

PRINT 'Inserting sample warehouses...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Warehouses] WHERE [Code] = 'WH001')
    INSERT INTO [dbo].[Warehouses] ([Code], [Name], [Address], [CreatedBy]) 
    VALUES ('WH001', 'Main Warehouse', 'Av. Corrientes 1234, CABA', 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Warehouses] WHERE [Code] = 'WH002')
    INSERT INTO [dbo].[Warehouses] ([Code], [Name], [Address], [CreatedBy]) 
    VALUES ('WH002', 'Secondary Warehouse', 'Av. Rivadavia 5678, CABA', 1);

PRINT 'Sample warehouses inserted.';
GO

-- ============================================
-- SEED SAMPLE PRODUCTS
-- ============================================

PRINT 'Inserting sample products...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [SKU] = 'CASE-IP14-BLK')
    INSERT INTO [dbo].[Products] ([SKU], [Name], [Description], [Category], [UnitPrice], [MinStockLevel], [CreatedBy]) 
    VALUES ('CASE-IP14-BLK', 'iPhone 14 Case Black', 'Silicone case for iPhone 14 - Black', 'Cases', 15.99, 10, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [SKU] = 'GLASS-IP14')
    INSERT INTO [dbo].[Products] ([SKU], [Name], [Description], [Category], [UnitPrice], [MinStockLevel], [CreatedBy]) 
    VALUES ('GLASS-IP14', 'iPhone 14 Screen Protector', 'Tempered glass screen protector for iPhone 14', 'Screen Protectors', 8.99, 20, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [SKU] = 'CASE-SAM-S23')
    INSERT INTO [dbo].[Products] ([SKU], [Name], [Description], [Category], [UnitPrice], [MinStockLevel], [CreatedBy]) 
    VALUES ('CASE-SAM-S23', 'Samsung S23 Case', 'Protective case for Samsung Galaxy S23', 'Cases', 14.99, 10, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [SKU] = 'SPK-BT-01')
    INSERT INTO [dbo].[Products] ([SKU], [Name], [Description], [Category], [UnitPrice], [MinStockLevel], [CreatedBy]) 
    VALUES ('SPK-BT-01', 'Bluetooth Speaker Mini', 'Portable Bluetooth speaker', 'Speakers', 29.99, 5, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [SKU] = 'CHARGER-USBC-20W')
    INSERT INTO [dbo].[Products] ([SKU], [Name], [Description], [Category], [UnitPrice], [MinStockLevel], [CreatedBy]) 
    VALUES ('CHARGER-USBC-20W', 'USB-C Charger 20W', 'Fast charger USB-C 20W', 'Chargers', 12.99, 15, 1);

PRINT 'Sample products inserted.';
GO

-- ============================================
-- SEED BASIC TRANSLATIONS (Spanish & English)
-- ============================================

PRINT 'Inserting translations...';

-- Common UI Labels
INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Login', 'es', 'Iniciar Sesión', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Login' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Login', 'en', 'Login', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Login' AND [Language] = 'en');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Username', 'es', 'Usuario', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Username' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Username', 'en', 'Username', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Username' AND [Language] = 'en');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Password', 'es', 'Contraseña', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Password' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Password', 'en', 'Password', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Password' AND [Language] = 'en');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Save', 'es', 'Guardar', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Save' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Save', 'en', 'Save', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Save' AND [Language] = 'en');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Cancel', 'es', 'Cancelar', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Cancel' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Common.Cancel', 'en', 'Cancel', 'Common' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Common.Cancel' AND [Language] = 'en');

-- Menu items
INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Menu.Users', 'es', 'Usuarios', 'Menu' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Menu.Users' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Menu.Users', 'en', 'Users', 'Menu' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Menu.Users' AND [Language] = 'en');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Menu.Products', 'es', 'Productos', 'Menu' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Menu.Products' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Menu.Products', 'en', 'Products', 'Menu' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Menu.Products' AND [Language] = 'en');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Menu.Stock', 'es', 'Inventario', 'Menu' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Menu.Stock' AND [Language] = 'es');

INSERT INTO [dbo].[Translations] ([ResourceKey], [Language], [ResourceValue], [Module])
SELECT 'Menu.Stock', 'en', 'Stock', 'Menu' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Translations] WHERE [ResourceKey] = 'Menu.Stock' AND [Language] = 'en');

PRINT 'Translations inserted.';
GO

PRINT '';
PRINT '================================================';
PRINT 'SEED DATA COMPLETED SUCCESSFULLY!';
PRINT '================================================';
PRINT 'Default Admin User:';
PRINT '  Username: admin';
PRINT '  Password: Will be set on first application run';
PRINT '';
PRINT 'Sample Data Created:';
PRINT '  - 4 Roles (Administrator, WarehouseManager, WarehouseOperator, Viewer)';
PRINT '  - 24 Permissions across all modules';
PRINT '  - 2 Warehouses';
PRINT '  - 5 Sample Products';
PRINT '  - Basic translations (ES/EN)';
PRINT '================================================';
GO
