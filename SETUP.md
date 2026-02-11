# Guía de Instalación y Configuración - Stock Manager

## Requisitos del Sistema

### Software Necesario

1. **.NET Framework 4.8 Runtime**
   - Descargar de: https://dotnet.microsoft.com/download/dotnet-framework/net48
   - Verificar instalación: Abrir PowerShell y ejecutar:
     ```powershell
     (Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full").Release -ge 528040
     ```
   - Debe retornar `True`

2. **SQL Server LocalDB o SQL Server Express**
   - **LocalDB** (recomendado para desarrollo):
     - Incluido con Visual Studio 2017+
     - O descargar: https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb
   
   - **SQL Server Express** (alternativa):
     - Descargar: https://www.microsoft.com/sql-server/sql-server-downloads

3. **Visual Studio 2017 o superior** (para desarrollo)
   - Community Edition es suficiente
   - Incluir: ".NET desktop development" workload

### Opcional

- **SQL Server Management Studio (SSMS)**: Para gestión de base de datos
  - Descargar: https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms

## Instalación Paso a Paso

### 1. Verificar SQL Server LocalDB

Abrir PowerShell o CMD y ejecutar:
```powershell
sqllocaldb info
```

Debe mostrar algo como:
```
MSSQLLocalDB
ProjectModels
v11.0
```

Si LocalDB no está instalado, descargarlo e instalarlo.

### 2. Crear la Instancia (si no existe)

```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start MSSQLLocalDB
```

### 3. Crear la Base de Datos

#### Opción A: Usando SSMS

1. Abrir SQL Server Management Studio
2. Conectarse a: `(localdb)\MSSQLLocalDB`
   - Server type: Database Engine
   - Authentication: Windows Authentication
3. Abrir archivo `Database/01_CreateSchema.sql`
4. Ejecutar (F5)
5. Abrir archivo `Database/02_SeedData.sql`
6. Ejecutar (F5)

#### Opción B: Usando SQLCMD (línea de comandos)

```powershell
# Navegar al directorio del proyecto
cd C:\path\to\tp_diploma_nk_2026

# Ejecutar el script de creación del esquema
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "Database\01_CreateSchema.sql"

# Ejecutar el script de datos semilla
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "Database\02_SeedData.sql"
```

#### Opción C: Desde Visual Studio

1. Abrir Visual Studio
2. View > SQL Server Object Explorer
3. Conectar a (localdb)\MSSQLLocalDB
4. Click derecho en Databases > Add New Database > StockManagerDB
5. Abrir `Database/01_CreateSchema.sql` y ejecutar
6. Abrir `Database/02_SeedData.sql` y ejecutar

### 4. Verificar la Instalación

Ejecutar esta consulta para verificar que las tablas se crearon:

```sql
USE StockManagerDB;
GO

SELECT 
    t.name AS TableName,
    COUNT(c.column_id) AS ColumnCount
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
GROUP BY t.name
ORDER BY t.name;
```

Debe mostrar 13 tablas:
- AuditLog
- AppLog
- Permissions
- Products
- RolePermissions
- Roles
- Stock
- StockMovementLines
- StockMovements
- Translations
- UserRoles
- Users
- Warehouses

### 5. Verificar Datos Semilla

```sql
-- Verificar usuario admin
SELECT Username, FullName, Email, IsActive FROM Users WHERE Username = 'admin';

-- Verificar roles
SELECT RoleId, RoleName, Description FROM Roles ORDER BY RoleName;

-- Verificar permisos
SELECT COUNT(*) AS TotalPermissions FROM Permissions WHERE IsActive = 1;

-- Verificar productos de ejemplo
SELECT SKU, Name, Category FROM Products ORDER BY Name;

-- Verificar almacenes
SELECT Code, Name, Address FROM Warehouses ORDER BY Code;
```

### 6. Configurar Connection String

Editar el archivo `UI/App.config`:

```xml
<connectionStrings>
    <add name="StockManagerDB" 
         connectionString="Server=(localdb)\MSSQLLocalDB;Database=StockManagerDB;Integrated Security=true;TrustServerCertificate=true;" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Alternativas de Connection String**:

- **LocalDB (default)**:
  ```
  Server=(localdb)\MSSQLLocalDB;Database=StockManagerDB;Integrated Security=true;TrustServerCertificate=true;
  ```

- **SQL Server Express con instancia por defecto**:
  ```
  Server=localhost\SQLEXPRESS;Database=StockManagerDB;Integrated Security=true;TrustServerCertificate=true;
  ```

- **SQL Server Express con SQL Authentication**:
  ```
  Server=localhost\SQLEXPRESS;Database=StockManagerDB;User Id=sa;Password=YourPassword;TrustServerCertificate=true;
  ```

- **SQL Server completo**:
  ```
  Server=localhost;Database=StockManagerDB;Integrated Security=true;TrustServerCertificate=true;
  ```

### 7. Compilar la Solución

1. Abrir `tp_diploma_nk_2026.sln` en Visual Studio
2. Menú: Build > Rebuild Solution
3. Verificar que compile sin errores

Debe mostrar:
```
========== Rebuild All: 5 succeeded, 0 failed, 0 skipped ==========
```

### 8. Ejecutar la Aplicación

1. En Visual Studio: Set `UI` as StartUp Project
   - Click derecho en proyecto UI > Set as StartUp Project
2. Presionar F5 o hacer click en "Start"

**Primera Ejecución**:
- El sistema detectará que la contraseña del admin no está inicializada
- Se mostrará un mensaje o formulario para configurarla
- Usar una contraseña fuerte (ej: Admin123!)

## Solución de Problemas

### Error: "Cannot open database 'StockManagerDB'"

**Causa**: Base de datos no creada.

**Solución**: Ejecutar scripts SQL (paso 3).

### Error: "Login failed for user"

**Causa**: Connection string incorrecta o permisos.

**Solución**:
1. Verificar que el usuario de Windows tenga permisos en SQL Server
2. Usar Integrated Security si es posible
3. Verificar el nombre de la instancia SQL Server

### Error: "A network-related or instance-specific error"

**Causa**: SQL Server no está ejecutándose o nombre de servidor incorrecto.

**Solución**:
```powershell
# Verificar instancias
sqllocaldb info

