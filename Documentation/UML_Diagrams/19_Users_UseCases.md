# Users - Use Case Diagrams

This document contains UML Class Diagrams and Sequence Diagrams for all User-related use cases.

---

## UC-01: AssignRolesToUser

### Class Diagram

```mermaid
classDiagram
    class UserRolesForm {
        -IUserService _userService
        -IRoleService _roleService
        -ILogService _logService
        +btnAssignRoles_Click(sender, e) void
        -LoadUserRoles(userId) void
        -LoadAllRoles() void
    }

    class IUserService {
        <<interface>>
        +AssignRoles(userId, roleIds) void
        +GetUserRoles(userId) List~Role~
    }

    class UserService {
        -IUserRepository _userRepository
        -ILogService _logService
        +AssignRoles(userId, roleIds) void
        +GetUserRoles(userId) List~Role~
    }

    class IUserRepository {
        <<interface>>
        +AssignRoles(userId, roleIds) void
        +GetUserRoles(userId) List~Role~
        +RemoveRole(userId, roleId) void
    }

    class UserRepository {
        +AssignRoles(userId, roleIds) void
        +GetUserRoles(userId) List~Role~
        +RemoveRole(userId, roleId) void
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class User {
        +int UserId
        +string Username
        +string FullName
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
    }

    UserRolesForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> DatabaseHelper : uses
    UserRepository --> Role : returns
    User "1" --> "many" Role : hasRoles
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UserRolesForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>UI: User selects roles to assign
    UI->>SVC: AssignRoles(userId, selectedRoleIds)
    activate SVC
    SVC->>REPO: AssignRoles(userId, roleIds)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: BEGIN TRANSACTION
    Note over REPO: DELETE FROM UserRoles WHERE UserId=@UserId
    loop For each roleId
        Note over REPO: INSERT INTO UserRoles (UserId, RoleId, AssignedAt) VALUES (...)
    end
    Note over REPO: COMMIT
    REPO-->>SVC: void
    deactivate REPO
    SVC-->>UI: void
    deactivate SVC
    UI->>UI: LoadUserRoles(userId)
    UI-->>UI: Show success message
```

---

## UC-02: ChangePassword

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        -IAuthenticationService _authService
        +btnChangePassword_Click(sender, e) void
        -ValidatePasswordInputs() bool
    }

    class IUserService {
        <<interface>>
        +UpdateUser(user) void
        +GetUserById(id) User
    }

    class IAuthenticationService {
        <<interface>>
        +HashPassword(password, out salt) string
        +VerifyPassword(password, hash, salt) bool
        +InitializeAdminPassword(username, newPassword) void
    }

    class AuthenticationService {
        -IUserRepository _userRepository
        +HashPassword(password, out salt) string
        +VerifyPassword(password, hash, salt) bool
        +InitializeAdminPassword(username, newPassword) void
        -HashPasswordWithSalt(password, salt) byte[]
    }

    class IUserRepository {
        <<interface>>
        +Update(user) void
        +GetById(id) User
    }

    class UserRepository {
        +Update(user) void
        +GetById(id) User
    }

    class User {
        +int UserId
        +string Username
        +string PasswordHash
        +string PasswordSalt
        +string FullName
        +string Email
    }

    UsersForm --> IUserService : uses
    UsersForm --> IAuthenticationService : uses
    AuthenticationService ..|> IAuthenticationService : implements
    AuthenticationService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> User : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant AUTH as AuthenticationService
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidatePasswordInputs()
    alt New passwords don't match
        UI-->>UI: Show mismatch error
    else Passwords match
        UI->>AUTH: VerifyPassword(currentPassword, user.Hash, user.Salt)
        activate AUTH
        Note over AUTH: PBKDF2 verification
        AUTH-->>UI: true/false
        deactivate AUTH
        alt Current password invalid
            UI-->>UI: Show invalid current password error
        else Current password valid
            UI->>AUTH: HashPassword(newPassword, out salt)
            activate AUTH
            Note over AUTH: Generate random salt + PBKDF2 hash
            AUTH-->>UI: newHash, newSalt
            deactivate AUTH
            UI->>SVC: UpdateUser(user with new hash/salt)
            activate SVC
            SVC->>REPO: Update(user)
            activate REPO
            REPO->>DB: GetConnection()
            DB-->>REPO: SqlConnection
            Note over REPO: UPDATE Users SET PasswordHash=@Hash, PasswordSalt=@Salt WHERE UserId=@Id
            REPO-->>SVC: void
            deactivate REPO
            SVC-->>UI: void
            deactivate SVC
            UI-->>UI: Show success message
        end
    end
