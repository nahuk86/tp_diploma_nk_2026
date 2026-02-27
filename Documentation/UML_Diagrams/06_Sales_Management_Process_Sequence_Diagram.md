# Sales Management Process - Sequence Diagrams (Per Use Case)

This document contains UML Sequence Diagrams organized per use case for all Sales Management operations.

---

## UC-01: CreateSale

```mermaid
sequenceDiagram
    participant User as Sales User
    participant UI as SalesForm
    participant SaleSvc as SaleService
    participant SaleRepo as SaleRepository
    participant StockRepo as StockRepository
    participant DB as Database

    User->>UI: Fill sale header + lines, click "Save"
    activate UI
    UI->>UI: ValidateForm()
    alt Validation Fails
        UI-->>User: Show validation errors
    else Validation Passes
        UI->>SaleSvc: CreateSale(sale, saleLines, currentUserId)
        activate SaleSvc
        SaleSvc->>SaleSvc: _saleLock.Wait() [acquire semaphore]
        Note over SaleSvc: Critical section: only one thread<br/>may execute from here at a time
        SaleSvc->>SaleSvc: ValidateSale(sale, saleLines)
        SaleSvc->>SaleSvc: GenerateSaleNumber()
        Note over SaleSvc: Format: SALE-{yyyyMMdd}-{seq}
        loop For each line
            SaleSvc->>StockRepo: GetByProduct(productId)
            StockRepo-->>SaleSvc: List~Stock~
            Note over SaleSvc: Verify available stock
        end
        SaleSvc->>SaleRepo: CreateWithLines(sale, saleLines)
        activate SaleRepo
        SaleRepo->>DB: BEGIN TRANSACTION
        Note over SaleRepo: INSERT INTO Sales (...) → saleId
        loop For each line
            Note over SaleRepo: INSERT INTO SaleLines (SaleId, ProductId, Qty, ...)
        end
        SaleRepo->>DB: COMMIT
        SaleRepo-->>SaleSvc: saleId
        deactivate SaleRepo
        SaleSvc->>SaleSvc: DeductInventoryForSale(saleLines, userId, saleNumber)
        loop For each product group
            SaleSvc->>StockRepo: GetByProduct(productId)
            StockRepo-->>SaleSvc: List~Stock~
            Note over SaleSvc: FIFO deduction across warehouses
            SaleSvc->>StockRepo: UpdateStock(productId, warehouseId, newQuantity, userId)
            StockRepo->>DB: UPDATE Stock SET Quantity=@NewQty, LastUpdated=@Now WHERE ProductId=@PId AND WarehouseId=@WId
        end
        SaleSvc->>SaleSvc: _saleLock.Release() [release semaphore - finally block]
        SaleSvc-->>UI: saleId
        deactivate SaleSvc
        UI->>SaleSvc: GetAllSalesWithDetails()
        SaleSvc-->>UI: List~Sale~
        UI-->>User: Show success & refresh grid
    end
    deactivate UI
```

---

## UC-02: DeleteSale

```mermaid
sequenceDiagram
    participant User as Sales User
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    User->>UI: Select sale, click "Delete"
    activate UI
    UI->>UI: Confirm deletion dialog
    alt User cancels
        UI-->>User: Do nothing
    else User confirms
        UI->>SVC: DeleteSale(saleId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Sales SET IsActive=0, UpdatedAt=@Now WHERE SaleId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>SVC: GetAllSales()
        SVC-->>UI: List~Sale~
        UI-->>User: Show success & refresh grid
    end
    deactivate UI
```

---

