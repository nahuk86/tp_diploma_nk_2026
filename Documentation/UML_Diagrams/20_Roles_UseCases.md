# Roles - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all Role-related use cases.

---

## UC-01: CreateRole

### Class Diagram

```mermaid
classDiagram
    class RolesForm {
        -IRoleService _roleService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateInputs() bool
        -LoadRoles() void
    }

    class IRoleService {
        <<interface>>
        +CreateRole(role) int
        +UpdateRole(role) void
        +DeleteRole(id, deletedBy) void
        +GetAllRoles() List~Role~
        +GetActiveRoles() List~Role~
        +GetRoleById(id) Role
        +GetRolePermissions(roleId) List~Permission~
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
    }

    class RoleService {
        -IRoleRepository _roleRepository
        -ILogService _logService
        +CreateRole(role) int
        -ValidateRole(role) void
    }

    class IRoleRepository {
        <<interface>>
        +Insert(role) int
        +GetByName(name) Role
        +GetById(id) Role
        +GetAll() List~Role~
    }

    class RoleRepository {
        -DatabaseHelper _db
        +Insert(role) int
        +GetByName(name) Role
        -MapRole(reader) Role
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
        +DateTime CreatedAt
        +int CreatedBy
    }

    RolesForm --> IRoleService : uses
    RolesForm --> ILogService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleService --> ILogService : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> DatabaseHelper : uses
    RoleRepository --> Role : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolesForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: CreateRole(role)
        activate SVC
        SVC->>SVC: ValidateRole(role)
        SVC->>REPO: GetByName(roleName)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: SELECT * FROM Roles WHERE RoleName=@Name
        REPO-->>SVC: null (role name is unique)
        deactivate REPO
        SVC->>REPO: Insert(role)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: INSERT INTO Roles (RoleName, Description, IsActive, ...) VALUES (...)
        REPO-->>SVC: newRoleId
        deactivate REPO
        SVC-->>UI: newRoleId
        deactivate SVC
        UI->>UI: LoadRoles()
        UI-->>UI: Show success message
    end
```

---

## UC-02: DeleteRole

### Class Diagram

```mermaid
classDiagram
    class RolesForm {
        -IRoleService _roleService
        +btnDelete_Click(sender, e) void
        -LoadRoles() void
    }

    class IRoleService {
        <<interface>>
        +DeleteRole(id, deletedBy) void
    }

    class RoleService {
        -IRoleRepository _roleRepository
        +DeleteRole(id, deletedBy) void
    }

    class IRoleRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
        +GetById(id) Role
    }

    class RoleRepository {
        +SoftDelete(id, deletedBy) void
    }

    class Role {
        +int RoleId
        +string RoleName
        +bool IsActive
    }

    RolesForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolesForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>UI: Confirm deletion dialog
    alt User cancels
        UI-->>UI: Do nothing
    else User confirms
        UI->>SVC: DeleteRole(roleId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Roles SET IsActive=0, UpdatedAt=@Now WHERE RoleId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadRoles()
        UI-->>UI: Show success message
    end
```

---

## UC-03: AssignPermissions

### Class Diagram

```mermaid
classDiagram
    class RolePermissionsForm {
        -IRoleService _roleService
        -IPermissionRepository _permissionRepo
        -ILogService _logService
        +btnAssignPermissions_Click(sender, e) void
        -LoadRolePermissions(roleId) void
        -LoadAllPermissions() void
    }

    class IRoleService {
        <<interface>>
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
        +GetRolePermissions(roleId) List~Permission~
    }

    class RoleService {
        -IRoleRepository _roleRepository
        -ILogService _logService
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
    }

    class IRoleRepository {
        <<interface>>
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
        +ClearPermissions(roleId) void
        +GetRolePermissions(roleId) List~Permission~
    }

    class RoleRepository {
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
        +ClearPermissions(roleId) void
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Description
        +string Module
        +bool IsActive
    }

    RolePermissionsForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> DatabaseHelper : uses
    RoleRepository --> Permission : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolePermissionsForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>UI: User checks/unchecks permissions
    UI->>SVC: AssignPermission(roleId, permissionId, userId)
    activate SVC
    SVC->>REPO: AssignPermission(roleId, permissionId, assignedBy)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: INSERT INTO RolePermissions (RoleId, PermissionId, AssignedAt, AssignedBy) VALUES (...)
    REPO-->>SVC: void
    deactivate REPO
    SVC-->>UI: void
    deactivate SVC
    UI->>UI: LoadRolePermissions(roleId)
    UI-->>UI: Show success message
```

---

## UC-04: GetActiveRoles

### Class Diagram

