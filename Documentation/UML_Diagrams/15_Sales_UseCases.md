# Sales - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Sales-related use cases.

---

## UC-01: CreateSale

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        -IClientService _clientService
        -IProductService _productService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateSale() bool
        -LoadSales() void
    }

    class ISaleService {
        <<interface>>
        +CreateSale(sale, lines) int
        +UpdateSale(sale) void
        +DeleteSale(id, deletedBy) void
        +GetAllSales() List~Sale~
        +GetAllSalesWithDetails() List~Sale~
        +GetSaleById(id) Sale
        +GetSaleByIdWithLines(id) Sale
        +GetSalesBySeller(seller) List~Sale~
        +GetSalesByClient(clientId) List~Sale~
        +GetSalesByDateRange(from, to) List~Sale~
        +GetAvailableStockByWarehouse(productId) Dictionary~int,int~
        +GetTotalAvailableStock(productId) int
    }

    class SaleService {
        -_saleLock$ SemaphoreSlim
        -ISaleRepository _saleRepository
        -IStockRepository _stockRepository
        -IProductRepository _productRepository
        -ILogService _logService
        +CreateSale(sale, lines) int
        -ValidateSale(sale, lines) void
        -DeductStock(lines, userId) void
    }

    class ISaleRepository {
        <<interface>>
        +CreateWithLines(sale, lines) int
        +GetById(id) Sale
        +GetAll() List~Sale~
    }

    class SaleRepository {
        +CreateWithLines(sale, lines) int
    }

    class IStockRepository {
        <<interface>>
        +GetCurrentStock(productId, warehouseId) int
        +UpdateStock(productId, warehouseId, quantity, updatedBy) void
    }

    class StockRepository {
        +GetCurrentStock(productId, warehouseId) int
        +UpdateStock(productId, warehouseId, quantity, updatedBy) void
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +int ClientId
        +string SellerName
        +decimal TotalAmount
        +string Notes
        +bool IsActive
        +List~SaleLine~ SaleLines
    }

    class SaleLine {
        +int SaleLineId
        +int SaleId
        +int ProductId
        +string ProductName
        +string SKU
        +int Quantity
        +decimal UnitPrice
        +decimal LineTotal
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleService --> IStockRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    StockRepository ..|> IStockRepository : implements
    SaleRepository --> DatabaseHelper : uses
    Sale "1" --* "many" SaleLine : contains
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant SREPO as SaleRepository
    participant STREPO as StockRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateSale()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: CreateSale(sale, lines)
        activate SVC
        SVC->>SVC: _saleLock.Wait() [acquire semaphore]
        Note over SVC: Critical section: only one thread<br/>may validate stock and create the sale
        SVC->>SVC: ValidateSale(sale, lines)
        loop For each sale line
            SVC->>STREPO: GetCurrentStock(productId, warehouseId)
            STREPO->>DB: GetConnection()
            DB-->>STREPO: SqlConnection
            STREPO-->>SVC: availableQty
            Note over SVC: Check if qty >= requestedQty
        end
        SVC->>SREPO: CreateWithLines(sale, lines)
        activate SREPO
        SREPO->>DB: GetConnection()
        DB-->>SREPO: SqlConnection
        Note over SREPO: BEGIN TRANSACTION
        Note over SREPO: INSERT INTO Sales ... (get saleId)
        Note over SREPO: INSERT INTO SaleLines (for each line)
        Note over SREPO: COMMIT
        SREPO-->>SVC: saleId
        deactivate SREPO
        SVC->>SVC: DeductStock(lines, userId)
        loop For each line
            SVC->>STREPO: UpdateStock(productId, warehouseId, -qty, userId)
            STREPO-->>SVC: void
        end
        SVC->>SVC: _saleLock.Release() [release semaphore - finally block]
        SVC-->>UI: saleId
        deactivate SVC
        UI->>UI: LoadSales()
        UI-->>UI: Show success
    end
```

---

## UC-02: DeleteSale

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +btnDelete_Click(sender, e) void
        -LoadSales() void
    }

    class ISaleService {
        <<interface>>
        +DeleteSale(id, deletedBy) void
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +DeleteSale(id, deletedBy) void
    }

    class ISaleRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
        +GetById(id) Sale
    }

    class SaleRepository {
        +SoftDelete(id, deletedBy) void
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +bool IsActive
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> Sale : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>UI: Confirm deletion dialog
    alt User cancels
        UI-->>UI: Do nothing
    else User confirms
        UI->>SVC: DeleteSale(saleId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Sales SET IsActive=0 WHERE SaleId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadSales()
        UI-->>UI: Show success
    end
```

---

## UC-03: GetAllSales

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +LoadSales() void
    }

    class ISaleService {
        <<interface>>
        +GetAllSales() List~Sale~
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetAllSales() List~Sale~
    }

    class ISaleRepository {
        <<interface>>
        +GetAll() List~Sale~
    }

    class SaleRepository {
        +GetAll() List~Sale~
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +string ClientName
        +string SellerName
        +decimal TotalAmount
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> Sale : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllSales()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT s.*, c.Nombre + ' ' + c.Apellido AS ClientName FROM Sales s JOIN Clients c ON s.ClientId=c.ClientId
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-04: GetAllSalesWithDetails

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +LoadSalesWithDetails() void
    }

    class ISaleService {
        <<interface>>
        +GetAllSalesWithDetails() List~Sale~
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetAllSalesWithDetails() List~Sale~
    }

    class ISaleRepository {
        <<interface>>
        +GetAllWithDetails() List~Sale~
    }

    class SaleRepository {
        +GetAllWithDetails() List~Sale~
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +int ClientId
        +string SellerName
        +decimal TotalAmount
        +List~SaleLine~ SaleLines
    }

    class SaleLine {
        +int SaleLineId
        +int SaleId
        +int ProductId
        +string ProductName
        +int Quantity
        +decimal UnitPrice
        +decimal LineTotal
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    Sale "1" --* "many" SaleLine : contains
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllSalesWithDetails()
    activate SVC
    SVC->>REPO: GetAllWithDetails()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT s.*, sl.*, p.Name FROM Sales s LEFT JOIN SaleLines sl ON s.SaleId=sl.SaleId LEFT JOIN Products p ON sl.ProductId=p.ProductId
    REPO-->>SVC: List~Sale~ with SaleLines
    deactivate REPO
    SVC-->>UI: List~Sale~ with SaleLines
    deactivate SVC
    UI->>UI: Bind master-detail view
```

---

## UC-05: GetAvailabelStockByWarehouse

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +LoadStockByWarehouse(productId) void
    }

    class ISaleService {
        <<interface>>
        +GetAvailableStockByWarehouse(productId) Dictionary~int,int~
    }

    class SaleService {
        -IStockRepository _stockRepository
        -IWarehouseRepository _warehouseRepository
        +GetAvailableStockByWarehouse(productId) Dictionary~int,int~
    }

    class IStockRepository {
        <<interface>>
        +GetByProduct(productId) List~Stock~
    }

    class StockRepository {
        +GetByProduct(productId) List~Stock~
    }

    class Stock {
        +int StockId
        +int ProductId
        +int WarehouseId
        +string WarehouseName
        +int Quantity
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> IStockRepository : uses
    StockRepository ..|> IStockRepository : implements
    StockRepository --> Stock : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant SREPO as StockRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects product
    UI->>SVC: GetAvailableStockByWarehouse(productId)
    activate SVC
    SVC->>SREPO: GetByProduct(productId)
    activate SREPO
    SREPO->>DB: GetConnection()
    DB-->>SREPO: SqlConnection
    Note over SREPO: SELECT s.*, w.Name AS WarehouseName FROM Stock s JOIN Warehouses w ON s.WarehouseId=w.WarehouseId WHERE s.ProductId=@ProductId AND s.Quantity > 0
    SREPO-->>SVC: List~Stock~
    deactivate SREPO
    SVC->>SVC: Convert to Dictionary[warehouseId, quantity]
    SVC-->>UI: Dictionary~int,int~
    deactivate SVC
    UI->>UI: Show stock per warehouse dropdown
```

---

## UC-06: GetSaleById

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +LoadSaleDetails(id) void
    }

    class ISaleService {
        <<interface>>
        +GetSaleById(id) Sale
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetSaleById(id) Sale
    }

    class ISaleRepository {
        <<interface>>
        +GetById(id) Sale
    }

    class SaleRepository {
        +GetById(id) Sale
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +int ClientId
        +string SellerName
        +decimal TotalAmount
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> Sale : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetSaleById(saleId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE SaleId=@Id
    REPO-->>SVC: Sale
    deactivate REPO
    SVC-->>UI: Sale
    deactivate SVC
    UI->>UI: Populate form fields
```

---

## UC-07: GetSaleByIdWithLines

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +LoadSaleWithLines(id) void
    }

    class ISaleService {
        <<interface>>
        +GetSaleByIdWithLines(id) Sale
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetSaleByIdWithLines(id) Sale
    }

    class ISaleRepository {
        <<interface>>
        +GetByIdWithLines(id) Sale
    }

    class SaleRepository {
        +GetByIdWithLines(id) Sale
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +decimal TotalAmount
        +List~SaleLine~ SaleLines
    }

    class SaleLine {
        +int SaleLineId
        +int ProductId
        +string ProductName
        +int Quantity
        +decimal UnitPrice
        +decimal LineTotal
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    Sale "1" --* "many" SaleLine : contains
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetSaleByIdWithLines(saleId)
    activate SVC
    SVC->>REPO: GetByIdWithLines(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT s.*, sl.*, p.Name FROM Sales s LEFT JOIN SaleLines sl ON s.SaleId=sl.SaleId LEFT JOIN Products p ON sl.ProductId=p.ProductId WHERE s.SaleId=@Id
    REPO-->>SVC: Sale with SaleLines
    deactivate REPO
    SVC-->>UI: Sale with SaleLines
    deactivate SVC
    UI->>UI: Populate sale header + lines grid
```

---

## UC-08: GetSaleByClient

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +FilterByClient(clientId) void
    }

    class ISaleService {
        <<interface>>
        +GetSalesByClient(clientId) List~Sale~
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetSalesByClient(clientId) List~Sale~
    }

    class ISaleRepository {
        <<interface>>
        +GetByClient(clientId) List~Sale~
    }

    class SaleRepository {
        +GetByClient(clientId) List~Sale~
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +int ClientId
        +decimal TotalAmount
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> Sale : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects client filter
    UI->>SVC: GetSalesByClient(clientId)
    activate SVC
    SVC->>REPO: GetByClient(clientId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE ClientId=@ClientId AND IsActive=1
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind filtered results to DataGridView
```

---

## UC-09: GetSaleByDateRange

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +FilterByDateRange(from, to) void
    }

    class ISaleService {
        <<interface>>
        +GetSalesByDateRange(from, to) List~Sale~
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetSalesByDateRange(from, to) List~Sale~
    }

    class ISaleRepository {
        <<interface>>
        +GetByDateRange(from, to) List~Sale~
    }

    class SaleRepository {
        +GetByDateRange(from, to) List~Sale~
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +decimal TotalAmount
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> Sale : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects date range
    UI->>SVC: GetSalesByDateRange(fromDate, toDate)
    activate SVC
    SVC->>REPO: GetByDateRange(from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE SaleDate BETWEEN @From AND @To AND IsActive=1
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind filtered results to DataGridView
```

---

## UC-10: GetSaleBySeller

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +FilterBySeller(sellerName) void
    }

    class ISaleService {
        <<interface>>
        +GetSalesBySeller(sellerName) List~Sale~
    }

    class SaleService {
        -ISaleRepository _saleRepository
        +GetSalesBySeller(sellerName) List~Sale~
    }

    class ISaleRepository {
        <<interface>>
        +GetBySeller(sellerName) List~Sale~
    }

    class SaleRepository {
        +GetBySeller(sellerName) List~Sale~
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +string SellerName
        +decimal TotalAmount
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> Sale : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>UI: User filters by seller
    UI->>SVC: GetSalesBySeller(sellerName)
    activate SVC
    SVC->>REPO: GetBySeller(sellerName)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE SellerName=@SellerName AND IsActive=1
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind filtered results to DataGridView
```

---

## UC-11: GetTotalAvailableStock

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +CheckTotalStock(productId) void
    }

    class ISaleService {
        <<interface>>
        +GetTotalAvailableStock(productId) int
    }

    class SaleService {
        -IStockRepository _stockRepository
        +GetTotalAvailableStock(productId) int
    }

    class IStockRepository {
        <<interface>>
        +GetCurrentStock(productId) int
        +GetByProduct(productId) List~Stock~
    }

    class StockRepository {
        +GetCurrentStock(productId) int
        +GetByProduct(productId) List~Stock~
    }

    class Stock {
        +int StockId
        +int ProductId
        +int WarehouseId
        +int Quantity
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> IStockRepository : uses
    StockRepository ..|> IStockRepository : implements
    StockRepository --> Stock : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant SREPO as StockRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetTotalAvailableStock(productId)
    activate SVC
    SVC->>SREPO: GetByProduct(productId)
    activate SREPO
    SREPO->>DB: GetConnection()
    DB-->>SREPO: SqlConnection
    Note over SREPO: SELECT SUM(Quantity) FROM Stock WHERE ProductId=@ProductId
    SREPO-->>SVC: List~Stock~
    deactivate SREPO
    SVC->>SVC: Sum all warehouse quantities
    SVC-->>UI: totalStock
    deactivate SVC
    UI->>UI: Show total available stock label
```

---

## UC-12: UpdateSale

### Class Diagram

```mermaid
classDiagram
    class SalesForm {
        -ISaleService _saleService
        +btnUpdate_Click(sender, e) void
        -ValidateSale() bool
        -LoadSales() void
    }

    class ISaleService {
        <<interface>>
        +UpdateSale(sale) void
        +GetSaleById(id) Sale
    }

    class SaleService {
        -ISaleRepository _saleRepository
        -ILogService _logService
        +UpdateSale(sale) void
    }

    class ISaleRepository {
        <<interface>>
        +Update(sale) void
        +GetById(id) Sale
    }

    class SaleRepository {
        +Update(sale) void
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Sale {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +int ClientId
        +string SellerName
        +decimal TotalAmount
        +string Notes
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    SalesForm --> ISaleService : uses
    SaleService ..|> ISaleService : implements
    SaleService --> ISaleRepository : uses
    SaleRepository ..|> ISaleRepository : implements
    SaleRepository --> DatabaseHelper : uses
    SaleRepository --> Sale : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateSale()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: UpdateSale(sale)
        activate SVC
        SVC->>REPO: Update(sale)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Sales SET SaleDate=@Date, SellerName=@Seller, Notes=@Notes, UpdatedAt=@Now WHERE SaleId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadSales()
        UI-->>UI: Show success
    end
```

---