## UC-03: GetAllSales

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    UI->>SVC: GetAllSales()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT s.*, c.Nombre + ' ' + c.Apellido AS ClientName FROM Sales s JOIN Clients c ON s.ClientId=c.ClientId ORDER BY SaleDate DESC
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-04: GetAllSalesWithDetails

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    UI->>SVC: GetAllSalesWithDetails()
    activate SVC
    SVC->>REPO: GetAllWithDetails()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT s.*, sl.*, p.Name FROM Sales s LEFT JOIN SaleLines sl ON s.SaleId=sl.SaleId LEFT JOIN Products p ON sl.ProductId=p.ProductId
    REPO->>REPO: MapSale + MapSaleLines for each row
    REPO-->>SVC: List~Sale~ with SaleLines
    deactivate REPO
    SVC-->>UI: List~Sale~ with SaleLines
    deactivate SVC
    UI->>UI: Bind master-detail view
```

---

## UC-05: GetAvailabelStockByWarehouse

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant SREPO as StockRepository
    participant DB as Database

    UI->>UI: User selects a product
    UI->>SVC: GetAvailableStockByWarehouse(productId)
    activate SVC
    SVC->>SREPO: GetByProduct(productId)
    activate SREPO
    SREPO->>DB: GetConnection()
    DB-->>SREPO: SqlConnection
    Note over SREPO: SELECT s.*, w.Name FROM Stock s JOIN Warehouses w ON s.WarehouseId=w.WarehouseId WHERE s.ProductId=@ProductId AND s.Quantity > 0
    SREPO-->>SVC: List~Stock~
    deactivate SREPO
    SVC->>SVC: Build Dictionary[warehouseId, quantity]
    SVC-->>UI: Dictionary~int,int~
    deactivate SVC
    UI->>UI: Show stock per warehouse in dropdown
```

---

## UC-06: GetSaleById

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

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

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

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
    UI->>UI: Populate header + lines grid
```

---

## UC-08: GetSaleByClient

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    UI->>UI: User selects client filter
    UI->>SVC: GetSalesByClient(clientId)
    activate SVC
    SVC->>REPO: GetByClient(clientId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE ClientId=@ClientId AND IsActive=1 ORDER BY SaleDate DESC
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind filtered results
```

---

## UC-09: GetSaleByDateRange

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    UI->>UI: User selects date range
    UI->>SVC: GetSalesByDateRange(startDate, endDate)
    activate SVC
    SVC->>REPO: GetByDateRange(startDate, endDate)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE SaleDate BETWEEN @Start AND @End AND IsActive=1
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind filtered results
```

---

## UC-10: GetSaleBySeller

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    UI->>UI: User filters by seller name
    UI->>SVC: GetSalesBySeller(sellerName)
    activate SVC
    SVC->>REPO: GetBySeller(sellerName)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Sales WHERE SellerName=@SellerName AND IsActive=1 ORDER BY SaleDate DESC
    REPO-->>SVC: List~Sale~
    deactivate REPO
    SVC-->>UI: List~Sale~
    deactivate SVC
    UI->>UI: Bind filtered results
```

---

## UC-11: GetTotalAvailableStock

```mermaid
sequenceDiagram
    participant UI as SalesForm
    participant SVC as SaleService
    participant SREPO as StockRepository
    participant DB as Database

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
    SVC-->>UI: totalStock (int)
    deactivate SVC
    UI->>UI: Show total available stock label
```

---

## UC-12: UpdateSale

```mermaid
sequenceDiagram
    participant User as Sales User
    participant UI as SalesForm
    participant SVC as SaleService
    participant REPO as SaleRepository
    participant DB as Database

    User->>UI: Modify sale fields, click "Save"
    activate UI
    UI->>UI: ValidateForm()
    alt Validation Fails
        UI-->>User: Show validation errors
    else Validation Passes
        UI->>SVC: UpdateSale(sale, currentUserId)
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
        UI->>SVC: GetAllSales()
        SVC-->>UI: List~Sale~
        UI-->>User: Show success & refresh grid
    end
    deactivate UI
```

---

## Business Rules Summary

| Use Case | Key Business Rules |
|----------|-------------------|
| CreateSale | Sale number auto-generated; stock validated before save; atomic transaction; SemaphoreSlim prevents concurrent overselling |
| DeleteSale | Soft-delete only (IsActive=0) |
| UpdateSale | Header fields only; lines managed separately |
| GetAvailabelStockByWarehouse | Only returns warehouses with Quantity > 0 |
| GetTotalAvailableStock | Sums across all warehouses |