```mermaid
classDiagram
    class RolesForm {
        -IRoleService _roleService
        +LoadActiveRoles() void
    }

    class IRoleService {
        <<interface>>
        +GetActiveRoles() List~Role~
    }

    class RoleService {
        -IRoleRepository _roleRepository
        +GetActiveRoles() List~Role~
    }

    class IRoleRepository {
        <<interface>>
        +GetAllActive() List~Role~
    }

    class RoleRepository {
        +GetAllActive() List~Role~
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
    }

    RolesForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolesForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetActiveRoles()
    activate SVC
    SVC->>REPO: GetAllActive()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Roles WHERE IsActive=1 ORDER BY RoleName
    REPO-->>SVC: List~Role~
    deactivate REPO
    SVC-->>UI: List~Role~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-05: GetAllPermissions

### Class Diagram

```mermaid
classDiagram
    class RolePermissionsForm {
        -IPermissionRepository _permissionRepo
        +LoadAllPermissions() void
    }

    class IPermissionRepository {
        <<interface>>
        +GetAll() List~Permission~
        +GetByModule(module) List~Permission~
        +GetByCode(code) Permission
        +GetUserPermissions(userId) List~Permission~
    }

    class PermissionRepository {
        -DatabaseHelper _db
        +GetAll() List~Permission~
        +GetByModule(module) List~Permission~
        -MapPermission(reader) Permission
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Description
        +string Module
        +bool IsActive
        +DateTime CreatedAt
    }

    RolePermissionsForm --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> DatabaseHelper : uses
    PermissionRepository --> Permission : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolePermissionsForm
    participant REPO as PermissionRepository
    participant DB as DatabaseHelper

    UI->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Permissions WHERE IsActive=1 ORDER BY Module, PermissionName
    REPO-->>UI: List~Permission~
    deactivate REPO
    UI->>UI: Group permissions by Module
    UI->>UI: Render permissions checklist
```

---

## UC-06: GetAllRoles

### Class Diagram

```mermaid
classDiagram
    class RolesForm {
        -IRoleService _roleService
        +LoadAllRoles() void
    }

    class IRoleService {
        <<interface>>
        +GetAllRoles() List~Role~
    }

    class RoleService {
        -IRoleRepository _roleRepository
        +GetAllRoles() List~Role~
    }

    class IRoleRepository {
        <<interface>>
        +GetAll() List~Role~
    }

    class RoleRepository {
        +GetAll() List~Role~
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
    }

    RolesForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolesForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllRoles()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Roles ORDER BY RoleName
    REPO-->>SVC: List~Role~
    deactivate REPO
    SVC-->>UI: List~Role~
    deactivate SVC
    UI->>UI: Bind to DataGridView (all including inactive)
```

---

## UC-07: GetRoleById

### Class Diagram

```mermaid
classDiagram
    class RolesForm {
        -IRoleService _roleService
        +LoadRoleDetails(id) void
    }

    class IRoleService {
        <<interface>>
        +GetRoleById(id) Role
    }

    class RoleService {
        -IRoleRepository _roleRepository
        +GetRoleById(id) Role
    }

    class IRoleRepository {
        <<interface>>
        +GetById(id) Role
    }

    class RoleRepository {
        +GetById(id) Role
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
        +DateTime CreatedAt
    }

    RolesForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolesForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetRoleById(roleId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Roles WHERE RoleId=@Id
    REPO-->>SVC: Role
    deactivate REPO
    alt Not found
        SVC-->>UI: null
        UI-->>UI: Show not found message
    else Found
        SVC-->>UI: Role
        deactivate SVC
        UI->>UI: Populate form fields
    end
```

---

## UC-08: GetRolePermissions

### Class Diagram

```mermaid
classDiagram
    class RolePermissionsForm {
        -IRoleService _roleService
        +LoadRolePermissions(roleId) void
    }

    class IRoleService {
        <<interface>>
        +GetRolePermissions(roleId) List~Permission~
    }

    class RoleService {
        -IRoleRepository _roleRepository
        +GetRolePermissions(roleId) List~Permission~
    }

    class IRoleRepository {
        <<interface>>
        +GetRolePermissions(roleId) List~Permission~
    }

    class RoleRepository {
        +GetRolePermissions(roleId) List~Permission~
    }

    class Role {
        +int RoleId
        +string RoleName
    }

    class Permission {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Module
    }

    RolePermissionsForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    Role "1" --> "many" Permission : hasPermissions
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolePermissionsForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetRolePermissions(roleId)
    activate SVC
    SVC->>REPO: GetRolePermissions(roleId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT p.* FROM Permissions p JOIN RolePermissions rp ON p.PermissionId=rp.PermissionId WHERE rp.RoleId=@RoleId AND p.IsActive=1
    REPO-->>SVC: List~Permission~
    deactivate REPO
    SVC-->>UI: List~Permission~
    deactivate SVC
    UI->>UI: Mark assigned permissions in checklist
```

---

## UC-09: UpdateRole

### Class Diagram

```mermaid
classDiagram
    class RolesForm {
        -IRoleService _roleService
        +btnUpdate_Click(sender, e) void
        -ValidateInputs() bool
        -LoadRoles() void
    }

    class IRoleService {
        <<interface>>
        +UpdateRole(role) void
    }

    class RoleService {
        -IRoleRepository _roleRepository
        -ILogService _logService
        +UpdateRole(role) void
        -ValidateRole(role) void
    }

    class IRoleRepository {
        <<interface>>
        +Update(role) void
        +GetByName(name) Role
    }

    class RoleRepository {
        +Update(role) void
        +GetByName(name) Role
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    RolesForm --> IRoleService : uses
    RoleService ..|> IRoleService : implements
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> DatabaseHelper : uses
    RoleRepository --> Role : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as RolesForm
    participant SVC as RoleService
    participant REPO as RoleRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: UpdateRole(role)
        activate SVC
        SVC->>SVC: ValidateRole(role)
        SVC->>REPO: GetByName(roleName)
        activate REPO
        REPO-->>SVC: null or same role (name available)
        deactivate REPO
        SVC->>REPO: Update(role)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Roles SET RoleName=@Name, Description=@Desc, IsActive=@Active, UpdatedAt=@Now WHERE RoleId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadRoles()
        UI-->>UI: Show success message
    end
```

---
