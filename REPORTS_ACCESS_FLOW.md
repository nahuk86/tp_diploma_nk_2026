# Diagrama de Flujo: Control de Acceso a Reportes

## Arquitectura del Sistema

```
┌─────────────────────────────────────────────────────────────────┐
│                    Sistema de Gestión de Stock                   │
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Sistema RBAC (Role-Based Access Control)       │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────┐     ┌──────────┐     ┌──────────────┐            │
│  │ Usuarios │────▶│  Roles   │────▶│  Permisos    │            │
│  └──────────┘     └──────────┘     └──────────────┘            │
│       │                │                    │                    │
│       │                │                    │                    │
│       └────────────────┴────────────────────┘                    │
│                         │                                        │
│                         ▼                                        │
│              ┌──────────────────────┐                           │
│              │ AuthorizationService │                           │
│              └──────────────────────┘                           │
└─────────────────────────────────────────────────────────────────┘
```

## Flujo de Verificación de Acceso a Reportes

```
┌────────────┐
│  Usuario   │
│ Inicia     │
│ Sesión     │
└─────┬──────┘
      │
      ▼
┌────────────────────┐
│ AuthService        │
│ Valida credenciales│
└─────┬──────────────┘
      │
      ▼
┌────────────────────────────┐
│ SessionContext             │
│ Carga UserId y sus roles   │
└─────┬──────────────────────┘
      │
      ▼
┌──────────────────────────────────┐
│ Form1.cs                          │
│ ConfigureMenuByPermissions()      │
└─────┬────────────────────────────┘
      │
      ▼
┌─────────────────────────────────────────────────┐
│ AuthorizationService.HasPermission()            │
│ Verifica: UserId tiene "Reports.View"?          │
└─────┬───────────────────────────────────────────┘
      │
      ├─────► SÍ ────────┐
      │                  │
      │                  ▼
      │         ┌─────────────────┐
      │         │ menuReports     │
      │         │ .Enabled = true │
      │         └─────────────────┘
      │                  │
      │                  ▼
      │         ┌─────────────────┐
      │         │ Usuario puede   │
      │         │ ver reportes    │
      │         └─────────────────┘
      │
      └─────► NO ────────┐
                        │
                        ▼
               ┌─────────────────┐
               │ menuReports     │
               │ .Enabled = false│
               └─────────────────┘
                        │
                        ▼
               ┌─────────────────┐
               │ Usuario NO puede│
               │ ver reportes    │
               └─────────────────┘
```

## Relación Entidad-Relación

```
┌──────────────┐         ┌──────────────┐
│   Users      │         │    Roles     │
├──────────────┤         ├──────────────┤
│ UserId (PK)  │    ┌───▶│ RoleId (PK)  │
│ Username     │    │    │ RoleName     │
│ FullName     │    │    │ Description  │
└──────┬───────┘    │    └──────┬───────┘
       │            │           │
       │ N          │           │ N
       └────────────┼───────────┘
                    │
                    │
             ┌──────┴────────┐
             │  UserRoles    │
             │  (Pivot)      │
             ├───────────────┤
             │ UserId (FK)   │
             │ RoleId (FK)   │
             └───────────────┘
                    
                    
┌──────────────┐         ┌──────────────────┐
│    Roles     │         │   Permissions    │
├──────────────┤         ├──────────────────┤
│ RoleId (PK)  │    ┌───▶│ PermissionId(PK) │
│ RoleName     │    │    │ PermissionCode   │──▶ "Reports.View"
└──────┬───────┘    │    │ PermissionName   │──▶ "View Reports"
       │            │    │ Module           │──▶ "Reports"
       │ N          │    └──────────────────┘
       └────────────┤
                    │
             ┌──────┴──────────┐
             │ RolePermissions │
             │   (Pivot)       │
             ├─────────────────┤
             │ RoleId (FK)     │
             │ PermissionId(FK)│
             └─────────────────┘
```

## Roles y Permisos: Vista Consolidada

```
┌──────────────────────────────────────────────────────────────┐
│                         Roles                                 │
├──────────────────┬───────────────────────────────────────────┤
│ Administrator    │ ✅ TODOS los permisos (incluyendo        │
│                  │    Reports.View)                          │
├──────────────────┼───────────────────────────────────────────┤
│ WarehouseManager │ ✅ Gestión completa de stock             │
│                  │ ✅ Reports.View                           │
│                  │ ✅ Products, Warehouses, Stock, Audit    │
├──────────────────┼───────────────────────────────────────────┤
│ Viewer           │ ✅ Solo lectura en todo el sistema       │
│                  │ ✅ Reports.View                           │
│                  │ ✅ Ver todos los módulos (sin editar)    │
├──────────────────┼───────────────────────────────────────────┤
│ Seller           │ ✅ Gestión de ventas y clientes          │
│                  │ ✅ Reports.View                           │
│                  │ ✅ Products.View, Sales, Clients         │
├──────────────────┼───────────────────────────────────────────┤
│ WarehouseOperator│ ✅ Operaciones de stock                  │
│                  │ ❌ Reports.View (NO por defecto)         │
│                  │ ✅ Stock movements                        │
└──────────────────┴───────────────────────────────────────────┘
```

