# Role & Permissions Management Process - Class Diagrams (Per Use Case)

This document contains UML Class Diagrams organized per use case for all Role and Permission operations.

---

## UC-01: CreateRole

```mermaid
classDiagram
    class RolesForm {
        -RoleService _roleService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateForm() bool
        -LoadRoles() void
    }

    class RoleService {
        -IRoleRepository _roleRepo
        -ILogService _logService
        +CreateRole(role) int
        -ValidateRole(role) void
    }

    class IRoleRepository {
        <<interface>>
        +Insert(role) int
        +GetByName(name) Role
    }

    class RoleRepository {
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

    RolesForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> DatabaseHelper : uses
    RoleRepository --> Role : maps
```

---

## UC-02: DeleteRole

```mermaid
classDiagram
    class RolesForm {
        -RoleService _roleService
        +btnDelete_Click(sender, e) void
        -LoadRoles() void
    }

    class RoleService {
        -IRoleRepository _roleRepo
        +DeleteRole(roleId) void
    }

    class IRoleRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
    }

    class RoleRepository {
        +SoftDelete(id, deletedBy) void
    }

    class Role {
        +int RoleId
        +string RoleName
        +bool IsActive
    }

    RolesForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : maps
```

---

## UC-03: AssignPermissions

```mermaid
classDiagram
    class RolePermissionsForm {
        -RoleService _roleService
        -ILogService _logService
        -int _roleId
        -CheckedListBox clbPermissions
        +LoadPermissions() void
        +LoadRolePermissions() void
        +btnSave_Click(sender, e) void
    }

    class RoleService {
        -IRoleRepository _roleRepo
        -IPermissionRepository _permissionRepo
        -IAuditLogRepository _auditRepo
        -ILogService _logService
        +AssignPermissions(roleId, permissionIds) void
        +GetAllPermissions() List~Permission~
        +GetRolePermissions(roleId) List~Permission~
    }

    class IRoleRepository {
        <<interface>>
        +GetRolePermissions(roleId) List~Permission~
        +AssignPermission(roleId, permissionId, assignedBy) void
        +ClearPermissions(roleId) void
    }

    class IPermissionRepository {
        <<interface>>
        +GetAllActive() List~Permission~
    }

    class RoleRepository {
        +GetRolePermissions(roleId) List~Permission~
        +AssignPermission(roleId, permissionId, assignedBy) void
        +ClearPermissions(roleId) void
    }

    class PermissionRepository {
        +GetAllActive() List~Permission~
        -MapPermission(reader) Permission
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class Permission {
        +int PermissionId
        +string PermissionName
        +string Description
        +string Category
        +bool IsActive
    }

    class RolePermission {
        <<join_table>>
        +int RoleId
        +int PermissionId
        +DateTime AssignedAt
        +int AssignedBy
    }

    RolePermissionsForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleService --> IPermissionRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    PermissionRepository ..|> IPermissionRepository : implements
    RoleRepository --> DatabaseHelper : uses
    RoleRepository --> Permission : returns
    Permission "many" --> "many" RolePermission : linked via
```

---

## UC-04: GetActiveRoles

```mermaid
classDiagram
    class RolesForm {
        -RoleService _roleService
        +LoadActiveRoles() void
    }

    class RoleService {
        -IRoleRepository _roleRepo
        +GetActiveRoles() List~Role~
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

    RolesForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : returns
```

---

## UC-05: GetAllPermissions

```mermaid
classDiagram
    class RolePermissionsForm {
        -RoleService _roleService
        +LoadPermissions() void
    }

    class RoleService {
        -IPermissionRepository _permRepo
        +GetAllPermissions() List~Permission~
    }

    class IPermissionRepository {
        <<interface>>
        +GetAll() List~Permission~
    }

    class PermissionRepository {
        +GetAll() List~Permission~
    }

    class Permission {
        +int PermissionId
        +string PermissionName
        +string Category
        +bool IsActive
    }

    RolePermissionsForm --> RoleService : uses
    RoleService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> Permission : returns
```

---

## UC-06: GetAllRoles

```mermaid
classDiagram
    class RolesForm {
        -RoleService _roleService
        +LoadRoles() void
    }

    class RoleService {
        -IRoleRepository _roleRepo
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
        +DateTime CreatedAt
    }

    RolesForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : returns
```

