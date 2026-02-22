# User Management Process - Sequence Diagrams (Per Use Case)

This document contains UML Sequence Diagrams organized per use case for all User Management operations.

---

## UC-01: CreateUser

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as UsersForm
    participant BLL as UserService
    participant Auth as AuthenticationService
    participant UserRepo as UserRepository
    participant DB as Database
    participant Session as SessionContext
    participant Log as ILogService

    Admin->>UI: Click "New" then fill form
    activate UI
    Admin->>UI: Click "Save"
    UI->>UI: ValidateForm()
    alt Validation Fails
        UI-->>Admin: Show validation errors
    else Validation Passes
        UI->>BLL: CreateUser(user, password)
        activate BLL
        BLL->>BLL: ValidateUser(user)
        BLL->>BLL: ValidatePassword(password)
        BLL->>UserRepo: GetByUsername(user.Username)
        UserRepo->>DB: SELECT * FROM Users WHERE Username=@Username
        DB-->>UserRepo: null
        UserRepo-->>BLL: null (no duplicate)
        alt Username already exists
            BLL-->>UI: throw InvalidOperationException
            UI-->>Admin: Show error
        else Username unique
            BLL->>UserRepo: GetByEmail(user.Email)
            DB-->>UserRepo: null
            UserRepo-->>BLL: null (no duplicate)
            alt Email already exists
                BLL-->>UI: throw InvalidOperationException
                UI-->>Admin: Show error
            else Email unique
                BLL->>Auth: HashPassword(password, out salt)
                Note over Auth: Generate 32-byte salt + PBKDF2 hash
                Auth-->>BLL: hash, salt
                BLL->>Session: Get CurrentUserId
                Session-->>BLL: currentUserId
                BLL->>UserRepo: Insert(user)
                activate UserRepo
                UserRepo->>DB: BEGIN TRANSACTION
                Note over UserRepo: INSERT INTO Users (Username, PasswordHash, PasswordSalt, FullName, Email, ...) VALUES (...)
                DB-->>UserRepo: userId
                UserRepo->>DB: COMMIT
                UserRepo-->>BLL: userId
                deactivate UserRepo
                BLL->>Log: Info("User created: username")
                BLL-->>UI: userId
                deactivate BLL
                UI->>BLL: GetAllUsers()
                BLL-->>UI: List~User~
                UI-->>Admin: Show success & refresh grid
            end
        end
    end
    deactivate UI
```

---

## UC-02: UpdateUser

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as UsersForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    Admin->>UI: Select user and modify fields
    Admin->>UI: Click "Save"
    activate UI
    UI->>UI: ValidateForm()
    alt Validation Fails
        UI-->>Admin: Show validation errors
    else Validation Passes
        UI->>BLL: UpdateUser(user)
        activate BLL
        BLL->>BLL: ValidateUser(user)
        BLL->>UserRepo: GetByUsername(username)
        UserRepo-->>BLL: null or same user
        BLL->>UserRepo: GetByEmail(email)
        UserRepo-->>BLL: null or same user
        BLL->>UserRepo: Update(user)
        activate UserRepo
        UserRepo->>DB: GetConnection()
        DB-->>UserRepo: SqlConnection
        Note over UserRepo: UPDATE Users SET Username=@U, FullName=@FN, Email=@E, UpdatedAt=@Now WHERE UserId=@Id
        UserRepo-->>BLL: void
        deactivate UserRepo
        BLL-->>UI: void
        deactivate BLL
        UI->>BLL: GetAllUsers()
        BLL-->>UI: List~User~
        UI-->>Admin: Show success & refresh grid
    end
    deactivate UI
```

---

## UC-03: DeleteUser

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as UsersForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    Admin->>UI: Select user and click "Delete"
    activate UI
    UI->>UI: Confirm deletion dialog
    Note over UI: Cannot delete own account
    alt User cancels
        UI-->>Admin: Do nothing
    else User confirms
        UI->>BLL: DeleteUser(userId)
        activate BLL
        BLL->>UserRepo: SoftDelete(id, deletedBy)
        activate UserRepo
        UserRepo->>DB: GetConnection()
        DB-->>UserRepo: SqlConnection
        Note over UserRepo: UPDATE Users SET IsActive=0, UpdatedAt=@Now WHERE UserId=@Id
        UserRepo-->>BLL: void
        deactivate UserRepo
        BLL-->>UI: void
        deactivate BLL
        UI->>BLL: GetAllUsers()
        BLL-->>UI: List~User~
        UI-->>Admin: Show success & refresh grid
    end
    deactivate UI
```

---

## UC-04: GetAllUsers

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as UsersForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    Admin->>UI: Open Users Form
    activate UI
    UI->>BLL: GetAllUsers()
    activate BLL
    BLL->>UserRepo: GetAll()
    activate UserRepo
    UserRepo->>DB: GetConnection()
    DB-->>UserRepo: SqlConnection
    Note over UserRepo: SELECT * FROM Users ORDER BY Username
    UserRepo-->>BLL: List~User~
    deactivate UserRepo
    BLL-->>UI: List~User~
    deactivate BLL
    UI->>UI: Bind to DataGridView
    UI-->>Admin: Display all users
    deactivate UI
```