```mermaid
sequenceDiagram
    participant User as Sales User
    participant UI as SalesForm<br/>(UI Layer)
    participant SaleSvc as SaleService<br/>(BLL)
    participant ClientSvc as ClientService<br/>(BLL)
    participant ProdSvc as ProductService<br/>(BLL)
    participant SaleRepo as SaleRepository<br/>(DAO)
    participant StockRepo as StockRepository<br/>(DAO)
    participant AuditRepo as AuditLogRepository<br/>(DAO)
    participant DB as Database
    participant Log as ILogService

    %% Load Form
    User->>UI: Open Sales Form
    activate UI
    
    UI->>SaleSvc: GetAllSalesWithDetails()
    activate SaleSvc
    SaleSvc->>SaleRepo: GetAllWithDetails()
    activate SaleRepo
    SaleRepo->>DB: SELECT s.*, sl.*, c.*, p.* FROM Sales s<br/>LEFT JOIN SaleLines sl ON...<br/>LEFT JOIN Clients c ON...<br/>LEFT JOIN Products p ON...
    activate DB
    DB-->>SaleRepo: ResultSet
    deactivate DB
    SaleRepo->>SaleRepo: MapSale(reader) + MapSaleLines(reader)
    SaleRepo-->>SaleSvc: List<Sale> with SaleLines
    deactivate SaleRepo
    SaleSvc-->>UI: List<Sale>
    deactivate SaleSvc
    
    UI->>ClientSvc: GetActiveClients()
    activate ClientSvc
    ClientSvc-->>UI: List<Client>
    deactivate ClientSvc
    
    UI->>ProdSvc: GetActiveProducts()
    activate ProdSvc
    ProdSvc-->>UI: List<Product>
    deactivate ProdSvc
    
    UI-->>User: Display sales list, populate dropdowns
    deactivate UI

    %% Create New Sale
    User->>UI: Click "New Sale" button
    activate UI
    UI->>UI: ClearForm()
    UI->>UI: Initialize empty SaleLines collection
    UI-->>User: Display empty sale form
    deactivate UI

    User->>UI: Select Client from dropdown
    User->>UI: Enter Seller Name
    User->>UI: Select Sale Date

    %% Add Sale Lines
    loop For each product
        User->>UI: Click "Add Line" button
        activate UI
        UI-->>User: Show product selection dialog
        deactivate UI
        
        User->>UI: Select Product and enter Quantity
        activate UI
        
        %% Check available stock
        UI->>SaleSvc: GetAvailableStockByWarehouse(productId)
        activate SaleSvc
        SaleSvc->>StockRepo: GetByProduct(productId)
        activate StockRepo
        StockRepo->>DB: SELECT * FROM Stock<br/>WHERE ProductId = @ProductId
        activate DB
        DB-->>StockRepo: ResultSet
        deactivate DB
        StockRepo-->>SaleSvc: List<Stock>
        deactivate StockRepo
        SaleSvc->>SaleSvc: Build warehouse stock dictionary
        SaleSvc-->>UI: Dictionary<warehouseId, quantity>
        deactivate SaleSvc
        
        alt Insufficient Stock
            UI-->>User: Show "Insufficient stock" error
        else Stock Available
            UI->>UI: Add line to dgvSaleLines
            UI->>UI: CalculateTotals()
            UI-->>User: Display updated sale lines and total
        end
        deactivate UI
    end

    %% Save Sale
    User->>UI: Click "Save" button
    activate UI
    UI->>UI: ValidateForm()
    
    alt Validation Fails
        UI-->>User: Show validation errors
    else Validation Passes
        
        UI->>SaleSvc: CreateSale(sale, saleLines, currentUserId)
        activate SaleSvc
        
        %% Acquire semaphore lock to prevent race conditions
        SaleSvc->>SaleSvc: _saleLock.Wait() [acquire semaphore]
        Note over SaleSvc: Critical section: only one thread at a time<br/>may validate stock and create the sale,<br/>preventing concurrent stock overselling

        %% Validate business rules
        SaleSvc->>SaleSvc: ValidateSale(sale, saleLines)
        Note over SaleSvc: Check:<br/>- Client exists<br/>- Seller name not empty<br/>- Sale lines count > 0<br/>- All quantities > 0
        
        %% Generate sale number
        SaleSvc->>SaleSvc: GenerateSaleNumber()
        Note over SaleSvc: Format: SALE-{yyyy}{MM}{dd}-{seq}
        
        %% Calculate total
        SaleSvc->>SaleSvc: Calculate TotalAmount = Sum(LineTotal)
        
        %% Set audit fields
        Note over SaleSvc: Set CreatedBy = currentUserId<br/>Set CreatedAt = DateTime.Now
        
        %% Create sale with lines (Transaction)
        SaleSvc->>SaleRepo: CreateWithLines(sale, saleLines)
        activate SaleRepo
        SaleRepo->>DB: BEGIN TRANSACTION
        activate DB
        
        Note over SaleRepo: INSERT INTO Sales<br/>(SaleNumber, SaleDate, ClientId,<br/>SellerName, TotalAmount,<br/>CreatedAt, CreatedBy)<br/>VALUES (...)
        SaleRepo->>DB: ExecuteScalar()
        DB-->>SaleRepo: saleId
        
        loop For each sale line
            Note over SaleRepo: INSERT INTO SaleLines<br/>(SaleId, ProductId, Quantity,<br/>UnitPrice, LineTotal)<br/>VALUES (...)
            SaleRepo->>DB: ExecuteNonQuery()
            DB-->>SaleRepo: Success
        end
        
        SaleRepo->>DB: COMMIT TRANSACTION
        deactivate DB
        SaleRepo-->>SaleSvc: saleId
        deactivate SaleRepo
        
        %% Deduct inventory from warehouses
        SaleSvc->>SaleSvc: DeductInventoryForSale(saleLines, userId, saleNumber)
        activate SaleSvc
        
        loop For each product group (FIFO across warehouses)
            SaleSvc->>StockRepo: GetByProduct(productId)
            activate StockRepo
            StockRepo->>DB: SELECT * FROM Stock WHERE ProductId=@ProductId AND Quantity>0 ORDER BY WarehouseName
            activate DB
            DB-->>StockRepo: ResultSet
            deactivate DB
            StockRepo-->>SaleSvc: List~Stock~
            deactivate StockRepo
            
            Note over SaleSvc: Calculate newQuantity = stock.Quantity - quantityToDeduct
            SaleSvc->>StockRepo: UpdateStock(productId, warehouseId, newQuantity, userId)
            activate StockRepo
            Note over StockRepo: UPDATE Stock<br/>SET Quantity = @NewQuantity,<br/>LastUpdated = @Now<br/>WHERE ProductId = @ProductId<br/>AND WarehouseId = @WarehouseId
            StockRepo->>DB: ExecuteNonQuery()
            activate DB
            DB-->>StockRepo: Success
            deactivate DB
            StockRepo-->>SaleSvc: void
            deactivate StockRepo
        end
        deactivate SaleSvc
        
        %% Log audit
        SaleSvc->>AuditRepo: LogChange("Sales", saleId, Insert, null, null, description, userId)
        activate AuditRepo
        AuditRepo->>DB: INSERT INTO AuditLog<br/>(TableName, RecordId, Action,<br/>FieldName, OldValue, NewValue,<br/>ChangedAt, ChangedBy)<br/>VALUES (...)
        activate DB
        DB-->>AuditRepo: Success
        deactivate DB
        deactivate AuditRepo
        
        %% Log info
        SaleSvc->>Log: Info($"Sale {saleNumber} created successfully")
        activate Log
        deactivate Log
        
        %% Release semaphore lock (always executed in finally block)
        SaleSvc->>SaleSvc: _saleLock.Release() [release semaphore - finally block]
        
        SaleSvc-->>UI: saleId
        deactivate SaleSvc
        
        %% Refresh display
        UI->>SaleSvc: GetAllSalesWithDetails()
        activate SaleSvc
        SaleSvc->>SaleRepo: GetAllWithDetails()
        activate SaleRepo
        SaleRepo->>DB: SELECT...
        activate DB
        DB-->>SaleRepo: ResultSet
        deactivate DB
        SaleRepo-->>SaleSvc: List<Sale>
        deactivate SaleRepo
        SaleSvc-->>UI: List<Sale>
        deactivate SaleSvc
        
        UI->>UI: ClearForm()
        UI-->>User: Show success message & refresh grid
    end
    deactivate UI
```

