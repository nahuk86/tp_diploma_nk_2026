-- ============================================
-- Stock Management System - Database Schema
-- .NET Framework 4.8 - SQL Server
-- ============================================

USE master;
GO

-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'StockManagerDB')
BEGIN
    CREATE DATABASE StockManagerDB;
END
GO

USE StockManagerDB;
GO

-- ============================================
-- USERS & SECURITY TABLES
-- ============================================

-- Users table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserId] INT PRIMARY KEY IDENTITY(1,1),
        [Username] NVARCHAR(50) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(255) NOT NULL,
        [PasswordSalt] NVARCHAR(255) NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [UpdatedAt] DATETIME NULL,
        [UpdatedBy] INT NULL,
        [LastLogin] DATETIME NULL
    );
    
    CREATE NONCLUSTERED INDEX IX_Users_Username ON [dbo].[Users]([Username]) WHERE [IsActive] = 1;
    CREATE NONCLUSTERED INDEX IX_Users_Email ON [dbo].[Users]([Email]) WHERE [IsActive] = 1;
END
GO

-- Roles table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Roles] (
        [RoleId] INT PRIMARY KEY IDENTITY(1,1),
        [RoleName] NVARCHAR(50) NOT NULL UNIQUE,
        [Description] NVARCHAR(200) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [UpdatedAt] DATETIME NULL,
        [UpdatedBy] INT NULL
    );
    
    CREATE NONCLUSTERED INDEX IX_Roles_RoleName ON [dbo].[Roles]([RoleName]) WHERE [IsActive] = 1;
END
GO

-- Permissions table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Permissions] (
        [PermissionId] INT PRIMARY KEY IDENTITY(1,1),
        [PermissionCode] NVARCHAR(100) NOT NULL UNIQUE,
        [PermissionName] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(200) NULL,
        [Module] NVARCHAR(50) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
    
    CREATE NONCLUSTERED INDEX IX_Permissions_Code ON [dbo].[Permissions]([PermissionCode]) WHERE [IsActive] = 1;
    CREATE NONCLUSTERED INDEX IX_Permissions_Module ON [dbo].[Permissions]([Module]) WHERE [IsActive] = 1;
END
GO

-- UserRoles (Many-to-Many)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserRoles] (
        [UserRoleId] INT PRIMARY KEY IDENTITY(1,1),
        [UserId] INT NOT NULL,
        [RoleId] INT NOT NULL,
        [AssignedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [AssignedBy] INT NULL,
        CONSTRAINT FK_UserRoles_Users FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId]),
        CONSTRAINT FK_UserRoles_Roles FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([RoleId]),
        CONSTRAINT UQ_UserRoles UNIQUE ([UserId], [RoleId])
    );
    
    CREATE NONCLUSTERED INDEX IX_UserRoles_UserId ON [dbo].[UserRoles]([UserId]);
    CREATE NONCLUSTERED INDEX IX_UserRoles_RoleId ON [dbo].[UserRoles]([RoleId]);
END
GO

-- RolePermissions (Many-to-Many)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolePermissions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RolePermissions] (
        [RolePermissionId] INT PRIMARY KEY IDENTITY(1,1),
        [RoleId] INT NOT NULL,
        [PermissionId] INT NOT NULL,
        [AssignedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [AssignedBy] INT NULL,
        CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles]([RoleId]),
        CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permissions]([PermissionId]),
        CONSTRAINT UQ_RolePermissions UNIQUE ([RoleId], [PermissionId])
    );
    
    CREATE NONCLUSTERED INDEX IX_RolePermissions_RoleId ON [dbo].[RolePermissions]([RoleId]);
    CREATE NONCLUSTERED INDEX IX_RolePermissions_PermissionId ON [dbo].[RolePermissions]([PermissionId]);
END
GO

-- ============================================
-- PRODUCTS & INVENTORY TABLES
-- ============================================