```

---

## UC-03: CreateUser

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        -IAuthenticationService _authService
        -ILogService _logService
        +btnSave_Click(sender, e) void
        -ValidateInputs() bool
        -LoadUsers() void
    }

    class IUserService {
        <<interface>>
        +CreateUser(user) int
    }

    class UserService {
        -IUserRepository _userRepository
        -IAuthenticationService _authService
        -ILogService _logService
        +CreateUser(user) int
        -ValidateUser(user) void
    }

    class IAuthenticationService {
        <<interface>>
        +HashPassword(password, out salt) string
    }

    class IUserRepository {
        <<interface>>
        +Insert(user) int
        +GetByUsername(username) User
        +GetByEmail(email) User
    }

    class UserRepository {
        +Insert(user) int
        +GetByUsername(username) User
        +GetByEmail(email) User
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class User {
        +int UserId
        +string Username
        +string PasswordHash
        +string PasswordSalt
        +string FullName
        +string Email
        +bool IsActive
        +DateTime CreatedAt
        +int CreatedBy
    }

    UsersForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserService --> IAuthenticationService : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> DatabaseHelper : uses
    UserRepository --> User : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant SVC as UserService
    participant AUTH as AuthenticationService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: CreateUser(user)
        activate SVC
        SVC->>SVC: ValidateUser(user)
        SVC->>REPO: GetByUsername(username)
        REPO-->>SVC: null (username available)
        SVC->>REPO: GetByEmail(email)
        REPO-->>SVC: null (email available)
        SVC->>AUTH: HashPassword(password, out salt)
        activate AUTH
        Note over AUTH: Generate salt + PBKDF2 hash
        AUTH-->>SVC: hash, salt
        deactivate AUTH
        SVC->>REPO: Insert(user)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: INSERT INTO Users (Username, PasswordHash, PasswordSalt, FullName, Email, ...) VALUES (...)
        REPO-->>SVC: newUserId
        deactivate REPO
        SVC-->>UI: newUserId
        deactivate SVC
        UI->>UI: LoadUsers()
        UI-->>UI: Show success message
    end
```

---

## UC-04: DeleteUser

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        +btnDelete_Click(sender, e) void
        -LoadUsers() void
    }

    class IUserService {
        <<interface>>
        +DeleteUser(id, deletedBy) void
    }

    class UserService {
        -IUserRepository _userRepository
        +DeleteUser(id, deletedBy) void
    }

    class IUserRepository {
        <<interface>>
        +SoftDelete(id, deletedBy) void
        +GetById(id) User
    }

    class UserRepository {
        +SoftDelete(id, deletedBy) void
    }

    class User {
        +int UserId
        +string Username
        +string FullName
        +bool IsActive
    }

    UsersForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> User : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>UI: Confirm deletion dialog
    Note over UI: Cannot delete own account
    alt User cancels
        UI-->>UI: Do nothing
    else User confirms
        UI->>SVC: DeleteUser(userId, currentUserId)
        activate SVC
        SVC->>REPO: SoftDelete(id, deletedBy)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Users SET IsActive=0, UpdatedAt=@Now WHERE UserId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadUsers()
        UI-->>UI: Show success message
    end
```

---

## UC-05: GetActiveUsers

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        +LoadActiveUsers() void
    }

    class IUserService {
        <<interface>>
        +GetActiveUsers() List~User~
    }

    class UserService {
        -IUserRepository _userRepository
        +GetActiveUsers() List~User~
    }

    class IUserRepository {
        <<interface>>
        +GetAllActive() List~User~
    }

    class UserRepository {
        +GetAllActive() List~User~
    }

    class User {
        +int UserId
        +string Username
        +string FullName
        +string Email
        +bool IsActive
        +DateTime LastLogin
    }

    UsersForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> User : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetActiveUsers()
    activate SVC
    SVC->>REPO: GetAllActive()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Users WHERE IsActive=1 ORDER BY FullName
    REPO-->>SVC: List~User~
    deactivate REPO
    SVC-->>UI: List~User~
    deactivate SVC
    UI->>UI: Bind to DataGridView
```

---

## UC-06: GetAllUsers

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        +LoadAllUsers() void
    }

    class IUserService {
        <<interface>>
        +GetAllUsers() List~User~
    }

    class UserService {
        -IUserRepository _userRepository
        +GetAllUsers() List~User~
    }

    class IUserRepository {
        <<interface>>
        +GetAll() List~User~
    }

    class UserRepository {
        +GetAll() List~User~
    }

    class User {
        +int UserId
        +string Username
        +string FullName
        +string Email
        +bool IsActive
    }

    UsersForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> User : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetAllUsers()
    activate SVC
    SVC->>REPO: GetAll()
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Users ORDER BY FullName
    REPO-->>SVC: List~User~
    deactivate REPO
    SVC-->>UI: List~User~
    deactivate SVC
    UI->>UI: Bind to DataGridView (all including inactive)
