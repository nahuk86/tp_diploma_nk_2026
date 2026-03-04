# Products - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Product-related use cases.

---

## UC-01: CreateProduct

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateInputs() bool
        -ClearForm() void
        -LoadProducts() void
    }


    class ProductService {
        -IProductRepository _productRepository
        -ILogService _logService
        +CreateProduct(product) int
        -ValidateProduct(product) void
    }

    class IProductRepository {
        <<interface>>
        +Insert(product) int
        +SKUExists(sku, excludeId) bool
        +GetById(id) Product
        +GetAll() List~Product~
    }

    class ProductRepository {
        -DatabaseHelper _db
        +Insert(product) int
        +SKUExists(sku, excludeId) bool
        -MapProduct(reader) Product
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Description
        +string Category
        +decimal UnitPrice
        +int MinStockLevel
        +bool IsActive
        +DateTime CreatedAt
        +int CreatedBy
    }

    ProductsForm --> ProductService : uses
    ProductsForm --> ILogService : uses
    ProductService --> IProductRepository : uses
    ProductService --> ILogService : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> DatabaseHelper : uses
    ProductRepository --> Product : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: CreateProduct(product)
        activate SVC
        SVC->>SVC: ValidateProduct(product)
        SVC->>REPO: SKUExists(sku, 0)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        REPO-->>SVC: false (SKU is unique)
        deactivate REPO
        SVC->>REPO: Insert(product)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: INSERT INTO Products ...
        REPO-->>SVC: newProductId
        deactivate REPO
        SVC-->>UI: newProductId
        deactivate SVC
        UI->>UI: LoadProducts()
        UI-->>UI: Show success message
    end
```

---

## UC-02: DeleteProduct

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        +btnDelete_Click(sender, e) void
        -LoadProducts() void
    }


    class ProductService {
        -IProductRepository _productRepository
        -ILogService _logService
        +DeleteProduct(id, deletedBy) void
    }

    class IProductRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
        +GetById(id) Product
    }

    class ProductRepository {
        +SoftDelete(id, deletedBy) void
        +GetById(id) Product
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Product {
        +int ProductId
        +string Name
        +bool IsActive
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> DatabaseHelper : uses
    ProductRepository --> Product : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>UI: Confirm deletion dialog
    alt User cancels
        UI-->>UI: Do nothing
    else User confirms
        UI->>SVC: DeleteProduct(productId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Products SET IsActive=0 ...
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadProducts()
        UI-->>UI: Show success message
    end
```

---

## UC-03: GetActiveProducts

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        +LoadActiveProducts() void
    }


    class ProductService {
        -IProductRepository _productRepository
        +GetActiveProducts() List~Product~
    }

    class IProductRepository {
        <<interface>>
        +GetAllActive() List~Product~
    }

    class ProductRepository {
        +GetAllActive() List~Product~
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Category
        +decimal UnitPrice
        +bool IsActive
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> Product : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetActiveProducts()
    activate SVC
    SVC->>REPO: GetAllActive()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Products WHERE IsActive=1
    REPO-->>SVC: List~Product~
    deactivate REPO
    SVC-->>UI: List~Product~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-04: GetAllProducts

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        +LoadAllProducts() void
    }


    class ProductService {
        -IProductRepository _productRepository
        +GetAllProducts() List~Product~
    }

    class IProductRepository {
        <<interface>>
        +GetAll() List~Product~
    }

    class ProductRepository {
        +GetAll() List~Product~
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Category
        +decimal UnitPrice
        +bool IsActive
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> Product : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllProducts()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Products (all, including inactive)
    REPO-->>SVC: List~Product~
    deactivate REPO
    SVC-->>UI: List~Product~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-05: GetProductById

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        +LoadProductDetails(id) void
    }


    class ProductService {
        -IProductRepository _productRepository
        +GetProductById(id) Product
    }

    class IProductRepository {
        <<interface>>
        +GetById(id) Product
    }

    class ProductRepository {
        +GetById(id) Product
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Description
        +string Category
        +decimal UnitPrice
        +int MinStockLevel
        +bool IsActive
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> Product : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetProductById(productId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Products WHERE ProductId=@Id
    REPO-->>SVC: Product
    deactivate REPO
    alt Product not found
        SVC-->>UI: null
        UI-->>UI: Show not found message
    else Product found
        SVC-->>UI: Product
        deactivate SVC
        UI->>UI: Populate form fields
    end
```

---

## UC-06: GetProductsByCategory

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        +FilterByCategory(category) void
    }


    class ProductService {
        -IProductRepository _productRepository
        +GetProductsByCategory(category) List~Product~
    }

    class IProductRepository {
        <<interface>>
        +GetByCategory(category) List~Product~
    }

    class ProductRepository {
        +GetByCategory(category) List~Product~
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Category
        +decimal UnitPrice
        +bool IsActive
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> Product : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects category filter
    UI->>SVC: GetProductsByCategory(category)
    activate SVC
    SVC->>REPO: GetByCategory(category)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Products WHERE Category=@Category AND IsActive=1
    REPO-->>SVC: List~Product~
    deactivate REPO
    SVC-->>UI: List~Product~
    deactivate SVC
    UI->>UI: Bind filtered results to DataGridView
```

---

## UC-07: SearchProduct

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        +txtSearch_TextChanged(sender, e) void
        -SearchProducts(term) void
    }


    class ProductService {
        -IProductRepository _productRepository
        +SearchProducts(term) List~Product~
    }

    class IProductRepository {
        <<interface>>
        +Search(term) List~Product~
    }

    class ProductRepository {
        +Search(term) List~Product~
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Description
        +string Category
        +decimal UnitPrice
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> Product : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>UI: User types search term
    UI->>SVC: SearchProducts(searchTerm)
    activate SVC
    SVC->>REPO: Search(term)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Products WHERE Name LIKE '%@Term%' OR SKU LIKE '%@Term%'
    REPO-->>SVC: List~Product~
    deactivate REPO
    SVC-->>UI: List~Product~
    deactivate SVC
    UI->>UI: Bind search results to DataGridView
```

---

## UC-08: UpdateProduct

### Class Diagram

```mermaid
classDiagram
    class ProductsForm {
        -ProductService _productService
        -ILogService _logService
        +btnUpdate_Click(sender, e) void
        -ValidateInputs() bool
        -LoadProducts() void
    }


    class ProductService {
        -IProductRepository _productRepository
        -ILogService _logService
        +UpdateProduct(product) void
        -ValidateProduct(product) void
    }

    class IProductRepository {
        <<interface>>
        +Update(product) void
        +SKUExists(sku, excludeId) bool
        +GetById(id) Product
    }

    class ProductRepository {
        +Update(product) void
        +SKUExists(sku, excludeId) bool
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Product {
        +int ProductId
        +string SKU
        +string Name
        +string Description
        +string Category
        +decimal UnitPrice
        +int MinStockLevel
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    ProductsForm --> ProductService : uses
    ProductService --> IProductRepository : uses
    ProductRepository ..|> IProductRepository : implements
    ProductRepository --> DatabaseHelper : uses
    ProductRepository --> Product : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ProductsForm
    participant SVC as ProductService
    participant REPO as ProductRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: UpdateProduct(product)
        activate SVC
        SVC->>SVC: ValidateProduct(product)
        SVC->>REPO: SKUExists(sku, productId)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        REPO-->>SVC: false (SKU is unique or belongs to this product)
        deactivate REPO
        SVC->>REPO: Update(product)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Products SET Name=@Name, ... WHERE ProductId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadProducts()
        UI-->>UI: Show success message
    end
```

---
