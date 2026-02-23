# UML Use Case Diagrams

This document contains UML Use Case Diagrams in Mermaid format for all subsystems of the **tp_diploma_nk_2026** inventory management system.  
Each diagram shows the **actors** involved and the **use cases** they can perform within each subsystem.

---

## Actors

| Actor | Description |
|-------|-------------|
| 👤 **Administrador** | Full system access: manages users, roles, permissions, warehouses and products |
| 👤 **Vendedor** | Sales operator: creates sales and manages clients |
| 👤 **Almacenista** | Warehouse operator: manages stock movements and warehouses |
| 👤 **Supervisor** | Oversight: consults reports and monitors stock |
| 👤 **Usuario** | Any authenticated user: login, logout and password change |
| ⚙️ **Sistema** | Internal system component: cross-cutting services (logging, localization) |

---

## 1. Autenticación / Login

```mermaid
flowchart LR
    usuario(["👤 Usuario"])
    admin(["👤 Administrador"])

    subgraph LOGIN["🔐 Sistema de Autenticación"]
        uc1(["Autenticarse en el Sistema"])
        uc2(["Cerrar Sesión"])
        uc3(["Inicializar Contraseña de Administrador"])
    end

    usuario --> uc1
    usuario --> uc2
    admin --> uc3
```

---

## 2. Gestión de Usuarios

```mermaid
flowchart LR
    admin(["👤 Administrador"])

    subgraph USERS["👥 Gestión de Usuarios"]
        uc1(["Crear Usuario"])
        uc2(["Actualizar Usuario"])
        uc3(["Eliminar Usuario"])
        uc4(["Consultar Todos los Usuarios"])
        uc5(["Consultar Usuarios Activos"])
        uc6(["Consultar Usuario por ID"])
        uc7(["Asignar Roles al Usuario"])
        uc8(["Consultar Roles del Usuario"])
        uc9(["Cambiar Contraseña"])
    end

    admin --> uc1
    admin --> uc2
    admin --> uc3
    admin --> uc4
    admin --> uc5
    admin --> uc6
    admin --> uc7
    admin --> uc8
    admin --> uc9
```

---

## 3. Gestión de Productos

```mermaid
flowchart LR
    admin(["👤 Administrador"])
    vendedor(["👤 Vendedor"])
    almacenista(["👤 Almacenista"])

    subgraph PRODUCTS["📦 Gestión de Productos"]
        uc1(["Crear Producto"])
        uc2(["Actualizar Producto"])
        uc3(["Eliminar Producto"])
        uc4(["Consultar Todos los Productos"])
        uc5(["Consultar Productos Activos"])
        uc6(["Consultar Producto por ID"])
        uc7(["Consultar Productos por Categoría"])
        uc8(["Buscar Producto"])
    end

    admin --> uc1
    admin --> uc2
    admin --> uc3
    admin --> uc4
    admin --> uc5
    admin --> uc6
    admin --> uc7
    admin --> uc8
    vendedor --> uc4
    vendedor --> uc5
    vendedor --> uc7
    vendedor --> uc8
    almacenista --> uc4
    almacenista --> uc5
    almacenista --> uc7
    almacenista --> uc8
```

---

## 4. Gestión de Ventas

```mermaid
flowchart LR
    vendedor(["👤 Vendedor"])
    supervisor(["👤 Supervisor"])

    subgraph SALES["🛒 Gestión de Ventas"]
        uc1(["Crear Venta"])
        uc2(["Eliminar Venta"])
        uc3(["Actualizar Venta"])
        uc4(["Consultar Todas las Ventas"])
        uc5(["Consultar Ventas con Detalles"])
        uc6(["Consultar Venta por ID"])
        uc7(["Consultar Venta por ID con Líneas"])
        uc8(["Consultar Ventas por Cliente"])
        uc9(["Consultar Ventas por Rango de Fechas"])
        uc10(["Consultar Ventas por Vendedor"])
        uc11(["Consultar Stock Disponible por Almacén"])
        uc12(["Consultar Stock Total Disponible"])
    end

    vendedor --> uc1
    vendedor --> uc2
    vendedor --> uc3
    vendedor --> uc4
    vendedor --> uc5
    vendedor --> uc6
    vendedor --> uc7
    vendedor --> uc8
    vendedor --> uc9
    vendedor --> uc10
    vendedor --> uc11
    vendedor --> uc12
    supervisor --> uc4
    supervisor --> uc5
    supervisor --> uc8
    supervisor --> uc9
    supervisor --> uc10
    supervisor --> uc11
    supervisor --> uc12
```

