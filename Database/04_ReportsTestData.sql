-- ============================================
-- Stock Management System - Reports Test Data
-- Comprehensive seed data for testing all 8 reports
-- ============================================

USE StockManagerDB;
GO

PRINT '================================================';
PRINT 'STARTING REPORTS TEST DATA SEED';
PRINT '================================================';
PRINT '';

-- ============================================
-- SEED CLIENTS
-- ============================================

PRINT 'Inserting clients...';

-- Clear existing clients if reseeding
DELETE FROM [dbo].[Clients] WHERE ClientId > 0;
DBCC CHECKIDENT ('[dbo].[Clients]', RESEED, 0);

INSERT INTO [dbo].[Clients] ([Nombre], [Apellido], [DNI], [Correo], [Telefono], [Direccion], [CreatedBy]) VALUES
('Juan', 'Pérez', '20345678', 'juan.perez@email.com', '11-4567-8901', 'Av. Corrientes 1234, CABA', 1),
('María', 'González', '27456789', 'maria.gonzalez@email.com', '11-4567-8902', 'Av. Santa Fe 567, CABA', 1),
('Carlos', 'Rodríguez', '30567890', 'carlos.rodriguez@email.com', '11-4567-8903', 'Av. Rivadavia 890, CABA', 1),
('Ana', 'Martínez', '33678901', 'ana.martinez@email.com', '11-4567-8904', 'Av. Cabildo 234, CABA', 1),
('Luis', 'Fernández', '25789012', 'luis.fernandez@email.com', '11-4567-8905', 'Av. Belgrano 456, CABA', 1),
('Laura', 'López', '28890123', 'laura.lopez@email.com', '11-4567-8906', 'Av. Callao 678, CABA', 1),
('Pedro', 'García', '31901234', 'pedro.garcia@email.com', '11-4567-8907', 'Av. Pueyrredón 901, CABA', 1),
('Sofía', 'Díaz', '26012345', 'sofia.diaz@email.com', '11-4567-8908', 'Av. Las Heras 123, CABA', 1),
('Diego', 'Sánchez', '29123456', 'diego.sanchez@email.com', '11-4567-8909', 'Av. Córdoba 345, CABA', 1),
('Valentina', 'Romero', '32234567', 'valentina.romero@email.com', '11-4567-8910', 'Av. 9 de Julio 567, CABA', 1),
('Mateo', 'Torres', '24345678', 'mateo.torres@email.com', '11-4567-8911', 'Av. San Martín 789, CABA', 1),
('Lucía', 'Flores', '27456781', 'lucia.flores@email.com', '11-4567-8912', 'Av. Libertador 1011, CABA', 1),
('Tomás', 'Benítez', '30567892', 'tomas.benitez@email.com', '11-4567-8913', 'Av. del Libertador 1213, CABA', 1),
('Emma', 'Vargas', '33678903', 'emma.vargas@email.com', '11-4567-8914', 'Av. Figueroa Alcorta 1415, CABA', 1),
('Martín', 'Castro', '25789014', 'martin.castro@email.com', '11-4567-8915', 'Av. Paseo Colón 1617, CABA', 1);

PRINT 'Clients inserted: 15';
GO

-- ============================================
-- SEED MORE PRODUCTS
-- ============================================

PRINT 'Inserting additional products...';

-- Clear existing products if reseeding (except sample ones)
-- Use soft delete to avoid foreign key constraint violations
UPDATE [dbo].[Products] SET IsActive = 0 WHERE ProductId > 5;

