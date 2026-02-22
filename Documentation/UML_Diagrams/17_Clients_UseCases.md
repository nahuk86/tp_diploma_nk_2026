# Clients - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Client-related use cases.

---

## UC-01: CreateClient

### Class Diagram

```mermaid
classDiagram
    class ClientsForm {
        -IClientService _clientService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateInputs() bool
        -LoadClients() void
    }

    class IClientService {
        <<interface>>
        +CreateClient(client) int
        +UpdateClient(client) void
        +DeleteClient(id, deletedBy) void
        +GetAllClients() List~Client~
        +GetActiveClients() List~Client~
        +GetClientById(id) Client
    }

    class ClientService {
        -IClientRepository _clientRepository
        -ILogService _logService
        +CreateClient(client) int
        -ValidateClient(client) void
    }

    class IClientRepository {
        <<interface>>
        +Insert(client) int
        +DNIExists(dni, excludeId) bool
        +GetById(id) Client
        +GetAll() List~Client~
    }

    class ClientRepository {
        -DatabaseHelper _db
        +Insert(client) int
        +DNIExists(dni, excludeId) bool
        -MapClient(reader) Client
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Client {
        +int ClientId
        +string Nombre
        +string Apellido
        +string Correo
        +string DNI
        +string Telefono
        +string Direccion
        +bool IsActive
        +DateTime CreatedAt
        +int CreatedBy
    }

    ClientsForm --> IClientService : uses
    ClientsForm --> ILogService : uses
    ClientService ..|> IClientService : implements
    ClientService --> IClientRepository : uses
    ClientService --> ILogService : uses
    ClientRepository ..|> IClientRepository : implements
    ClientRepository --> DatabaseHelper : uses
    ClientRepository --> Client : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ClientsForm
    participant SVC as ClientService
    participant REPO as ClientRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: CreateClient(client)
        activate SVC
        SVC->>SVC: ValidateClient(client)
        SVC->>REPO: DNIExists(dni, 0)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: SELECT COUNT(*) FROM Clients WHERE DNI=@DNI AND ClientId != @ExcludeId
        REPO-->>SVC: false (DNI is unique)
        deactivate REPO
        SVC->>REPO: Insert(client)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: INSERT INTO Clients (Nombre, Apellido, DNI, Correo, ...) VALUES (...)
        REPO-->>SVC: newClientId
        deactivate REPO
        SVC-->>UI: newClientId
        deactivate SVC
        UI->>UI: LoadClients()
        UI-->>UI: Show success message
    end
```

---

## UC-02: DeleteClient

### Class Diagram

```mermaid
classDiagram
    class ClientsForm {
        -IClientService _clientService
        +btnDelete_Click(sender, e) void
        -LoadClients() void
    }

    class IClientService {
        <<interface>>
        +DeleteClient(id, deletedBy) void
    }

    class ClientService {
        -IClientRepository _clientRepository
        +DeleteClient(id, deletedBy) void
    }

    class IClientRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
        +GetById(id) Client
    }

    class ClientRepository {
        +SoftDelete(id, deletedBy) void
    }

    class Client {
        +int ClientId
        +string Nombre
        +string Apellido
        +bool IsActive
    }

    ClientsForm --> IClientService : uses
    ClientService ..|> IClientService : implements
    ClientService --> IClientRepository : uses
    ClientRepository ..|> IClientRepository : implements
    ClientRepository --> Client : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ClientsForm
    participant SVC as ClientService
    participant REPO as ClientRepository
    participant DB as DatabaseHelper

    UI->>UI: Confirm deletion dialog
    alt User cancels
        UI-->>UI: Do nothing
    else User confirms
        UI->>SVC: DeleteClient(clientId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Clients SET IsActive=0, UpdatedAt=@Now, UpdatedBy=@UserId WHERE ClientId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadClients()
        UI-->>UI: Show success message
    end
```

---

## UC-03: GetActiveClients

### Class Diagram

