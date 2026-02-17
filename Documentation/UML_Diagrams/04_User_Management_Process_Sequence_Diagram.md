# User Management Process - Sequence Diagram (Create User)

## UML Sequence Diagram (Mermaid Format)

```mermaid
sequenceDiagram
    participant User as Administrator
    participant UI as UsersForm<br/>(UI Layer)
    participant BLL as UserService<br/>(BLL Layer)
    participant Auth as AuthenticationService<br/>(Services)
    participant UserRepo as UserRepository<br/>(DAO)
    participant AuditRepo as AuditLogRepository<br/>(DAO)
    participant DB as Database
    participant Session as SessionContext
    participant Log as ILogService

    %% Load Form
    User->>UI: Open Users Form
    activate UI
    UI->>BLL: GetAllUsers()
    activate BLL
    BLL->>UserRepo: GetAll()
    activate UserRepo
    UserRepo->>DB: SELECT * FROM Users ORDER BY Username
    activate DB
    DB-->>UserRepo: ResultSet
    deactivate DB
    UserRepo->>UserRepo: MapUser(reader) for each row
    UserRepo-->>BLL: List<User>
    deactivate UserRepo
    BLL-->>UI: List<User>
    deactivate BLL
    UI-->>User: Display users in grid
    deactivate UI

    %% Create New User
    User->>UI: Click "New" button
    activate UI
    UI->>UI: ClearForm()
    UI-->>User: Display empty form
    deactivate UI

    User->>UI: Enter user data (username, fullname, email, password)
    activate UI
    
    User->>UI: Click "Save" button
    UI->>UI: ValidateForm()
    
    alt Validation Fails
        UI-->>User: Show validation errors
    else Validation Passes
        UI->>BLL: CreateUser(user, password)
        activate BLL
        
        Note over BLL: Validate business rules
        BLL->>BLL: ValidateUser(user)
        BLL->>BLL: ValidatePassword(password)
        
        %% Check duplicate username
        BLL->>UserRepo: GetByUsername(user.Username)
        activate UserRepo
        UserRepo->>DB: SELECT * FROM Users WHERE Username = @Username
        activate DB
        DB-->>UserRepo: ResultSet (should be empty)
        deactivate DB
        UserRepo-->>BLL: null (no duplicate)
        deactivate UserRepo
        
        alt Username already exists
            BLL->>Log: Warning("Duplicate username attempt")
            activate Log
            deactivate Log
            BLL-->>UI: throw InvalidOperationException
            UI-->>User: Show error message
        else Username is unique
            
            %% Check duplicate email
            BLL->>UserRepo: GetByEmail(user.Email)
            activate UserRepo
            UserRepo->>DB: SELECT * FROM Users WHERE Email = @Email
            activate DB
            DB-->>UserRepo: ResultSet (should be empty)
            deactivate DB
            UserRepo-->>BLL: null (no duplicate)
            deactivate UserRepo
            
            alt Email already exists
                BLL->>Log: Warning("Duplicate email attempt")
                activate Log
                deactivate Log
                BLL-->>UI: throw InvalidOperationException
                UI-->>User: Show error message
            else Email is unique
                
                %% Hash password
                BLL->>Auth: HashPassword(password, out salt)
                activate Auth
                Note over Auth: Generate random salt (32 bytes)<br/>PBKDF2 with 10000 iterations
                Auth-->>BLL: hash, salt
                deactivate Auth
                
                %% Set audit fields
                BLL->>Session: Get CurrentUserId
                activate Session
                Session-->>BLL: currentUserId
                deactivate Session
                
                Note over BLL: Set user.PasswordHash = hash<br/>Set user.PasswordSalt = salt<br/>Set user.CreatedBy = currentUserId<br/>Set user.CreatedAt = DateTime.Now<br/>Set user.IsActive = true
                
                %% Insert user
                BLL->>UserRepo: Insert(user)
                activate UserRepo
                UserRepo->>DB: BEGIN TRANSACTION
                activate DB
                Note over UserRepo: INSERT INTO Users<br/>(Username, PasswordHash, PasswordSalt,<br/>FullName, Email, IsActive,<br/>CreatedAt, CreatedBy)<br/>VALUES (...)
                UserRepo->>DB: ExecuteScalar()
                DB-->>UserRepo: userId (SCOPE_IDENTITY)
                UserRepo->>DB: COMMIT TRANSACTION
                deactivate DB
                UserRepo-->>BLL: userId
                deactivate UserRepo
                
                %% Log audit
                BLL->>Session: Get CurrentUserId
                activate Session
                Session-->>BLL: currentUserId
                deactivate Session
                
                BLL->>AuditRepo: LogChange("Users", userId, Insert, null, null, "Created user {username}", currentUserId)
                activate AuditRepo
                Note over AuditRepo: INSERT INTO AuditLog<br/>(TableName, RecordId, Action,<br/>Description, ChangeDate, ChangedBy)<br/>VALUES (...)
                AuditRepo->>DB: ExecuteNonQuery()
                activate DB
                DB-->>AuditRepo: Success
                deactivate DB
                deactivate AuditRepo
                
                %% Log info
                BLL->>Log: Info("User created: {username} by {currentUser}")
                activate Log
                deactivate Log
                
                BLL-->>UI: userId
                deactivate BLL
                
                %% Refresh grid
                UI->>BLL: GetAllUsers()
                activate BLL
                BLL->>UserRepo: GetAll()
                activate UserRepo
                UserRepo->>DB: SELECT * FROM Users ORDER BY Username
                activate DB
                DB-->>UserRepo: ResultSet
                deactivate DB
                UserRepo-->>BLL: List<User>
                deactivate UserRepo
                BLL-->>UI: List<User>
                deactivate BLL
                
                UI->>UI: ClearForm()
                UI-->>User: Show success message & refresh grid
            end
        end
    end
    deactivate UI
```