---

## UC-07: GetRoleById

```mermaid
classDiagram
    class RolesForm {
        -RoleService _roleService
        +LoadRoleDetails(id) void
    }

    class RoleService {
        -IRoleRepository _roleRepo
        +GetRoleById(roleId) Role
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
        +DateTime UpdatedAt
    }

    RolesForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : returns
```

---

## UC-08: GetRolePermissions

```mermaid
classDiagram
    class RolePermissionsForm {
        -RoleService _roleService
        +LoadRolePermissions(roleId) void
    }

    class RoleService {
        -IPermissionRepository _permRepo
        +GetRolePermissions(roleId) List~Permission~
    }

    class IPermissionRepository {
        <<interface>>
        +GetRolePermissions(roleId) List~Permission~
    }

    class PermissionRepository {
        +GetRolePermissions(roleId) List~Permission~
    }

    class Permission {
        +int PermissionId
        +string PermissionName
        +string Category
        +bool IsActive
    }

    RolePermissionsForm --> RoleService : uses
    RoleService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
    PermissionRepository --> Permission : returns
```

---

## UC-09: UpdateRole

```mermaid
classDiagram
    class RolesForm {
        -RoleService _roleService
        +btnSave_Click(sender, e) void
        -ValidateForm() bool
    }

    class RoleService {
        -IRoleRepository _roleRepo
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
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    RolesForm --> RoleService : uses
    RoleService --> IRoleRepository : uses
    RoleRepository ..|> IRoleRepository : implements
    RoleRepository --> Role : maps
```

---

## UC-10: GetUserPermissions

```mermaid
classDiagram
    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        -ILogService _logService
        +GetUserPermissions(userId) List~string~
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    class PermissionRepository {
        +GetUserPermissions(userId) List~string~
    }

    class IAuthorizationService {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
```

---

## UC-11: HasAllPermissions

```mermaid
classDiagram
    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        +HasAllPermissions(userId, permissionCodes) bool
    }

    class IAuthorizationService {
        <<interface>>
        +HasAllPermissions(userId, permissionCodes) bool
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
```

---

## UC-12: HasAnyPermission

```mermaid
classDiagram
    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        +HasAnyPermission(userId, permissionCodes) bool
    }

    class IAuthorizationService {
        <<interface>>
        +HasAnyPermission(userId, permissionCodes) bool
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
```

---

## UC-13: HasPermission

```mermaid
classDiagram
    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        -ILogService _logService
        +HasPermission(userId, permissionCode) bool
    }

    class IAuthorizationService {
        <<interface>>
        +HasPermission(userId, permissionCode) bool
    }

    class IPermissionRepository {
        <<interface>>
        +GetUserPermissions(userId) List~string~
    }

    class PermissionRepository {
        +GetUserPermissions(userId) List~string~
    }

    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    PermissionRepository ..|> IPermissionRepository : implements
```

---

## Layer Communication Flow

```
┌─────────────────────┐
│    UI LAYER         │  RolesForm / RolePermissionsForm
└──────────┬──────────┘
           │ uses
           ▼
┌─────────────────────┐
│   BLL LAYER         │  RoleService
└──────────┬──────────┘
           │ calls
           ├──────────────────────┐
           ▼                      ▼
┌─────────────────────┐  ┌─────────────────────┐
│   DAO LAYER         │  │    SERVICES         │
│ RoleRepository      │  │ AuthorizationService│
│ PermissionRepository│  │ LogService          │
│ AuditLogRepository  │  │ SessionContext      │
└─────────────────────┘  └─────────────────────┘
```

## RBAC Model