---

## UC-05: GetActiveUsers

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    UI->>BLL: GetActiveUsers()
    activate BLL
    BLL->>UserRepo: GetAllActive()
    activate UserRepo
    UserRepo->>DB: GetConnection()
    DB-->>UserRepo: SqlConnection
    Note over UserRepo: SELECT * FROM Users WHERE IsActive=1 ORDER BY FullName
    UserRepo-->>BLL: List~User~
    deactivate UserRepo
    BLL-->>UI: List~User~
    deactivate BLL
    UI->>UI: Bind active users to DataGridView
```

---

## UC-06: GetUserById

```mermaid
sequenceDiagram
    participant UI as UsersForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    UI->>BLL: GetUserById(userId)
    activate BLL
    BLL->>UserRepo: GetById(id)
    activate UserRepo
    UserRepo->>DB: GetConnection()
    DB-->>UserRepo: SqlConnection
    Note over UserRepo: SELECT * FROM Users WHERE UserId=@Id
    UserRepo-->>BLL: User
    deactivate UserRepo
    alt Not found
        BLL-->>UI: null
        UI-->>UI: Show not found message
    else Found
        BLL-->>UI: User
        deactivate BLL
        UI->>UI: Populate form fields
    end
```

---

## UC-07: AssignRolesToUser

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as UserRolesForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    Admin->>UI: Select user, choose roles, click "Save"
    activate UI
    UI->>BLL: AssignRolesToUser(userId, selectedRoleIds)
    activate BLL
    BLL->>UserRepo: AssignRoles(userId, roleIds)
    activate UserRepo
    UserRepo->>DB: GetConnection()
    DB-->>UserRepo: SqlConnection
    Note over UserRepo: BEGIN TRANSACTION
    Note over UserRepo: DELETE FROM UserRoles WHERE UserId=@UserId
    loop For each roleId
        Note over UserRepo: INSERT INTO UserRoles (UserId, RoleId, AssignedAt) VALUES (...)
    end
    Note over UserRepo: COMMIT
    UserRepo-->>BLL: void
    deactivate UserRepo
    BLL-->>UI: void
    deactivate BLL
    UI->>UI: LoadUserRoles(userId)
    UI-->>Admin: Show success message
    deactivate UI
```

---

## UC-08: GetUserRoles

```mermaid
sequenceDiagram
    participant UI as UserRolesForm
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    UI->>BLL: GetUserRoles(userId)
    activate BLL
    BLL->>UserRepo: GetUserRoles(userId)
    activate UserRepo
    UserRepo->>DB: GetConnection()
    DB-->>UserRepo: SqlConnection
    Note over UserRepo: SELECT r.* FROM Roles r JOIN UserRoles ur ON r.RoleId=ur.RoleId WHERE ur.UserId=@UserId AND r.IsActive=1
    UserRepo-->>BLL: List~Role~
    deactivate UserRepo
    BLL-->>UI: List~Role~
    deactivate BLL
    UI->>UI: Show assigned roles in list
```

---

## UC-09: ChangePassword

```mermaid
sequenceDiagram
    participant Admin as Administrator
    participant UI as UsersForm
    participant AUTH as AuthenticationService
    participant BLL as UserService
    participant UserRepo as UserRepository
    participant DB as Database

    Admin->>UI: Open change password dialog
    Admin->>UI: Enter current + new + confirm password
    Admin->>UI: Click Save
    activate UI
    UI->>UI: ValidatePasswordInputs()
    alt New passwords don't match
        UI-->>Admin: Show mismatch error
    else Valid inputs
        UI->>AUTH: VerifyPassword(currentPassword, user.Hash, user.Salt)
        Note over AUTH: PBKDF2 verification
        alt Current password invalid
            AUTH-->>UI: false
            UI-->>Admin: Show invalid current password error
        else Current password valid
            AUTH-->>UI: true
            UI->>AUTH: HashPassword(newPassword, out salt)
            Note over AUTH: Generate random salt + PBKDF2 hash
            AUTH-->>UI: newHash, newSalt
            UI->>BLL: UpdateUser(user with new hash/salt)
            activate BLL
            BLL->>UserRepo: Update(user)
            activate UserRepo
            UserRepo->>DB: GetConnection()
            DB-->>UserRepo: SqlConnection
            Note over UserRepo: UPDATE Users SET PasswordHash=@Hash, PasswordSalt=@Salt WHERE UserId=@Id
            UserRepo-->>BLL: void
            deactivate UserRepo
            BLL-->>UI: void
            deactivate BLL
            UI-->>Admin: Show "Password changed successfully"
        end
    end
    deactivate UI
```

---

## Business Rules Summary

| Use Case | Key Validations |
|----------|----------------|
| CreateUser | Username unique, email unique, password strength |
| UpdateUser | Username unique (excluding self), email unique (excluding self) |
| DeleteUser | Cannot delete own account; soft-delete (IsActive=0) |
| ChangePassword | Current password verified before update |
| AssignRolesToUser | Atomic: delete all existing roles, then insert selected ones |

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