---

## 5. Gestión de Movimientos de Stock

```mermaid
flowchart LR
    almacenista(["👤 Almacenista"])
    supervisor(["👤 Supervisor"])

    subgraph MOVEMENTS["🔄 Gestión de Movimientos de Stock"]
        uc1(["Crear Movimiento"])
        uc2(["Consultar Todos los Movimientos"])
        uc3(["Consultar Movimiento por ID"])
        uc4(["Consultar Líneas de Movimiento"])
        uc5(["Consultar Movimientos por Rango de Fechas"])
        uc6(["Consultar Movimientos por Tipo"])
        uc7(["Actualizar Precios de Producto"])
        uc8(["Actualizar Stock por Movimiento"])
    end

    almacenista --> uc1
    almacenista --> uc2
    almacenista --> uc3
    almacenista --> uc4
    almacenista --> uc5
    almacenista --> uc6
    almacenista --> uc7
    almacenista --> uc8
    supervisor --> uc2
    supervisor --> uc3
    supervisor --> uc4
    supervisor --> uc5
    supervisor --> uc6
```

---

## 6. Gestión de Almacenes

```mermaid
flowchart LR
    admin(["👤 Administrador"])
    almacenista(["👤 Almacenista"])

    subgraph WAREHOUSES["🏭 Gestión de Almacenes"]
        uc1(["Crear Almacén"])
        uc2(["Eliminar Almacén"])
        uc3(["Actualizar Almacén"])
        uc4(["Consultar Todos los Almacenes"])
        uc5(["Consultar Almacenes Activos"])
        uc6(["Consultar Almacén por ID"])
    end

    admin --> uc1
    admin --> uc2
    admin --> uc3
    admin --> uc4
    admin --> uc5
    admin --> uc6
    almacenista --> uc4
    almacenista --> uc5
    almacenista --> uc6
```

---

## 7. Gestión de Clientes

```mermaid
flowchart LR
    admin(["👤 Administrador"])
    vendedor(["👤 Vendedor"])

    subgraph CLIENTS["🤝 Gestión de Clientes"]
        uc1(["Crear Cliente"])
        uc2(["Eliminar Cliente"])
        uc3(["Actualizar Cliente"])
        uc4(["Consultar Todos los Clientes"])
        uc5(["Consultar Clientes Activos"])
        uc6(["Consultar Cliente por ID"])
    end

    admin --> uc1
    admin --> uc2
    admin --> uc3
    admin --> uc4
    admin --> uc5
    admin --> uc6
    vendedor --> uc1
    vendedor --> uc3
    vendedor --> uc4
    vendedor --> uc5
    vendedor --> uc6
```

---

## 8. Gestión de Reportes

```mermaid
flowchart LR
    supervisor(["👤 Supervisor"])
    admin(["👤 Administrador"])

    subgraph REPORTS["📊 Gestión de Reportes"]
        uc1(["Generar Reporte de Ventas por Categoría"])
        uc2(["Generar Reporte de Ranking de Clientes por Producto"])
        uc3(["Generar Reporte de Compras por Cliente"])
        uc4(["Generar Reporte de Variación de Precios"])
        uc5(["Generar Reporte de Ingresos por Fecha"])
        uc6(["Generar Reporte de Desempeño de Vendedores"])
        uc7(["Generar Reporte de Productos Top"])
    end

    supervisor --> uc1
    supervisor --> uc2
    supervisor --> uc3
    supervisor --> uc4
    supervisor --> uc5
    supervisor --> uc6
    supervisor --> uc7
    admin --> uc1
    admin --> uc2
    admin --> uc3
    admin --> uc4
    admin --> uc5
    admin --> uc6
    admin --> uc7
```

