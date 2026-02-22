# Stock Movement Process - Sequence Diagrams (Per Use Case)

This document contains UML Sequence Diagrams organized per use case for all Stock Movement operations.

---

## UC-01: CreateMovement

```mermaid
sequenceDiagram
    participant User as Warehouse Manager
    participant UI as StockMovementForm
    participant MoveSvc as StockMovementService
    participant MoveRepo as StockMovementRepository
    participant StockRepo as StockRepository
    participant DB as Database

    User->>UI: Fill movement form + lines, click "Save"
    activate UI
    UI->>UI: ValidateForm()
    alt Validation Fails
        UI-->>User: Show validation errors
    else Validation Passes
        UI->>MoveSvc: CreateMovement(movement, lines)
        activate MoveSvc
        MoveSvc->>MoveSvc: ValidateMovement(movement, lines)
        loop For each line
            MoveSvc->>MoveSvc: ValidateStockAvailability(type, sourceWh, productId, quantity)
            MoveSvc->>StockRepo: GetByProductAndWarehouse(productId, sourceWh)
            StockRepo->>DB: SELECT * FROM Stock WHERE ProductId=@P AND WarehouseId=@W
            DB-->>StockRepo: Stock
            StockRepo-->>MoveSvc: Stock entity
        end
        MoveSvc->>MoveRepo: GenerateMovementNumber(movementType)
        MoveRepo->>DB: SELECT COUNT(*) FROM StockMovements WHERE MovementType=@T AND YEAR(MovementDate)=YEAR(GETDATE())
        DB-->>MoveRepo: count
        MoveRepo-->>MoveSvc: movementNumber (e.g. TRF-2026-0001)
        MoveSvc->>MoveRepo: Insert(movement)
        activate MoveRepo
        MoveRepo->>DB: BEGIN TRANSACTION
        Note over MoveRepo: INSERT INTO StockMovements (...) → movementId
        MoveRepo-->>MoveSvc: movementId
        loop For each line
            MoveSvc->>MoveRepo: InsertLine(line)
            Note over MoveRepo: INSERT INTO StockMovementLines (...)
            MoveRepo-->>MoveSvc: void
            MoveSvc->>MoveSvc: UpdateStockForMovement(type, sourceWh, destWh, productId, qty)
            alt Entry
                MoveSvc->>StockRepo: AddStock(productId, destWh, qty)
            else Exit
                MoveSvc->>StockRepo: DeductStock(productId, sourceWh, qty)
            else Transfer
                MoveSvc->>StockRepo: DeductStock(productId, sourceWh, qty)
                MoveSvc->>StockRepo: AddStock(productId, destWh, qty)
            else Adjustment
                MoveSvc->>StockRepo: UpdateQuantity(productId, sourceWh, qty)
            end
        end
        MoveRepo->>DB: COMMIT TRANSACTION
        deactivate MoveRepo
        MoveSvc-->>UI: movementId
        deactivate MoveSvc
        UI->>MoveSvc: GetAllMovements()
        MoveSvc-->>UI: List~StockMovement~
        UI-->>User: Show success & refresh grid
    end
    deactivate UI
```

---

## UC-02: GetAllMovements

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as Database

    UI->>SVC: GetAllMovements()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements ORDER BY MovementDate DESC
    REPO->>REPO: MapStockMovement(reader) for each row
    REPO-->>SVC: List~StockMovement~
    deactivate REPO
    SVC-->>UI: List~StockMovement~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-03: GetAllMovementsById

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as Database

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

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as Database

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

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as Database

    UI->>UI: User selects date range
    UI->>SVC: GetMovementsByDateRange(startDate, endDate)
    activate SVC
    SVC->>REPO: GetByDateRange(startDate, endDate)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements WHERE MovementDate BETWEEN @Start AND @End ORDER BY MovementDate DESC
    REPO-->>SVC: List~StockMovement~
    deactivate REPO
    SVC-->>UI: List~StockMovement~
    deactivate SVC
    UI->>UI: Bind filtered results
