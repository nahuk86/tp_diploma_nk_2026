# Diagrama de dominio de la solución

```mermaid
  
classDiagram
    direction TB

    %% ──────────────────────────────────
    %% ENUMERACIONES
    %% ──────────────────────────────────
    class TipoMovimiento {
        <<enumeración>>
        Ingreso
        Egreso
        Transferencia
        Ajuste
    }

    class NivelLog {
        <<enumeración>>
        Depuración
        Información
        Advertencia
        Error
        Fatal
    }

    %% ──────────────────────────────────
    %% SEGURIDAD Y CONTROL DE ACCESO
    %% ──────────────────────────────────
    class Usuario {
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

    class Rol {
        +int RoleId
        +string RoleName
        +string Description
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
    }

    class Permiso {
        +int PermissionId
        +string PermissionCode
        +string PermissionName
        +string Description
        +string Module
        +bool IsActive
        +DateTime CreatedAt
    }

    %% ──────────────────────────────────
    %% AUDITORÍA
    %% ──────────────────────────────────
    class RegistroAuditoria {
        +int AuditId
        +string TableName
        +int RecordId
        +string Action
        +string FieldName
        +string OldValue
        +string NewValue
        +DateTime ChangedAt
        +int? ChangedBy
        +string ChangedByUsername
    }

    %% ──────────────────────────────────
    %% CATÁLOGO DE PRODUCTOS
    %% ──────────────────────────────────
    class Producto {
        +int ProductId
        +string SKU
        +string Name
        +string Description
        +string Category
        +decimal UnitPrice
        +int MinStockLevel
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
    }

    %% ──────────────────────────────────
    %% ALMACENES
    %% ──────────────────────────────────
    class Almacen {
        +int WarehouseId
        +string Code
        +string Name
        +string Address
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
    }

    %% ──────────────────────────────────
    %% CLIENTES
    %% ──────────────────────────────────
    class Cliente {
        +int ClientId
        +string Nombre
        +string Apellido
        +string Correo
        +string DNI
        +string Telefono
        +string Direccion
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
    }

    %% ──────────────────────────────────
    %% VENTAS
    %% ──────────────────────────────────
    class Venta {
        +int SaleId
        +string SaleNumber
        +DateTime SaleDate
        +int? ClientId
        +string SellerName
        +decimal TotalAmount
        +string Notes
        +bool IsActive
        +DateTime CreatedAt
        +int? CreatedBy
        +DateTime? UpdatedAt
        +int? UpdatedBy
        +List~LineaVenta~ SaleLines
    }

    class LineaVenta {
        +int SaleLineId
        +int SaleId
        +int ProductId
        +int Quantity
        +decimal UnitPrice
        +decimal LineTotal
        +string ProductName
        +string SKU
    }

    %% ──────────────────────────────────
    %% STOCK / INVENTARIO
    %% ──────────────────────────────────
    class Stock {
        +int StockId
        +int ProductId
        +int WarehouseId
        +int Quantity
        +DateTime LastUpdated
        +int? UpdatedBy
        +string ProductName
        +string ProductSKU
        +string WarehouseName
    }

    %% ──────────────────────────────────
    %% MOVIMIENTOS DE STOCK
    %% ──────────────────────────────────
    class MovimientoStock {
        +int MovementId
        +string MovementNumber
        +TipoMovimiento MovementType
        +DateTime MovementDate
        +int? SourceWarehouseId
        +int? DestinationWarehouseId
        +string Reason
        +string Notes
        +DateTime CreatedAt
        +int CreatedBy
        +string CreatedByUsername
        +string SourceWarehouseName
        +string DestinationWarehouseName
    }

    class LineaMovimientoStock {
        +int LineId
        +int MovementId
        +int ProductId
        +int Quantity
        +decimal? UnitPrice
        +string ProductName
        +string ProductSKU
    }

    %% ══════════════════════════════════
    %% RELACIONES
    %% ══════════════════════════════════

    %% Seguridad: Usuarios ↔ Roles ↔ Permisos (muchos-a-muchos implícito)
    Usuario "0..*" -- "0..*" Rol : tiene asignado
    Rol "0..*" -- "0..*" Permiso : otorga

    %% Auditoría: RegistroAuditoria registra cambios realizados por Usuarios
    Usuario "1" -- "0..*" RegistroAuditoria : realiza

    %% Relaciones de Ventas
    Cliente "1" -- "0..*" Venta : realiza
    Venta "1" *-- "1..*" LineaVenta : contiene
    Producto "1" -- "0..*" LineaVenta : referenciado en

    %% Stock: Producto almacenado en Almacén
    Producto "1" -- "0..*" Stock : registrado en
    Almacen "1" -- "0..*" Stock : almacena

    %% Movimientos de Stock
    MovimientoStock "1" *-- "1..*" LineaMovimientoStock : contiene
    Producto "1" -- "0..*" LineaMovimientoStock : trasladado mediante
    MovimientoStock --> TipoMovimiento : usa
    Almacen "0..1" -- "0..*" MovimientoStock : origen
    Almacen "0..1" -- "0..*" MovimientoStock : destino
    Usuario "1" -- "0..*" MovimientoStock : creado por

    ```