# Iniciar instancia
sqllocaldb start MSSQLLocalDB
```

### Error: "Could not load file or assembly"

**Causa**: Referencias faltantes o .NET Framework no instalado.

**Solución**:
1. Verificar .NET Framework 4.8 instalado
2. En Visual Studio: Project > Restore NuGet Packages
3. Rebuild Solution

### Los logs no se generan

**Causa**: Permisos de escritura en carpeta Logs.

**Solución**:
1. Verificar que existe carpeta "Logs" en el directorio de ejecución
2. Dar permisos de escritura al usuario actual
3. O cambiar ruta en App.config:
   ```xml
   <add key="LogDirectory" value="C:\Temp\StockManagerLogs" />
   ```

## Inicialización de Contraseña Admin

La primera vez que ejecute la aplicación:

1. El sistema detectará automáticamente que `admin` tiene password placeholder
2. Se mostrará el formulario "Configuración Inicial" para configurar la contraseña
3. La contraseña debe cumplir los siguientes requisitos:
   - Mínimo 8 caracteres
   - Al menos una letra mayúscula
   - Al menos un número
4. Confirme la contraseña ingresándola dos veces
5. Haga clic en "Configurar"
6. Una vez configurada, podrá iniciar sesión con:
   - **Usuario**: admin
   - **Contraseña**: La que configuró

**Contraseña Sugerida**: `Admin123!`

### Si necesita reinicializar la contraseña admin manualmente

Ejecute este SQL script en la base de datos:

```sql
UPDATE Users 
SET PasswordHash = 'HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP',
    PasswordSalt = 'SALT_PLACEHOLDER_WILL_BE_GENERATED_BY_APP'
WHERE Username = 'admin';
```

La próxima vez que inicie la aplicación, verá nuevamente el formulario de configuración inicial.

## Configuración Adicional

### Cambiar Idioma por Defecto

En `UI/App.config`:
```xml
<add key="DefaultLanguage" value="en" />  <!-- Cambiar a "en" para inglés -->
```

### Configurar Ruta de Logs

```xml
<add key="LogDirectory" value="C:\MyApp\Logs" />
<add key="LogFilePrefix" value="MyStockManager" />
```

### Conexión a SQL Server Remoto

```xml
<add name="StockManagerDB" 
     connectionString="Server=192.168.1.100;Database=StockManagerDB;User Id=stockuser;Password=SecurePass123!;TrustServerCertificate=true;" 
     providerName="System.Data.SqlClient" />
```

## Backup y Restore

### Crear Backup

```sql
BACKUP DATABASE StockManagerDB 
TO DISK = 'C:\Backups\StockManagerDB.bak'
WITH FORMAT, INIT, 
NAME = 'StockManagerDB-Full Backup';
GO
```

### Restaurar Backup

```sql
RESTORE DATABASE StockManagerDB 
FROM DISK = 'C:\Backups\StockManagerDB.bak'
WITH REPLACE;
GO
```

## Datos de Prueba

El sistema incluye datos semilla:

**Usuario Admin**:
- Username: `admin`
- Email: `admin@stockmanager.com`
- Role: Administrator (todos los permisos)

**Almacenes**:
- WH001: Main Warehouse
- WH002: Secondary Warehouse

**Productos de Ejemplo**:
- CASE-IP14-BLK: iPhone 14 Case Black
- GLASS-IP14: iPhone 14 Screen Protector
- CASE-SAM-S23: Samsung S23 Case
- SPK-BT-01: Bluetooth Speaker Mini
- CHARGER-USBC-20W: USB-C Charger 20W

**Roles**:
- Administrator: Acceso completo
- WarehouseManager: Gestión de stock y productos
- WarehouseOperator: Operación de movimientos
- Viewer: Solo lectura

## Próximos Pasos Después de la Instalación

1. Inicializar contraseña admin
2. Crear usuarios adicionales
3. Asignar roles a usuarios
4. Registrar productos reales
5. Configurar almacenes
6. Iniciar registro de stock

## Soporte

Para problemas o consultas:
1. Revisar logs en carpeta `Logs/`
2. Consultar `README.md` para arquitectura
3. Contactar al equipo de desarrollo