```
User → UserRoles → Role → RolePermissions → Permission
```
        -RoleService _roleService
        -IAuthorizationService _authService
        -ILocalizationService _localizationService
        -ILogService _logService
        -DataGridView dgvRoles
        -TextBox txtRoleName
        -TextBox txtDescription
        -CheckBox chkIsActive
        -Button btnNew
        -Button btnSave
        -Button btnDelete
        -Button btnManagePermissions
        +RolesForm(services...)
        +LoadRoles() void
        +btnNew_Click(sender, e) void
        +btnSave_Click(sender, e) void
        +btnDelete_Click(sender, e) void
        +btnManagePermissions_Click(sender, e) void
        -ValidateForm() bool
        -ClearForm() void
    }

    class RolePermissionsForm {
        -RoleService _roleService
        -ILocalizationService _localizationService
        -ILogService _logService
        -int _roleId
        -string _roleName
        -CheckedListBox clbPermissions
        -Button btnSave
        -Button btnCancel
        +RolePermissionsForm(roleId, roleName, services...)
        +LoadPermissions() void
        +LoadRolePermissions() void
        +btnSave_Click(sender, e) void
    }

    %% BLL Layer
    class RoleService {
        -IRoleRepository _roleRepo
        -IPermissionRepository _permRepo
        -IAuditLogRepository _auditRepo
        -ILogService _logService
        +RoleService(repos, services...)
        +GetAllRoles() List~Role~
        +GetActiveRoles() List~Role~
        +GetRoleById(roleId) Role
        +CreateRole(role) int
        +UpdateRole(role) void
        +DeleteRole(roleId) void
        +GetRolePermissions(roleId) List~Permission~
        +AssignPermissions(roleId, permissionIds) void
        +RemovePermission(roleId, permissionId) void
        -ValidateRole(role) void
    }

    %% Services Layer
    class AuthorizationService {
        -IPermissionRepository _permissionRepository
        -ILogService _logService
        +AuthorizationService(permissionRepository, logService)
        +HasPermission(userId, permissionCode) bool
        +HasAnyPermission(userId, permissionCodes) bool
        +HasAllPermissions(userId, permissionCodes) bool
        +GetUserPermissions(userId) List~string~
    }

    class LoggingAuthorizationDecorator {
        -IAuthorizationService _inner
        -ILogService _logService
        +LoggingAuthorizationDecorator(inner, logService)
        +HasPermission(userId, permissionCode) bool
        +HasAnyPermission(userId, permissionCodes) bool
        +HasAllPermissions(userId, permissionCodes) bool
        +GetUserPermissions(userId) List~string~
    }

    class IAuthorizationService {
        <<interface>>
        +HasPermission(userId, permissionCode) bool
        +HasAnyPermission(userId, permissionCodes) bool
        +HasAllPermissions(userId, permissionCodes) bool
        +GetUserPermissions(userId) List~string~
    }

    class SessionContext {
        <<singleton>>
        -_instance SessionContext$
        +Instance SessionContext$
        +CurrentUser User
        +CurrentUserId int?
        +CurrentUsername string
        +Clear() void
    }

    %% Composite Pattern - Permission Rules
    class IPermissionRule {
        <<interface>>
        +Evaluate(userId, permissionRepository) bool
    }

    class SinglePermissionRule {
        -string _permissionCode
        +SinglePermissionRule(permissionCode)
        +Evaluate(userId, permissionRepository) bool
    }

    class AndPermissionRule {
        -IEnumerable~IPermissionRule~ _rules
        +AndPermissionRule(rules)
        +AndPermissionRule(permissionCodes)
        +Evaluate(userId, permissionRepository) bool
    }

    class OrPermissionRule {
        -IEnumerable~IPermissionRule~ _rules
        +OrPermissionRule(rules)
        +OrPermissionRule(permissionCodes)
        +Evaluate(userId, permissionRepository) bool
    }

    %% DAO Layer
    class RoleRepository {
        +GetAll() List~Role~
        +GetAllActive() List~Role~
        +GetById(id) Role
        +GetByName(name) Role
        +Insert(entity) int
        +Update(entity) void
        +Delete(id) void
        +SoftDelete(id, deletedBy) void
        +GetRolePermissions(roleId) List~Permission~
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
        +ClearPermissions(roleId) void
        -MapRole(reader) Role
    }

    class IRoleRepository {
        <<interface>>
        +GetAll() List~Role~
        +GetAllActive() List~Role~
        +GetById(id) Role
        +Insert(entity) int
        +Update(entity) void
        +Delete(id) void
        +SoftDelete(id, deletedBy) void
        +GetByName(name) Role
        +GetRolePermissions(roleId) List~Permission~
        +AssignPermission(roleId, permissionId, assignedBy) void
        +RemovePermission(roleId, permissionId) void
        +ClearPermissions(roleId) void
    }

    class PermissionRepository {
        +GetAll() List~Permission~
        +GetAllActive() List~Permission~
        +GetById(id) Permission
        +GetByCode(permissionCode) Permission
        +GetByModule(module) List~Permission~
        +GetUserPermissions(userId) List~string~
        -MapPermission(reader) Permission
    }

    class IPermissionRepository {
        <<interface>>
        +GetAll() List~Permission~
        +GetAllActive() List~Permission~
        +GetById(id) Permission
        +Insert(entity) int
        +Update(entity) void
        +Delete(id) void
        +SoftDelete(id, deletedBy) void
        +GetByCode(permissionCode) Permission
        +GetByModule(module) List~Permission~
        +GetUserPermissions(userId) List~string~
    }

    %% Domain Layer
    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
    }

    class Permission {
        +int PermissionId
        +string PermissionName
        +string Description
        +string Category
        +bool IsActive
    }

    class User {
        +int UserId
        +string Username
        +string FullName
        +string Email
        +bool IsActive
    }

    class RolePermission {
        <<join_table>>
        +int RoleId
        +int PermissionId
        +DateTime AssignedAt
        +int AssignedBy
    }

    class UserRole {
        <<join_table>>
        +int UserId
        +int RoleId
        +DateTime AssignedAt
        +int AssignedBy
    }

    %% Relationships
    RolesForm --> RoleService : uses
    RolesForm --> IAuthorizationService : uses
    RolesForm --> RolePermissionsForm : opens
    
    RolePermissionsForm --> RoleService : uses
    
    RoleService --> IRoleRepository : uses
    RoleService --> IPermissionRepository : uses
    RoleService --> IAuditLogRepository : uses
    RoleService --> SessionContext : uses
    
    AuthorizationService ..|> IAuthorizationService : implements
    AuthorizationService --> IPermissionRepository : uses
    
    LoggingAuthorizationDecorator ..|> IAuthorizationService : implements
    LoggingAuthorizationDecorator --> IAuthorizationService : decorates
    
    SinglePermissionRule ..|> IPermissionRule : implements
    AndPermissionRule ..|> IPermissionRule : implements
    OrPermissionRule ..|> IPermissionRule : implements
    AndPermissionRule --> IPermissionRule : composes
    OrPermissionRule --> IPermissionRule : composes
    
    RoleRepository ..|> IRoleRepository : implements
    PermissionRepository ..|> IPermissionRepository : implements
    
    RoleRepository --> Role : returns
    PermissionRepository --> Permission : returns
    
    Role "*" --> "*" Permission : has via RolePermission
    User "*" --> "*" Role : has via UserRole
    User "*" --> "*" Permission : inherits via roles
