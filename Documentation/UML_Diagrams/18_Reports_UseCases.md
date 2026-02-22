# Reports - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Report-related use cases.

---

## UC-01: GetCategorySalesReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        -IAuthorizationService _authService
        -ILogService _logService
        +LoadCategorySalesReport(from, to) void
    }

    class IReportService {
        <<interface>>
        +GetCategorySalesReport(from, to) List~CategorySalesReportDTO~
        +GetTopProductsReport(top, from, to) List~TopProductsReportDTO~
        +GetClientPurchasesReport(clientId, from, to) List~ClientPurchasesReportDTO~
        +GetPriceVariationReport(productId, from, to) List~PriceVariationReportDTO~
        +GetRevenueByDateReport(from, to, groupBy) List~RevenueByDateReportDTO~
        +GetSellerPerformanceReport(from, to) List~SellerPerformanceReportDTO~
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        -ILogService _logService
        +GetCategorySalesReport(from, to) List~CategorySalesReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetCategorySalesReport(from, to) List~CategorySalesReportDTO~
    }

    class ReportRepository {
        -DatabaseHelper _db
        +GetCategorySalesReport(from, to) List~CategorySalesReportDTO~
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class CategorySalesReportDTO {
        +string Category
        +int TotalQuantitySold
        +decimal TotalRevenue
        +int NumberOfProducts
        +int NumberOfSales
    }

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> DatabaseHelper : uses
    ReportRepository --> CategorySalesReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects date range
    UI->>SVC: GetCategorySalesReport(fromDate, toDate)
    activate SVC
    SVC->>REPO: GetCategorySalesReport(from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.Category, SUM(sl.Quantity) AS TotalQty, SUM(sl.LineTotal) AS Revenue FROM SaleLines sl JOIN Products p ON sl.ProductId=p.ProductId JOIN Sales s ON sl.SaleId=s.SaleId WHERE s.SaleDate BETWEEN @From AND @To GROUP BY p.Category
    REPO-->>SVC: List~CategorySalesReportDTO~
    deactivate REPO
    SVC-->>UI: List~CategorySalesReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView
    UI->>UI: Render chart
```

---

## UC-02: GetClientProductRankingReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        +LoadClientProductRankingReport(clientId) void
    }

    class IReportService {
        <<interface>>
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
    }

    class ReportRepository {
        +GetClientProductRankingReport(clientId) List~ClientProductRankingReportDTO~
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

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> ClientProductRankingReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects client
    UI->>SVC: GetClientProductRankingReport(clientId)
    activate SVC
    SVC->>REPO: GetClientProductRankingReport(clientId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.Name, p.SKU, SUM(sl.Quantity) AS TotalQty, SUM(sl.LineTotal) AS TotalSpent, COUNT(DISTINCT sl.SaleId) AS PurchaseCount, RANK() OVER (ORDER BY SUM(sl.Quantity) DESC) AS Rank FROM SaleLines sl JOIN Products p ON sl.ProductId=p.ProductId JOIN Sales s ON sl.SaleId=s.SaleId WHERE s.ClientId=@ClientId GROUP BY p.ProductId, p.Name, p.SKU
    REPO-->>SVC: List~ClientProductRankingReportDTO~
    deactivate REPO
    SVC-->>UI: List~ClientProductRankingReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView with ranking
```

---

## UC-03: GetClientPurchasesReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        +LoadClientPurchasesReport(clientId, from, to) void
    }

    class IReportService {
        <<interface>>
        +GetClientPurchasesReport(clientId, from, to) List~ClientPurchasesReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        +GetClientPurchasesReport(clientId, from, to) List~ClientPurchasesReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetClientPurchasesReport(clientId, from, to) List~ClientPurchasesReportDTO~
    }

    class ReportRepository {
        +GetClientPurchasesReport(clientId, from, to) List~ClientPurchasesReportDTO~
    }

    class ClientPurchasesReportDTO {
        +int ClientId
        +string ClientName
        +string SaleNumber
        +DateTime SaleDate
        +decimal TotalAmount
        +int TotalItems
        +string SellerName
    }

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> ClientPurchasesReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects client and date range
    UI->>SVC: GetClientPurchasesReport(clientId, fromDate, toDate)
    activate SVC
    SVC->>REPO: GetClientPurchasesReport(clientId, from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT c.Nombre + ' ' + c.Apellido AS ClientName, s.SaleNumber, s.SaleDate, s.TotalAmount, s.SellerName, COUNT(sl.SaleLineId) AS TotalItems FROM Sales s JOIN Clients c ON s.ClientId=c.ClientId LEFT JOIN SaleLines sl ON s.SaleId=sl.SaleId WHERE s.ClientId=@ClientId AND s.SaleDate BETWEEN @From AND @To GROUP BY s.SaleId
    REPO-->>SVC: List~ClientPurchasesReportDTO~
    deactivate REPO
    SVC-->>UI: List~ClientPurchasesReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-04: GetPriceVariationReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        +LoadPriceVariationReport(productId, from, to) void
    }

    class IReportService {
        <<interface>>
        +GetPriceVariationReport(productId, from, to) List~PriceVariationReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        +GetPriceVariationReport(productId, from, to) List~PriceVariationReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetPriceVariationReport(productId, from, to) List~PriceVariationReportDTO~
    }

    class ReportRepository {
        +GetPriceVariationReport(productId, from, to) List~PriceVariationReportDTO~
    }

    class PriceVariationReportDTO {
        +int ProductId
        +string ProductName
        +string SKU
        +decimal OldPrice
        +decimal NewPrice
        +decimal VariationAmount
        +decimal VariationPercentage
        +DateTime ChangeDate
        +string ChangedBy
    }

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> PriceVariationReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects product and date range
    UI->>SVC: GetPriceVariationReport(productId, fromDate, toDate)
    activate SVC
    SVC->>REPO: GetPriceVariationReport(productId, from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.Name, p.SKU, al.OldValue AS OldPrice, al.NewValue AS NewPrice, al.ChangedAt, al.ChangedByUsername FROM AuditLog al JOIN Products p ON al.RecordId=p.ProductId WHERE al.TableName='Products' AND al.FieldName='UnitPrice' AND al.ChangedAt BETWEEN @From AND @To
    REPO-->>SVC: List~PriceVariationReportDTO~
    deactivate REPO
    SVC-->>UI: List~PriceVariationReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView with price trend chart
```

---

## UC-05: GetRevenueByDateReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        +LoadRevenueByDateReport(from, to, groupBy) void
    }

    class IReportService {
        <<interface>>
        +GetRevenueByDateReport(from, to, groupBy) List~RevenueByDateReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        +GetRevenueByDateReport(from, to, groupBy) List~RevenueByDateReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetRevenueByDateReport(from, to, groupBy) List~RevenueByDateReportDTO~
    }

    class ReportRepository {
        +GetRevenueByDateReport(from, to, groupBy) List~RevenueByDateReportDTO~
    }

    class RevenueByDateReportDTO {
        +DateTime PeriodDate
        +string PeriodLabel
        +decimal TotalRevenue
        +int TotalSales
        +int TotalItemsSold
    }

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> RevenueByDateReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects date range and grouping (Day/Week/Month)
    UI->>SVC: GetRevenueByDateReport(from, to, groupBy)
    activate SVC
    SVC->>REPO: GetRevenueByDateReport(from, to, groupBy)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT CAST(SaleDate AS DATE) AS PeriodDate, SUM(TotalAmount) AS Revenue, COUNT(*) AS TotalSales FROM Sales WHERE SaleDate BETWEEN @From AND @To AND IsActive=1 GROUP BY CAST(SaleDate AS DATE) ORDER BY PeriodDate
    REPO-->>SVC: List~RevenueByDateReportDTO~
    deactivate REPO
    SVC-->>UI: List~RevenueByDateReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView
    UI->>UI: Render revenue trend chart
```

---

## UC-06: GetSellerPerformanceReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        +LoadSellerPerformanceReport(from, to) void
    }

    class IReportService {
        <<interface>>
        +GetSellerPerformanceReport(from, to) List~SellerPerformanceReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        +GetSellerPerformanceReport(from, to) List~SellerPerformanceReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetSellerPerformanceReport(from, to) List~SellerPerformanceReportDTO~
    }

    class ReportRepository {
        +GetSellerPerformanceReport(from, to) List~SellerPerformanceReportDTO~
    }

    class SellerPerformanceReportDTO {
        +string SellerName
        +int TotalSales
        +decimal TotalRevenue
        +decimal AverageSaleAmount
        +int TotalItemsSold
        +int UniqueClientsServed
    }

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> SellerPerformanceReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects date range
    UI->>SVC: GetSellerPerformanceReport(fromDate, toDate)
    activate SVC
    SVC->>REPO: GetSellerPerformanceReport(from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT SellerName, COUNT(*) AS TotalSales, SUM(TotalAmount) AS Revenue, AVG(TotalAmount) AS AvgSale, COUNT(DISTINCT ClientId) AS UniqueClients FROM Sales WHERE SaleDate BETWEEN @From AND @To AND IsActive=1 GROUP BY SellerName ORDER BY Revenue DESC
    REPO-->>SVC: List~SellerPerformanceReportDTO~
    deactivate REPO
    SVC-->>UI: List~SellerPerformanceReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView with ranking
```

---

## UC-07: GetTopProductsReport

### Class Diagram

```mermaid
classDiagram
    class ReportsForm {
        -IReportService _reportService
        +LoadTopProductsReport(top, from, to) void
    }

    class IReportService {
        <<interface>>
        +GetTopProductsReport(top, from, to) List~TopProductsReportDTO~
    }

    class ReportService {
        -IReportRepository _reportRepository
        +GetTopProductsReport(top, from, to) List~TopProductsReportDTO~
    }

    class IReportRepository {
        <<interface>>
        +GetTopProductsReport(top, from, to) List~TopProductsReportDTO~
    }

    class ReportRepository {
        +GetTopProductsReport(top, from, to) List~TopProductsReportDTO~
    }

    class TopProductsReportDTO {
        +int ProductId
        +string ProductName
        +string SKU
        +string Category
        +int TotalQuantitySold
        +decimal TotalRevenue
        +int NumberOfSales
        +int Rank
    }

    ReportsForm --> IReportService : uses
    ReportService ..|> IReportService : implements
    ReportService --> IReportRepository : uses
    ReportRepository ..|> IReportRepository : implements
    ReportRepository --> TopProductsReportDTO : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ReportsForm
    participant SVC as ReportService
    participant REPO as ReportRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects top N and date range
    UI->>SVC: GetTopProductsReport(top, fromDate, toDate)
    activate SVC
    SVC->>REPO: GetTopProductsReport(top, from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT TOP @Top p.ProductId, p.Name, p.SKU, p.Category, SUM(sl.Quantity) AS TotalQtySold, SUM(sl.LineTotal) AS TotalRevenue, COUNT(DISTINCT sl.SaleId) AS SalesCount FROM SaleLines sl JOIN Products p ON sl.ProductId=p.ProductId JOIN Sales s ON sl.SaleId=s.SaleId WHERE s.SaleDate BETWEEN @From AND @To GROUP BY p.ProductId, p.Name, p.SKU, p.Category ORDER BY TotalQtySold DESC
    REPO-->>SVC: List~TopProductsReportDTO~
    deactivate REPO
    SVC-->>UI: List~TopProductsReportDTO~
    deactivate SVC
    UI->>UI: Bind to DataGridView
    UI->>UI: Render bar chart with top products
```

---
