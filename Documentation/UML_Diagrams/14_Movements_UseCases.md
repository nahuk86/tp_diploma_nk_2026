# Stock Movements - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Stock Movement-related use cases.

---

## UC-01: CreateMovement

### Class Diagram

```mermaid
classDiagram
    class StockMovementForm {
        -IStockMovementService _movementService
        -IWarehouseService _warehouseService
        -IProductService _productService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateMovement() bool
        -LoadMovements() void
    }

    class IStockMovementService {
        <<interface>>
        +CreateMovement(movement, lines) int
        +GetAllMovements() List~StockMovement~
        +GetMovementById(id) StockMovement
        +GetMovementsByType(type) List~StockMovement~
        +GetMovementsByDateRange(from, to) List~StockMovement~
        +GetMovementLines(movementId) List~StockMovementLine~
        +UpdateProductPrices(movementId) void
        +UpdateStockForMovement(movementId) void
    }

    class StockMovementService {
        -IStockMovementRepository _movementRepository
        -IStockRepository _stockRepository
        -IProductRepository _productRepository
        -IWarehouseRepository _warehouseRepository
        -ILogService _logService
        +CreateMovement(movement, lines) int
        -ValidateMovement(movement, lines) void
        -ApplyStockChanges(movement, lines) void
    }

    class IStockMovementRepository {
        <<interface>>
        +Insert(movement) int
        +InsertLine(line) void
        +GenerateMovementNumber() string
        +GetById(id) StockMovement
        +GetAll() List~StockMovement~
    }

    class StockMovementRepository {
        +Insert(movement) int
        +InsertLine(line) void
        +GenerateMovementNumber() string
    }

    class IStockRepository {
        <<interface>>
        +UpdateStock(productId, warehouseId, quantity, updatedBy) void
        +GetByProductAndWarehouse(productId, warehouseId) Stock
    }

    class StockRepository {
        +UpdateStock(productId, warehouseId, quantity, updatedBy) void
        +GetByProductAndWarehouse(productId, warehouseId) Stock
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class StockMovement {
        +int MovementId
        +string MovementNumber
        +MovementType MovementType
        +DateTime MovementDate
        +int SourceWarehouseId
        +int DestinationWarehouseId
        +string Reason
        +string Notes
        +DateTime CreatedAt
        +int CreatedBy
    }

    class StockMovementLine {
        +int LineId
        +int MovementId
        +int ProductId
        +int Quantity
        +decimal UnitPrice
    }

    class MovementType {
        <<enumeration>>
        In
        Out
        Transfer
        Adjustment
    }

    StockMovementForm --> IStockMovementService : uses
    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementService --> IStockRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockRepository ..|> IStockRepository : implements
    StockMovementRepository --> DatabaseHelper : uses
    StockMovementRepository --> StockMovement : maps
    StockMovementRepository --> StockMovementLine : maps
    StockMovement --> MovementType : has
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant MREPO as StockMovementRepository
    participant SREPO as StockRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateMovement()
    alt Validation fails
        UI-->>UI: Show error
    else Validation passes
        UI->>SVC: CreateMovement(movement, lines)
        activate SVC
        SVC->>MREPO: GenerateMovementNumber()
        MREPO-->>SVC: "MOV-20260222-001"
        SVC->>SVC: ValidateMovement(movement, lines)
        loop For each line
            SVC->>SREPO: GetByProductAndWarehouse(productId, warehouseId)
            SREPO->>DB: GetConnection()
            DB-->>SREPO: SqlConnection
            SREPO-->>SVC: Stock
        end
        SVC->>MREPO: Insert(movement)
        activate MREPO
        MREPO->>DB: GetConnection()
        DB-->>MREPO: SqlConnection
        Note over MREPO: INSERT INTO StockMovements ...
        MREPO-->>SVC: movementId
        deactivate MREPO
        loop For each line
            SVC->>MREPO: InsertLine(line)
            MREPO-->>SVC: void
        end
        SVC->>SVC: ApplyStockChanges(movement, lines)
        loop For each line
            SVC->>SREPO: UpdateStock(productId, warehouseId, qty, userId)
            SREPO-->>SVC: void
        end
        SVC-->>UI: movementId
        deactivate SVC
        UI->>UI: LoadMovements()
        UI-->>UI: Show success
    end
```

