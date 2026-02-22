# Warehouses - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Warehouse-related use cases.

---

## UC-01: CreateWareHouse

### Class Diagram

```mermaid
classDiagram
    class WarehousesForm {
        -IWarehouseService _warehouseService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateInputs() bool
        -LoadWarehouses() void
    }

    class IWarehouseService {
        <<interface>>
        +CreateWarehouse(warehouse) int
        +UpdateWarehouse(warehouse) void
        +DeleteWarehouse(id, deletedBy) void
        +GetAllWarehouses() List~Warehouse~
        +GetActiveWarehouses() List~Warehouse~
        +GetWarehouseById(id) Warehouse
    }

    class WarehouseService {
        -IWarehouseRepository _warehouseRepository
        -ILogService _logService
        +CreateWarehouse(warehouse) int
        -ValidateWarehouse(warehouse) void
    }

    class IWarehouseRepository {
        <<interface>>
        +Insert(warehouse) int
        +CodeExists(code, excludeId) bool
        +GetById(id) Warehouse
        +GetAll() List~Warehouse~
    }

    class WarehouseRepository {
        -DatabaseHelper _db
        +Insert(warehouse) int
        +CodeExists(code, excludeId) bool
        -MapWarehouse(reader) Warehouse
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Warehouse {
        +int WarehouseId
        +string Code
        +string Name
        +string Address
        +bool IsActive
        +DateTime CreatedAt
        +int CreatedBy
    }

    WarehousesForm --> IWarehouseService : uses
    WarehousesForm --> ILogService : uses
    WarehouseService ..|> IWarehouseService : implements
    WarehouseService --> IWarehouseRepository : uses
    WarehouseService --> ILogService : uses
    WarehouseRepository ..|> IWarehouseRepository : implements
    WarehouseRepository --> DatabaseHelper : uses
    WarehouseRepository --> Warehouse : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as WarehousesForm
    participant SVC as WarehouseService
    participant REPO as WarehouseRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: CreateWarehouse(warehouse)
        activate SVC
        SVC->>SVC: ValidateWarehouse(warehouse)
        SVC->>REPO: CodeExists(code, 0)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        REPO-->>SVC: false (code is unique)
        deactivate REPO
        SVC->>REPO: Insert(warehouse)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: INSERT INTO Warehouses (Code, Name, Address, ...) VALUES (...)
        REPO-->>SVC: newWarehouseId
        deactivate REPO
        SVC-->>UI: newWarehouseId
        deactivate SVC
        UI->>UI: LoadWarehouses()
        UI-->>UI: Show success message
    end
```

---

## UC-02: DeleteWarehouse

### Class Diagram

```mermaid
classDiagram
    class WarehousesForm {
        -IWarehouseService _warehouseService
        +btnDelete_Click(sender, e) void
        -LoadWarehouses() void
    }

    class IWarehouseService {
        <<interface>>
        +DeleteWarehouse(id, deletedBy) void
    }

    class WarehouseService {
        -IWarehouseRepository _warehouseRepository
        +DeleteWarehouse(id, deletedBy) void
    }

    class IWarehouseRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
        +GetById(id) Warehouse
    }

    class WarehouseRepository {
        +SoftDelete(id, deletedBy) void
    }

    class Warehouse {
        +int WarehouseId
        +string Code
        +string Name
        +bool IsActive
    }

    WarehousesForm --> IWarehouseService : uses
    WarehouseService ..|> IWarehouseService : implements
    WarehouseService --> IWarehouseRepository : uses
    WarehouseRepository ..|> IWarehouseRepository : implements
    WarehouseRepository --> Warehouse : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as WarehousesForm
    participant SVC as WarehouseService
    participant REPO as WarehouseRepository
    participant DB as DatabaseHelper

    UI->>UI: Confirm deletion dialog
    alt User cancels
        UI-->>UI: Do nothing
    else User confirms
        UI->>SVC: DeleteWarehouse(warehouseId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Warehouses SET IsActive=0 WHERE WarehouseId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadWarehouses()
        UI-->>UI: Show success message
    end
```

---

## UC-03: GetAllActiveWarehouses

### Class Diagram