INSERT INTO [dbo].[Products] ([SKU], [Name], [Description], [Category], [UnitPrice], [MinStockLevel], [CreatedBy]) VALUES
-- Electronics - More variety
('EARBUDS-BT-01', 'Wireless Earbuds', 'Bluetooth wireless earbuds with charging case', 'Audio', 49.99, 10, 1),
('CABLE-USBC-2M', 'USB-C Cable 2m', 'USB-C to USB-C cable, 2 meters', 'Cables', 9.99, 25, 1),
('CABLE-LIGHT-1M', 'Lightning Cable 1m', 'Lightning to USB cable, 1 meter', 'Cables', 8.99, 25, 1),
('ADAPTER-USBC-HDMI', 'USB-C to HDMI Adapter', 'USB-C to HDMI adapter for displays', 'Adapters', 19.99, 8, 1),
('HOLDER-CAR-01', 'Car Phone Holder', 'Universal car phone holder', 'Holders', 14.99, 15, 1),

-- Cases - More models
('CASE-IP13-RED', 'iPhone 13 Case Red', 'Silicone case for iPhone 13 - Red', 'Cases', 14.99, 10, 1),
('CASE-IP13-BLU', 'iPhone 13 Case Blue', 'Silicone case for iPhone 13 - Blue', 'Cases', 14.99, 10, 1),
('CASE-SAM-A54', 'Samsung A54 Case', 'Protective case for Samsung Galaxy A54', 'Cases', 12.99, 12, 1),
('CASE-XIAOMI-12', 'Xiaomi 12 Case', 'Protective case for Xiaomi 12', 'Cases', 11.99, 12, 1),

-- Screen Protectors
('GLASS-IP13', 'iPhone 13 Screen Protector', 'Tempered glass for iPhone 13', 'Screen Protectors', 7.99, 20, 1),
('GLASS-SAM-A54', 'Samsung A54 Screen Protector', 'Tempered glass for Samsung A54', 'Screen Protectors', 7.99, 20, 1),
('GLASS-XIAOMI-12', 'Xiaomi 12 Screen Protector', 'Tempered glass for Xiaomi 12', 'Screen Protectors', 6.99, 20, 1),

-- Power & Chargers
('CHARGER-FAST-30W', 'Fast Charger 30W', 'USB-C fast charger 30W', 'Chargers', 16.99, 12, 1),
('POWERBANK-10K', 'Power Bank 10000mAh', 'Portable power bank 10000mAh', 'Power Banks', 24.99, 8, 1),
('POWERBANK-20K', 'Power Bank 20000mAh', 'Portable power bank 20000mAh', 'Power Banks', 39.99, 5, 1),

-- Audio - More options
('SPK-BT-MINI-02', 'Bluetooth Speaker Mini Pro', 'Enhanced mini Bluetooth speaker', 'Speakers', 34.99, 5, 1),
('HEADPHONES-01', 'Over-Ear Headphones', 'Wired over-ear headphones', 'Audio', 29.99, 8, 1),
('HEADPHONES-BT-02', 'Bluetooth Headphones', 'Wireless Bluetooth headphones', 'Audio', 59.99, 6, 1),

-- Accessories
('STAND-TABLET-01', 'Tablet Stand', 'Adjustable tablet stand', 'Holders', 19.99, 10, 1),
('MOUSE-WIRELESS-01', 'Wireless Mouse', 'Wireless optical mouse', 'Peripherals', 12.99, 15, 1),
('KEYBOARD-BT-01', 'Bluetooth Keyboard', 'Compact Bluetooth keyboard', 'Peripherals', 34.99, 8, 1);

PRINT 'Additional products inserted: 21';
GO

-- ============================================
-- SEED WAREHOUSES (if not exist)
-- ============================================

PRINT 'Checking warehouses...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[Warehouses] WHERE [Code] = 'WH003')
    INSERT INTO [dbo].[Warehouses] ([Code], [Name], [Address], [CreatedBy]) 
    VALUES ('WH003', 'Distribution Center', 'Av. Gral. Paz 9999, CABA', 1);

PRINT 'Warehouses ready: 3';
GO

-- ============================================
-- SEED STOCK MOVEMENTS (Initial Stock)
-- ============================================

PRINT 'Creating initial stock movements...';

