# UML Diagrams Documentation

This directory contains comprehensive UML class diagrams and sequence diagrams for all major processes in the **tp_diploma_nk_2026** inventory management system. All diagrams are provided in **Mermaid format** following UML standards and are organized **per use case**.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture Layers](#architecture-layers)
- [Diagram Index](#diagram-index)
- [How to Use](#how-to-use)
- [Mermaid Rendering](#mermaid-rendering)

---

## 🏗️ Overview

The system follows a layered architecture pattern with clear separation of concerns:

- **UI Layer**: Windows Forms for user interface
- **BLL Layer**: Business Logic Layer for business rules
- **DAO Layer**: Data Access Objects for database operations
- **Services Layer**: Cross-cutting concerns (authentication, authorization, logging, localization)
- **Domain Layer**: Entities and DTOs

Each file contains diagrams organized **per use case**, where each use case has:
1. **Class Diagram**: Shows the classes, attributes, methods, and relationships relevant to that use case
2. **Sequence Diagram**: Shows the interaction flow between layers for that use case

---

## 🏛️ Architecture Layers

```
┌─────────────────────────────────────────────────────────┐
│  UI LAYER (Windows Forms)                               │
│  - LoginForm, UsersForm, SalesForm, etc.                │
└────────────────────┬────────────────────────────────────┘
                     │ uses
                     ▼
┌─────────────────────────────────────────────────────────┐
│  BLL LAYER (Business Logic)                             │
│  - UserService, SaleService, StockMovementService, etc. │
└────────────────────┬────────────────────────────────────┘
                     │ calls
        ┌────────────┴──────────────┐
        ▼                           ▼
┌───────────────────────┐  ┌───────────────────────────┐
│  DAO LAYER            │  │  SERVICES LAYER           │
│  - UserRepository     │  │  - AuthenticationService  │
│  - SaleRepository     │  │  - AuthorizationService   │
│  - ProductRepository  │  │  - LocalizationService    │
│  - DatabaseHelper     │  │  - LogService             │
└───────────┬───────────┘  └───────────────────────────┘
            │ returns
            ▼
┌─────────────────────────────────────────────────────────┐
│  DOMAIN LAYER                                            │
│  - Entities: User, Sale, Product, Stock, etc.            │
│  - DTOs: Report DTOs                                     │
│  - Enums: MovementType, AuditAction, LogLevel           │
└─────────────────────────────────────────────────────────┘
```

---

## 📊 Diagram Index

### 1. Login Process — Per Use Case (Class Diagrams)
- **[01_Login_Process_Class_Diagram.md](./01_Login_Process_Class_Diagram.md)**
  - UC-01: Authenticate
  - UC-02: InitializeAdminPassword

### 2. Login Process — Per Use Case (Sequence Diagrams)
- **[02_Login_Process_Sequence_Diagram.md](./02_Login_Process_Sequence_Diagram.md)**
  - UC-01: Authenticate
  - UC-02: InitializeAdminPassword

---

### 3. User Management Process — Per Use Case (Class Diagrams)
- **[03_User_Management_Process_Class_Diagram.md](./03_User_Management_Process_Class_Diagram.md)**
  - UC-01: CreateUser | UC-02: UpdateUser | UC-03: DeleteUser
  - UC-04: GetAllUsers | UC-05: GetActiveUsers | UC-06: GetUserById
  - UC-07: AssignRolesToUser | UC-08: GetUserRoles | UC-09: ChangePassword

### 4. User Management Process — Per Use Case (Sequence Diagrams)
- **[04_User_Management_Process_Sequence_Diagram.md](./04_User_Management_Process_Sequence_Diagram.md)**
  - UC-01 through UC-09 (same use cases)

---

### 5. Sales Management Process — Per Use Case (Class Diagrams)
- **[05_Sales_Management_Process_Class_Diagram.md](./05_Sales_Management_Process_Class_Diagram.md)**
  - UC-01: CreateSale | UC-02: DeleteSale | UC-03: GetAllSales
  - UC-04: GetAllSalesWithDetails | UC-05: GetAvailabelStockByWarehouse
  - UC-06: GetSaleById | UC-07: GetSaleByIdWithLines | UC-08: GetSaleByClient
  - UC-09: GetSaleByDateRange | UC-10: GetSaleBySeller
  - UC-11: GetTotalAvailableStock | UC-12: UpdateSale

### 6. Sales Management Process — Per Use Case (Sequence Diagrams)
- **[06_Sales_Management_Process_Sequence_Diagram.md](./06_Sales_Management_Process_Sequence_Diagram.md)**
  - UC-01 through UC-12 (same use cases)

---

### 7. Stock Movement Process — Per Use Case (Class Diagrams)
- **[07_Stock_Movement_Process_Class_Diagram.md](./07_Stock_Movement_Process_Class_Diagram.md)**
  - UC-01: CreateMovement | UC-02: GetAllMovements | UC-03: GetAllMovementsById
  - UC-04: GetMovementLines | UC-05: GetMovementsByDateRange | UC-06: GetMovementsByType
  - UC-07: UpdateProductPrices | UC-08: UpdateStockForMovement

### 8. Stock Movement Process — Per Use Case (Sequence Diagrams)
- **[08_Stock_Movement_Process_Sequence_Diagram.md](./08_Stock_Movement_Process_Sequence_Diagram.md)**
  - UC-01 through UC-08 (same use cases)

---

### 9. Reports Management Process — Per Use Case (Class Diagrams)
- **[09_Reports_Management_Process_Class_Diagram.md](./09_Reports_Management_Process_Class_Diagram.md)**
  - UC-01: GetCategorySalesReport | UC-02: GetClientProductRankingReport
  - UC-03: GetClientPurchasesReport | UC-04: GetPriceVariationReport
  - UC-05: GetRevenueByDateReport | UC-06: GetSellerPerformanceReport
  - UC-07: GetTopProductsReport

### 10. Reports Management Process — Per Use Case (Sequence Diagrams)
- **[10_Reports_Management_Process_Sequence_Diagram.md](./10_Reports_Management_Process_Sequence_Diagram.md)**
  - UC-01 through UC-07 (same use cases)

---

### 11. Role & Permissions Management — Per Use Case (Class Diagrams)
- **[11_Role_Permissions_Process_Class_Diagram.md](./11_Role_Permissions_Process_Class_Diagram.md)**
  - UC-01: CreateRole | UC-02: DeleteRole | UC-03: AssignPermissions
  - UC-04: GetActiveRoles | UC-05: GetAllPermissions | UC-06: GetAllRoles
  - UC-07: GetRoleById | UC-08: GetRolePermissions | UC-09: UpdateRole
  - UC-10: GetUserPermissions | UC-11: HasAllPermissions | UC-12: HasAnyPermission | UC-13: HasPermission

### 12. Role & Permissions Management — Per Use Case (Sequence Diagrams)
- **[12_Role_Permissions_Process_Sequence_Diagram.md](./12_Role_Permissions_Process_Sequence_Diagram.md)**
  - UC-01 through UC-13 (same use cases)

---

### 13. Products — Individual Use Cases
- **[13_Products_UseCases.md](./13_Products_UseCases.md)**
  - CreateProduct, DeleteProduct, GetActiveProducts, GetAllProducts, GetProductById, GetProductsByCategory, SearchProduct, UpdateProduct

---

### 14. Stock Movements — Individual Use Cases
- **[14_Movements_UseCases.md](./14_Movements_UseCases.md)**
  - CreateMovement, GetAllMovements, GetAllMovementsById, GetMovementLines, GetMovementsByDateRange, GetMovementsByType, UpdateProductPrices, UpdateStockForMovement

---

### 15. Sales — Individual Use Cases
- **[15_Sales_UseCases.md](./15_Sales_UseCases.md)**
  - CreateSale, DeleteSale, GetAllSales, GetAllSalesWithDetails, GetAvailabelStockByWarehouse, GetSaleById, GetSaleByIdWithLines, GetSaleByClient, GetSaleByDateRange, GetSaleBySeller, GetTotalAvailableStock, UpdateSale

---

### 16. Warehouses — Individual Use Cases
- **[16_Warehouses_UseCases.md](./16_Warehouses_UseCases.md)**
  - CreateWareHouse, DeleteWarehouse, GetAllActiveWarehouses, GetAllWarehouses, GetWarehousesById, UpdateWarehouse

---

### 17. Clients — Individual Use Cases
- **[17_Clients_UseCases.md](./17_Clients_UseCases.md)**
  - CreateClient, DeleteClient, GetActiveClients, GetAllClients, GetClientById, UpdateClient

---

### 18. Reports — Individual Use Cases
- **[18_Reports_UseCases.md](./18_Reports_UseCases.md)**
  - GetCategorySalesReport, GetClientProductRankingReport, GetClientPurchasesReport, GetPriceVariationReport, GetRevenueByDateReport, GetSellerPerformanceReport, GetTopProductsReport

---

### 19. Users — Individual Use Cases
- **[19_Users_UseCases.md](./19_Users_UseCases.md)**
  - AssignRolesToUser, ChangePassword, CreateUser, DeleteUser, GetActiveUsers, GetAllUsers, GetUserById, GetUserRoles, UpdateUser

---

### 20. Roles — Individual Use Cases
- **[20_Roles_UseCases.md](./20_Roles_UseCases.md)**
  - CreateRole, DeleteRole, AssignPermissions, GetActiveRoles, GetAllPermissions, GetAllRoles, GetRoleById, GetRolePermissions, UpdateRole

---

### 21. Permissions & Authorization — Individual Use Cases
- **[21_Permissions_UseCases.md](./21_Permissions_UseCases.md)**
  - GetUserPermissions, HasAllPermissions, HasAnyPermission, HasPermission

---

### 22. Localization — Individual Use Cases
- **[22_Localization_UseCases.md](./22_Localization_UseCases.md)**
  - LoadAllTranslations, LoadDefaultTranslations, OnLanguageChanged, SetLanguage

---

## 🎯 How to Use

### For Developers
1. **Understanding Architecture**: Start with class diagrams to understand system structure
2. **Implementing Features**: Use sequence diagrams to understand interaction flow
3. **Debugging**: Follow sequence diagrams to trace execution flow
4. **Code Review**: Reference diagrams to ensure adherence to architecture

### For Architects
1. **System Design**: Use as reference for architectural decisions
2. **Documentation**: Keep diagrams updated as system evolves
3. **Training**: Use diagrams to onboard new team members
4. **Communication**: Share with stakeholders to explain system design

### For Business Analysts
1. **Process Understanding**: Sequence diagrams show complete business flow
2. **Requirements Validation**: Verify business rules are correctly implemented
3. **Gap Analysis**: Compare diagrams with requirements to find gaps

---

## 🖼️ Mermaid Rendering

All diagrams are in Mermaid format and can be rendered in:

### GitHub
Simply view the `.md` files on GitHub - Mermaid diagrams render automatically

### VS Code
Install the **Mermaid Preview** extension:
```
ext install bierner.markdown-mermaid
```

### Online Editors
- [Mermaid Live Editor](https://mermaid.live/)
- Copy/paste the diagram code from markdown files

### Documentation Sites
- GitHub Pages
- GitBook
- MkDocs with Mermaid plugin

---

## 📐 Diagram Standards

All diagrams follow these standards:

### Class Diagrams
- **Visibility**: - (private), + (public), # (protected)
- **Types**: Return types and parameter types specified
- **Relationships**: 
  - `-->` uses/dependency
  - `..|>` implements
  - `--|>` inherits
  - `--*` composition

### Sequence Diagrams
- **Participants**: Organized by layer (UI → BLL → DAO → DB)
- **Activation**: Shows when objects are active
- **Alt/Opt/Loop**: Control flow clearly marked
- **Notes**: Explain complex operations

---

## 🔄 Maintenance

### Keeping Diagrams Updated
1. **Code Changes**: Update diagrams when changing class structure or flow
2. **New Features**: Add diagrams for new major features
3. **Refactoring**: Update diagrams to reflect architectural changes
4. **Documentation**: Keep this README updated with new diagrams

---

## 📚 Additional Resources

### Related Documentation
- [README.md](../../README.md) - Project overview
- [SETUP.md](../../SETUP.md) - Installation guide
- [PROJECT_SUMMARY.md](../../PROJECT_SUMMARY.md) - Technical summary
- [IMPLEMENTATION.md](../../IMPLEMENTATION.md) - Implementation details

### External References
- [Mermaid Documentation](https://mermaid.js.org/)
- [UML Class Diagram Guide](https://www.uml-diagrams.org/class-diagrams-overview.html)
- [UML Sequence Diagram Guide](https://www.uml-diagrams.org/sequence-diagrams.html)

---

**Last Updated**: 2026-02-22  
**Version**: 2.0  
**Author**: Development Team

