# Reports Management Process - Class Diagrams (Per Use Case)

This document contains UML Class Diagrams organized per use case for all Report operations.

---

## UC-01: GetCategorySalesReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        -IAuthorizationService _authService
        -ILogService _logService
        +LoadCategorySalesReport(from, to) void
    }

    class ReportService {
        -IReportRepository _reportRepo
        -ILogService _logService
        +GetCategorySalesReport(startDate, endDate, orderBy) List~CategorySalesReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetCategorySalesReport(startDate, endDate, orderBy) List~CategorySalesReportDTO~
    }

    class ReportRepository {
        -DatabaseHelper _db
        +GetCategorySalesReport(startDate, endDate, orderBy) List~CategorySalesReportDTO~
        -MapCategorySalesReport(reader) CategorySalesReportDTO
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class CategorySalesReportDTO {
        +string Category
        +int TotalUnits
        +decimal TotalRevenue
        +int TransactionCount
        +decimal AveragePrice
        +decimal RevenuePercentage
    }

    ReportsForm --> ReportService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> DatabaseHelper : uses
    ReportRepository --> CategorySalesReportDTO : returns
```

---

## UC-02: GetClientProductRankingReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        +LoadClientProductRankingReport(clientId) void
    }

    class ReportService {
        -IReportRepository _reportRepo
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
    }

    class ReportRepository {
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
        -MapClientProductRankingReport(reader) ClientProductRankingReportDTO
    }

    class ClientProductRankingReportDTO {
        +int ClientId
        +string ClientName
        +string ProductName
        +string SKU
        +int TotalQuantity
        +decimal TotalSpent
        +int PurchaseCount
        +int Rank
    }

    ReportsForm --> ReportService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> ClientProductRankingReportDTO : returns
```

---

## UC-03: GetClientPurchasesReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        +LoadClientPurchasesReport(clientId, from, to) void
    }

    class ReportService {
        -IReportRepository _reportRepo
        +GetClientPurchasesReport(startDate, endDate, clientId, topN) List~ClientPurchasesReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetClientPurchasesReport(startDate, endDate, clientId, topN) List~ClientPurchasesReportDTO~
    }

    class ReportRepository {
        +GetClientPurchasesReport(startDate, endDate, clientId, topN) List~ClientPurchasesReportDTO~
        -MapClientPurchasesReport(reader) ClientPurchasesReportDTO
    }

    class ClientPurchasesReportDTO {
        +string ClientName
        +string ClientDNI
        +int TotalPurchases
        +decimal TotalSpent
        +DateTime LastPurchaseDate
        +decimal AverageTicket
    }

    ReportsForm --> ReportService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> ClientPurchasesReportDTO : returns
```

---

## UC-04: GetPriceVariationReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        +LoadPriceVariationReport(productId, from, to) void
    }

    class ReportService {
        -IReportRepository _reportRepo
        +GetPriceVariationReport(startDate, endDate, productId, category) List~PriceVariationReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetPriceVariationReport(startDate, endDate, productId, category) List~PriceVariationReportDTO~
    }

    class ReportRepository {
        +GetPriceVariationReport(startDate, endDate, productId, category) List~PriceVariationReportDTO~
        -MapPriceVariationReport(reader) PriceVariationReportDTO
    }

    class PriceVariationReportDTO {
        +string ProductName
        +string SKU
        +string Category
        +DateTime SaleDate
        +decimal UnitPrice
        +decimal PriceVariation
        +decimal PercentageChange
    }

    ReportsForm --> ReportService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> PriceVariationReportDTO : returns
```

---

## UC-05: GetRevenueByDateReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        +LoadRevenueByDateReport(from, to, groupBy) void
    }

    class ReportService {
        -IReportRepository _reportRepo
        +GetRevenueByDateReport(startDate, endDate, groupBy) List~RevenueByDateReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetRevenueByDateReport(startDate, endDate, groupBy) List~RevenueByDateReportDTO~
    }

    class ReportRepository {
        +GetRevenueByDateReport(startDate, endDate, groupBy) List~RevenueByDateReportDTO~
    }

    class RevenueByDateReportDTO {
        +DateTime PeriodDate
        +string PeriodLabel
        +decimal TotalRevenue
        +int TotalSales
        +int TotalItemsSold
    }

    ReportsForm --> ReportService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> RevenueByDateReportDTO : returns