DECLARE @AdminUserId INT = 1;
DECLARE @MainWarehouseId INT = (SELECT TOP 1 WarehouseId FROM [dbo].[Warehouses] WHERE Code = 'WH001');
DECLARE @SecondaryWarehouseId INT = (SELECT TOP 1 WarehouseId FROM [dbo].[Warehouses] WHERE Code = 'WH002');
DECLARE @DistributionWarehouseId INT = (SELECT TOP 1 WarehouseId FROM [dbo].[Warehouses] WHERE Code = 'WH003');

-- Movement 1: Initial stock to Main Warehouse
INSERT INTO [dbo].[StockMovements] ([MovementNumber], [MovementType], [MovementDate], [SourceWarehouseId], [DestinationWarehouseId], [Notes], [CreatedBy])
VALUES ('SM-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-001', 'IN', DATEADD(DAY, -90, GETDATE()), NULL, @MainWarehouseId, 'Initial stock - Main Warehouse', @AdminUserId);

DECLARE @Movement1Id INT = SCOPE_IDENTITY();

-- Add lines for Movement 1 (varied quantities)
INSERT INTO [dbo].[StockMovementLines] ([MovementId], [ProductId], [Quantity])
SELECT @Movement1Id, ProductId, 
    CASE 
        WHEN Category = 'Cases' THEN 50
        WHEN Category = 'Screen Protectors' THEN 100
        WHEN Category = 'Chargers' THEN 40
        WHEN Category = 'Speakers' THEN 20
        WHEN Category = 'Audio' THEN 30
        WHEN Category = 'Cables' THEN 80
        WHEN Category = 'Power Banks' THEN 25
        ELSE 30
    END
FROM [dbo].[Products]
WHERE ProductId <= 15; -- First batch of products

-- Movement 2: Initial stock to Secondary Warehouse
INSERT INTO [dbo].[StockMovements] ([MovementNumber], [MovementType], [MovementDate], [SourceWarehouseId], [DestinationWarehouseId], [Notes], [CreatedBy])
VALUES ('SM-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-002', 'IN', DATEADD(DAY, -85, GETDATE()), NULL, @SecondaryWarehouseId, 'Initial stock - Secondary Warehouse', @AdminUserId);

DECLARE @Movement2Id INT = SCOPE_IDENTITY();

-- Add lines for Movement 2
INSERT INTO [dbo].[StockMovementLines] ([MovementId], [ProductId], [Quantity])
SELECT @Movement2Id, ProductId, 
    CASE 
        WHEN Category = 'Cases' THEN 40
        WHEN Category = 'Screen Protectors' THEN 80
        WHEN Category = 'Chargers' THEN 30
        WHEN Category = 'Speakers' THEN 15
        WHEN Category = 'Audio' THEN 25
        ELSE 25
    END
FROM [dbo].[Products]
WHERE ProductId <= 20;

-- Movement 3: Initial stock to Distribution Center
INSERT INTO [dbo].[StockMovements] ([MovementNumber], [MovementType], [MovementDate], [SourceWarehouseId], [DestinationWarehouseId], [Notes], [CreatedBy])
VALUES ('SM-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-003', 'IN', DATEADD(DAY, -80, GETDATE()), NULL, @DistributionWarehouseId, 'Initial stock - Distribution Center', @AdminUserId);

DECLARE @Movement3Id INT = SCOPE_IDENTITY();

-- Add lines for Movement 3
INSERT INTO [dbo].[StockMovementLines] ([MovementId], [ProductId], [Quantity])
SELECT @Movement3Id, ProductId, 
    CASE 
        WHEN Category IN ('Cases', 'Screen Protectors') THEN 60
        WHEN Category IN ('Chargers', 'Cables') THEN 50
        ELSE 35
    END
FROM [dbo].[Products];