```

---

## UC-06: GetMovementsByType

```mermaid
sequenceDiagram
    participant UI as StockMovementForm
    participant SVC as StockMovementService
    participant REPO as StockMovementRepository
    participant DB as Database

    UI->>UI: User selects movement type filter
    UI->>SVC: GetMovementsByType(movementType)
    activate SVC
    SVC->>REPO: GetByType(movementType)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM StockMovements WHERE MovementType=@Type ORDER BY MovementDate DESC
    REPO-->>SVC: List~StockMovement~
    deactivate REPO
    SVC-->>UI: List~StockMovement~
    deactivate SVC
    UI->>UI: Bind filtered results
```

---

## UC-07: UpdateProductPrices

```mermaid
sequenceDiagram
    participant SVC as StockMovementService
    participant MREPO as StockMovementRepository
    participant PREPO as ProductRepository
    participant DB as Database

    Note over SVC: Triggered after Entry movement creation
    SVC->>SVC: CheckPriceUpdates(movementId)
    SVC->>MREPO: GetMovementLines(movementId)
    MREPO->>DB: GetConnection()
    DB-->>MREPO: SqlConnection
    MREPO-->>SVC: List~StockMovementLine~
    loop For each line where line.UnitPrice > 0
        SVC->>PREPO: GetById(productId)
        PREPO-->>SVC: Product
        SVC->>PREPO: Update(product with new UnitPrice)
        activate PREPO
        PREPO->>DB: GetConnection()
        DB-->>PREPO: SqlConnection
        Note over PREPO: UPDATE Products SET UnitPrice=@NewPrice WHERE ProductId=@Id
        PREPO-->>SVC: void
        deactivate PREPO
    end
    SVC-->>SVC: Prices updated
```

---

## UC-08: UpdateStockForMovement

```mermaid
sequenceDiagram
    participant SVC as StockMovementService
    participant MREPO as StockMovementRepository
    participant SREPO as StockRepository
    participant DB as Database

    SVC->>MREPO: GetById(movementId)
    MREPO-->>SVC: StockMovement
    SVC->>MREPO: GetMovementLines(movementId)
    MREPO-->>SVC: List~StockMovementLine~
    alt MovementType = Entry
        loop For each line
            SVC->>SREPO: AddStock(productId, destWh, +qty)
            SREPO->>DB: MERGE INTO Stock SET Quantity=Quantity+@Qty ...
            DB-->>SREPO: Success
        end
    else MovementType = Exit
        loop For each line
            SVC->>SREPO: DeductStock(productId, sourceWh, qty)
            SREPO->>DB: UPDATE Stock SET Quantity=Quantity-@Qty ...
            alt Stock goes negative
                SREPO->>DB: ROLLBACK
                SREPO-->>SVC: throw Exception
            else
                DB-->>SREPO: Success
            end
        end
    else MovementType = Transfer
        loop For each line
            SVC->>SREPO: DeductStock(productId, sourceWh, qty)
            SREPO-->>SVC: void
            SVC->>SREPO: AddStock(productId, destWh, qty)
            SREPO-->>SVC: void
        end
    else MovementType = Adjustment
        loop For each line
            SVC->>SREPO: UpdateQuantity(productId, warehouseId, newQty)
            SREPO-->>SVC: void
        end
    end
    SVC-->>SVC: Stock updated