```

---

## UC-06: GetSellerPerformanceReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        +LoadSellerPerformanceReport(from, to) void
    }

    class ReportService {
        -IReportRepository _reportRepo
        +GetSellerPerformanceReport(startDate, endDate, sellerName, category) List~SellerPerformanceReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetSellerPerformanceReport(startDate, endDate, sellerName, category) List~SellerPerformanceReportDTO~
    }

    class ReportRepository {
        +GetSellerPerformanceReport(startDate, endDate, sellerName, category) List~SellerPerformanceReportDTO~
        -MapSellerPerformanceReport(reader) SellerPerformanceReportDTO
    }

    class SellerPerformanceReportDTO {
        +string SellerName
        +int SalesCount
        +int TotalUnits
        +decimal TotalRevenue
        +decimal AverageTicket
        +decimal MinSale
        +decimal MaxSale
    }

    ReportsForm --> ReportService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> SellerPerformanceReportDTO : returns
```

---

## UC-07: GetTopProductsReport

```mermaid
classDiagram
    class ReportsForm {
        -ReportService _reportService
        -IAuthorizationService _authService
        -ILogService _logService
        +LoadTopProductsReport(from, to, category, topN, orderBy) void
        +btnGenerate_Click(sender, e) void
        +btnExport_Click(sender, e) void
        -GenerateReport() void
        -ExportToExcel() void
    }

    class ReportService {
        -IReportRepository _reportRepo
        -ILogService _logService
        +GetTopProductsReport(startDate, endDate, category, topN, orderBy) List~TopProductsReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetTopProductsReport(startDate, endDate, category, topN, orderBy) List~TopProductsReportDTO~
    }

    class ReportRepository {
        +GetTopProductsReport(startDate, endDate, category, topN, orderBy) List~TopProductsReportDTO~
        -MapTopProductsReport(reader) TopProductsReportDTO
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
        +CreateParameter(name, value) SqlParameter
    }

    class IAuthorizationService {
        <<interface>>
        +HasAnyPermission(userId, permissions) bool
    }

    class TopProductsReportDTO {
        +string ProductName
        +string SKU
        +string Category
        +int UnitsSold
        +decimal Revenue
        +int TransactionCount
    }

    ReportsForm --> ReportService : uses
    ReportsForm --> IAuthorizationService : uses
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> DatabaseHelper : uses
    ReportRepository --> TopProductsReportDTO : returns
```

---

## Layer Communication Flow

```
┌────────────────────┐
│    UI LAYER        │  ReportsForm
└─────────┬──────────┘
          │ uses
          ▼
┌────────────────────┐
│   BLL LAYER        │  ReportService
└─────────┬──────────┘
          │ calls
          ▼
┌────────────────────┐
│   DAO LAYER        │  ReportRepository
│                    │  DatabaseHelper
└─────────┬──────────┘
          │ returns
          ▼
┌────────────────────┐
│  DOMAIN LAYER      │  Report DTOs (CategorySalesReportDTO,
│                    │  ClientPurchasesReportDTO, etc.)
└────────────────────┘
```

## Available Reports Summary