```

## Layer Communication Flow

```
┌─────────────────────┐
│    UI LAYER         │  RolesForm
│                     │  RolePermissionsForm
└──────────┬──────────┘
           │ uses
           ▼
┌─────────────────────┐
│   BLL LAYER         │  RoleService
└──────────┬──────────┘
           │ calls
           ├──────────────────────┐
           ▼                      ▼
┌─────────────────────┐  ┌─────────────────────┐
│   DAO LAYER         │  │    SERVICES         │
│                     │  │     LAYER           │
│ RoleRepository      │  │ AuthorizationService│
│ PermissionRepository│  │ LoggingAuthorizatio-│
│ AuditLogRepository  │  │   nDecorator        │
│                     │  │ LogService          │
│                     │  │ SessionContext      │
└──────────┬──────────┘  └─────────────────────┘
           │ returns
           ▼
┌─────────────────────┐
│  DOMAIN LAYER       │  Role
│                     │  Permission
│                     │  User
│                     │  RolePermission (join)
│                     │  UserRole (join)
└─────────────────────┘
```

## Permission Categories

### User Management
- **VIEW_USERS**: View user list
- **CREATE_USERS**: Create new users
- **EDIT_USERS**: Modify user information
- **DELETE_USERS**: Delete/deactivate users
- **ASSIGN_ROLES**: Assign roles to users

### Role Management
- **VIEW_ROLES**: View role list
- **CREATE_ROLES**: Create new roles
- **EDIT_ROLES**: Modify role information
- **DELETE_ROLES**: Delete/deactivate roles
- **MANAGE_PERMISSIONS**: Assign permissions to roles

### Sales Management
- **VIEW_SALES**: View sales list
- **CREATE_SALES**: Create new sales
- **EDIT_SALES**: Modify sales
- **DELETE_SALES**: Delete sales

### Product Management
- **VIEW_PRODUCTS**: View product list
- **CREATE_PRODUCTS**: Add new products
- **EDIT_PRODUCTS**: Modify products
- **DELETE_PRODUCTS**: Delete products

### Stock Management
- **VIEW_STOCK**: View stock levels
- **CREATE_STOCK_MOVEMENTS**: Create inventory movements
- **VIEW_STOCK_MOVEMENTS**: View movement history
- **ADJUST_STOCK**: Perform stock adjustments

### Warehouse Management
- **VIEW_WAREHOUSES**: View warehouse list
- **CREATE_WAREHOUSES**: Add new warehouses
- **EDIT_WAREHOUSES**: Modify warehouses
- **DELETE_WAREHOUSES**: Delete warehouses

### Client Management
- **VIEW_CLIENTS**: View client list
- **CREATE_CLIENTS**: Add new clients
- **EDIT_CLIENTS**: Modify client information
- **DELETE_CLIENTS**: Delete clients

### Reports
- **VIEW_REPORTS_GENERAL**: View basic sales reports
- **VIEW_REPORTS_CLIENTS**: View client-related reports
- **VIEW_REPORTS_ADVANCED**: View advanced analytics
- **VIEW_REPORTS_INVENTORY**: View inventory reports

### System Administration
- **VIEW_AUDIT_LOG**: View system audit logs
- **MANAGE_SYSTEM_SETTINGS**: Modify system configuration
- **VIEW_USER_MANUAL**: Access user documentation

## Role-Based Access Control (RBAC) Model

### Hierarchy
```
User → Assigned Roles → Role Permissions → Permission Checks
```

### Default Roles

1. **Administrator**
   - All permissions
   - Full system access
   - Can manage all users and roles

2. **Manager**
   - View and create: Sales, Products, Clients
   - View reports (general and advanced)
   - View stock and movements
   - Cannot manage users or roles

3. **Sales Representative**
   - View and create: Sales, Clients
   - View products and stock (read-only)
   - View basic reports
   - Cannot edit products or manage stock

4. **Warehouse Operator**
   - View and create: Stock movements
   - View products and warehouses
   - View inventory reports
   - Cannot access sales or clients

5. **Auditor**
   - Read-only access to all data
   - View all reports
   - View audit logs
   - Cannot create, edit, or delete anything

## Key Operations

### Create Role Flow
1. UI validates role name and description
2. RoleService checks for duplicate role name
3. Create role in database
4. Log audit entry
5. Refresh role list

### Assign Permissions Flow
1. Open RolePermissionsForm with roleId
2. Load all available permissions (grouped by category)
3. Load current role permissions (check assigned)
4. User selects/deselects permissions
5. Save changes in transaction
6. Log audit entry
7. Clear permission cache for users with this role

### Permission Check Flow
1. Application component requests permission check
2. AuthorizationService retrieves user's roles from cache/database
3. For each role, retrieve permissions
4. Check if requested permission exists in user's permission set
5. Return true/false
6. Cache result for performance

## Security Features

1. **Least Privilege**: Users granted minimum permissions needed
2. **Role Hierarchy**: Organize permissions logically
3. **Audit Trail**: All role/permission changes logged
4. **Cache Invalidation**: Permission changes reflected immediately
5. **Soft Delete**: Roles deactivated, not deleted (preserves history)
6. **Transaction Safety**: Permission assignments atomic

## Design Patterns Applied (Post-Refactoring)

### Singleton Pattern — SessionContext
`SessionContext` is implemented as a Singleton to guarantee a single instance throughout the application lifecycle.

```csharp
// Access via: SessionContext.Instance.CurrentUser
```

### Composite Pattern — Permission Rules
Allows building flexible RBAC permission checks using tree structures with AND/OR logic.

```
IPermissionRule
    ├── SinglePermissionRule   (leaf: checks one permission code)
    ├── AndPermissionRule      (composite: ALL rules must pass)
    └── OrPermissionRule       (composite: AT LEAST ONE rule must pass)
```

### Decorator Pattern — LoggingAuthorizationDecorator
Wraps `IAuthorizationService` to add detailed debug logging without modifying the core `AuthorizationService` implementation.

```
IAuthorizationService
    ├── AuthorizationService               (concrete implementation)
    └── LoggingAuthorizationDecorator      (adds logging transparently)
```
