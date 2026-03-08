-- ============================================================
-- ADMIN ACCESS RECOVERY SCRIPT
-- ============================================================
-- Use this script when the admin PasswordHash / PasswordSalt
-- have been accidentally deleted or corrupted.
--
-- What this script does:
--   1. If the 'admin' user row still exists, it resets the
--      PasswordHash and PasswordSalt columns back to the
--      well-known placeholder values that the application
--      recognises as "not yet initialised".
--   2. If the 'admin' user row was fully deleted, it recreates
--      it with the same placeholder values and re-assigns the
--      Administrator role.
--   3. Makes sure IsActive = 1 so the user can log in once the
--      password has been set.
--
-- After running this script:
--   • Start the application.
--   • The startup check in Program.cs detects the placeholder
--     and opens AdminPasswordInitForm automatically.
--   • Enter and confirm a new password (min 8 chars, one
--     uppercase letter, one digit) and click "Configurar".
--   • You will be redirected to the login screen and can log in
--     with username "admin" and the new password.
-- ============================================================

PRINT 'Starting admin access recovery...';
GO

-- -------------------------------------------------------
-- CASE 1: admin row exists — reset password fields only
-- -------------------------------------------------------
IF EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
BEGIN
    UPDATE [dbo].[Users]
    SET
        [PasswordHash] = 'HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
        [PasswordSalt] = 'SALT_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
        [IsActive]     = 1
    WHERE [Username] = 'admin';

    PRINT 'Admin user found. PasswordHash and PasswordSalt have been reset to placeholders.';
    PRINT 'IsActive has been set to 1.';
END
-- -------------------------------------------------------
-- CASE 2: admin row was deleted — recreate it
-- -------------------------------------------------------
ELSE
BEGIN
    INSERT INTO [dbo].[Users]
        ([Username], [PasswordHash], [PasswordSalt], [FullName], [Email], [IsActive])
    VALUES
        ('admin',
         'HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
         'SALT_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
         'System Administrator',
         'admin@stockmanager.com',
         1);

    -- SCOPE_IDENTITY() is called in the same batch as the INSERT above, so it is safe.
    DECLARE @NewAdminId   INT = SCOPE_IDENTITY();
    DECLARE @AdminRoleId  INT;
    -- Role name must match the value seeded in 02_SeedData.sql ('Administrator').
    SELECT @AdminRoleId = [RoleId] FROM [dbo].[Roles] WHERE [RoleName] = 'Administrator';

    IF @AdminRoleId IS NOT NULL
    BEGIN
        -- Assign the Administrator role (guard against duplicate)
        IF NOT EXISTS (
            SELECT 1 FROM [dbo].[UserRoles]
            WHERE [UserId] = @NewAdminId AND [RoleId] = @AdminRoleId
        )
        BEGIN
            INSERT INTO [dbo].[UserRoles] ([UserId], [RoleId], [AssignedBy])
            VALUES (@NewAdminId, @AdminRoleId, @NewAdminId);
        END;

        PRINT 'Admin user recreated and Administrator role assigned.';
    END
    ELSE
    BEGIN
        PRINT 'WARNING: Administrator role not found. User created without a role.';
        PRINT 'Run 02_SeedData.sql first to create the required roles, then re-run this script.';
    END;
END
GO

PRINT '';
PRINT '=======================================================';
PRINT 'Recovery complete.';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Start the application.';
PRINT '  2. The password-initialisation form will open automatically.';
PRINT '  3. Enter a new password (min 8 chars, 1 uppercase, 1 digit).';
PRINT '  4. Log in with username "admin" and the new password.';
PRINT '=======================================================';
GO
