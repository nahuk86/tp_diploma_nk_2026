# Login Process - Class Diagrams (Per Use Case)

This document contains UML Class Diagrams organized per use case for all Login-related operations.

---

## UC-01: Authenticate

```mermaid
classDiagram
    class LoginForm {
        -IAuthenticationService _authService
        -ILogService _logService
        -ILocalizationService _localizationService
        -TextBox txtUsername
        -TextBox txtPassword
        -Button btnLogin
        -Button btnCancel
        +LoginForm(authService, logService, localizationService)
        +btnLogin_Click(sender, e) void
        -ApplyLocalization() void
        -SetControlsEnabled(enabled) void
    }

    class IAuthenticationService {
        <<interface>>
        +Authenticate(username, password) User
        +HashPassword(password, out salt) string
        +VerifyPassword(password, hash, salt) bool
        +InitializeAdminPassword(username, newPassword) void
    }

    class AuthenticationService {
        -IUserRepository _userRepository
        -ILogService _logService
        +Authenticate(username, password) User
        +VerifyPassword(password, hash, salt) bool
        -HashPasswordWithSalt(password, salt) byte[]
    }

    class IUserRepository {
        <<interface>>
        +GetByUsername(username) User
        +UpdateLastLogin(userId) void
    }

    class UserRepository {
        +GetByUsername(username) User
        +UpdateLastLogin(userId) void
        -MapUser(reader) User
    }

    class DatabaseHelper {
        <<static>>
        +GetConnection() SqlConnection
    }

    class SessionContext {
        <<singleton>>
        -_instance SessionContext$
        -_currentUser User
        +Instance SessionContext$
        +CurrentUser User
        +CurrentUserId int?
        +CurrentUsername string
        +Clear() void
    }

    class ILogService {
        <<interface>>
        +Debug(message, logger) void
        +Info(message, logger) void
        +Warning(message, logger) void
        +Error(message, exception, logger) void
        +Fatal(message, exception, logger) void
        +Log(level, message, exception, logger) void
    }

    class ILocalizationService {
        <<interface>>
        +GetString(key) string
        +SetLanguage(languageCode) void
    }

    class User {
        +int UserId
        +string Username
        +string PasswordHash
        +string PasswordSalt
        +string FullName
        +string Email
        +bool IsActive
        +DateTime? LastLogin
    }

    LoginForm --> IAuthenticationService : uses
    LoginForm --> ILogService : uses
    LoginForm --> ILocalizationService : uses
    LoginForm --> SessionContext : sets CurrentUser
    AuthenticationService ..|> IAuthenticationService : implements
    AuthenticationService --> IUserRepository : uses
    AuthenticationService --> ILogService : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> DatabaseHelper : uses
    UserRepository --> User : returns
    SessionContext --> User : holds
```

---

## UC-02: InitializeAdminPassword

```mermaid
classDiagram
    class AdminPasswordInitForm {
        -IAuthenticationService _authService
        -ILogService _logService
        -TextBox txtNewPassword
        -TextBox txtConfirmPassword
        -Button btnSave
        +AdminPasswordInitForm(authService, logService)
        +btnSave_Click(sender, e) void
        -ValidatePasswords() bool
    }

    class IAuthenticationService {
        <<interface>>
        +InitializeAdminPassword(username, newPassword) void
        +HashPassword(password, out salt) string
    }

    class AuthenticationService {
        -IUserRepository _userRepository
        -ILogService _logService
        +InitializeAdminPassword(username, newPassword) void
        +HashPassword(password, out salt) string
        -HashPasswordWithSalt(password, salt) byte[]
    }

    class IUserRepository {
        <<interface>>
        +GetByUsername(username) User
        +Update(user) void
    }

    class UserRepository {
        +GetByUsername(username) User
        +Update(user) void
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
        +bool IsActive
    }

    AdminPasswordInitForm --> IAuthenticationService : uses
    AdminPasswordInitForm --> ILogService : uses
    AuthenticationService ..|> IAuthenticationService : implements
    AuthenticationService --> IUserRepository : uses
    UserRepository ..|> IUserRepository : implements
    UserRepository --> DatabaseHelper : uses
    UserRepository --> User : maps
```

---

## Layer Communication Flow

```
┌─────────────┐
│  UI LAYER   │  LoginForm / AdminPasswordInitForm
└─────┬───────┘
      │ uses
      ▼
┌─────────────┐
│  SERVICES   │  IAuthenticationService
│   LAYER     │  AuthenticationService (PBKDF2)
└─────┬───────┘
      │ uses
      ▼
┌─────────────┐
│  DAO LAYER  │  IUserRepository / UserRepository
└─────┬───────┘
      │ returns
      ▼
┌─────────────┐
│   DOMAIN    │  User Entity
└─────────────┘
```

## Key Design Patterns

1. **Dependency Injection**: All dependencies injected through constructors
2. **Repository Pattern**: Data access abstracted through IUserRepository
3. **Service Layer**: Business logic in AuthenticationService
4. **Singleton Pattern**: SessionContext maintains application state
5. **Interface Segregation**: Each service has a dedicated interface