```

---

## Business Rules Summary

| Use Case | Key Business Rules |
|----------|-------------------|
| CreateMovement | Movement number auto-generated per type+year; stock validated; atomic transaction |
| UpdateStockForMovement | Entry/Exit/Transfer/Adjustment have different stock operations |
| UpdateProductPrices | Only triggered for Entry movements with unit price specified |
| GetMovementsByType | Supports: Entry, Exit, Transfer, Adjustment |

```mermaid
sequenceDiagram
    participant User as Warehouse Manager
    participant UI as StockMovementForm<br/>(UI Layer)
    participant MoveSvc as StockMovementService<br/>(BLL)
    participant ProdSvc as ProductService<br/>(BLL)
    participant WhSvc as WarehouseService<br/>(BLL)
    participant MoveRepo as StockMovementRepository<br/>(DAO)
    participant StockRepo as StockRepository<br/>(DAO)
    participant AuditRepo as AuditLogRepository<br/>(DAO)
    participant DB as Database
    participant Log as ILogService

    %% Load Form
    User->>UI: Open Stock Movement Form
    activate UI
    
    UI->>MoveSvc: GetAllMovements()
    activate MoveSvc
    MoveSvc->>MoveRepo: GetAll()
    activate MoveRepo
    MoveRepo->>DB: SELECT * FROM StockMovements<br/>ORDER BY MovementDate DESC
    activate DB
    DB-->>MoveRepo: ResultSet
    deactivate DB
    MoveRepo->>MoveRepo: MapStockMovement(reader)
    MoveRepo-->>MoveSvc: List<StockMovement>
    deactivate MoveRepo
    MoveSvc-->>UI: List<StockMovement>
    deactivate MoveSvc
    
    UI->>ProdSvc: GetActiveProducts()
    activate ProdSvc
    ProdSvc-->>UI: List<Product>
    deactivate ProdSvc
    
    UI->>WhSvc: GetActiveWarehouses()
    activate WhSvc
    WhSvc-->>UI: List<Warehouse>
    deactivate WhSvc
    
    UI-->>User: Display movements, populate dropdowns
    deactivate UI

    %% Create New Transfer Movement
    User->>UI: Click "New Movement" button
    activate UI
    UI->>UI: ClearForm()
    UI-->>User: Display empty form
    deactivate UI

    User->>UI: Select MovementType = "Transfer"
    activate UI
    UI->>UI: UpdateWarehouseVisibility()
    Note over UI: Show both Source and<br/>Destination Warehouse fields
    UI-->>User: Display warehouse fields
    deactivate UI

    User->>UI: Select Source Warehouse
    User->>UI: Select Destination Warehouse
    User->>UI: Enter Movement Date and Remarks

    %% Add Movement Lines
    loop For each product to transfer
        User->>UI: Click "Add Line" button
        activate UI
        UI-->>User: Show product selection dialog
        deactivate UI
        
        User->>UI: Select Product and enter Quantity
        activate UI
        
        %% Check available stock in source warehouse
        UI->>MoveSvc: GetCurrentStock(productId, sourceWarehouseId)
        activate MoveSvc
        MoveSvc->>StockRepo: GetByProductAndWarehouse(productId, sourceWarehouseId)
        activate StockRepo
        StockRepo->>DB: SELECT * FROM Stock<br/>WHERE ProductId = @ProductId<br/>AND WarehouseId = @WarehouseId
        activate DB
        DB-->>StockRepo: ResultSet
        deactivate DB
        StockRepo->>StockRepo: MapStock(reader)
        StockRepo-->>MoveSvc: Stock entity (or null)
        deactivate StockRepo
        MoveSvc-->>UI: currentQuantity
        deactivate MoveSvc
        
        alt Insufficient Stock
            UI-->>User: Show "Insufficient stock in source warehouse" error
        else Stock Available
            UI->>UI: Add line to dgvMovementLines
            UI-->>User: Display updated movement lines
        end
        deactivate UI
    end

    %% Save Movement
    User->>UI: Click "Save" button
    activate UI
    UI->>UI: ValidateForm()
    Note over UI: Check:<br/>- Movement type selected<br/>- Source warehouse selected<br/>- Destination warehouse selected<br/>- At least one line added<br/>- Source != Destination
    
    alt Validation Fails
        UI-->>User: Show validation errors
    else Validation Passes
        
        UI->>MoveSvc: CreateMovement(movement, lines)
        activate MoveSvc
        
        %% Validate movement
        MoveSvc->>MoveSvc: ValidateMovement(movement, lines)
        Note over MoveSvc: Business validations:<br/>- Lines count > 0<br/>- All quantities > 0<br/>- Source != Destination<br/>- Warehouses exist
        
        %% Validate stock availability for each line
        loop For each line
            MoveSvc->>MoveSvc: ValidateStockAvailability(Transfer, sourceWh, productId, quantity)
            MoveSvc->>StockRepo: GetByProductAndWarehouse(productId, sourceWh)
            activate StockRepo
            StockRepo->>DB: SELECT...
            activate DB
            DB-->>StockRepo: Stock
            deactivate DB
            StockRepo-->>MoveSvc: Stock entity
            deactivate StockRepo
            
            alt Insufficient Stock
                MoveSvc->>Log: Warning("Insufficient stock")
                activate Log
                deactivate Log
                MoveSvc-->>UI: throw Exception("Insufficient stock")
                UI-->>User: Show error message
            end
        end
        
        %% Generate movement number
        MoveSvc->>MoveRepo: GenerateMovementNumber(Transfer)
        activate MoveRepo
        MoveRepo->>DB: SELECT COUNT(*) FROM StockMovements<br/>WHERE MovementType = @Type<br/>AND YEAR(MovementDate) = YEAR(GETDATE())
        activate DB
        DB-->>MoveRepo: count
        deactivate DB
        Note over MoveRepo: Format: TRF-{yyyy}-{seq:0000}
        MoveRepo-->>MoveSvc: movementNumber
        deactivate MoveRepo
        
        Note over MoveSvc: Set CreatedAt = DateTime.Now<br/>Set CreatedBy = CurrentUserId
        
        %% Insert movement header
        MoveSvc->>MoveRepo: Insert(movement)
        activate MoveRepo
        MoveRepo->>DB: BEGIN TRANSACTION
        activate DB
        Note over MoveRepo: INSERT INTO StockMovements<br/>(MovementNumber, MovementDate,<br/>MovementType, SourceWarehouseId,<br/>DestinationWarehouseId, Remarks,<br/>CreatedAt, CreatedBy)<br/>VALUES (...)
        MoveRepo->>DB: ExecuteScalar()
        DB-->>MoveRepo: movementId
        deactivate MoveRepo
        
        %% Insert lines and update stock
        loop For each line
            %% Insert movement line
            MoveSvc->>MoveRepo: InsertLine(line)
            activate MoveRepo
            Note over MoveRepo: INSERT INTO StockMovementLines<br/>(MovementId, ProductId, Quantity)<br/>VALUES (...)
            MoveRepo->>DB: ExecuteNonQuery()
            DB-->>MoveRepo: Success
            deactivate MoveRepo
            
            %% Update stock - Transfer operation
            MoveSvc->>MoveSvc: UpdateStockForMovement(Transfer, sourceWh, destWh, productId, quantity)
            activate MoveSvc
            
            %% Deduct from source warehouse
            MoveSvc->>StockRepo: DeductStock(productId, sourceWh, quantity)
            activate StockRepo
            Note over StockRepo: UPDATE Stock<br/>SET Quantity = Quantity - @Quantity,<br/>LastUpdated = @LastUpdated<br/>WHERE ProductId = @ProductId<br/>AND WarehouseId = @SourceWh
            StockRepo->>DB: ExecuteNonQuery()
            
            alt Stock goes negative
                StockRepo->>DB: ROLLBACK
                DB-->>StockRepo: Rolled back
                StockRepo-->>MoveSvc: throw Exception
                MoveSvc-->>UI: throw Exception
                UI-->>User: Show "Stock deduction failed" error
            else Stock valid
                DB-->>StockRepo: Success
            end
            deactivate StockRepo
            
            %% Add to destination warehouse
            MoveSvc->>StockRepo: AddStock(productId, destWh, quantity)
            activate StockRepo
            
            alt Stock record doesn't exist
                Note over StockRepo: INSERT INTO Stock<br/>(ProductId, WarehouseId,<br/>Quantity, LastUpdated)<br/>VALUES (...)
                StockRepo->>DB: ExecuteNonQuery()
                DB-->>StockRepo: Success
            else Stock record exists
                Note over StockRepo: UPDATE Stock<br/>SET Quantity = Quantity + @Quantity,<br/>LastUpdated = @LastUpdated<br/>WHERE ProductId = @ProductId<br/>AND WarehouseId = @DestWh
                StockRepo->>DB: ExecuteNonQuery()
                DB-->>StockRepo: Success
            end
            deactivate StockRepo
            deactivate MoveSvc
        end
        
        %% Commit transaction
        MoveRepo->>DB: COMMIT TRANSACTION
        deactivate DB
        
        %% Log audit
        MoveSvc->>AuditRepo: LogChange("StockMovements", movementId, Insert, null, null, description, userId)
        activate AuditRepo
        AuditRepo->>DB: INSERT INTO AuditLog<br/>(TableName, RecordId, Action,<br/>Description, ChangeDate, ChangedBy)<br/>VALUES (...)
        activate DB
        DB-->>AuditRepo: Success
        deactivate DB
        deactivate AuditRepo
        
        %% Log info
        MoveSvc->>Log: Info($"Stock movement created: {movementNumber} (Transfer)")
        activate Log
        deactivate Log
        
        MoveSvc-->>UI: movementId
        deactivate MoveSvc
        
        %% Refresh display
        UI->>MoveSvc: GetAllMovements()
        activate MoveSvc
        MoveSvc->>MoveRepo: GetAll()
        activate MoveRepo
        MoveRepo->>DB: SELECT...
        activate DB
        DB-->>MoveRepo: ResultSet
        deactivate DB
        MoveRepo-->>MoveSvc: List<StockMovement>
        deactivate MoveRepo
        MoveSvc-->>UI: List<StockMovement>
        deactivate MoveSvc
        
        UI->>UI: ClearForm()
        UI-->>User: Show success message & refresh grid
    end
    deactivate UI