-- Movement 4: Transfer between warehouses
INSERT INTO [dbo].[StockMovements] ([MovementNumber], [MovementType], [MovementDate], [SourceWarehouseId], [DestinationWarehouseId], [Notes], [CreatedBy])
VALUES ('SM-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-004', 'TRANSFER', DATEADD(DAY, -60, GETDATE()), @MainWarehouseId, @SecondaryWarehouseId, 'Stock transfer for rebalancing', @AdminUserId);

DECLARE @Movement4Id INT = SCOPE_IDENTITY();

INSERT INTO [dbo].[StockMovementLines] ([MovementId], [ProductId], [Quantity])
SELECT @Movement4Id, ProductId, 15
FROM [dbo].[Products]
WHERE Category IN ('Cases', 'Screen Protectors')
AND ProductId <= 10;

-- Movement 5: Stock adjustment
INSERT INTO [dbo].[StockMovements] ([MovementNumber], [MovementType], [MovementDate], [SourceWarehouseId], [DestinationWarehouseId], [Notes], [CreatedBy])
VALUES ('SM-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-005', 'ADJUSTMENT', DATEADD(DAY, -45, GETDATE()), @MainWarehouseId, @MainWarehouseId, 'Inventory adjustment', @AdminUserId);

DECLARE @Movement5Id INT = SCOPE_IDENTITY();

INSERT INTO [dbo].[StockMovementLines] ([MovementId], [ProductId], [Quantity])
VALUES (@Movement5Id, (SELECT TOP 1 ProductId FROM Products WHERE SKU = 'CASE-IP14-BLK'), 5),
       (@Movement5Id, (SELECT TOP 1 ProductId FROM Products WHERE SKU = 'GLASS-IP14'), 10);

PRINT 'Stock movements created: 5';
GO

-- ============================================
-- SEED SALES
-- ============================================

PRINT 'Creating sales transactions...';

DECLARE @Clients TABLE (ClientId INT, ClientName VARCHAR(100));
INSERT INTO @Clients SELECT ClientId, Nombre + ' ' + Apellido FROM [dbo].[Clients];

DECLARE @Products TABLE (ProductId INT, UnitPrice DECIMAL(18,2));
INSERT INTO @Products SELECT ProductId, UnitPrice FROM [dbo].[Products];

DECLARE @Sellers TABLE (SellerName VARCHAR(100));
INSERT INTO @Sellers VALUES ('Juan Pérez'), ('María González'), ('Carlos Rodríguez'), ('Ana Martínez');

-- Generate sales over last 90 days
DECLARE @DaysAgo INT = 90;
DECLARE @CurrentDay INT = 0;
DECLARE @ClientId INT;
DECLARE @SellerName VARCHAR(100);
DECLARE @SaleDate DATE;
DECLARE @SaleId INT;
DECLARE @ProductId INT;
DECLARE @Quantity INT;
DECLARE @UnitPrice DECIMAL(18,2);
DECLARE @LineTotal DECIMAL(18,2);
DECLARE @TotalAmount DECIMAL(18,2);
DECLARE @SaleCounter INT;
DECLARE @SaleNumberCounter INT = 1;