## Sequence Flow Description

### Phase 1: Form Load & Display Users
1. Administrator opens Users Form
2. UsersForm calls UserService.GetAllUsers()
3. UserService retrieves all users from UserRepository
4. UserRepository executes SQL query and maps results
5. Users are displayed in data grid

### Phase 2: Prepare New User
6. User clicks "New" button
7. Form clears all input fields
8. Ready to enter new user data

### Phase 3: Input & Validation
9. Administrator enters user information
10. Clicks "Save" button
11. UsersForm validates form inputs (client-side)

### Phase 4: Business Validation
12. UserService validates business rules:
    - ValidateUser: checks required fields, format
    - ValidatePassword: checks strength requirements
13. Check username uniqueness via UserRepository
14. Check email uniqueness via UserRepository

### Phase 5: Password Hashing
15. UserService calls AuthenticationService.HashPassword()
16. Generate cryptographically secure random salt (32 bytes)
17. Hash password using PBKDF2 with 10,000 iterations
18. Returns hash and salt to UserService

### Phase 6: Set Audit Fields
19. UserService retrieves current user ID from SessionContext
20. Sets audit fields:
    - PasswordHash, PasswordSalt
    - CreatedBy, CreatedAt
    - IsActive = true

### Phase 7: Database Insert
21. UserService calls UserRepository.Insert()
22. UserRepository begins database transaction
23. Executes INSERT statement with parameterized query
24. Retrieves new userId via SCOPE_IDENTITY()
25. Commits transaction
26. Returns userId

### Phase 8: Audit Logging
27. UserService logs change to AuditLogRepository
28. Creates audit record with action "Insert"
29. Stores who created the user and when

### Phase 9: Completion
30. UserService logs info message
31. Returns userId to UsersForm
32. UsersForm refreshes the data grid
33. Displays success message to administrator
34. Clears form for next operation

## Validation Rules

### Client-Side (UI)
- Username: Required, non-empty
- Full Name: Required, non-empty
- Email: Valid format (if provided)
- Password: Required, minimum length

### Business Logic (BLL)
- Username: Unique across system
- Email: Unique across system (if provided)
- Password: Minimum 8 characters, complexity requirements
- All fields: SQL injection prevention via parameterized queries

## Security Features

1. **Password Security**: PBKDF2 hashing with 32-byte salt and 10,000 iterations
2. **Parameterized Queries**: All database operations use parameters
3. **Transaction Support**: Database operations wrapped in transactions
4. **Audit Trail**: All changes logged with user context
5. **Session Tracking**: Current user tracked via SessionContext
6. **Soft Delete**: Users deactivated rather than deleted (preserves history)