```

---

## UC-07: GetUserById

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        +LoadUserDetails(id) void
    }

    class IUserService {
        <<interface>>
        +GetUserById(id) User
    }

    class UserService {
        -IUserRepository _userRepository
        +GetUserById(id) User
    }

    class IUserRepository {
        <<interface>>
        +GetById(id) User
    }

    class UserRepository {
        +GetById(id) User
    }

    class User {
        +int UserId
        +string Username
        +string FullName
        +string Email
        +bool IsActive
        +DateTime CreatedAt
        +DateTime LastLogin
    }

    UsersForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> User : returns
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetUserById(userId)
    activate SVC
    SVC->>REPO: GetById(id)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT * FROM Users WHERE UserId=@Id
    REPO-->>SVC: User
    deactivate REPO
    alt Not found
        SVC-->>UI: null
        UI-->>UI: Show not found message
    else Found
        SVC-->>UI: User
        deactivate SVC
        UI->>UI: Populate form fields
    end
```

---

## UC-08: GetUserRoles

### Class Diagram

```mermaid
classDiagram
    class UserRolesForm {
        -IUserService _userService
        +LoadUserRoles(userId) void
    }

    class IUserService {
        <<interface>>
        +GetUserRoles(userId) List~Role~
    }

    class UserService {
        -IUserRepository _userRepository
        +GetUserRoles(userId) List~Role~
    }

    class IUserRepository {
        <<interface>>
        +GetUserRoles(userId) List~Role~
    }

    class UserRepository {
        +GetUserRoles(userId) List~Role~
    }

    class User {
        +int UserId
        +string Username
        +string FullName
    }

    class Role {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
    }

    UserRolesForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> Role : returns
    User "1" --> "many" Role : hasRoles
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UserRolesForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>SVC: GetUserRoles(userId)
    activate SVC
    SVC->>REPO: GetUserRoles(userId)
    activate REPO
    REPO->>DB: GetConnection()
    DB-->>REPO: SqlConnection
    Note over REPO: SELECT r.* FROM Roles r JOIN UserRoles ur ON r.RoleId=ur.RoleId WHERE ur.UserId=@UserId AND r.IsActive=1
    REPO-->>SVC: List~Role~
    deactivate REPO
    SVC-->>UI: List~Role~
    deactivate SVC
    UI->>UI: Show assigned roles in list
```

---

## UC-09: UpdateUser

### Class Diagram

```mermaid
classDiagram
    class UsersForm {
        -IUserService _userService
        +btnUpdate_Click(sender, e) void
        -ValidateInputs() bool
        -LoadUsers() void
    }

    class IUserService {
        <<interface>>
        +UpdateUser(user) void
    }

    class UserService {
        -IUserRepository _userRepository
        -ILogService _logService
        +UpdateUser(user) void
        -ValidateUser(user) void
    }

    class IUserRepository {
        <<interface>>
        +Update(user) void
        +GetByUsername(username) User
        +GetByEmail(email) User
    }

    class UserRepository {
        +Update(user) void
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class User {
        +int UserId
        +string Username
        +string FullName
        +string Email
        +bool IsActive
        +DateTime UpdatedAt
        +int UpdatedBy
    }

    UsersForm --> IUserService : uses
    UserService ..|> IUserService : implements
    UserService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> DatabaseHelper : uses
    UserRepository --> User : maps
```

### Sequence Diagram

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant SVC as UserService
    participant REPO as UserRepository
    participant DB as DatabaseHelper

    UI->>UI: ValidateInputs()
    alt Validation fails
        UI-->>UI: Show validation error
    else Validation passes
        UI->>SVC: UpdateUser(user)
        activate SVC
        SVC->>SVC: ValidateUser(user)
        SVC->>REPO: GetByUsername(username)
        REPO-->>SVC: null or same user (username available)
        SVC->>REPO: GetByEmail(email)
        REPO-->>SVC: null or same user (email available)
        SVC->>REPO: Update(user)
        activate REPO
        REPO->>DB: GetConnection()
        DB-->>REPO: SqlConnection
        Note over REPO: UPDATE Users SET Username=@User, FullName=@Name, Email=@Email, UpdatedAt=@Now WHERE UserId=@Id
        REPO-->>SVC: void
        deactivate REPO
        SVC-->>UI: void
        deactivate SVC
        UI->>UI: LoadUsers()
        UI-->>UI: Show success message
    end
```

---