---

## 9. Gestión de Roles y Permisos

```mermaid
flowchart LR
    admin(["👤 Administrador"])

    subgraph ROLES["🔑 Gestión de Roles y Permisos"]
        uc1(["Crear Rol"])
        uc2(["Eliminar Rol"])
        uc3(["Actualizar Rol"])
        uc4(["Consultar Todos los Roles"])
        uc5(["Consultar Roles Activos"])
        uc6(["Consultar Rol por ID"])
        uc7(["Asignar Permisos a Rol"])
        uc8(["Consultar Permisos de Rol"])
        uc9(["Consultar Todos los Permisos"])
        uc10(["Verificar Permiso de Usuario"])
        uc11(["Verificar Todos los Permisos"])
        uc12(["Verificar Algún Permiso"])
        uc13(["Consultar Permisos del Usuario"])
    end

    admin --> uc1
    admin --> uc2
    admin --> uc3
    admin --> uc4
    admin --> uc5
    admin --> uc6
    admin --> uc7
    admin --> uc8
    admin --> uc9
    admin --> uc10
    admin --> uc11
    admin --> uc12
    admin --> uc13
```

---

## 10. Localización del Sistema

```mermaid
flowchart LR
    sistema(["⚙️ Sistema"])
    usuario(["👤 Usuario"])

    subgraph LOCALIZATION["🌐 Localización del Sistema"]
        uc1(["Cargar Todas las Traducciones"])
        uc2(["Cargar Traducciones Predeterminadas"])
        uc3(["Cambiar Idioma"])
        uc4(["Responder al Cambio de Idioma"])
    end

    sistema --> uc1
    sistema --> uc2
    sistema --> uc4
    usuario --> uc3
    uc3 -.->|"«include»"| uc4
```

---

## Diagrama de Visión General del Sistema

```mermaid
flowchart TB
    admin(["👤 Administrador"])
    vendedor(["👤 Vendedor"])
    almacenista(["👤 Almacenista"])
    supervisor(["👤 Supervisor"])
    usuario(["👤 Usuario"])
    sistema(["⚙️ Sistema"])

    subgraph SYS["🏢 Sistema de Gestión de Inventario"]
        subgraph AUTH["🔐 Autenticación"]
            login(["Autenticarse"])
            logout(["Cerrar Sesión"])
        end
        subgraph MGMT["⚙️ Administración"]
            usermgmt(["Gestionar Usuarios"])
            rolesmgmt(["Gestionar Roles y Permisos"])
            whmgmt(["Gestionar Almacenes"])
        end
        subgraph OPS["📋 Operaciones"]
            prodmgmt(["Gestionar Productos"])
            clientmgmt(["Gestionar Clientes"])
            salesmgmt(["Gestionar Ventas"])
            movmgmt(["Gestionar Movimientos de Stock"])
        end
        subgraph INTEL["📊 Inteligencia de Negocio"]
            reportsmgmt(["Generar Reportes"])
        end
        subgraph CROSS["🔧 Servicios Transversales"]
            localization(["Localización"])
            logging(["Registro de Auditoría"])
        end
    end

    usuario --> login
    usuario --> logout
    admin --> usermgmt
    admin --> rolesmgmt
    admin --> whmgmt
    admin --> prodmgmt
    admin --> clientmgmt
    admin --> reportsmgmt
    vendedor --> salesmgmt
    vendedor --> clientmgmt
    vendedor --> prodmgmt
    almacenista --> movmgmt
    almacenista --> whmgmt
    almacenista --> prodmgmt
    supervisor --> reportsmgmt
    supervisor --> movmgmt
    supervisor --> salesmgmt
    sistema --> localization
    sistema --> logging
```

---

**Last Updated**: 2026-02-23  
**Version**: 1.0  
**Author**: Development Team