## Sequence Flow Description

### Phase 1: Form Initialization
1. Sales user opens the Sales Form
2. Form loads existing sales with details (joins Sales, SaleLines, Clients, Products)
3. Form populates client dropdown with active clients
4. Form populates product dropdown with active products
5. Display sales in main grid

### Phase 2: Create New Sale
6. User clicks "New Sale" button
7. Form clears all fields and initializes empty sale lines collection
8. User selects client, enters seller name, and sets sale date

### Phase 3: Add Sale Lines
9. For each product to sell:
   - User clicks "Add Line"
   - Selects product and enters quantity
   - System checks available stock across warehouses
   - If stock insufficient, show error
   - If stock available, add line to grid
   - Recalculate sale total

### Phase 4: Form Validation
10. User clicks "Save"
11. Form validates all inputs (client selected, seller name entered, lines added)

### Phase 5: Business Validation
12. SaleService validates business rules:
    - Client exists
    - Seller name not empty
    - Sale has at least one line
    - All quantities are positive
    - All prices are valid

### Phase 6: Sale Number Generation
13. Generate unique sale number with format: SALE-YYYYMMDD-###

### Phase 7: Calculate Total
14. Sum all line totals to get sale total amount

### Phase 8: Database Transaction - Create Sale
15. Begin database transaction
16. Insert sale header into Sales table
17. Retrieve new saleId via SCOPE_IDENTITY()
18. Insert each sale line into SaleLines table
19. Commit transaction (atomic operation)