---

## UC-02: GetAllMovements

### Class Diagram

```mermaid
classDiagram
    class StockMovementForm {
        -IStockMovementService _movementService
        +LoadMovements() void
    }

    class IStockMovementService {
        <<interface>>
        +GetAllMovements() List~StockMovement~
    }

    class StockMovementService {
        -IStockMovementRepository _movementRepository
        +GetAllMovements() List~StockMovement~
    }

    class IStockMovementRepository {
        <<interface>>
        +GetAll() List~StockMovement~
    }

    class StockMovementRepository {
        +GetAll() List~StockMovement~
    }

    class StockMovement {
        +int MovementId
        +string MovementNumber
        +MovementType MovementType
        +DateTime MovementDate
        +string Reason
    }

    StockMovementForm --> IStockMovementService : uses
    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockMovementRepository --> StockMovement : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllMovements()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements ORDER BY MovementDate DESC
    REPO-->>SVC: List~StockMovement~
    deactivate REPO
    SVC-->>UI: List~StockMovement~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-03: GetAllMovementsById

### Class Diagram

```mermaid
classDiagram
    class StockMovementForm {
        -IStockMovementService _movementService
        +LoadMovementDetails(id) void
    }

    class IStockMovementService {
        <<interface>>
        +GetMovementById(id) StockMovement
    }

    class StockMovementService {
        -IStockMovementRepository _movementRepository
        +GetMovementById(id) StockMovement
    }

    class IStockMovementRepository {
        <<interface>>
        +GetById(id) StockMovement
    }

    class StockMovementRepository {
        +GetById(id) StockMovement
    }

    class StockMovement {
        +int MovementId
        +string MovementNumber
        +MovementType MovementType
        +DateTime MovementDate
        +int SourceWarehouseId
        +int DestinationWarehouseId
        +string Reason
        +string Notes
    }

    StockMovementForm --> IStockMovementService : uses
    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockMovementRepository --> StockMovement : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetMovementById(movementId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements WHERE MovementId=@Id
    REPO-->>SVC: StockMovement
    deactivate REPO
    alt Not found
        SVC-->>UI: null
        UI-->>UI: Show not found message
    else Found
        SVC-->>UI: StockMovement
        deactivate SVC
        UI->>UI: Populate form details
    end
```

---

## UC-04: GetMovementLines

### Class Diagram

```mermaid
classDiagram
    class StockMovementForm {
        -IStockMovementService _movementService
        +LoadMovementLines(movementId) void
    }

    class IStockMovementService {
        <<interface>>
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class StockMovementService {
        -IStockMovementRepository _movementRepository
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class IStockMovementRepository {
        <<interface>>
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class StockMovementRepository {
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class StockMovementLine {
        +int LineId
        +int MovementId
        +int ProductId
        +string ProductName
        +string SKU
        +int Quantity
        +decimal UnitPrice
    }

    StockMovementForm --> IStockMovementService : uses
    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockMovementRepository --> StockMovementLine : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetMovementLines(movementId)
    activate SVC
    SVC->>REPO: GetMovementLines(movementId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT sml.*, p.Name, p.SKU FROM StockMovementLines sml JOIN Products p ON sml.ProductId=p.ProductId WHERE sml.MovementId=@Id
    REPO-->>SVC: List~StockMovementLine~
    deactivate REPO
    SVC-->>UI: List~StockMovementLine~
    deactivate SVC
    UI->>UI: Bind lines to DataGridView
```

---

## UC-05: GetMovementsByDateRange

### Class Diagram

```mermaid
classDiagram
    class StockMovementForm {
        -IStockMovementService _movementService
        +FilterByDateRange(from, to) void
    }

    class IStockMovementService {
        <<interface>>
        +GetMovementsByDateRange(from, to) List~StockMovement~
    }

    class StockMovementService {
        -IStockMovementRepository _movementRepository
        +GetMovementsByDateRange(from, to) List~StockMovement~
    }

    class IStockMovementRepository {
        <<interface>>
        +GetByDateRange(from, to) List~StockMovement~
    }

    class StockMovementRepository {
        +GetByDateRange(from, to) List~StockMovement~
    }

    class StockMovement {
        +int MovementId
        +string MovementNumber
        +MovementType MovementType
        +DateTime MovementDate
    }

    StockMovementForm --> IStockMovementService : uses
    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockMovementRepository --> StockMovement : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects date range
    UI->>SVC: GetMovementsByDateRange(fromDate, toDate)
    activate SVC
    SVC->>REPO: GetByDateRange(from, to)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements WHERE MovementDate BETWEEN @From AND @To
    REPO-->>SVC: List~StockMovement~
    deactivate REPO
    SVC-->>UI: List~StockMovement~
    deactivate SVC
    UI->>UI: Bind filtered results to DataGridView
```

---

## UC-06: GetMovementsByType

### Class Diagram

```mermaid
classDiagram
    class StockMovementForm {
        -IStockMovementService _movementService
        +FilterByType(type) void
    }

    class IStockMovementService {
        <<interface>>
        +GetMovementsByType(type) List~StockMovement~
    }

    class StockMovementService {
        -IStockMovementRepository _movementRepository
        +GetMovementsByType(type) List~StockMovement~
    }

    class IStockMovementRepository {
        <<interface>>
        +GetByType(type) List~StockMovement~
    }

    class StockMovementRepository {
        +GetByType(type) List~StockMovement~
    }

    class StockMovement {
        +int MovementId
        +string MovementNumber
        +MovementType MovementType
        +DateTime MovementDate
    }

    class MovementType {
        <<enumeration>>
        In
        Out
        Transfer
        Adjustment
    }

    StockMovementForm --> IStockMovementService : uses
    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockMovementRepository --> StockMovement : returns
    StockMovement --> MovementType : has
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects movement type filter
    UI->>SVC: GetMovementsByType(MovementType.In)
    activate SVC
    SVC->>REPO: GetByType(type)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements WHERE MovementType=@Type
    REPO-->>SVC: List~StockMovement~
    deactivate REPO
    SVC-->>UI: List~StockMovement~
    deactivate SVC
    UI->>UI: Bind filtered results to DataGridView
```

---

## UC-07: UpdateProductPrices

### Class Diagram

```mermaid
classDiagram
    class StockMovementService {
        -IStockMovementRepository _movementRepository
        -IProductRepository _productRepository
        -ILogService _logService
        +UpdateProductPrices(movementId) void
        +CheckPriceUpdates(movementId) bool
    }

    class IStockMovementService {
        <<interface>>
        +UpdateProductPrices(movementId) void
        +CheckPriceUpdates(movementId) bool
    }

    class IStockMovementRepository {
        <<interface>>
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class IProductRepository {
        <<interface>>
        +Update(product) void
        +GetById(id) Product
    }

    class StockMovementRepository {
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class ProductRepository {
        +Update(product) void
        +GetById(id) Product
    }

    class StockMovementLine {
        +int LineId
        +int MovementId
        +int ProductId
        +int Quantity
        +decimal UnitPrice
    }

    class Product {
        +int ProductId
        +string Name
        +decimal UnitPrice
    }

    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementService --> IProductRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    ProductRepository ..|> IProductRepository : implements
    StockMovementLine --> Product : references
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant SVC as StockMovementService
    participant MREPO as StockMovementRepository
    participant PREPO as ProductRepository
    participant DB as DatabaseHelper

    Note over SVC: Called after movement creation (type=In)
    SVC->>SVC: CheckPriceUpdates(movementId)
    SVC->>MREPO: GetMovementLines(movementId)
    activate MREPO
    MREPO->>DB: GetConnection()
    DB-->>MREPO: SqlConnection
    MREPO-->>SVC: List~StockMovementLine~
    deactivate MREPO
    loop For each line with new price
        SVC->>PREPO: GetById(productId)
        PREPO-->>SVC: Product
        SVC->>PREPO: Update(product with new UnitPrice)
        activate PREPO
        PREPO->>DB: GetConnection()
        DB-->>PREPO: SqlConnection
        Note over PREPO: UPDATE Products SET UnitPrice=@Price WHERE ProductId=@Id
        PREPO-->>SVC: void
        deactivate PREPO
    end
    SVC-->>SVC: Prices updated
```

---

## UC-08: UpdateStockForMovement

### Class Diagram

```mermaid
classDiagram
    class StockMovementService {
        -IStockMovementRepository _movementRepository
        -IStockRepository _stockRepository
        -ILogService _logService
        +UpdateStockForMovement(movementId) void
    }

    class IStockMovementService {
        <<interface>>
        +UpdateStockForMovement(movementId) void
    }

    class IStockMovementRepository {
        <<interface>>
        +GetById(id) StockMovement
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class IStockRepository {
        <<interface>>
        +UpdateStock(productId, warehouseId, quantity, updatedBy) void
        +GetByProductAndWarehouse(productId, warehouseId) Stock
    }

    class StockMovementRepository {
        +GetById(id) StockMovement
        +GetMovementLines(movementId) List~StockMovementLine~
    }

    class StockRepository {
        +UpdateStock(productId, warehouseId, quantity, updatedBy) void
        +GetByProductAndWarehouse(productId, warehouseId) Stock
    }

    class StockMovement {
        +int MovementId
        +MovementType MovementType
        +int SourceWarehouseId
        +int DestinationWarehouseId
    }

    class Stock {
        +int StockId
        +int ProductId
        +int WarehouseId
        +int Quantity
        +DateTime LastUpdated
    }

    StockMovementService ..|> IStockMovementService : implements
    StockMovementService --> IStockMovementRepository : uses
    StockMovementService --> IStockRepository : uses
    StockMovementRepository ..|> IStockMovementRepository : implements
    StockRepository ..|> IStockRepository : implements
    StockMovement --> Stock : affects
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant SVC as StockMovementService
    participant MREPO as StockMovementRepository
    participant SREPO as StockRepository
    participant DB as DatabaseHelper

    SVC->>MREPO: GetById(movementId)
    MREPO-->>SVC: StockMovement
    SVC->>MREPO: GetMovementLines(movementId)
    MREPO-->>SVC: List~StockMovementLine~

    alt MovementType = In
        loop For each line
            SVC->>SREPO: UpdateStock(productId, destWarehouseId, +qty, userId)
            SREPO->>DB: GetConnection()
            DB-->>SREPO: SqlConnection
            Note over SREPO: MERGE Stock SET Quantity = Quantity + @Qty
            SREPO-->>SVC: void
        end
    else MovementType = Out
        loop For each line
            SVC->>SREPO: UpdateStock(productId, srcWarehouseId, -qty, userId)
            SREPO-->>SVC: void
        end
    else MovementType = Transfer
        loop For each line
            SVC->>SREPO: UpdateStock(productId, srcWarehouseId, -qty, userId)
            SREPO-->>SVC: void
            SVC->>SREPO: UpdateStock(productId, destWarehouseId, +qty, userId)
            SREPO-->>SVC: void
        end
    else MovementType = Adjustment
        loop For each line
            SVC->>SREPO: UpdateStock(productId, warehouseId, adjustedQty, userId)
            SREPO-->>SVC: void
        end
    end
    SVC-->>SVC: Stock updated
```

---