## Flujo de Administración de Permisos

```
┌─────────────────────────────────────────────────────────────┐
│              Administrador Gestiona Acceso                   │
└─────────────────────────────────────────────────────────────┘
                           │
                           ▼
         ┌─────────────────────────────────┐
         │  Opción A: Modificar Rol        │
         └─────────────────────────────────┘
                           │
                           ▼
         ┌─────────────────────────────────────┐
         │ 1. Admin → Roles → Seleccionar Rol  │
         │ 2. Asignar Permisos                 │
         │ 3. Marcar/Desmarcar Reports.View    │
         │ 4. Guardar                          │
         └─────────────────────────────────────┘
                           │
                           ▼
         ┌─────────────────────────────────────┐
         │ Todos los usuarios con ese rol      │
         │ reciben/pierden acceso a reportes   │
         └─────────────────────────────────────┘
                           │
                           ▼
                   ┌───────────────┐
                   │ Usuario cierra│
                   │ y vuelve a    │
                   │ iniciar sesión│
                   └───────────────┘
                           │
                           ▼
                   ┌───────────────┐
                   │ Acceso        │
                   │ actualizado   │
                   └───────────────┘


         ┌─────────────────────────────────┐
         │  Opción B: Crear Nuevo Rol      │
         └─────────────────────────────────┘
                           │
                           ▼
         ┌─────────────────────────────────────┐
         │ 1. Admin → Roles → Nuevo            │
         │ 2. Definir nombre y descripción     │
         │ 3. Asignar Permisos                 │
         │ 4. Incluir Reports.View si necesario│
         │ 5. Guardar                          │
         └─────────────────────────────────────┘
                           │
                           ▼
         ┌─────────────────────────────────────┐
         │ Admin → Usuarios → Seleccionar      │
         │ Asignar Roles → Marcar nuevo rol    │
         └─────────────────────────────────────┘
                           │
                           ▼
                   ┌───────────────┐
                   │ Usuario recibe│
                   │ nuevo rol y   │
                   │ permisos      │
                   └───────────────┘
```

## Ejemplo de Escenario Real

```
Escenario: Operador necesita acceso temporal a reportes

┌─────────────────────────────────────────────┐
│ Estado Inicial:                              │
│ - Usuario: "juan.perez"                      │
│ - Rol: WarehouseOperator                     │
│ - Acceso a Reportes: ❌ NO                   │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│ Admin decide dar acceso temporal             │
│                                              │
│ Pasos:                                       │
│ 1. Login como Admin                          │
│ 2. Admin → Roles                             │
│ 3. Seleccionar "WarehouseOperator"           │
│ 4. Click "Asignar Permisos"                  │
│ 5. ✅ Marcar "Reports.View"                  │
│ 6. Click "Guardar"                           │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│ Base de datos actualizada:                   │
│                                              │
│ INSERT INTO RolePermissions                  │
│ (RoleId, PermissionId)                       │
│ VALUES                                       │
│ (@WarehouseOperatorId, @ReportsViewId)       │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│ Usuario "juan.perez":                        │
│ 1. Cierra sesión                             │
│ 2. Vuelve a iniciar sesión                   │
└─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────┐
│ Estado Final:                                │
│ - Usuario: "juan.perez"                      │
│ - Rol: WarehouseOperator                     │
│ - Acceso a Reportes: ✅ SÍ                   │
│ - Menu "Reportes" visible y habilitado       │
└─────────────────────────────────────────────┘
```

## Tabla de Decisión: ¿Quién puede ver reportes?

```
┌──────────────────┬─────────────┬──────────────────┐
│ Rol              │ Reports.View│ Puede Ver        │
│                  │ Asignado?   │ Reportes?        │
├──────────────────┼─────────────┼──────────────────┤
│ Administrator    │     ✅      │      ✅          │
├──────────────────┼─────────────┼──────────────────┤
│ WarehouseManager │     ✅      │      ✅          │
├──────────────────┼─────────────┼──────────────────┤
│ Viewer           │     ✅      │      ✅          │
├──────────────────┼─────────────┼──────────────────┤
│ Seller           │     ✅      │      ✅          │
├──────────────────┼─────────────┼──────────────────┤
│ WarehouseOperator│     ❌      │      ❌          │
├──────────────────┼─────────────┼──────────────────┤
│ Rol Personalizado│   Depende   │    Depende       │
│                  │  del Admin  │   del Admin      │
└──────────────────┴─────────────┴──────────────────┘
```