### Phase 9: Inventory Deduction
20. For each sale line:
    - Begin transaction
    - Deduct quantity from warehouse stock
    - Validate stock doesn't go negative
    - Commit or rollback based on validation
    - Update LastUpdated timestamp

### Phase 10: Audit & Logging
21. Log change to AuditLog table with full sale details
22. Write info log message with sale number

### Phase 11: Completion
23. Return saleId to form
24. Refresh sales grid
25. Clear form for next sale
26. Display success message

## Business Rules Enforced

1. **Stock Validation**: Must have sufficient stock before creating sale
2. **Atomic Operations**: Sale and lines created in single transaction
3. **Inventory Consistency**: Stock deducted immediately upon sale creation
4. **Unique Sale Numbers**: Auto-generated sequential numbers by date
5. **Client Association**: Every sale must have a valid client
6. **Audit Trail**: All operations logged with user and timestamp
7. **Negative Stock Prevention**: Transaction rollback if stock goes negative
8. **Concurrency Control**: `_saleLock` (SemaphoreSlim(1,1)) serializes concurrent CreateSale calls; only one thread can execute the critical section (stock check + sale insert + stock deduction) at a time, preventing race conditions and overselling

## Error Handling

1. **Insufficient Stock**: Rollback transaction, display error to user
2. **Database Errors**: Rollback all changes, log error, display message
3. **Validation Errors**: Prevent save, highlight invalid fields
4. **Concurrency**: SemaphoreSlim lock in SaleService ensures serialized execution; `_saleLock.Release()` is always called in the `finally` block to guarantee the semaphore is released even when exceptions occur
