# Login Process - Class Diagram

## UML Class Diagram (Mermaid Format)

```mermaid
classDiagram
    %% UI Layer
    class LoginForm {
        -IAuthenticationService _authService
        -ILogService _logService
        -ILocalizationService _localizationService
        -TextBox txtUsername
        -TextBox txtPassword
        -Button btnLogin
        -Button btnCancel
        -Label lblUsername
        -Label lblPassword
        +LoginForm(authService, logService, localizationService)
        +btnLogin_Click(sender, e) void
        -ApplyLocalization() void
        -SetControlsEnabled(enabled) void
    }

    %% Services Layer
    class AuthenticationService {
        -IUserRepository _userRepository
        -ILogService _logService
        +AuthenticationService(userRepository, logService)
        +Authenticate(username, password) User
        +HashPassword(password, out salt) string
        +VerifyPassword(password, hash, salt) bool
        +InitializeAdminPassword(username, newPassword) void
        -HashPasswordWithSalt(password, salt) byte[]
    }

    class IAuthenticationService {
        <<interface>>
        +Authenticate(username, password) User
        +HashPassword(password, out salt) string
        +VerifyPassword(password, hash, salt) bool
        +InitializeAdminPassword(username, newPassword) void
    }

    class SessionContext {
        <<static>>
        +CurrentUser User
        +CurrentUserId int
        +CurrentUsername string
    }

    class ILogService {
        <<interface>>
        +Info(message) void
        +Warning(message) void
        +Error(message, exception) void
    }

    class ILocalizationService {
        <<interface>>
        +GetString(key) string
        +SetLanguage(languageCode) void
    }

    %% DAO Layer
    class UserRepository {
        +GetById(id) User
        +GetByUsername(username) User
        +GetByEmail(email) User
        +GetAll() List~User~
        +GetAllActive() List~User~
        +Insert(entity) int
        +Update(entity) void
        +Delete(id) void
        +SoftDelete(id, deletedBy) void
        +UpdateLastLogin(userId) void
        +GetUserRoles(userId) List~Role~
        +AssignRole(userId, roleId, assignedBy) void
        +RemoveRole(userId, roleId) void
        +AssignRoles(userId, roleIds) void
        -MapUser(reader) User
        -MapRole(reader) Role
    }

    class IUserRepository {
        <<interface>>
        +GetById(id) User
        +GetByUsername(username) User
        +GetByEmail(email) User
        +GetAll() List~User~
        +Insert(entity) int
        +Update(entity) void
        +Delete(id) void
        +UpdateLastLogin(userId) void
        +GetUserRoles(userId) List~Role~
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
        +CreateParameter(name, value) SqlParameter
    }

    %% Domain Layer
    class User {
        +int UserId
        +string Username
        +string PasswordHash
        +string PasswordSalt
        +string FullName
        +string Email
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
        +DateTime? LastLogin
    }

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

    %% Relationships
    LoginForm --> IAuthenticationService : uses
    LoginForm --> ILogService : uses
    LoginForm --> ILocalizationService : uses
    LoginForm --> SessionContext : sets CurrentUser
    
    AuthenticationService ..|> IAuthenticationService : implements
    AuthenticationService --> IUserRepository : uses
    AuthenticationService --> ILogService : uses
    AuthenticationService --> User : returns
    
    UserRepository ..|> IUserRepository : implements
    UserRepository --> DatabaseHelper : uses
    UserRepository --> User : returns
    UserRepository --> Role : returns
    
    SessionContext --> User : holds
```

## Layer Communication Flow

```
┌─────────────┐
│  UI LAYER   │  LoginForm
└─────┬───────┘
      │ uses
      ▼
┌─────────────┐
│  SERVICES   │  IAuthenticationService
│   LAYER     │  ILogService
└─────┬───────┘  ILocalizationService
      │ calls
      ▼
┌─────────────┐
│ SERVICES    │  AuthenticationService
│    IMPL     │  
└─────┬───────┘
      │ uses
      ▼
┌─────────────┐
│  DAO LAYER  │  IUserRepository
└─────┬───────┘
      │ implements
      ▼
┌─────────────┐
│     DAO     │  UserRepository
│    IMPL     │  DatabaseHelper
└─────┬───────┘
      │ returns
      ▼
┌─────────────┐
│   DOMAIN    │  User Entity
│   LAYER     │  
└─────────────┘
```

## Key Design Patterns

1. **Dependency Injection**: All dependencies are injected through constructors
2. **Repository Pattern**: Data access is abstracted through IUserRepository
3. **Service Layer**: Business logic is encapsulated in AuthenticationService
4. **Singleton Pattern**: SessionContext maintains application state
5. **Interface Segregation**: Each service has a dedicated interface