-- Products table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Products] (
        [ProductId] INT PRIMARY KEY IDENTITY(1,1),
        [SKU] NVARCHAR(50) NOT NULL UNIQUE,
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Category] NVARCHAR(50) NOT NULL,
        [UnitPrice] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [MinStockLevel] INT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [UpdatedAt] DATETIME NULL,
        [UpdatedBy] INT NULL
    );
    
    CREATE NONCLUSTERED INDEX IX_Products_SKU ON [dbo].[Products]([SKU]) WHERE [IsActive] = 1;
    CREATE NONCLUSTERED INDEX IX_Products_Name ON [dbo].[Products]([Name]) WHERE [IsActive] = 1;
    CREATE NONCLUSTERED INDEX IX_Products_Category ON [dbo].[Products]([Category]) WHERE [IsActive] = 1;
END
GO

-- Warehouses table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Warehouses]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Warehouses] (
        [WarehouseId] INT PRIMARY KEY IDENTITY(1,1),
        [Code] NVARCHAR(20) NOT NULL UNIQUE,
        [Name] NVARCHAR(100) NOT NULL,
        [Address] NVARCHAR(200) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [UpdatedAt] DATETIME NULL,
        [UpdatedBy] INT NULL
    );
    
    CREATE NONCLUSTERED INDEX IX_Warehouses_Code ON [dbo].[Warehouses]([Code]) WHERE [IsActive] = 1;
END
GO

-- Stock table (current stock by product and warehouse)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Stock]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Stock] (
        [StockId] INT PRIMARY KEY IDENTITY(1,1),
        [ProductId] INT NOT NULL,
        [WarehouseId] INT NOT NULL,
        [Quantity] INT NOT NULL DEFAULT 0,
        [LastUpdated] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedBy] INT NULL,
        CONSTRAINT FK_Stock_Products FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products]([ProductId]),
        CONSTRAINT FK_Stock_Warehouses FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouses]([WarehouseId]),
        CONSTRAINT UQ_Stock_Product_Warehouse UNIQUE ([ProductId], [WarehouseId])
    );
    
    CREATE NONCLUSTERED INDEX IX_Stock_ProductId ON [dbo].[Stock]([ProductId]);
    CREATE NONCLUSTERED INDEX IX_Stock_WarehouseId ON [dbo].[Stock]([WarehouseId]);
END
GO

-- StockMovements table (header/master record)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StockMovements]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StockMovements] (
        [MovementId] INT PRIMARY KEY IDENTITY(1,1),
        [MovementNumber] NVARCHAR(20) NOT NULL UNIQUE,
        [MovementType] NVARCHAR(20) NOT NULL, -- 'IN', 'OUT', 'TRANSFER', 'ADJUSTMENT'
        [MovementDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [SourceWarehouseId] INT NULL,
        [DestinationWarehouseId] INT NULL,
        [Reason] NVARCHAR(500) NULL,
        [Notes] NVARCHAR(1000) NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NOT NULL,
        CONSTRAINT FK_StockMovements_SourceWarehouse FOREIGN KEY ([SourceWarehouseId]) REFERENCES [dbo].[Warehouses]([WarehouseId]),
        CONSTRAINT FK_StockMovements_DestinationWarehouse FOREIGN KEY ([DestinationWarehouseId]) REFERENCES [dbo].[Warehouses]([WarehouseId]),
        CONSTRAINT FK_StockMovements_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users]([UserId])
    );
    
    CREATE NONCLUSTERED INDEX IX_StockMovements_Number ON [dbo].[StockMovements]([MovementNumber]);
    CREATE NONCLUSTERED INDEX IX_StockMovements_Type ON [dbo].[StockMovements]([MovementType]);
    CREATE NONCLUSTERED INDEX IX_StockMovements_Date ON [dbo].[StockMovements]([MovementDate]);
END
GO

-- StockMovementLines table (detail/line items)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StockMovementLines]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StockMovementLines] (
        [LineId] INT PRIMARY KEY IDENTITY(1,1),
        [MovementId] INT NOT NULL,
        [ProductId] INT NOT NULL,
        [Quantity] INT NOT NULL,
        [UnitPrice] DECIMAL(18,2) NULL,
        CONSTRAINT FK_StockMovementLines_Movements FOREIGN KEY ([MovementId]) REFERENCES [dbo].[StockMovements]([MovementId]),
        CONSTRAINT FK_StockMovementLines_Products FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products]([ProductId])
    );
    
    CREATE NONCLUSTERED INDEX IX_StockMovementLines_MovementId ON [dbo].[StockMovementLines]([MovementId]);
    CREATE NONCLUSTERED INDEX IX_StockMovementLines_ProductId ON [dbo].[StockMovementLines]([ProductId]);