```mermaid
classDiagram
    class ClientsForm {
        -IClientService _clientService
        +LoadActiveClients() void
    }

    class IClientService {
        <<interface>>
        +GetActiveClients() List~Client~
    }

    class ClientService {
        -IClientRepository _clientRepository
        +GetActiveClients() List~Client~
    }

    class IClientRepository {
        <<interface>>
        +GetAllActive() List~Client~
    }

    class ClientRepository {
        +GetAllActive() List~Client~
    }

    class Client {
        +int ClientId
        +string Nombre
        +string Apellido
        +string DNI
        +string Correo
        +bool IsActive
    }

    ClientsForm --> IClientService : uses
    ClientService ..|> IClientService : implements
    ClientService --> IClientRepository : uses
    ClientRepository ..|> IClientRepository : implements
    ClientRepository --> Client : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ClientsForm
    participant SVC as ClientService
    participant REPO as ClientRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetActiveClients()
    activate SVC
    SVC->>REPO: GetAllActive()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Clients WHERE IsActive=1 ORDER BY Apellido, Nombre
    REPO-->>SVC: List~Client~
    deactivate REPO
    SVC-->>UI: List~Client~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-04: GetAllClients

### Class Diagram

```mermaid
classDiagram
    class ClientsForm {
        -IClientService _clientService
        +LoadAllClients() void
    }

    class IClientService {
        <<interface>>
        +GetAllClients() List~Client~
    }

    class ClientService {
        -IClientRepository _clientRepository
        +GetAllClients() List~Client~
    }

    class IClientRepository {
        <<interface>>
        +GetAll() List~Client~
    }

    class ClientRepository {
        +GetAll() List~Client~
    }

    class Client {
        +int ClientId
        +string Nombre
        +string Apellido
        +string DNI
        +string Correo
        +bool IsActive
    }

    ClientsForm --> IClientService : uses
    ClientService ..|> IClientService : implements
    ClientService --> IClientRepository : uses
    ClientRepository ..|> IClientRepository : implements
    ClientRepository --> Client : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ClientsForm
    participant SVC as ClientService
    participant REPO as ClientRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllClients()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Clients ORDER BY Apellido, Nombre
    REPO-->>SVC: List~Client~
    deactivate REPO
    SVC-->>UI: List~Client~
    deactivate SVC
    UI->>UI: Bind to DataGridView (all including inactive)
```

---

## UC-05: GetClientById

### Class Diagram

```mermaid
classDiagram
    class ClientsForm {
        -IClientService _clientService
        +LoadClientDetails(id) void
    }

    class IClientService {
        <<interface>>
        +GetClientById(id) Client
    }

    class ClientService {
        -IClientRepository _clientRepository
        +GetClientById(id) Client
    }

    class IClientRepository {
        <<interface>>
        +GetById(id) Client
    }

    class ClientRepository {
        +GetById(id) Client
    }

    class Client {
        +int ClientId
        +string Nombre
        +string Apellido
        +string DNI
        +string Correo
        +string Telefono
        +string Direccion
        +bool IsActive
    }

    ClientsForm --> IClientService : uses
    ClientService ..|> IClientService : implements
    ClientService --> IClientRepository : uses
    ClientRepository ..|> IClientRepository : implements
    ClientRepository --> Client : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ClientsForm
    participant SVC as ClientService
    participant REPO as ClientRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetClientById(clientId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Clients WHERE ClientId=@Id
    REPO-->>SVC: Client
    deactivate REPO
    alt Not found
        SVC-->>UI: null
        UI-->>UI: Show not found message
    else Found
        SVC-->>UI: Client
        deactivate SVC
        UI->>UI: Populate form fields
    end
```

---

## UC-06: UpdateClient

### Class Diagram

```mermaid
classDiagram
    class ClientsForm {
        -IClientService _clientService
        +btnUpdate_Click(sender, e) void
        -ValidateInputs() bool
        -LoadClients() void
    }

    class IClientService {
        <<interface>>
        +UpdateClient(client) void
    }

    class ClientService {
        -IClientRepository _clientRepository
        -ILogService _logService
        +UpdateClient(client) void
        -ValidateClient(client) void
    }

    class IClientRepository {
        <<interface>>
        +Update(client) void
        +DNIExists(dni, excludeId) bool
    }

    class ClientRepository {
        +Update(client) void
        +DNIExists(dni, excludeId) bool
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Client {
        +int ClientId
        +string Nombre
        +string Apellido
        +string DNI
        +string Correo
        +string Telefono
        +string Direccion
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    ClientsForm --> IClientService : uses
    ClientService ..|> IClientService : implements
    ClientService --> IClientRepository : uses
    ClientRepository ..|> IClientRepository : implements
    ClientRepository --> DatabaseHelper : uses
    ClientRepository --> Client : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as ClientsForm
    participant SVC as ClientService
    participant REPO as ClientRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: UpdateClient(client)
        activate SVC
        SVC->>SVC: ValidateClient(client)
        SVC->>REPO: DNIExists(dni, clientId)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        REPO-->>SVC: false (DNI unique or same client)
        deactivate REPO
        SVC->>REPO: Update(client)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Clients SET Nombre=@Nombre, Apellido=@Ap, DNI=@DNI, ... WHERE ClientId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadClients()
        UI-->>UI: Show success message
    end
```

---