WHILE @CurrentDay < @DaysAgo
BEGIN
    -- Generate 1-3 sales per day (random)
    DECLARE @SalesPerDay INT = 1 + (ABS(CHECKSUM(NEWID())) % 3);
    DECLARE @SaleCounter INT = 0;
    
    WHILE @SaleCounter < @SalesPerDay
    BEGIN
        SET @SaleDate = DATEADD(DAY, -@CurrentDay, GETDATE());
        
        -- Random client
        SELECT TOP 1 @ClientId = ClientId FROM @Clients ORDER BY NEWID();
        
        -- Random seller
        SELECT TOP 1 @SellerName = SellerName FROM @Sellers ORDER BY NEWID();
        
        -- Create sale
        INSERT INTO [dbo].[Sales] ([SaleNumber], [SaleDate], [ClientId], [SellerName], [TotalAmount], [CreatedBy])
        VALUES ('SALE-' + FORMAT(GETDATE(), 'yyyyMMdd') + '-' + RIGHT('000' + CAST(@SaleNumberCounter AS VARCHAR(3)), 3), @SaleDate, @ClientId, @SellerName, 0, 1); -- TotalAmount will be updated
        
        SET @SaleId = SCOPE_IDENTITY();
        SET @TotalAmount = 0;
        SET @SaleNumberCounter = @SaleNumberCounter + 1;
        
        -- Add 1-5 products to this sale
        DECLARE @ProductsInSale INT = 1 + (ABS(CHECKSUM(NEWID())) % 5);
        DECLARE @ProductCounter INT = 0;
        
        WHILE @ProductCounter < @ProductsInSale
        BEGIN
            -- Random product
            SELECT TOP 1 @ProductId = ProductId, @UnitPrice = UnitPrice 
            FROM @Products ORDER BY NEWID();
            
            -- Random quantity (1-10)
            SET @Quantity = 1 + (ABS(CHECKSUM(NEWID())) % 10);
            
            -- Apply small random discount (0-15%)
            SET @UnitPrice = @UnitPrice * (1 - (ABS(CHECKSUM(NEWID())) % 16) / 100.0);
            SET @LineTotal = @Quantity * @UnitPrice;
            
            -- Add sale line
            INSERT INTO [dbo].[SaleLines] ([SaleId], [ProductId], [Quantity], [UnitPrice], [LineTotal])
            VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @LineTotal);
            
            SET @TotalAmount = @TotalAmount + @LineTotal;
            SET @ProductCounter = @ProductCounter + 1;
        END
        
        -- Update sale total
        UPDATE [dbo].[Sales] SET TotalAmount = @TotalAmount WHERE SaleId = @SaleId;
        
        SET @SaleCounter = @SaleCounter + 1;
    END
    
    SET @CurrentDay = @CurrentDay + 1;
END

PRINT 'Sales transactions created successfully';
GO

-- ============================================
-- DISPLAY SUMMARY
-- ============================================

PRINT '';
PRINT '================================================';
PRINT 'REPORTS TEST DATA SEED COMPLETED!';
PRINT '================================================';
PRINT '';

SELECT 'Clients' AS Entity, COUNT(*) AS Count FROM [dbo].[Clients] WHERE IsActive = 1
UNION ALL
SELECT 'Products', COUNT(*) FROM [dbo].[Products] WHERE IsActive = 1
UNION ALL
SELECT 'Warehouses', COUNT(*) FROM [dbo].[Warehouses] WHERE IsActive = 1
UNION ALL
SELECT 'Stock Movements', COUNT(*) FROM [dbo].[StockMovements]
UNION ALL
SELECT 'Sales', COUNT(*) FROM [dbo].[Sales] WHERE IsActive = 1
UNION ALL
SELECT 'Sale Lines', COUNT(*) FROM [dbo].[SaleLines];

PRINT '';
PRINT 'Data Summary:';
PRINT '  - 15 Clients with realistic information';
PRINT '  - 26 Products across multiple categories';
PRINT '  - 3 Warehouses';
PRINT '  - 5 Stock movements (initial stock + transfers)';
PRINT '  - 90-270 Sales over last 90 days';
PRINT '  - Multiple sale lines per sale';
PRINT '  - 4 Different sellers';
PRINT '';
PRINT 'All 8 reports should now return data:';
PRINT '  1. Top Products - Multiple products sold';
PRINT '  2. Client Purchases - 15 clients with purchases';
PRINT '  3. Price Variation - Various prices per product';
PRINT '  4. Seller Performance - 4 sellers tracked';
PRINT '  5. Category Sales - Multiple categories';
PRINT '  6. Revenue by Date - 90 days of data';
PRINT '  7. Client Product Ranking - Client-product combinations';
PRINT '  8. Client Ticket Average - Multiple purchases per client';
PRINT '================================================';
GO