| Use Case | Filters | Key Metrics |
|----------|---------|-------------|
| GetCategorySalesReport | Date range, orderBy | Units, revenue, avg price, % of total |
| GetClientProductRankingReport | ClientId | Products ranked by qty/spend |
| GetClientPurchasesReport | ClientId, date range, topN | Total purchases, spend, avg ticket |
| GetPriceVariationReport | ProductId, category, date range | Price at sale, variation, % change |
| GetRevenueByDateReport | Date range, groupBy (Day/Week/Month) | Revenue, sales count, items sold |
| GetSellerPerformanceReport | Date range, seller, category | Sales count, revenue, avg/min/max |
| GetTopProductsReport | Date range, category, topN, orderBy | Units sold, revenue, transaction count |
        -ReportService _reportService
        -IAuthorizationService _authService
        -ILocalizationService _localizationService
        -ILogService _logService
        -ComboBox cboReportType
        -DateTimePicker dtpStartDate
        -DateTimePicker dtpEndDate
        -ComboBox cboCategory
        -ComboBox cboClient
        -ComboBox cboSeller
        -DataGridView dgvReport
        -Button btnGenerate
        -Button btnExport
        -Button btnPrint
        +ReportsForm(services...)
        +LoadReportTypes() void
        +btnGenerate_Click(sender, e) void
        +btnExport_Click(sender, e) void
        +btnPrint_Click(sender, e) void
        +cboReportType_SelectedIndexChanged(sender, e) void
        -ShowReportFilters() void
        -GenerateReport() void
        -ExportToExcel() void
        -PrintReport() void
    }

    %% BLL Layer
    class ReportService {
        -IReportRepository _reportRepo
        -ILogService _logService
        +ReportService(reportRepo, logService)
        +GetTopProductsReport(startDate, endDate, category, topN, orderBy) List~TopProductsReportDTO~
        +GetClientPurchasesReport(startDate, endDate, clientId, topN) List~ClientPurchasesReportDTO~
        +GetPriceVariationReport(startDate, endDate, productId, category) List~PriceVariationReportDTO~
        +GetSellerPerformanceReport(startDate, endDate, sellerName, category) List~SellerPerformanceReportDTO~
        +GetCategorySalesReport(startDate, endDate, orderBy) List~CategorySalesReportDTO~
        +GetLowStockReport(warehouseId, threshold) List~LowStockReportDTO~
        +GetStockMovementsReport(startDate, endDate, warehouseId, movementType) List~StockMovementsReportDTO~
    }

    %% Services Layer
    class IAuthorizationService {
        <<interface>>
        +HasPermission(userId, permission) bool
        +HasAnyPermission(userId, permissions) bool
        +GetUserPermissions(userId) List~Permission~
    }

    class ILogService {
        <<interface>>
        +Info(message) void
        +Warning(message) void
        +Error(message, exception) void
    }

    %% DAO Layer
    class ReportRepository {
        +GetTopProductsReport(startDate, endDate, category, topN, orderBy) List~TopProductsReportDTO~
        +GetClientPurchasesReport(startDate, endDate, clientId, topN) List~ClientPurchasesReportDTO~
        +GetPriceVariationReport(startDate, endDate, productId, category) List~PriceVariationReportDTO~
        +GetSellerPerformanceReport(startDate, endDate, sellerName, category) List~SellerPerformanceReportDTO~
        +GetCategorySalesReport(startDate, endDate, orderBy) List~CategorySalesReportDTO~
        +GetLowStockReport(warehouseId, threshold) List~LowStockReportDTO~
        +GetStockMovementsReport(startDate, endDate, warehouseId, movementType) List~StockMovementsReportDTO~
        -MapTopProductsReport(reader) TopProductsReportDTO
        -MapClientPurchasesReport(reader) ClientPurchasesReportDTO
        -MapPriceVariationReport(reader) PriceVariationReportDTO
        -MapSellerPerformanceReport(reader) SellerPerformanceReportDTO
        -MapCategorySalesReport(reader) CategorySalesReportDTO
        -MapLowStockReport(reader) LowStockReportDTO
        -MapStockMovementsReport(reader) StockMovementsReportDTO
    }

    class IReportRepository {
        <<interface>>
        +GetTopProductsReport(startDate, endDate, category, topN, orderBy) List~TopProductsReportDTO~
        +GetClientPurchasesReport(startDate, endDate, clientId, topN) List~ClientPurchasesReportDTO~
        +GetPriceVariationReport(startDate, endDate, productId, category) List~PriceVariationReportDTO~
        +GetSellerPerformanceReport(startDate, endDate, sellerName, category) List~SellerPerformanceReportDTO~
        +GetCategorySalesReport(startDate, endDate, orderBy) List~CategorySalesReportDTO~
        +GetLowStockReport(warehouseId, threshold) List~LowStockReportDTO~
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
        +CreateParameter(name, value) SqlParameter
    }

    %% Domain Layer - Report DTOs
    class TopProductsReportDTO {
        +string ProductName
        +string SKU
        +string Category
        +int UnitsSold
        +decimal Revenue
        +int TransactionCount
    }

    class ClientPurchasesReportDTO {
        +string ClientName
        +string ClientDNI
        +int TotalPurchases
        +decimal TotalSpent
        +DateTime? LastPurchaseDate
        +decimal AverageTicket
    }

    class PriceVariationReportDTO {
        +string ProductName
        +string SKU
        +string Category
        +DateTime SaleDate
        +decimal UnitPrice
        +decimal PriceVariation
        +decimal PercentageChange
    }

    class SellerPerformanceReportDTO {
        +string SellerName
        +int SalesCount
        +int TotalUnits
        +decimal TotalRevenue
        +decimal AverageTicket
        +decimal MinSale
        +decimal MaxSale
    }

    class CategorySalesReportDTO {
        +string Category
        +int TotalUnits
        +decimal TotalRevenue
        +int TransactionCount
        +decimal AveragePrice
        +decimal RevenuePercentage
    }

    class LowStockReportDTO {
        +string ProductName
        +string SKU
        +string Category
        +string WarehouseName
        +int CurrentStock
        +int MinStockLevel
        +int Deficit
    }

    class StockMovementsReportDTO {
        +string MovementNumber
        +DateTime MovementDate
        +string MovementType
        +string ProductName
        +string SourceWarehouse
        +string DestinationWarehouse
        +int Quantity
        +string CreatedByUser
    }

    %% Relationships
    ReportsForm --> ReportService : uses
    ReportsForm --> IAuthorizationService : uses
    ReportsForm --> ILocalizationService : uses
    ReportsForm --> ILogService : uses
    
    ReportService --> IReportRepository : uses
    ReportService --> ILogService : uses
    
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> DatabaseHelper : uses
    ReportRepository --> TopProductsReportDTO : returns
    ReportRepository --> ClientPurchasesReportDTO : returns
    ReportRepository --> PriceVariationReportDTO : returns
    ReportRepository --> SellerPerformanceReportDTO : returns
    ReportRepository --> CategorySalesReportDTO : returns
    ReportRepository --> LowStockReportDTO : returns
    ReportRepository --> StockMovementsReportDTO : returns
