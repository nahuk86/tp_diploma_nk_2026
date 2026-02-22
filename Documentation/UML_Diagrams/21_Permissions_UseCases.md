# Permissions & Authorization - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Permission and Authorization-related use cases.

---

## UC-01: GetUserPermissions

### Class Diagram

```mermaid
classDiagram
    class IAuthorizationService {
        <<interface>>
        +HasPermission(userId, permissionCode) bool
        +HasAnyPermission(userId, permissionCodes) bool
        +HasAllPermissions(userId, permissionCodes) bool
        +GetUserPermissions(userId) List~string~
    }

    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        -ILogService _logService
        +HasPermission(userId, permissionCode) bool
        +HasAnyPermission(userId, permissionCodes) bool
        +HasAllPermissions(userId, permissionCodes) bool
        +GetUserPermissions(userId) List~string~
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
        +GetByCode(code) Permission
        +GetByModule(module) List~Permission~
        +GetAll() List~Permission~
    }

    class PermissionRepository {
        -DatabaseHelper _db
        +GetUserPermissions(userId) List~string~
        +GetByCode(code) Permission
        +GetByModule(module) List~Permission~
        -MapPermission(reader) Permission
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class SessionContext {
        <<singleton>>
        +CurrentUser User
        +CurrentUserId int?
        +CurrentUsername string
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Description
        +string Module
        +bool IsActive
    }

    class User {
        +int UserId
        +string Username
        +string FullName
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> DatabaseHelper : uses
    PermissionRepository --> Permission : returns
    SessionContext --> User : holds
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as AnyForm
    participant AUTH as AuthorizationService
    participant REPO as PermissionRepository
    participant DB as DatabaseHelper
    participant SESSION as SessionContext

    UI->>SESSION: Get CurrentUserId
    SESSION-->>UI: userId
    UI->>AUTH: GetUserPermissions(userId)
    activate AUTH
    AUTH->>REPO: GetUserPermissions(userId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.* FROM Permissions p JOIN RolePermissions rp ON p.PermissionId=rp.PermissionId JOIN UserRoles ur ON rp.RoleId=ur.RoleId WHERE ur.UserId=@UserId AND p.IsActive=1
    REPO-->>AUTH: List~Permission~
    deactivate REPO
    AUTH-->>UI: List~Permission~
    deactivate AUTH
    UI->>UI: Enable/disable UI elements based on permissions
```

---

## UC-02: HasAllPermissions

### Class Diagram

```mermaid
classDiagram
    class IAuthorizationService {
        <<interface>>
        +HasAllPermissions(userId, permissionCodes) bool
    }

    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        +HasAllPermissions(userId, permissionCodes) bool
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    class PermissionRepository {
        +GetUserPermissions(userId) List~string~
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Module
        +bool IsActive
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> Permission : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as AnyForm
    participant AUTH as AuthorizationService
    participant REPO as PermissionRepository
    participant DB as DatabaseHelper

    UI->>AUTH: HasAllPermissions(userId, ["CRUD_Products", "View_Products"])
    activate AUTH
    AUTH->>REPO: GetUserPermissions(userId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.PermissionCode FROM Permissions p JOIN RolePermissions rp ON p.PermissionId=rp.PermissionId JOIN UserRoles ur ON rp.RoleId=ur.RoleId WHERE ur.UserId=@UserId AND p.IsActive=1
    REPO-->>AUTH: List~Permission~
    deactivate REPO
    Note over AUTH: Check that EVERY required code is in userPermissions
    alt All permissions present
        AUTH-->>UI: true
    else At least one missing
        AUTH-->>UI: false
    end
    deactivate AUTH
    UI->>UI: Allow or restrict operation
```

---

## UC-03: HasAnyPermission

### Class Diagram

```mermaid
classDiagram
    class IAuthorizationService {
        <<interface>>
        +HasAnyPermission(userId, permissionCodes) bool
    }

    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        +HasAnyPermission(userId, permissionCodes) bool
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    class PermissionRepository {
        +GetUserPermissions(userId) List~string~
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Module
        +bool IsActive
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> Permission : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as AnyForm
    participant AUTH as AuthorizationService
    participant REPO as PermissionRepository
    participant DB as DatabaseHelper

    UI->>AUTH: HasAnyPermission(userId, ["CRUD_Reports", "View_Reports"])
    activate AUTH
    AUTH->>REPO: GetUserPermissions(userId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.PermissionCode FROM Permissions p ... WHERE ur.UserId=@UserId AND p.IsActive=1
    REPO-->>AUTH: List~Permission~
    deactivate REPO
    Note over AUTH: Check if ANY required code is in userPermissions
    alt At least one permission present
        AUTH-->>UI: true
    else None of the permissions present
        AUTH-->>UI: false
    end
    deactivate AUTH
    UI->>UI: Show or hide UI section
```

---

## UC-04: HasPermission

### Class Diagram

```mermaid
classDiagram
    class AnyForm {
        -IAuthorizationService _authService
        -SessionContext _session
        +CheckAccess() void
        -ApplyPermissions() void
    }

    class IAuthorizationService {
        <<interface>>
        +HasPermission(userId, permissionCode) bool
    }

    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        -ILogService _logService
        +HasPermission(userId, permissionCode) bool
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    class PermissionRepository {
        +GetUserPermissions(userId) List~string~
    }

    class SessionContext {
        <<singleton>>
        +CurrentUserId int?
        +CurrentUsername string
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Module
    }

    AnyForm --> IAuthorizationService : uses
    AnyForm --> SessionContext : reads
    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> Permission : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as AnyForm
    participant SESSION as SessionContext
    participant AUTH as AuthorizationService
    participant REPO as PermissionRepository
    participant DB as DatabaseHelper

    UI->>SESSION: Get CurrentUserId
    SESSION-->>UI: userId
    UI->>AUTH: HasPermission(userId, "CRUD_Products")
    activate AUTH
    AUTH->>REPO: GetUserPermissions(userId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.PermissionCode FROM Permissions p JOIN RolePermissions rp ON p.PermissionId=rp.PermissionId JOIN UserRoles ur ON rp.RoleId=ur.RoleId WHERE ur.UserId=@UserId AND p.IsActive=1
    REPO-->>AUTH: List~Permission~
    deactivate REPO
    Note over AUTH: Check if "CRUD_Products" is in userPermissions
    alt Permission found
        AUTH-->>UI: true
        UI->>UI: Enable buttons/controls
    else Permission not found
        AUTH-->>UI: false
        UI->>UI: Disable/hide buttons
        UI->>UI: Show "Access Denied" if action attempted
    end
    deactivate AUTH
```

---