END
GO

-- ============================================
-- AUDIT & LOGGING TABLES
-- ============================================

-- AuditLog table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AuditLog] (
        [AuditId] INT PRIMARY KEY IDENTITY(1,1),
        [TableName] NVARCHAR(100) NOT NULL,
        [RecordId] INT NOT NULL,
        [Action] NVARCHAR(20) NOT NULL, -- 'INSERT', 'UPDATE', 'DELETE'
        [FieldName] NVARCHAR(100) NULL,
        [OldValue] NVARCHAR(MAX) NULL,
        [NewValue] NVARCHAR(MAX) NULL,
        [ChangedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ChangedBy] INT NULL,
        CONSTRAINT FK_AuditLog_ChangedBy FOREIGN KEY ([ChangedBy]) REFERENCES [dbo].[Users]([UserId])
    );
    
    CREATE NONCLUSTERED INDEX IX_AuditLog_Table ON [dbo].[AuditLog]([TableName], [RecordId]);
    CREATE NONCLUSTERED INDEX IX_AuditLog_Date ON [dbo].[AuditLog]([ChangedAt]);
END
GO

-- AppLog table (for application logging)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppLog] (
        [LogId] INT PRIMARY KEY IDENTITY(1,1),
        [LogLevel] NVARCHAR(20) NOT NULL, -- 'DEBUG', 'INFO', 'WARNING', 'ERROR', 'FATAL'
        [Logger] NVARCHAR(100) NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        [Exception] NVARCHAR(MAX) NULL,
        [Username] NVARCHAR(50) NULL,
        [MachineName] NVARCHAR(100) NULL,
        [LoggedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
    
    CREATE NONCLUSTERED INDEX IX_AppLog_Level ON [dbo].[AppLog]([LogLevel]);
    CREATE NONCLUSTERED INDEX IX_AppLog_Date ON [dbo].[AppLog]([LoggedAt]);
END
GO

-- ============================================
-- CLIENTS TABLE
-- ============================================

-- Clients table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Clients] (
        [ClientId] INT PRIMARY KEY IDENTITY(1,1),
        [Nombre] NVARCHAR(100) NOT NULL,
        [Apellido] NVARCHAR(100) NOT NULL,
        [Correo] NVARCHAR(100) NULL,
        [DNI] NVARCHAR(20) NOT NULL UNIQUE,
        [Telefono] NVARCHAR(20) NULL,
        [Direccion] NVARCHAR(200) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [CreatedBy] INT NULL,
        [UpdatedAt] DATETIME NULL,
        [UpdatedBy] INT NULL,
        CONSTRAINT FK_Clients_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[Users]([UserId]),
        CONSTRAINT FK_Clients_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[Users]([UserId])
    );
    
    CREATE NONCLUSTERED INDEX IX_Clients_DNI ON [dbo].[Clients]([DNI]) WHERE [IsActive] = 1;
    CREATE NONCLUSTERED INDEX IX_Clients_Nombre ON [dbo].[Clients]([Nombre]) WHERE [IsActive] = 1;
    CREATE NONCLUSTERED INDEX IX_Clients_Apellido ON [dbo].[Clients]([Apellido]) WHERE [IsActive] = 1;
END
GO

-- ============================================
-- LOCALIZATION TABLE
-- ============================================

-- Translations table for multi-language support
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Translations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Translations] (
        [TranslationId] INT PRIMARY KEY IDENTITY(1,1),
        [ResourceKey] NVARCHAR(100) NOT NULL,
        [Language] NVARCHAR(10) NOT NULL, -- 'es', 'en'
        [ResourceValue] NVARCHAR(500) NOT NULL,
        [Module] NVARCHAR(50) NULL,
        CONSTRAINT UQ_Translations UNIQUE ([ResourceKey], [Language])
    );
    
    CREATE NONCLUSTERED INDEX IX_Translations_Key ON [dbo].[Translations]([ResourceKey]);
    CREATE NONCLUSTERED INDEX IX_Translations_Language ON [dbo].[Translations]([Language]);
END
GO

PRINT 'Database schema created successfully!';
GO