```mermaid
classDiagram
    class WarehousesForm {
        -IWarehouseService _warehouseService
        +LoadActiveWarehouses() void
    }

    class IWarehouseService {
        <<interface>>
        +GetActiveWarehouses() List~Warehouse~
    }

    class WarehouseService {
        -IWarehouseRepository _warehouseRepository
        +GetActiveWarehouses() List~Warehouse~
    }

    class IWarehouseRepository {
        <<interface>>
        +GetAllActive() List~Warehouse~
    }

    class WarehouseRepository {
        +GetAllActive() List~Warehouse~
    }

    class Warehouse {
        +int WarehouseId
        +string Code
        +string Name
        +string Address
        +bool IsActive
    }

    WarehousesForm --> IWarehouseService : uses
    WarehouseService ..|> IWarehouseService : implements
    WarehouseService --> IWarehouseRepository : uses
    WarehouseRepository ..|> IWarehouseRepository : implements
    WarehouseRepository --> Warehouse : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as WarehousesForm
    participant SVC as WarehouseService
    participant REPO as WarehouseRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetActiveWarehouses()
    activate SVC
    SVC->>REPO: GetAllActive()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Warehouses WHERE IsActive=1 ORDER BY Name
    REPO-->>SVC: List~Warehouse~
    deactivate REPO
    SVC-->>UI: List~Warehouse~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-04: GetAllWarehouses

### Class Diagram

```mermaid
classDiagram
    class WarehousesForm {
        -IWarehouseService _warehouseService
        +LoadAllWarehouses() void
    }

    class IWarehouseService {
        <<interface>>
        +GetAllWarehouses() List~Warehouse~
    }

    class WarehouseService {
        -IWarehouseRepository _warehouseRepository
        +GetAllWarehouses() List~Warehouse~
    }

    class IWarehouseRepository {
        <<interface>>
        +GetAll() List~Warehouse~
    }

    class WarehouseRepository {
        +GetAll() List~Warehouse~
    }

    class Warehouse {
        +int WarehouseId
        +string Code
        +string Name
        +string Address
        +bool IsActive
    }

    WarehousesForm --> IWarehouseService : uses
    WarehouseService ..|> IWarehouseService : implements
    WarehouseService --> IWarehouseRepository : uses
    WarehouseRepository ..|> IWarehouseRepository : implements
    WarehouseRepository --> Warehouse : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as WarehousesForm
    participant SVC as WarehouseService
    participant REPO as WarehouseRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllWarehouses()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Warehouses ORDER BY Name
    REPO-->>SVC: List~Warehouse~
    deactivate REPO
    SVC-->>UI: List~Warehouse~
    deactivate SVC
    UI->>UI: Bind to DataGridView (all including inactive)
```

---

## UC-05: GetWarehousesById

### Class Diagram

```mermaid
classDiagram
    class WarehousesForm {
        -IWarehouseService _warehouseService
        +LoadWarehouseDetails(id) void
    }

    class IWarehouseService {
        <<interface>>
        +GetWarehouseById(id) Warehouse
    }

    class WarehouseService {
        -IWarehouseRepository _warehouseRepository
        +GetWarehouseById(id) Warehouse
    }

    class IWarehouseRepository {
        <<interface>>
        +GetById(id) Warehouse
    }

    class WarehouseRepository {
        +GetById(id) Warehouse
    }

    class Warehouse {
        +int WarehouseId
        +string Code
        +string Name
        +string Address
        +bool IsActive
        +DateTime CreatedAt
    }

    WarehousesForm --> IWarehouseService : uses
    WarehouseService ..|> IWarehouseService : implements
    WarehouseService --> IWarehouseRepository : uses
    WarehouseRepository ..|> IWarehouseRepository : implements
    WarehouseRepository --> Warehouse : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as WarehousesForm
    participant SVC as WarehouseService
    participant REPO as WarehouseRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetWarehouseById(warehouseId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Warehouses WHERE WarehouseId=@Id
    REPO-->>SVC: Warehouse
    deactivate REPO
    alt Not found
        SVC-->>UI: null
        UI-->>UI: Show not found message
    else Found
        SVC-->>UI: Warehouse
        deactivate SVC
        UI->>UI: Populate form fields
    end
```

---

## UC-06: UpdateWarehouse

### Class Diagram

```mermaid
classDiagram
    class WarehousesForm {
        -IWarehouseService _warehouseService
        +btnUpdate_Click(sender, e) void
        -ValidateInputs() bool
        -LoadWarehouses() void
    }

    class IWarehouseService {
        <<interface>>
        +UpdateWarehouse(warehouse) void
    }

    class WarehouseService {
        -IWarehouseRepository _warehouseRepository
        -ILogService _logService
        +UpdateWarehouse(warehouse) void
        -ValidateWarehouse(warehouse) void
    }

    class IWarehouseRepository {
        <<interface>>
        +Update(warehouse) void
        +CodeExists(code, excludeId) bool
    }

    class WarehouseRepository {
        +Update(warehouse) void
        +CodeExists(code, excludeId) bool
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Warehouse {
        +int WarehouseId
        +string Code
        +string Name
        +string Address
        +bool IsActive
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    WarehousesForm --> IWarehouseService : uses
    WarehouseService ..|> IWarehouseService : implements
    WarehouseService --> IWarehouseRepository : uses
    WarehouseRepository ..|> IWarehouseRepository : implements
    WarehouseRepository --> DatabaseHelper : uses
    WarehouseRepository --> Warehouse : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as WarehousesForm
    participant SVC as WarehouseService
    participant REPO as WarehouseRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: UpdateWarehouse(warehouse)
        activate SVC
        SVC->>SVC: ValidateWarehouse(warehouse)
        SVC->>REPO: CodeExists(code, warehouseId)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        REPO-->>SVC: false (code is unique or same warehouse)
        deactivate REPO
        SVC->>REPO: Update(warehouse)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Warehouses SET Code=@Code, Name=@Name, Address=@Addr, UpdatedAt=@Now WHERE WarehouseId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadWarehouses()
        UI-->>UI: Show success message
    end
```

---
