# Login Process - Sequence Diagram

## UML Sequence Diagram (Mermaid Format)

```mermaid
sequenceDiagram
    participant User as User
    participant UI as LoginForm<br/>(UI Layer)
    participant Loc as ILocalizationService<br/>(Services)
    participant Auth as AuthenticationService<br/>(Services)
    participant UserRepo as UserRepository<br/>(DAO)
    participant DB as DatabaseHelper<br/>(DAO)
    participant Session as SessionContext<br/>(Services)
    participant Log as ILogService<br/>(Services)

    %% Form Load
    User->>UI: Open Login Form
    activate UI
    UI->>Loc: GetString("Common.Login")
    activate Loc
    Loc-->>UI: "Iniciar Sesión"
    deactivate Loc
    UI->>Loc: GetString("Common.Username")
    activate Loc
    Loc-->>UI: "Usuario"
    deactivate Loc
    UI->>Loc: GetString("Common.Password")
    activate Loc
    Loc-->>UI: "Contraseña"
    deactivate Loc
    UI-->>User: Display Login Form
    deactivate UI

    %% Login Attempt
    User->>UI: Enter credentials and click Login
    activate UI
    
    Note over UI: Validate input fields
    alt Username is empty
        UI->>Loc: GetString("Login.UsernameRequired")
        activate Loc
        Loc-->>UI: "Por favor ingrese su usuario."
        deactivate Loc
        UI-->>User: Show validation message
    else Password is empty
        UI->>Loc: GetString("Login.PasswordRequired")
        activate Loc
        Loc-->>UI: "Por favor ingrese su contraseña."
        deactivate Loc
        UI-->>User: Show validation message
    else Valid input
        Note over UI: Disable controls, set wait cursor
        
        UI->>Auth: Authenticate(username, password)
        activate Auth
        
        Note over Auth: Validate credentials not empty
        Auth->>Log: Warning("Authentication attempt...")
        activate Log
        deactivate Log
        
        Auth->>UserRepo: GetByUsername(username)
        activate UserRepo
        UserRepo->>DB: GetConnection()
        activate DB
        DB-->>UserRepo: SqlConnection
        deactivate DB
        
        Note over UserRepo: Execute SQL:<br/>SELECT * FROM Users<br/>WHERE Username = @Username
        UserRepo->>DB: ExecuteReader()
        activate DB
        DB-->>UserRepo: SqlDataReader
        deactivate DB
        
        UserRepo->>UserRepo: MapUser(reader)
        UserRepo-->>Auth: User entity
        deactivate UserRepo
        
        alt User not found
            Auth->>Log: Warning("User not found")
            activate Log
            deactivate Log
            Auth-->>UI: null
            UI->>Loc: GetString("Login.InvalidCredentials")
            activate Loc
            Loc-->>UI: "Usuario o contraseña incorrectos."
            deactivate Loc
            UI-->>User: Show error message
        else User is inactive
            Auth->>Log: Warning("User is inactive")
            activate Log
            deactivate Log
            Auth-->>UI: null
            UI->>Loc: GetString("Login.InvalidCredentials")
            activate Loc
            Loc-->>UI: "Usuario o contraseña incorrectos."
            deactivate Loc
            UI-->>User: Show error message
        else Password not initialized
            Auth->>Log: Warning("Password not initialized")
            activate Log
            deactivate Log
            Auth-->>UI: null
            UI-->>User: Show initialization required message
        else Invalid password
            Auth->>Auth: VerifyPassword(password, hash, salt)
            Note over Auth: PBKDF2 hash verification<br/>10000 iterations
            Auth->>Log: Warning("Invalid password")
            activate Log
            deactivate Log
            Auth-->>UI: null
            UI->>Loc: GetString("Login.InvalidCredentials")
            activate Loc
            Loc-->>UI: "Usuario o contraseña incorrectos."
            deactivate Loc
            UI-->>User: Show error message
        else Valid credentials
            Note over Auth: Password verified successfully
            
            Auth->>UserRepo: UpdateLastLogin(userId)
            activate UserRepo
            UserRepo->>DB: GetConnection()
            activate DB
            DB-->>UserRepo: SqlConnection
            deactivate DB
            Note over UserRepo: Execute SQL:<br/>UPDATE Users<br/>SET LastLogin = @LastLogin<br/>WHERE UserId = @UserId
            UserRepo->>DB: ExecuteNonQuery()
            activate DB
            DB-->>UserRepo: Success
            deactivate DB
            deactivate UserRepo
            
            Auth->>Log: Info("User authenticated successfully")
            activate Log
            deactivate Log
            
            Auth-->>UI: User entity
            deactivate Auth
            
            UI->>Session: Set CurrentUser = user
            activate Session
            Session-->>UI: Session established
            deactivate Session
            
            UI->>Log: Info("User logged in successfully")
            activate Log
            deactivate Log
            
            Note over UI: Set DialogResult = OK
            UI-->>User: Close form, proceed to main form
        end
        
        Note over UI: Re-enable controls, restore cursor
    end
    deactivate UI
```

## Sequence Flow Description

### Phase 1: Form Initialization
1. User opens the Login Form
2. LoginForm requests localized strings from ILocalizationService
3. Form displays with translated labels and buttons

### Phase 2: User Input Validation
4. User enters username and password
5. LoginForm validates input fields
6. If validation fails, display localized error message

### Phase 3: Authentication Process
7. LoginForm calls AuthenticationService.Authenticate()
8. AuthenticationService logs the authentication attempt
9. AuthenticationService calls UserRepository.GetByUsername()
10. UserRepository executes SQL query to retrieve user from database
11. UserRepository maps SQL result to User entity

### Phase 4: Credential Verification
12. AuthenticationService checks:
    - User exists
    - User is active
    - Password is initialized
    - Password matches (PBKDF2 hash verification)

### Phase 5: Success Flow
13. If valid: Update user's last login timestamp
14. Log successful authentication
15. Set SessionContext.CurrentUser
16. Close login form with DialogResult.OK
17. Application proceeds to main form

### Phase 6: Failure Flow
13. If invalid: Log warning with reason
14. Return null to LoginForm
15. Display localized error message
16. Clear password field and refocus

## Security Considerations

1. **Password Hashing**: PBKDF2 with 10,000 iterations
2. **Salt Storage**: Unique salt per user stored separately
3. **Login Attempts**: All attempts are logged (including failures)
4. **No Information Leakage**: Generic error message for all failures
5. **Session Management**: Centralized through SessionContext
6. **Audit Trail**: UpdateLastLogin tracks user activity