```

## Layer Communication Flow

```
┌────────────────────┐
│    UI LAYER        │  ReportsForm
└─────────┬──────────┘
          │ uses
          ▼
┌────────────────────┐
│   BLL LAYER        │  ReportService
└─────────┬──────────┘
          │ calls
          ├───────────────────┐
          ▼                   ▼
┌────────────────────┐  ┌────────────────────┐
│   DAO LAYER        │  │    SERVICES        │
│                    │  │     LAYER          │
│ ReportRepository   │  │ AuthService        │
│ DatabaseHelper     │  │ LogService         │
└─────────┬──────────┘  └────────────────────┘
          │ returns
          ▼
┌────────────────────┐
│  DOMAIN LAYER      │  Report DTOs:
│                    │  - TopProductsReportDTO
│                    │  - ClientPurchasesReportDTO
│                    │  - PriceVariationReportDTO
│                    │  - SellerPerformanceReportDTO
│                    │  - CategorySalesReportDTO
│                    │  - LowStockReportDTO
│                    │  - StockMovementsReportDTO
└────────────────────┘
```

## Available Reports

### 1. Top Products Report
**Purpose**: Identify best-selling products by units or revenue  
**Filters**:
- Date range (start/end)
- Category (optional)
- Top N products (optional)
- Order by: units sold or revenue

**SQL Complexity**: Complex aggregate query with GROUP BY and ORDER BY

### 2. Client Purchases Report
**Purpose**: Analyze customer purchasing behavior  
**Filters**:
- Date range (start/end)
- Specific client (optional)
- Top N clients (optional)

**Metrics**: Total purchases, total spent, last purchase date, average ticket

### 3. Price Variation Report
**Purpose**: Track product price changes over time  
**Filters**:
- Date range (start/end)
- Specific product (optional)
- Category (optional)

**Metrics**: Price at each sale, variation from previous sale, percentage change

### 4. Seller Performance Report
**Purpose**: Evaluate sales team performance  
**Filters**:
- Date range (start/end)
- Specific seller (optional)
- Category (optional)

**Metrics**: Sales count, total units, revenue, average/min/max sale amounts

### 5. Category Sales Report
**Purpose**: Compare performance across product categories  
**Filters**:
- Date range (start/end)
- Order by: revenue or units

**Metrics**: Total units, revenue, transaction count, average price, revenue percentage

### 6. Low Stock Report
**Purpose**: Identify products below minimum stock levels  
**Filters**:
- Warehouse (optional)
- Custom threshold (optional)

**Metrics**: Current stock, minimum stock level, deficit quantity

### 7. Stock Movements Report
**Purpose**: Track inventory movements and transfers  
**Filters**:
- Date range (start/end)
- Warehouse (optional)
- Movement type (Entry/Exit/Transfer/Adjustment)

**Metrics**: Movement details, quantities, source/destination, user who created

## Permission-Based Access

Reports are protected by role-based permissions:
- **VIEW_REPORTS_GENERAL**: Basic sales reports (Top Products, Category Sales)
- **VIEW_REPORTS_CLIENTS**: Client-related reports (Client Purchases)
- **VIEW_REPORTS_ADVANCED**: Advanced reports (Price Variation, Seller Performance)
- **VIEW_REPORTS_INVENTORY**: Inventory reports (Low Stock, Stock Movements)

## Key Features

1. **Dynamic Filtering**: Each report supports multiple filter combinations
2. **Date Range Support**: All reports support date range filtering
3. **Export Capabilities**: Reports can be exported to Excel, PDF
4. **Permission Control**: Access controlled via authorization service
5. **Logging**: All report generation logged for audit
6. **Performance**: Optimized SQL queries with proper indexing