```

## Sequence Flow Description

### Phase 1: Form Initialization
1. Warehouse manager opens Stock Movement Form
2. Form loads existing movements ordered by date
3. Form populates product dropdown with active products
4. Form populates warehouse dropdown with active warehouses
5. Display movements in main grid

### Phase 2: Select Movement Type
6. User clicks "New Movement"
7. Form clears all fields
8. User selects "Transfer" as movement type
9. Form shows both source and destination warehouse fields

### Phase 3: Select Warehouses
10. User selects source warehouse (where stock will be taken from)
11. User selects destination warehouse (where stock will go to)
12. Form validates source ≠ destination
13. User enters movement date and remarks

### Phase 4: Add Movement Lines
14. For each product to transfer:
    - User clicks "Add Line"
    - Selects product and quantity
    - System checks available stock in source warehouse
    - If insufficient, show error
    - If available, add line to grid

### Phase 5: Form Validation
15. User clicks "Save"
16. Form validates:
    - Movement type selected
    - Source and destination warehouses selected
    - At least one line added
    - Source ≠ Destination

### Phase 6: Business Validation
17. StockMovementService validates business rules
18. For each line, verify sufficient stock in source warehouse
19. If any line has insufficient stock, abort with error

### Phase 7: Generate Movement Number
20. Generate unique movement number: TRF-YYYY-####
21. Set audit fields (CreatedAt, CreatedBy)

### Phase 8: Database Transaction - Create Movement
22. Begin database transaction
23. Insert movement header into StockMovements table
24. Retrieve new movementId

### Phase 9: Process Each Line
25. For each movement line:
    - Insert line into StockMovementLines table
    - Deduct quantity from source warehouse stock
    - Add quantity to destination warehouse stock
    - Update LastUpdated timestamp
    - Validate stock doesn't go negative

### Phase 10: Transaction Commit
26. All operations successful → Commit transaction
27. Any operation fails → Rollback entire transaction

### Phase 11: Audit & Logging
28. Log change to AuditLog table
29. Write info log message with movement details

### Phase 12: Completion
30. Return movementId to form
31. Refresh movements grid
32. Clear form for next movement
33. Display success message

## Business Rules Enforced

1. **Stock Availability**: Must have sufficient stock in source warehouse
2. **Warehouse Validation**: Source and destination must be different
3. **Atomic Operations**: All stock updates in single transaction
4. **Movement Number**: Auto-generated, unique, sequential by type and year
5. **Stock Consistency**: Deduct from source = Add to destination
6. **Negative Stock Prevention**: Transaction rollback if stock would go negative
7. **Audit Trail**: All operations logged with user and timestamp

## Movement Types Summary

### Entry (IN-YYYY-####)
- **Stock Operation**: Add to destination warehouse only
- **Warehouses**: Destination required, source not applicable

### Exit (OUT-YYYY-####)
- **Stock Operation**: Deduct from source warehouse only
- **Warehouses**: Source required, destination not applicable

### Transfer (TRF-YYYY-####)
- **Stock Operation**: Deduct from source, add to destination
- **Warehouses**: Both source and destination required

### Adjustment (ADJ-YYYY-####)
- **Stock Operation**: Set absolute quantity (can increase or decrease)
- **Warehouses**: Source required, destination not applicable

## Error Handling

1. **Insufficient Stock**: Rollback transaction, display error
2. **Warehouse Validation**: Prevent save if source = destination
3. **Database Errors**: Rollback all changes, log error
4. **Concurrency**: Transaction isolation handles concurrent updates
5. **Stock Consistency**: Verify quantities match between source and destination
