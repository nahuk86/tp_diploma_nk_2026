# Login Process - Sequence Diagrams (Per Use Case)

This document contains UML Sequence Diagrams organized per use case for all Login-related operations.

---

## UC-01: Authenticate

```mermaid
sequenceDiagram
    participant User as User
    participant UI as LoginForm
    participant Loc as ILocalizationService
    participant Auth as AuthenticationService
    participant UserRepo as UserRepository
    participant DB as DatabaseHelper
    participant Session as SessionContext
    participant Log as ILogService

    User->>UI: Open Login Form
    activate UI
    UI->>Loc: GetString("Common.Login")
    Loc-->>UI: "Iniciar Sesión"
    UI->>Loc: GetString("Common.Username")
    Loc-->>UI: "Usuario"
    UI-->>User: Display Login Form
    deactivate UI

    User->>UI: Enter credentials and click Login
    activate UI
    Note over UI: Validate input fields
    alt Username or Password empty
        UI->>Loc: GetString("Login.FieldRequired")
        Loc-->>UI: validation message
        UI-->>User: Show validation message
    else Valid input
        UI->>Auth: Authenticate(username, password)
        activate Auth
        Auth->>UserRepo: GetByUsername(username)
        activate UserRepo
        UserRepo->>DB: GetConnection()
        DB-->>UserRepo: SqlConnection
        Note over UserRepo: SELECT * FROM Users WHERE Username=@Username
        UserRepo-->>Auth: User entity (or null)
        deactivate UserRepo

        alt User not found or inactive
            Auth->>Log: Warning("User not found or inactive")
            Auth-->>UI: null
            UI->>Loc: GetString("Login.InvalidCredentials")
            Loc-->>UI: "Usuario o contraseña incorrectos."
            UI-->>User: Show error message
        else Password not initialized
            Auth-->>UI: null
            UI-->>User: Show initialization required message
        else Invalid password
            Auth->>Auth: VerifyPassword(password, hash, salt)
            Note over Auth: PBKDF2 hash verification (10000 iterations)
            Auth->>Log: Warning("Invalid password for user")
            Auth-->>UI: null
            UI-->>User: Show error message
        else Valid credentials
            Auth->>Auth: VerifyPassword(password, hash, salt)
            Auth->>UserRepo: UpdateLastLogin(userId)
            activate UserRepo
            UserRepo->>DB: GetConnection()
            DB-->>UserRepo: SqlConnection
            Note over UserRepo: UPDATE Users SET LastLogin=@Now WHERE UserId=@Id
            UserRepo-->>Auth: void
            deactivate UserRepo
            Auth->>Log: Info("User authenticated successfully")
            Auth-->>UI: User entity
            deactivate Auth
            UI->>Session: Instance.CurrentUser = user
            Session-->>UI: Session established
            Note over UI: Set DialogResult = OK
            UI-->>User: Close form, proceed to main form
        end
    end
    deactivate UI
```

---

## UC-02: InitializeAdminPassword

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as AdminPasswordInitForm
    participant Auth as AuthenticationService
    participant UserRepo as UserRepository
    participant DB as DatabaseHelper
    participant Log as ILogService

    Note over Admin,UI: First-time setup — admin password not yet initialized
    Admin->>UI: Open Admin Password Init Form
    activate UI
    UI-->>Admin: Display password form

    Admin->>UI: Enter new password and confirm password
    Admin->>UI: Click Save

    UI->>UI: ValidatePasswords()
    alt Passwords do not match
        UI-->>Admin: Show "Passwords do not match" error
    else Password too short
        UI-->>Admin: Show "Password too short" error
    else Valid password
        UI->>Auth: InitializeAdminPassword("admin", newPassword)
        activate Auth
        Auth->>Auth: HashPassword(newPassword, out salt)
        Note over Auth: Generate random 32-byte salt + PBKDF2 hash
        Auth->>UserRepo: GetByUsername("admin")
        activate UserRepo
        UserRepo->>DB: GetConnection()
        DB-->>UserRepo: SqlConnection
        UserRepo-->>Auth: admin User entity
        deactivate UserRepo
        Auth->>Auth: Set user.PasswordHash = hash
        Auth->>Auth: Set user.PasswordSalt = salt
        Auth->>UserRepo: Update(user)
        activate UserRepo
        UserRepo->>DB: GetConnection()
        DB-->>UserRepo: SqlConnection
        Note over UserRepo: UPDATE Users SET PasswordHash=@Hash, PasswordSalt=@Salt WHERE UserId=@Id
        UserRepo-->>Auth: void
        deactivate UserRepo
        Auth->>Log: Info("Admin password initialized successfully")
        Auth-->>UI: void
        deactivate Auth
        UI-->>Admin: Show "Password initialized. Please log in."
        UI->>UI: Close form → redirect to LoginForm
    end
    deactivate UI
```

---

## Sequence Flow Summary

### UC-01: Authenticate
1. User opens the Login Form; localized labels are applied
2. User enters credentials and clicks Login
3. Form validates input fields (non-empty check)
4. `AuthenticationService.Authenticate()` is called
5. `UserRepository.GetByUsername()` fetches the user record
6. Service verifies: user exists, is active, password is initialized, hash matches
7. On success: `UpdateLastLogin()` is called, `SessionContext` is set, form closes
8. On failure: localized error message is shown; no information leak about specific cause

### UC-02: InitializeAdminPassword
1. Administrator opens the password initialization form (first-time setup)
2. Enters and confirms new password
3. Form validates matching and minimum length
4. `AuthenticationService.InitializeAdminPassword()` generates a PBKDF2 hash + salt
5. `UserRepository.Update()` persists the new credentials
6. Administrator is redirected to the Login Form

## Security Features

1. **Password Hashing**: PBKDF2 with 10,000 iterations and 32-byte random salt
2. **Generic Error Messages**: All authentication failures return the same message
3. **Audit Logging**: All authentication attempts are logged
4. **Session Management**: Centralized through `SessionContext`
5. **Last Login Tracking**: `UpdateLastLogin` records successful logins
