# Descripción de Casos de Uso y Diagramas de Secuencia

Este documento presenta, para cada caso de uso del sistema, una descripción estructurada con: **introducción**, **precondición**, **entradas**, **proceso** y **salida**. Además, para cada diagrama de secuencia documentado se incluye una descripción paso a paso de lo que realiza el sistema.

---

## Índice

1. [Módulo: Login / Autenticación](#1-módulo-login--autenticación)
2. [Módulo: Productos](#2-módulo-productos)
3. [Módulo: Movimientos de Stock](#3-módulo-movimientos-de-stock)
4. [Módulo: Ventas](#4-módulo-ventas)
5. [Módulo: Depósitos (Warehouses)](#5-módulo-depósitos-warehouses)
6. [Módulo: Clientes](#6-módulo-clientes)
7. [Módulo: Reportes](#7-módulo-reportes)
8. [Módulo: Usuarios](#8-módulo-usuarios)
9. [Módulo: Roles](#9-módulo-roles)
10. [Módulo: Permisos y Autorización](#10-módulo-permisos-y-autorización)
11. [Módulo: Localización](#11-módulo-localización)

---

## 1. Módulo: Login / Autenticación

### UC-01: Authenticate (Autenticar usuario)

**Introducción:**
Permite al usuario ingresar al sistema utilizando sus credenciales (nombre de usuario y contraseña). Es el punto de entrada al sistema y garantiza que sólo usuarios válidos y activos puedan acceder.

**Precondición:**
- El sistema debe estar en ejecución.
- El usuario debe tener una cuenta activa en la base de datos.
- La contraseña del usuario debe haber sido inicializada previamente (hash y salt definidos).

**Entradas:**
- Nombre de usuario (`username`): texto no vacío.
- Contraseña (`password`): texto no vacío.

**Proceso:**
1. El formulario de login valida que los campos no estén vacíos.
2. Se invoca `AuthenticationService.Authenticate(username, password)`.
3. El servicio consulta `UserRepository.GetByUsername(username)` para obtener el usuario.
4. Se verifica que el usuario exista, esté activo y tenga contraseña inicializada.
5. Se verifica la contraseña mediante PBKDF2 (10.000 iteraciones con sal aleatoria de 32 bytes).
6. Si la validación es exitosa, se actualiza el último login (`UpdateLastLogin`) y se establece la sesión en `SessionContext`.
7. En caso de fallo, se muestra un mensaje de error genérico (sin revelar la causa específica) y se registra el intento en el log de auditoría.

**Salida:**
- Éxito: sesión iniciada, formulario cerrado, acceso al sistema.
- Fallo: mensaje de error "Usuario o contraseña incorrectos", el formulario permanece abierto.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: Authenticate:**

1. El usuario abre el formulario de login; los textos se obtienen del servicio de localización.
2. El usuario ingresa nombre de usuario y contraseña y hace clic en "Iniciar sesión".
3. El formulario valida que ambos campos estén completos.
4. Si algún campo está vacío, se muestra un mensaje de validación.
5. Si los campos son válidos, se llama a `AuthenticationService.Authenticate(username, password)`.
6. El servicio llama a `UserRepository.GetByUsername(username)`, que consulta la base de datos.
7. Si el usuario no existe o está inactivo, se registra una advertencia en el log y se retorna `null` al formulario.
8. Si la contraseña no está inicializada, se retorna `null` con un mensaje de inicialización requerida.
9. Si la contraseña no coincide (fallo de verificación PBKDF2), se registra advertencia y se retorna `null`.
10. Si las credenciales son válidas, se llama a `UserRepository.UpdateLastLogin(userId)` para registrar el acceso.
11. Se registra en el log: "Usuario autenticado exitosamente".
12. Se retorna la entidad `User` al formulario.
13. El formulario establece `SessionContext.Instance.CurrentUser = user`.
14. Se cierra el formulario con `DialogResult = OK` y se accede al formulario principal.

---

### UC-02: InitializeAdminPassword (Inicializar contraseña de administrador)

**Introducción:**
Permite al administrador del sistema establecer su contraseña por primera vez, en el proceso de configuración inicial. Es un caso de uso de un solo uso: una vez inicializada la contraseña, este flujo no vuelve a ejecutarse.

**Precondición:**
- El sistema acaba de instalarse por primera vez.
- El usuario administrador (`admin`) existe en la base de datos pero sin contraseña inicializada.

**Entradas:**
- Nueva contraseña (`newPassword`): texto con longitud mínima.
- Confirmación de contraseña (`confirmPassword`): debe coincidir con la nueva contraseña.

**Proceso:**
1. El formulario valida que las contraseñas coincidan y cumplan con la longitud mínima.
2. Se invoca `AuthenticationService.InitializeAdminPassword("admin", newPassword)`.
3. El servicio genera un sal aleatorio de 32 bytes y aplica hashing PBKDF2.
4. Se obtiene el usuario administrador con `UserRepository.GetByUsername("admin")`.
5. Se asignan el hash y el salt al usuario y se persiste con `UserRepository.Update(user)`.
6. Se registra el evento en el log de auditoría.
7. El formulario redirige al formulario de login.

**Salida:**
- Éxito: contraseña inicializada, redirección al login con mensaje de confirmación.
- Fallo: mensaje de error indicando que las contraseñas no coinciden o son demasiado cortas.

---

**Descripción paso a paso del diagrama de secuencia — UC-02: InitializeAdminPassword:**

1. El administrador abre el formulario de inicialización de contraseña (primer arranque del sistema).
2. El formulario se muestra con campos para nueva contraseña y confirmación.
3. El administrador ingresa la nueva contraseña y su confirmación, luego hace clic en "Guardar".
4. El formulario ejecuta `ValidatePasswords()`: verifica que las contraseñas coincidan y tengan la longitud mínima.
5. Si las contraseñas no coinciden, se muestra "Las contraseñas no coinciden".
6. Si la contraseña es demasiado corta, se muestra "Contraseña demasiado corta".
7. Si la validación es exitosa, se llama a `AuthenticationService.InitializeAdminPassword("admin", newPassword)`.
8. El servicio genera un salt aleatorio de 32 bytes y crea el hash PBKDF2.
9. Se consulta `UserRepository.GetByUsername("admin")` para obtener la entidad del administrador.
10. Se asignan `user.PasswordHash = hash` y `user.PasswordSalt = salt`.
11. Se llama a `UserRepository.Update(user)`, que ejecuta `UPDATE Users SET PasswordHash=@Hash, PasswordSalt=@Salt WHERE UserId=@Id`.
12. Se registra en el log: "Contraseña de administrador inicializada exitosamente".
13. El formulario muestra "Contraseña inicializada. Por favor, inicie sesión." y redirige al formulario de login.

---

## 2. Módulo: Productos

### UC-01: CreateProduct (Crear producto)

**Introducción:**
Permite registrar un nuevo producto en el catálogo del sistema, incluyendo su SKU, nombre, categoría, precio unitario y nivel mínimo de stock.

**Precondición:**
- El usuario debe tener sesión iniciada y permiso de creación de productos.
- El SKU ingresado no debe existir previamente en el sistema.

**Entradas:**
- SKU (código único del producto).
- Nombre del producto.
- Descripción (opcional).
- Categoría.
- Precio unitario (decimal positivo).
- Nivel mínimo de stock (entero no negativo).

**Proceso:**
1. El formulario valida los campos obligatorios.
2. Se invoca `ProductService.CreateProduct(product)`.
3. El servicio ejecuta `ValidateProduct(product)` (reglas de negocio).
4. Se verifica que el SKU sea único con `ProductRepository.SKUExists(sku, 0)`.
5. Se inserta el producto en la base de datos con `ProductRepository.Insert(product)`.
6. Se registra el evento en el log de auditoría.

**Salida:**
- Éxito: nuevo `productId` retornado, lista de productos recargada, mensaje de éxito.
- Fallo: mensaje de error de validación o SKU duplicado.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: CreateProduct:**

1. El usuario completa el formulario de producto.
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un mensaje de error de validación.
4. Si la validación pasa, se llama a `ProductService.CreateProduct(product)`.
5. El servicio ejecuta `ValidateProduct(product)` internamente.
6. Se consulta `ProductRepository.SKUExists(sku, 0)` para verificar unicidad del SKU.
7. El repositorio obtiene una conexión con `DatabaseHelper.GetConnection()` y consulta la base de datos.
8. Si el SKU es único (retorna `false`), se continúa.
9. Se llama a `ProductRepository.Insert(product)`, que ejecuta `INSERT INTO Products ...`.
10. El repositorio retorna el nuevo `productId`.
11. El servicio retorna el `productId` al formulario.
12. El formulario ejecuta `LoadProducts()` para actualizar la grilla.
13. Se muestra un mensaje de éxito al usuario.

---

### UC-02: DeleteProduct (Eliminar producto)

**Introducción:**
Permite desactivar un producto del catálogo (borrado lógico). El producto no se elimina físicamente de la base de datos, sino que se marca como inactivo.

**Precondición:**
- El usuario debe tener permiso de eliminación de productos.
- El producto a eliminar debe existir y estar activo.

**Entradas:**
- Identificador del producto (`productId`).
- Identificador del usuario que realiza la acción (`deletedBy`).

**Proceso:**
1. Se muestra un diálogo de confirmación al usuario.
2. Si el usuario confirma, se invoca `ProductService.DeleteProduct(productId, currentUserId)`.
3. El servicio llama a `ProductRepository.SoftDelete(id, deletedBy)`.
4. Se ejecuta `UPDATE Products SET IsActive=0` en la base de datos.

**Salida:**
- Éxito: producto desactivado, lista de productos recargada, mensaje de éxito.
- Cancelado: no se realiza ninguna acción.

---

**Descripción paso a paso del diagrama de secuencia — UC-02: DeleteProduct:**

1. El usuario selecciona un producto en la grilla y hace clic en "Eliminar".
2. El formulario muestra un diálogo de confirmación de borrado.
3. Si el usuario cancela, no se realiza ninguna acción.
4. Si el usuario confirma, se llama a `ProductService.DeleteProduct(productId, currentUserId)`.
5. El servicio llama a `ProductRepository.SoftDelete(id, deletedBy)`.
6. El repositorio obtiene una conexión y ejecuta `UPDATE Products SET IsActive=0 ...`.
7. El repositorio retorna `void` al servicio.
8. El servicio retorna `void` al formulario.
9. El formulario ejecuta `LoadProducts()` para actualizar la grilla.
10. Se muestra un mensaje de éxito al usuario.

---

### UC-03: GetActiveProducts (Obtener productos activos)

**Introducción:**
Recupera todos los productos que se encuentran activos en el sistema para ser presentados en el formulario de gestión de productos.

**Precondición:**
- El usuario debe tener sesión iniciada.

**Entradas:**
- Ninguna entrada del usuario (la consulta no lleva parámetros).

**Proceso:**
1. El formulario invoca `ProductService.GetActiveProducts()`.
2. El servicio consulta `ProductRepository.GetAllActive()`.
3. El repositorio ejecuta `SELECT * FROM Products WHERE IsActive=1`.

**Salida:**
- Lista de objetos `Product` activos, enlazada al componente `DataGridView`.

---

**Descripción paso a paso del diagrama de secuencia — UC-03: GetActiveProducts:**

1. El formulario llama a `ProductService.GetActiveProducts()`.
2. El servicio llama a `ProductRepository.GetAllActive()`.
3. El repositorio obtiene una conexión de base de datos.
4. Se ejecuta `SELECT * FROM Products WHERE IsActive=1`.
5. El repositorio mapea los resultados a una lista de objetos `Product`.
6. El repositorio retorna `List<Product>` al servicio.
7. El servicio retorna `List<Product>` al formulario.
8. El formulario enlaza la lista al `DataGridView`.

---

### UC-04: GetAllProducts (Obtener todos los productos)

**Introducción:**
Recupera todos los productos del sistema, incluyendo los inactivos, para tareas administrativas de auditoría o gestión.

**Precondición:**
- El usuario debe tener sesión iniciada con permisos administrativos.

**Entradas:**
- Ninguna.

**Proceso:**
1. El formulario invoca `ProductService.GetAllProducts()`.
2. El servicio consulta `ProductRepository.GetAll()`.
3. El repositorio ejecuta `SELECT * FROM Products` (todos, incluyendo inactivos).

**Salida:**
- Lista completa de objetos `Product`, enlazada al `DataGridView`.

---

**Descripción paso a paso del diagrama de secuencia — UC-04: GetAllProducts:**

1. El formulario llama a `ProductService.GetAllProducts()`.
2. El servicio llama a `ProductRepository.GetAll()`.
3. El repositorio obtiene una conexión de base de datos.
4. Se ejecuta `SELECT * FROM Products` (todos, incluyendo inactivos).
5. El repositorio retorna `List<Product>` al servicio.
6. El servicio retorna `List<Product>` al formulario.
7. El formulario enlaza la lista al `DataGridView`.

---

### UC-05: GetProductById (Obtener producto por ID)

**Introducción:**
Recupera los datos de un producto específico a partir de su identificador, para ser mostrado en el formulario de edición.

**Precondición:**
- El producto con el ID especificado debe existir en la base de datos.

**Entradas:**
- `productId` (entero).

**Proceso:**
1. El formulario invoca `ProductService.GetProductById(productId)`.
2. El servicio consulta `ProductRepository.GetById(id)`.
3. El repositorio ejecuta `SELECT * FROM Products WHERE ProductId=@Id`.

**Salida:**
- Objeto `Product` con todos sus atributos, o `null` si no se encuentra.

---

**Descripción paso a paso del diagrama de secuencia — UC-05: GetProductById:**

1. El formulario llama a `ProductService.GetProductById(productId)`.
2. El servicio llama a `ProductRepository.GetById(id)`.
3. El repositorio obtiene conexión y ejecuta `SELECT * FROM Products WHERE ProductId=@Id`.
4. Si el producto no existe, el servicio retorna `null` y el formulario muestra un mensaje de "no encontrado".
5. Si el producto existe, el servicio retorna el objeto `Product`.
6. El formulario rellena los campos del formulario con los datos del producto.

---

### UC-06: GetProductsByCategory (Filtrar productos por categoría)

**Introducción:**
Permite filtrar el catálogo de productos activos según una categoría específica seleccionada por el usuario.

**Precondición:**
- Debe existir al menos una categoría con productos activos.

**Entradas:**
- `category` (cadena de texto, nombre de la categoría).

**Proceso:**
1. El usuario selecciona una categoría en el filtro del formulario.
2. El formulario llama a `ProductService.GetProductsByCategory(category)`.
3. El servicio consulta `ProductRepository.GetByCategory(category)`.
4. El repositorio ejecuta `SELECT * FROM Products WHERE Category=@Category AND IsActive=1`.

**Salida:**
- Lista filtrada de productos activos de la categoría seleccionada.

---

**Descripción paso a paso del diagrama de secuencia — UC-06: GetProductsByCategory:**

1. El usuario selecciona una categoría en el filtro del formulario.
2. El formulario llama a `ProductService.GetProductsByCategory(category)`.
3. El servicio llama a `ProductRepository.GetByCategory(category)`.
4. El repositorio obtiene conexión y ejecuta `SELECT * FROM Products WHERE Category=@Category AND IsActive=1`.
5. El repositorio retorna `List<Product>` al servicio.
6. El servicio retorna `List<Product>` al formulario.
7. El formulario enlaza los resultados filtrados al `DataGridView`.

---

### UC-07: SearchProduct (Buscar producto)

**Introducción:**
Permite buscar productos en el catálogo mediante un término de búsqueda que se compara contra el nombre y el SKU del producto.

**Precondición:**
- Debe haber al menos un producto activo en el sistema.

**Entradas:**
- `searchTerm` (cadena de texto; puede ser parte del nombre o del SKU).

**Proceso:**
1. El usuario escribe un término en el campo de búsqueda.
2. El formulario llama a `ProductService.SearchProducts(searchTerm)`.
3. El servicio consulta `ProductRepository.Search(term)`.
4. El repositorio ejecuta `SELECT * FROM Products WHERE Name LIKE '%@Term%' OR SKU LIKE '%@Term%'`.

**Salida:**
- Lista de productos que coinciden con el término de búsqueda.

---

**Descripción paso a paso del diagrama de secuencia — UC-07: SearchProduct:**

1. El usuario escribe un término de búsqueda en el campo correspondiente.
2. El formulario llama a `ProductService.SearchProducts(searchTerm)`.
3. El servicio llama a `ProductRepository.Search(term)`.
4. El repositorio obtiene conexión y ejecuta la consulta con `LIKE`.
5. El repositorio retorna la lista de coincidencias al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los resultados al `DataGridView`.

---

### UC-08: UpdateProduct (Actualizar producto)

**Introducción:**
Permite modificar los datos de un producto existente, como su nombre, descripción, categoría, precio unitario o nivel mínimo de stock.

**Precondición:**
- El producto a actualizar debe existir y estar activo.
- El usuario debe tener permiso de edición de productos.
- El SKU modificado no debe estar en uso por otro producto diferente.

**Entradas:**
- Objeto `Product` con todos los campos actualizados (incluyendo `ProductId`).

**Proceso:**
1. El formulario valida los campos obligatorios.
2. Se invoca `ProductService.UpdateProduct(product)`.
3. El servicio ejecuta `ValidateProduct(product)`.
4. Se verifica unicidad del SKU excluyendo el producto actual con `ProductRepository.SKUExists(sku, productId)`.
5. Se actualiza el registro con `ProductRepository.Update(product)`.
6. Se ejecuta `UPDATE Products SET Name=@Name, ... WHERE ProductId=@Id`.

**Salida:**
- Éxito: datos actualizados, lista de productos recargada, mensaje de éxito.
- Fallo: mensaje de error de validación o SKU duplicado.

---

**Descripción paso a paso del diagrama de secuencia — UC-08: UpdateProduct:**

1. El usuario modifica los campos del producto en el formulario.
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un mensaje de error.
4. Si la validación pasa, se llama a `ProductService.UpdateProduct(product)`.
5. El servicio ejecuta `ValidateProduct(product)` internamente.
6. Se consulta `ProductRepository.SKUExists(sku, productId)` para verificar unicidad (excluyendo el producto actual).
7. Si el SKU es único, se llama a `ProductRepository.Update(product)`.
8. El repositorio obtiene conexión y ejecuta `UPDATE Products SET Name=@Name, ... WHERE ProductId=@Id`.
9. El repositorio retorna `void` al servicio.
10. El servicio retorna `void` al formulario.
11. El formulario ejecuta `LoadProducts()` para actualizar la grilla.
12. Se muestra un mensaje de éxito al usuario.

---

## 3. Módulo: Movimientos de Stock

### UC-01: CreateMovement (Registrar movimiento de stock)

**Introducción:**
Permite registrar un movimiento de stock (entrada, salida, transferencia o ajuste) con sus líneas de detalle por producto, actualizando automáticamente el stock de los depósitos afectados.

**Precondición:**
- Deben existir depósitos activos y productos activos en el sistema.
- El usuario debe tener permiso para crear movimientos.
- Para movimientos de salida o transferencia, debe haber stock suficiente.

**Entradas:**
- Tipo de movimiento (`MovementType`: In, Out, Transfer, Adjustment).
- Depósito origen y/o destino.
- Fecha del movimiento.
- Motivo y notas.
- Líneas del movimiento: lista de productos con cantidad y precio unitario.

**Proceso:**
1. El formulario valida el movimiento con `ValidateMovement()`.
2. Se invoca `StockMovementService.CreateMovement(movement, lines)`.
3. Se genera el número de movimiento con `GenerateMovementNumber()`.
4. El servicio valida internamente el movimiento y líneas.
5. Para cada línea, verifica el stock disponible.
6. Se inserta el movimiento principal en la base de datos.
7. Se insertan las líneas del movimiento.
8. Se aplican los cambios de stock según el tipo de movimiento.

**Salida:**
- Éxito: `movementId` retornado, lista de movimientos recargada, mensaje de éxito.
- Fallo: mensaje de error de validación o stock insuficiente.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: CreateMovement:**

1. El usuario completa el formulario de movimiento de stock.
2. El formulario ejecuta `ValidateMovement()`.
3. Si la validación falla, se muestra un mensaje de error.
4. Si la validación pasa, se llama a `StockMovementService.CreateMovement(movement, lines)`.
5. El servicio llama a `StockMovementRepository.GenerateMovementNumber()` para obtener el número de movimiento (ej.: "MOV-20260222-001").
6. El servicio ejecuta `ValidateMovement(movement, lines)` internamente.
7. Para cada línea, el servicio consulta el stock disponible con `StockRepository.GetByProductAndWarehouse(productId, warehouseId)`.
8. El servicio llama a `StockMovementRepository.Insert(movement)`, que ejecuta `INSERT INTO StockMovements ...`.
9. El repositorio retorna el nuevo `movementId`.
10. Para cada línea, el servicio llama a `StockMovementRepository.InsertLine(line)`.
11. El servicio ejecuta `ApplyStockChanges(movement, lines)`: para cada línea, llama a `StockRepository.UpdateStock(productId, warehouseId, qty, userId)`.
12. El servicio retorna el `movementId` al formulario.
13. El formulario ejecuta `LoadMovements()` para actualizar la grilla.
14. Se muestra un mensaje de éxito al usuario.

---

### UC-02: GetAllMovements (Obtener todos los movimientos)

**Introducción:**
Recupera el historial completo de movimientos de stock para su visualización en el formulario de gestión.

**Precondición:**
- El usuario debe tener sesión iniciada con permiso de lectura de movimientos.

**Entradas:**
- Ninguna.

**Proceso:**
1. El formulario llama a `StockMovementService.GetAllMovements()`.
2. El servicio consulta `StockMovementRepository.GetAll()`.
3. El repositorio ejecuta `SELECT * FROM StockMovements ORDER BY MovementDate DESC`.

**Salida:**
- Lista de objetos `StockMovement` ordenada por fecha descendente.

---

**Descripción paso a paso del diagrama de secuencia — UC-02: GetAllMovements:**

1. El formulario llama a `StockMovementService.GetAllMovements()`.
2. El servicio llama a `StockMovementRepository.GetAll()`.
3. El repositorio obtiene conexión y ejecuta `SELECT * FROM StockMovements ORDER BY MovementDate DESC`.
4. El repositorio retorna `List<StockMovement>` al servicio.
5. El servicio retorna la lista al formulario.
6. El formulario enlaza los datos al `DataGridView`.

---

### UC-03: GetMovementById (Obtener movimiento por ID)

**Introducción:**
Recupera los datos de un movimiento de stock específico para su visualización detallada.

**Precondición:**
- El movimiento con el ID especificado debe existir.

**Entradas:**
- `movementId` (entero).

**Proceso:**
1. El formulario llama a `StockMovementService.GetMovementById(movementId)`.
2. El servicio consulta `StockMovementRepository.GetById(id)`.
3. Se ejecuta `SELECT * FROM StockMovements WHERE MovementId=@Id`.

**Salida:**
- Objeto `StockMovement` con todos sus atributos, o `null` si no se encuentra.

---

**Descripción paso a paso del diagrama de secuencia — UC-03: GetMovementById:**

1. El formulario llama a `StockMovementService.GetMovementById(movementId)`.
2. El servicio llama a `StockMovementRepository.GetById(id)`.
3. El repositorio obtiene conexión y ejecuta `SELECT * FROM StockMovements WHERE MovementId=@Id`.
4. Si no se encuentra, el servicio retorna `null` y el formulario muestra "no encontrado".
5. Si se encuentra, el servicio retorna el objeto `StockMovement`.
6. El formulario rellena los campos de detalle del movimiento.

---

### UC-04: GetMovementLines (Obtener líneas de un movimiento)

**Introducción:**
Recupera las líneas de detalle de un movimiento de stock, mostrando qué productos y cantidades fueron afectados.

**Precondición:**
- El movimiento con el ID especificado debe existir.

**Entradas:**
- `movementId` (entero).

**Proceso:**
1. El formulario llama a `StockMovementService.GetMovementLines(movementId)`.
2. El servicio consulta `StockMovementRepository.GetMovementLines(movementId)`.
3. El repositorio ejecuta un JOIN entre `StockMovementLines` y `Products`.

**Salida:**
- Lista de `StockMovementLine` con nombre y SKU del producto incluidos.

---

**Descripción paso a paso del diagrama de secuencia — UC-04: GetMovementLines:**

1. El formulario llama a `StockMovementService.GetMovementLines(movementId)`.
2. El servicio llama a `StockMovementRepository.GetMovementLines(movementId)`.
3. El repositorio obtiene conexión y ejecuta `SELECT sml.*, p.Name, p.SKU FROM StockMovementLines sml JOIN Products p ON sml.ProductId=p.ProductId WHERE sml.MovementId=@Id`.
4. El repositorio retorna `List<StockMovementLine>` al servicio.
5. El servicio retorna la lista al formulario.
6. El formulario enlaza las líneas al `DataGridView`.

---

### UC-05: GetMovementsByDateRange (Filtrar movimientos por rango de fechas)

**Introducción:**
Permite filtrar el historial de movimientos de stock entre dos fechas específicas.

**Precondición:**
- Las fechas de inicio y fin deben ser válidas; la fecha de inicio debe ser anterior o igual a la de fin.

**Entradas:**
- `fromDate` (fecha de inicio).
- `toDate` (fecha de fin).

**Proceso:**
1. El usuario selecciona el rango de fechas en el formulario.
2. El formulario llama a `StockMovementService.GetMovementsByDateRange(fromDate, toDate)`.
3. El servicio consulta `StockMovementRepository.GetByDateRange(from, to)`.
4. Se ejecuta `SELECT * FROM StockMovements WHERE MovementDate BETWEEN @From AND @To`.

**Salida:**
- Lista de movimientos dentro del rango de fechas indicado.

---

**Descripción paso a paso del diagrama de secuencia — UC-05: GetMovementsByDateRange:**

1. El usuario selecciona el rango de fechas en el formulario.
2. El formulario llama a `StockMovementService.GetMovementsByDateRange(fromDate, toDate)`.
3. El servicio llama a `StockMovementRepository.GetByDateRange(from, to)`.
4. El repositorio obtiene conexión y ejecuta la consulta con `BETWEEN @From AND @To`.
5. El repositorio retorna la lista al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los resultados filtrados al `DataGridView`.

---

### UC-06: GetMovementsByType (Filtrar movimientos por tipo)

**Introducción:**
Permite filtrar el historial de movimientos de stock según su tipo (Entrada, Salida, Transferencia o Ajuste).

**Precondición:**
- Debe existir al menos un movimiento del tipo seleccionado.

**Entradas:**
- `movementType` (enumeración: In, Out, Transfer, Adjustment).

**Proceso:**
1. El usuario selecciona el tipo de movimiento en el filtro.
2. El formulario llama a `StockMovementService.GetMovementsByType(movementType)`.
3. El servicio consulta `StockMovementRepository.GetByType(type)`.
4. Se ejecuta `SELECT * FROM StockMovements WHERE MovementType=@Type`.

**Salida:**
- Lista de movimientos del tipo seleccionado.

---

**Descripción paso a paso del diagrama de secuencia — UC-06: GetMovementsByType:**

1. El usuario selecciona el tipo de movimiento en el filtro del formulario.
2. El formulario llama a `StockMovementService.GetMovementsByType(MovementType.In)` (o el tipo seleccionado).
3. El servicio llama a `StockMovementRepository.GetByType(type)`.
4. El repositorio obtiene conexión y ejecuta `SELECT * FROM StockMovements WHERE MovementType=@Type`.
5. El repositorio retorna la lista al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los resultados filtrados al `DataGridView`.

---

### UC-07: UpdateProductPrices (Actualizar precios de productos al recibir stock)

**Introducción:**
Cuando se registra un movimiento de tipo Entrada (In), actualiza automáticamente el precio unitario de los productos afectados si el precio en la línea del movimiento difiere del precio vigente.

**Precondición:**
- Se ha creado previamente un movimiento de tipo Entrada.
- Al menos una línea del movimiento tiene un precio diferente al precio actual del producto.

**Entradas:**
- `movementId` (ID del movimiento de entrada).

**Proceso:**
1. El servicio llama a `CheckPriceUpdates(movementId)`.
2. Se obtienen las líneas del movimiento con `StockMovementRepository.GetMovementLines(movementId)`.
3. Para cada línea con precio diferente al del producto, se obtiene el producto y se actualiza su precio unitario.

**Salida:**
- Precios de productos actualizados en la base de datos.

---

**Descripción paso a paso del diagrama de secuencia — UC-07: UpdateProductPrices:**

1. El servicio detecta que el movimiento es de tipo Entrada.
2. Se llama a `StockMovementService.CheckPriceUpdates(movementId)`.
3. Se llama a `StockMovementRepository.GetMovementLines(movementId)` para obtener las líneas.
4. Para cada línea que contiene un precio diferente al del producto:
   - Se obtiene el producto con `ProductRepository.GetById(productId)`.
   - Se actualiza el precio con `ProductRepository.Update(product with new UnitPrice)`.
   - El repositorio ejecuta `UPDATE Products SET UnitPrice=@Price WHERE ProductId=@Id`.
5. Una vez procesadas todas las líneas, los precios quedan actualizados.

---

### UC-08: UpdateStockForMovement (Actualizar stock según tipo de movimiento)

**Introducción:**
Actualiza los niveles de stock en los depósitos afectados por un movimiento, aplicando la lógica correspondiente al tipo de movimiento (sumar, restar o trasladar cantidades).

**Precondición:**
- El movimiento debe existir y sus líneas deben estar registradas.
- Para movimientos de salida y transferencia, debe haber stock suficiente en el depósito origen.

**Entradas:**
- `movementId` (ID del movimiento).

**Proceso:**
1. Se obtiene el movimiento con `StockMovementRepository.GetById(movementId)`.
2. Se obtienen las líneas con `StockMovementRepository.GetMovementLines(movementId)`.
3. Según el tipo:
   - **In**: incrementa el stock en el depósito destino.
   - **Out**: decrementa el stock en el depósito origen.
   - **Transfer**: decrementa en origen e incrementa en destino.
   - **Adjustment**: aplica la cantidad ajustada en el depósito correspondiente.

**Salida:**
- Stock de los depósitos afectados actualizado.

---

**Descripción paso a paso del diagrama de secuencia — UC-08: UpdateStockForMovement:**

1. Se llama a `StockMovementRepository.GetById(movementId)` para obtener el movimiento.
2. Se llama a `StockMovementRepository.GetMovementLines(movementId)` para obtener las líneas.
3. Si el tipo es **In**: para cada línea, se llama a `StockRepository.UpdateStock(productId, destWarehouseId, +qty, userId)`, ejecutando `MERGE Stock SET Quantity = Quantity + @Qty`.
4. Si el tipo es **Out**: para cada línea, se llama a `StockRepository.UpdateStock(productId, srcWarehouseId, -qty, userId)`.
5. Si el tipo es **Transfer**: para cada línea, se decrementa en el depósito origen y se incrementa en el depósito destino.
6. Si el tipo es **Adjustment**: para cada línea, se establece la cantidad ajustada en el depósito correspondiente.
7. Una vez procesadas todas las líneas, el stock queda actualizado.

---

## 4. Módulo: Ventas

### UC-01: CreateSale (Registrar venta)

**Introducción:**
Permite registrar una nueva venta en el sistema, asociada a un cliente y con sus líneas de detalle por producto. El proceso descuenta automáticamente el stock disponible de los depósitos correspondientes.

**Precondición:**
- Debe existir al menos un cliente activo y un producto activo con stock disponible.
- El usuario debe tener permiso para crear ventas.

**Entradas:**
- Datos de la venta: cliente, vendedor, fecha, notas.
- Líneas de venta: lista de productos con cantidad, precio unitario y depósito de origen.

**Proceso:**
1. El formulario valida la venta con `ValidateSale()`.
2. Se invoca `SaleService.CreateSale(sale, lines)`.
3. El servicio valida la venta y sus líneas (stock suficiente por depósito).
4. Se crea la venta con sus líneas en una operación atómica con `SaleRepository.CreateWithLines(sale, lines)`.
5. Se descuenta el stock de cada depósito con `StockRepository.UpdateStock(...)`.

**Salida:**
- Éxito: `saleId` retornado, lista de ventas recargada, mensaje de éxito.
- Fallo: mensaje de error de validación o stock insuficiente.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: CreateSale:**

1. El usuario completa el formulario de venta con los datos del cliente, vendedor y líneas de productos.
2. El formulario ejecuta `ValidateSale()`.
3. Si la validación falla, se muestra el error correspondiente.
4. Si la validación pasa, se llama a `SaleService.CreateSale(sale, lines)` (protegido por `SemaphoreSlim` para evitar condiciones de carrera).
5. El servicio ejecuta `ValidateSale(sale, lines)`: verifica que cada línea tenga stock suficiente consultando `StockRepository.GetCurrentStock(productId, warehouseId)`.
6. Si el stock es insuficiente, se lanza una excepción con el mensaje correspondiente.
7. Se llama a `SaleRepository.CreateWithLines(sale, lines)`, que ejecuta la inserción en transacción.
8. El repositorio crea el encabezado de la venta y luego todas las líneas, retornando el nuevo `saleId`.
9. El servicio llama a `DeductStock(lines, userId)`: para cada línea, ejecuta `StockRepository.UpdateStock(productId, warehouseId, -quantity, userId)`.
10. El servicio retorna el `saleId` al formulario.
11. El formulario ejecuta `LoadSales()` para actualizar la grilla.
12. Se muestra un mensaje de éxito al usuario.

---

### UC-02: DeleteSale (Eliminar venta)

**Introducción:**
Permite desactivar una venta existente (borrado lógico). La venta no se elimina físicamente, sino que se marca como inactiva.

**Precondición:**
- La venta debe existir y estar activa.
- El usuario debe tener permiso de eliminación de ventas.

**Entradas:**
- `saleId` (entero).
- `deletedBy` (ID del usuario que elimina).

**Proceso:**
1. Se muestra un diálogo de confirmación.
2. Si el usuario confirma, se invoca `SaleService.DeleteSale(saleId, currentUserId)`.
3. El servicio llama a `SaleRepository.SoftDelete(id, deletedBy)`.
4. Se ejecuta `UPDATE Sales SET IsActive=0 ...`.

**Salida:**
- Venta desactivada, lista de ventas recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-02: DeleteSale:**

1. El usuario selecciona una venta y hace clic en "Eliminar".
2. Se muestra un diálogo de confirmación.
3. Si el usuario cancela, no se realiza ninguna acción.
4. Si confirma, se llama a `SaleService.DeleteSale(saleId, currentUserId)`.
5. El servicio llama a `SaleRepository.SoftDelete(id, deletedBy)`.
6. El repositorio ejecuta `UPDATE Sales SET IsActive=0 ...`.
7. El formulario recarga la lista de ventas y muestra un mensaje de éxito.

---

### UC-03: GetAllSales / GetAllSalesWithDetails (Obtener ventas)

**Introducción:**
Recupera el listado de ventas del sistema para su visualización en el formulario de gestión.

**Precondición:**
- El usuario debe tener permiso de lectura de ventas.

**Entradas:**
- Ninguna.

**Proceso:**
1. El formulario llama a `SaleService.GetAllSalesWithDetails()`.
2. El servicio consulta `SaleRepository.GetAllWithDetails()`.
3. Se ejecuta una consulta con JOIN para incluir datos del cliente y detalles de la venta.

**Salida:**
- Lista de objetos `Sale` con datos del cliente incluidos.

---

**Descripción paso a paso del diagrama de secuencia — UC-03: GetAllSalesWithDetails:**

1. El formulario llama a `SaleService.GetAllSalesWithDetails()`.
2. El servicio llama a `SaleRepository.GetAllWithDetails()`.
3. El repositorio obtiene conexión y ejecuta un `SELECT` con JOIN entre `Sales` y `Clients`.
4. El repositorio retorna `List<Sale>` al servicio.
5. El servicio retorna la lista al formulario.
6. El formulario enlaza los datos al `DataGridView`.

---

## 5. Módulo: Depósitos (Warehouses)

### UC-01: CreateWarehouse (Crear depósito)

**Introducción:**
Permite registrar un nuevo depósito en el sistema con su código único, nombre y dirección.

**Precondición:**
- El usuario debe tener permiso de creación de depósitos.
- El código del depósito no debe existir previamente.

**Entradas:**
- Código del depósito (único).
- Nombre.
- Dirección (opcional).

**Proceso:**
1. El formulario valida los campos obligatorios.
2. Se invoca `WarehouseService.CreateWarehouse(warehouse)`.
3. El servicio ejecuta `ValidateWarehouse(warehouse)`.
4. Se verifica unicidad del código con `WarehouseRepository.CodeExists(code, 0)`.
5. Se inserta el depósito con `WarehouseRepository.Insert(warehouse)`.

**Salida:**
- Nuevo `warehouseId` retornado, lista de depósitos recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: CreateWarehouse:**

1. El usuario completa el formulario de depósito.
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un error.
4. Si pasa, se llama a `WarehouseService.CreateWarehouse(warehouse)`.
5. El servicio ejecuta `ValidateWarehouse(warehouse)`.
6. Se consulta `WarehouseRepository.CodeExists(code, 0)`.
7. El repositorio verifica en la base de datos que el código no exista.
8. Si es único, se llama a `WarehouseRepository.Insert(warehouse)`, que ejecuta `INSERT INTO Warehouses (Code, Name, Address, ...) VALUES (...)`.
9. El repositorio retorna el nuevo `warehouseId`.
10. El servicio retorna el `warehouseId` al formulario.
11. El formulario ejecuta `LoadWarehouses()` y muestra mensaje de éxito.

---

### UC-02: DeleteWarehouse (Eliminar depósito)

**Introducción:**
Permite desactivar un depósito (borrado lógico), marcándolo como inactivo sin eliminar su historial.

**Precondición:**
- El depósito debe existir y estar activo.

**Entradas:**
- `warehouseId`, `deletedBy`.

**Proceso:**
1. Diálogo de confirmación.
2. Si confirma: `WarehouseService.DeleteWarehouse(warehouseId, currentUserId)`.
3. `WarehouseRepository.SoftDelete(id, deletedBy)` ejecuta `UPDATE Warehouses SET IsActive=0`.

**Salida:**
- Depósito desactivado, lista recargada, mensaje de éxito.

---

### UC-03: GetActiveWarehouses (Obtener depósitos activos)

**Introducción:**
Recupera todos los depósitos activos para ser usados en selecciones de formularios o gestión.

**Entradas:**
- Ninguna.

**Proceso:**
1. `WarehouseService.GetActiveWarehouses()` → `WarehouseRepository.GetAllActive()`.
2. Ejecuta `SELECT * FROM Warehouses WHERE IsActive=1 ORDER BY Name`.

**Salida:**
- Lista de depósitos activos.

---

### UC-04: GetAllWarehouses (Obtener todos los depósitos)

**Introducción:**
Recupera todos los depósitos del sistema, incluyendo los inactivos.

**Entradas:**
- Ninguna.

**Proceso:**
1. `WarehouseService.GetAllWarehouses()` → `WarehouseRepository.GetAll()`.
2. Ejecuta `SELECT * FROM Warehouses ORDER BY Name`.

**Salida:**
- Lista completa de depósitos (activos e inactivos).

---

### UC-05: GetWarehouseById (Obtener depósito por ID)

**Introducción:**
Recupera los datos de un depósito específico para visualizarlo en el formulario de edición.

**Entradas:**
- `warehouseId`.

**Proceso:**
1. `WarehouseService.GetWarehouseById(warehouseId)` → `WarehouseRepository.GetById(id)`.
2. Ejecuta `SELECT * FROM Warehouses WHERE WarehouseId=@Id`.

**Salida:**
- Objeto `Warehouse` o `null` si no existe.

---

### UC-06: UpdateWarehouse (Actualizar depósito)

**Introducción:**
Permite modificar el código, nombre o dirección de un depósito existente.

**Precondición:**
- El depósito debe existir; el nuevo código no debe estar en uso por otro depósito.

**Entradas:**
- Objeto `Warehouse` con datos actualizados.

**Proceso:**
1. Validación del formulario.
2. `WarehouseService.UpdateWarehouse(warehouse)`.
3. Verificación de unicidad del código (excluyendo el depósito actual).
4. `WarehouseRepository.Update(warehouse)` ejecuta `UPDATE Warehouses SET Code=@Code, ...`.

**Salida:**
- Depósito actualizado, lista recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-06: UpdateWarehouse:**

1. El usuario modifica los campos del depósito y hace clic en "Guardar".
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un error.
4. Si pasa, se llama a `WarehouseService.UpdateWarehouse(warehouse)`.
5. El servicio ejecuta `ValidateWarehouse(warehouse)`.
6. Se consulta `WarehouseRepository.CodeExists(code, warehouseId)` para verificar unicidad (excluyendo el depósito actual).
7. Si el código es único, se llama a `WarehouseRepository.Update(warehouse)`.
8. El repositorio ejecuta `UPDATE Warehouses SET Code=@Code, Name=@Name, Address=@Addr, UpdatedAt=@Now WHERE WarehouseId=@Id`.
9. El formulario ejecuta `LoadWarehouses()` y muestra mensaje de éxito.

---

## 6. Módulo: Clientes

### UC-01: CreateClient (Crear cliente)

**Introducción:**
Permite registrar un nuevo cliente en el sistema con sus datos personales y de contacto.

**Precondición:**
- El usuario debe tener permiso de creación de clientes.
- El DNI no debe estar registrado previamente.

**Entradas:**
- Nombre, apellido, DNI (único), correo electrónico, teléfono, dirección.

**Proceso:**
1. Validación del formulario.
2. `ClientService.CreateClient(client)`.
3. Verificación de unicidad del DNI con `ClientRepository.DNIExists(dni, 0)`.
4. Inserción con `ClientRepository.Insert(client)`.

**Salida:**
- Nuevo `clientId` retornado, lista de clientes recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: CreateClient:**

1. El usuario completa el formulario de cliente.
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un error.
4. Si pasa, se llama a `ClientService.CreateClient(client)`.
5. El servicio ejecuta `ValidateClient(client)`.
6. Se consulta `ClientRepository.DNIExists(dni, 0)` para verificar unicidad del DNI.
7. Si el DNI es único, se llama a `ClientRepository.Insert(client)`, que ejecuta `INSERT INTO Clients (Nombre, Apellido, DNI, Correo, ...) VALUES (...)`.
8. El repositorio retorna el nuevo `clientId`.
9. El servicio retorna el `clientId` al formulario.
10. El formulario ejecuta `LoadClients()` y muestra mensaje de éxito.

---

### UC-02: DeleteClient (Eliminar cliente)

**Introducción:**
Desactiva un cliente del sistema (borrado lógico).

**Precondición:**
- El cliente debe existir y estar activo.

**Entradas:**
- `clientId`, `deletedBy`.

**Proceso:**
1. Diálogo de confirmación.
2. `ClientService.DeleteClient(clientId, currentUserId)`.
3. `ClientRepository.SoftDelete(id, deletedBy)` ejecuta `UPDATE Clients SET IsActive=0, UpdatedAt=@Now, UpdatedBy=@UserId WHERE ClientId=@Id`.

**Salida:**
- Cliente desactivado, lista recargada, mensaje de éxito.

---

### UC-03: GetActiveClients (Obtener clientes activos)

**Introducción:**
Recupera la lista de clientes activos.

**Entradas:**
- Ninguna.

**Proceso:**
1. `ClientService.GetActiveClients()` → `ClientRepository.GetAllActive()`.
2. Ejecuta `SELECT * FROM Clients WHERE IsActive=1 ORDER BY Apellido, Nombre`.

**Salida:**
- Lista de clientes activos.

---

### UC-04: GetAllClients (Obtener todos los clientes)

**Introducción:**
Recupera todos los clientes, incluyendo inactivos.

**Entradas:**
- Ninguna.

**Proceso:**
1. `ClientService.GetAllClients()` → `ClientRepository.GetAll()`.
2. Ejecuta `SELECT * FROM Clients ORDER BY Apellido, Nombre`.

**Salida:**
- Lista completa de clientes.

---

### UC-05: GetClientById (Obtener cliente por ID)

**Introducción:**
Recupera los datos de un cliente específico por su ID.

**Entradas:**
- `clientId`.

**Proceso:**
1. `ClientService.GetClientById(clientId)` → `ClientRepository.GetById(id)`.
2. Ejecuta `SELECT * FROM Clients WHERE ClientId=@Id`.

**Salida:**
- Objeto `Client` o `null`.

---

### UC-06: UpdateClient (Actualizar cliente)

**Introducción:**
Permite modificar los datos de un cliente existente.

**Precondición:**
- El cliente debe existir; el DNI modificado no debe pertenecer a otro cliente.

**Entradas:**
- Objeto `Client` con datos actualizados.

**Proceso:**
1. Validación del formulario.
2. `ClientService.UpdateClient(client)`.
3. Verificación de unicidad del DNI (excluyendo el cliente actual).
4. `ClientRepository.Update(client)` ejecuta `UPDATE Clients SET Nombre=@Nombre, ... WHERE ClientId=@Id`.

**Salida:**
- Cliente actualizado, lista recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-06: UpdateClient:**

1. El usuario modifica los datos del cliente en el formulario.
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un error.
4. Si pasa, se llama a `ClientService.UpdateClient(client)`.
5. El servicio ejecuta `ValidateClient(client)`.
6. Se consulta `ClientRepository.DNIExists(dni, clientId)` para verificar unicidad (excluyendo el cliente actual).
7. Si el DNI es único, se llama a `ClientRepository.Update(client)`.
8. El repositorio ejecuta `UPDATE Clients SET Nombre=@Nombre, Apellido=@Ap, DNI=@DNI, ... WHERE ClientId=@Id`.
9. El formulario ejecuta `LoadClients()` y muestra mensaje de éxito.

---

## 7. Módulo: Reportes

### UC-01: GetCategorySalesReport (Reporte de ventas por categoría)

**Introducción:**
Genera un reporte de ventas agrupado por categoría de producto para un período determinado, mostrando cantidades vendidas, ingresos y número de ventas por categoría.

**Precondición:**
- El usuario debe tener permiso de acceso a reportes.
- Deben existir ventas en el período seleccionado.

**Entradas:**
- `fromDate`, `toDate` (rango de fechas).

**Proceso:**
1. El usuario selecciona el rango de fechas.
2. `ReportService.GetCategorySalesReport(fromDate, toDate)`.
3. El repositorio ejecuta un `SELECT` con `GROUP BY p.Category` sobre ventas y líneas de venta.

**Salida:**
- Lista de `CategorySalesReportDTO` enlazada al `DataGridView` y gráfico.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: GetCategorySalesReport:**

1. El usuario selecciona el rango de fechas en el formulario de reportes.
2. El formulario llama a `ReportService.GetCategorySalesReport(fromDate, toDate)`.
3. El servicio llama a `ReportRepository.GetCategorySalesReport(from, to)`.
4. El repositorio obtiene conexión y ejecuta `SELECT p.Category, SUM(sl.Quantity) AS TotalQty, SUM(sl.LineTotal) AS Revenue FROM SaleLines sl JOIN Products p ... WHERE s.SaleDate BETWEEN @From AND @To GROUP BY p.Category`.
5. El repositorio retorna `List<CategorySalesReportDTO>` al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView` y renderiza el gráfico.

---

### UC-02: GetClientProductRankingReport (Ranking de productos por cliente)

**Introducción:**
Genera un ranking de los productos más comprados por un cliente específico, ordenados por cantidad total adquirida.

**Precondición:**
- El cliente seleccionado debe tener historial de compras.

**Entradas:**
- `clientId` (ID del cliente).

**Proceso:**
1. El usuario selecciona el cliente.
2. `ReportService.GetClientProductRankingReport(clientId)`.
3. El repositorio ejecuta un `SELECT` con `RANK() OVER (ORDER BY SUM(sl.Quantity) DESC)`.

**Salida:**
- Lista de `ClientProductRankingReportDTO` con posición de ranking.

---

**Descripción paso a paso del diagrama de secuencia — UC-02: GetClientProductRankingReport:**

1. El usuario selecciona un cliente en el formulario de reportes.
2. El formulario llama a `ReportService.GetClientProductRankingReport(clientId)`.
3. El servicio llama a `ReportRepository.GetClientProductRankingReport(clientId)`.
4. El repositorio ejecuta un `SELECT` con `RANK()` calculando el ranking por cantidad total comprada.
5. El repositorio retorna `List<ClientProductRankingReportDTO>` al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView` con indicador de posición.

---

### UC-03: GetClientPurchasesReport (Reporte de compras por cliente)

**Introducción:**
Muestra el historial de compras de un cliente específico en un período determinado.

**Precondición:**
- El cliente seleccionado debe existir y haber realizado al menos una compra en el período.

**Entradas:**
- `clientId`, `fromDate`, `toDate`.

**Proceso:**
1. El usuario selecciona cliente y rango de fechas.
2. `ReportService.GetClientPurchasesReport(clientId, fromDate, toDate)`.
3. El repositorio ejecuta un `SELECT` con JOIN entre `Sales`, `Clients` y `SaleLines`.

**Salida:**
- Lista de `ClientPurchasesReportDTO` con número de venta, fecha, monto y vendedor.

---

**Descripción paso a paso del diagrama de secuencia — UC-03: GetClientPurchasesReport:**

1. El usuario selecciona un cliente y el rango de fechas.
2. El formulario llama a `ReportService.GetClientPurchasesReport(clientId, fromDate, toDate)`.
3. El servicio llama a `ReportRepository.GetClientPurchasesReport(clientId, from, to)`.
4. El repositorio ejecuta el `SELECT` con `JOIN` entre tablas y filtros por cliente y fecha.
5. El repositorio retorna `List<ClientPurchasesReportDTO>` al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView`.

---

### UC-04: GetPriceVariationReport (Reporte de variación de precios)

**Introducción:**
Muestra el historial de cambios de precio de un producto en un período determinado, calculando la variación absoluta y porcentual.

**Precondición:**
- Debe existir al menos un cambio de precio registrado en el log de auditoría para el producto y período seleccionados.

**Entradas:**
- `productId`, `fromDate`, `toDate`.

**Proceso:**
1. El usuario selecciona producto y rango de fechas.
2. `ReportService.GetPriceVariationReport(productId, fromDate, toDate)`.
3. El repositorio consulta el `AuditLog` para cambios en el campo `UnitPrice` de la tabla `Products`.

**Salida:**
- Lista de `PriceVariationReportDTO` con precio anterior, nuevo y porcentaje de variación.

---

**Descripción paso a paso del diagrama de secuencia — UC-04: GetPriceVariationReport:**

1. El usuario selecciona un producto y el rango de fechas.
2. El formulario llama a `ReportService.GetPriceVariationReport(productId, fromDate, toDate)`.
3. El servicio llama a `ReportRepository.GetPriceVariationReport(productId, from, to)`.
4. El repositorio ejecuta `SELECT p.Name, p.SKU, al.OldValue AS OldPrice, al.NewValue AS NewPrice, al.ChangedAt FROM AuditLog al JOIN Products p ... WHERE al.TableName='Products' AND al.FieldName='UnitPrice' AND al.ChangedAt BETWEEN @From AND @To`.
5. El repositorio retorna la lista al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView` y renderiza el gráfico de tendencia de precios.

---

### UC-05: GetRevenueByDateReport (Reporte de ingresos por fecha)

**Introducción:**
Genera un reporte de ingresos agrupados por día, semana o mes en un período determinado.

**Precondición:**
- Deben existir ventas activas en el período seleccionado.

**Entradas:**
- `fromDate`, `toDate`, `groupBy` (Day / Week / Month).

**Proceso:**
1. El usuario selecciona el rango y agrupación.
2. `ReportService.GetRevenueByDateReport(from, to, groupBy)`.
3. El repositorio ejecuta un `SELECT` con `GROUP BY CAST(SaleDate AS DATE)`.

**Salida:**
- Lista de `RevenueByDateReportDTO` con fecha/período, ingresos totales y cantidad de ventas.

---

**Descripción paso a paso del diagrama de secuencia — UC-05: GetRevenueByDateReport:**

1. El usuario selecciona el rango de fechas y el tipo de agrupación (Día/Semana/Mes).
2. El formulario llama a `ReportService.GetRevenueByDateReport(from, to, groupBy)`.
3. El servicio llama a `ReportRepository.GetRevenueByDateReport(from, to, groupBy)`.
4. El repositorio ejecuta `SELECT CAST(SaleDate AS DATE) AS PeriodDate, SUM(TotalAmount) AS Revenue, COUNT(*) AS TotalSales FROM Sales WHERE SaleDate BETWEEN @From AND @To AND IsActive=1 GROUP BY CAST(SaleDate AS DATE) ORDER BY PeriodDate`.
5. El repositorio retorna `List<RevenueByDateReportDTO>` al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView` y renderiza el gráfico de tendencia de ingresos.

---

### UC-06: GetSellerPerformanceReport (Reporte de desempeño de vendedores)

**Introducción:**
Genera un reporte comparativo del desempeño de los vendedores en un período determinado, mostrando número de ventas, ingresos, ticket promedio y clientes únicos atendidos.

**Precondición:**
- Deben existir ventas activas en el período seleccionado.

**Entradas:**
- `fromDate`, `toDate`.

**Proceso:**
1. El usuario selecciona el rango de fechas.
2. `ReportService.GetSellerPerformanceReport(fromDate, toDate)`.
3. El repositorio ejecuta un `SELECT` con `GROUP BY SellerName ORDER BY Revenue DESC`.

**Salida:**
- Lista de `SellerPerformanceReportDTO` ordenada por ingresos descendentes.

---

**Descripción paso a paso del diagrama de secuencia — UC-06: GetSellerPerformanceReport:**

1. El usuario selecciona el rango de fechas.
2. El formulario llama a `ReportService.GetSellerPerformanceReport(fromDate, toDate)`.
3. El servicio llama a `ReportRepository.GetSellerPerformanceReport(from, to)`.
4. El repositorio ejecuta `SELECT SellerName, COUNT(*) AS TotalSales, SUM(TotalAmount) AS Revenue, AVG(TotalAmount) AS AvgSale, COUNT(DISTINCT ClientId) AS UniqueClients FROM Sales WHERE SaleDate BETWEEN @From AND @To AND IsActive=1 GROUP BY SellerName ORDER BY Revenue DESC`.
5. El repositorio retorna `List<SellerPerformanceReportDTO>` al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView` con indicador de ranking.

---

### UC-07: GetTopProductsReport (Reporte de productos más vendidos)

**Introducción:**
Genera un ranking de los N productos más vendidos en un período determinado, por cantidad vendida.

**Precondición:**
- Deben existir ventas activas con líneas de detalle en el período seleccionado.

**Entradas:**
- `top` (cantidad de productos a mostrar, ej.: 10).
- `fromDate`, `toDate`.

**Proceso:**
1. El usuario selecciona los parámetros.
2. `ReportService.GetTopProductsReport(top, fromDate, toDate)`.
3. El repositorio ejecuta `SELECT TOP @Top ... ORDER BY TotalQtySold DESC`.

**Salida:**
- Lista de `TopProductsReportDTO` con nombre, SKU, categoría, cantidad vendida e ingresos, con ranking.

---

**Descripción paso a paso del diagrama de secuencia — UC-07: GetTopProductsReport:**

1. El usuario selecciona el número de productos (Top N) y el rango de fechas.
2. El formulario llama a `ReportService.GetTopProductsReport(top, fromDate, toDate)`.
3. El servicio llama a `ReportRepository.GetTopProductsReport(top, from, to)`.
4. El repositorio ejecuta `SELECT TOP @Top p.ProductId, p.Name, p.SKU, p.Category, SUM(sl.Quantity) AS TotalQtySold, SUM(sl.LineTotal) AS TotalRevenue, COUNT(DISTINCT sl.SaleId) AS SalesCount FROM SaleLines sl JOIN Products p ... WHERE s.SaleDate BETWEEN @From AND @To GROUP BY ... ORDER BY TotalQtySold DESC`.
5. El repositorio retorna `List<TopProductsReportDTO>` al servicio.
6. El servicio retorna la lista al formulario.
7. El formulario enlaza los datos al `DataGridView` y renderiza el gráfico de barras.

---

## 8. Módulo: Usuarios

### UC-01: AssignRolesToUser (Asignar roles a un usuario)

**Introducción:**
Permite asignar o reasignar un conjunto de roles a un usuario del sistema. La operación reemplaza atómicamente todos los roles anteriores por los nuevos seleccionados.

**Precondición:**
- El usuario objetivo debe existir y estar activo.
- El operador debe tener permiso de gestión de roles.

**Entradas:**
- `userId` (ID del usuario objetivo).
- `selectedRoleIds` (lista de IDs de roles a asignar).

**Proceso:**
1. El operador selecciona los roles en el formulario.
2. `UserService.AssignRoles(userId, selectedRoleIds)`.
3. `UserRepository.AssignRoles(userId, roleIds)` ejecuta en transacción: elimina todos los roles actuales y luego inserta los nuevos.

**Salida:**
- Roles actualizados, lista de roles del usuario recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: AssignRolesToUser:**

1. El administrador abre el formulario de roles de usuario.
2. El administrador selecciona los roles a asignar y hace clic en "Guardar".
3. El formulario llama a `UserService.AssignRoles(userId, selectedRoleIds)`.
4. El servicio llama a `UserRepository.AssignRoles(userId, roleIds)`.
5. El repositorio obtiene una conexión y abre una transacción.
6. Se ejecuta `DELETE FROM UserRoles WHERE UserId=@UserId` para eliminar todos los roles anteriores.
7. Para cada `roleId` seleccionado, se ejecuta `INSERT INTO UserRoles (UserId, RoleId, AssignedAt) VALUES (...)`.
8. Se confirma la transacción (`COMMIT`).
9. El repositorio retorna `void` al servicio.
10. El servicio retorna `void` al formulario.
11. El formulario ejecuta `LoadUserRoles(userId)` para actualizar la vista.
12. Se muestra un mensaje de éxito.

---

### UC-02: ChangePassword (Cambiar contraseña de usuario)

**Introducción:**
Permite a un usuario cambiar su contraseña actual, previa verificación de la contraseña vigente.

**Precondición:**
- El usuario debe conocer su contraseña actual.

**Entradas:**
- Contraseña actual (`currentPassword`).
- Nueva contraseña (`newPassword`).
- Confirmación de la nueva contraseña.

**Proceso:**
1. El formulario valida que las contraseñas nuevas coincidan.
2. Se verifica la contraseña actual con `AuthenticationService.VerifyPassword(...)`.
3. Se genera un nuevo hash+salt con `AuthenticationService.HashPassword(newPassword, out salt)`.
4. Se actualiza el usuario con `UserService.UpdateUser(user with new hash/salt)`.

**Salida:**
- Contraseña actualizada, mensaje de éxito.

---

### UC-03: CreateUser (Crear usuario)

**Introducción:**
Permite al administrador crear un nuevo usuario del sistema con sus datos personales, credenciales y configuración inicial.

**Precondición:**
- El nombre de usuario y el correo electrónico no deben estar en uso.
- El administrador debe tener permiso de creación de usuarios.

**Entradas:**
- Nombre de usuario (único), nombre completo, correo electrónico (único), contraseña inicial.

**Proceso:**
1. Validación del formulario.
2. `UserService.CreateUser(user, password)`.
3. Verificación de unicidad de usuario y email.
4. Hash de la contraseña con PBKDF2.
5. Inserción en transacción con auditoría.

**Salida:**
- Nuevo `userId` retornado, lista de usuarios recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-03: CreateUser:**

1. El administrador completa el formulario de nuevo usuario (nombre de usuario, nombre completo, email, contraseña).
2. El formulario ejecuta `ValidateForm()`.
3. Si la validación falla, se muestran los errores correspondientes.
4. Si pasa, se llama a `UserService.CreateUser(user, password)`.
5. El servicio ejecuta `ValidateUser(user)` y `ValidatePassword(password)`.
6. Se consulta `UserRepository.GetByUsername(username)` para verificar unicidad.
7. Si el usuario ya existe, se muestra un error de duplicado.
8. Si es único, se verifica unicidad del email con `UserRepository.GetByEmail(email)`.
9. Si el email ya existe, se muestra un error.
10. Si el email es único, se genera el hash con `AuthenticationService.HashPassword(password, out salt)`.
11. Se obtiene el usuario actual de `SessionContext`.
12. Se llama a `UserRepository.Insert(user)`, que ejecuta `INSERT INTO Users (...)` en transacción.
13. Se registra en el log de auditoría con `AuditLogRepository.LogChange(...)`.
14. El servicio retorna el `userId` al formulario.
15. El formulario actualiza la grilla y muestra mensaje de éxito.

---

### UC-04: DeleteUser (Eliminar usuario)

**Introducción:**
Permite desactivar un usuario del sistema (borrado lógico). No se puede eliminar la propia cuenta activa.

**Precondición:**
- El usuario objetivo debe existir y estar activo.
- El administrador no puede eliminar su propia cuenta.

**Entradas:**
- `userId` (ID del usuario a desactivar).

**Proceso:**
1. Diálogo de confirmación; se verifica que no sea la propia cuenta.
2. `UserService.DeleteUser(userId)`.
3. `UserRepository.SoftDelete(id, deletedBy)` ejecuta `UPDATE Users SET IsActive=0`.

**Salida:**
- Usuario desactivado, lista recargada, mensaje de éxito.

---

### UC-05: GetAllUsers (Obtener todos los usuarios)

**Introducción:**
Recupera la lista completa de usuarios del sistema para su gestión.

**Entradas:**
- Ninguna.

**Proceso:**
1. `UserService.GetAllUsers()` → `UserRepository.GetAll()`.
2. Ejecuta `SELECT * FROM Users ORDER BY Username`.

**Salida:**
- Lista de usuarios enlazada al `DataGridView`.

---

### UC-06: GetActiveUsers (Obtener usuarios activos)

**Introducción:**
Recupera solo los usuarios activos del sistema.

**Entradas:**
- Ninguna.

**Proceso:**
1. `UserService.GetActiveUsers()` → `UserRepository.GetAllActive()`.
2. Ejecuta `SELECT * FROM Users WHERE IsActive=1 ORDER BY FullName`.

**Salida:**
- Lista de usuarios activos.

---

### UC-07: GetUserById (Obtener usuario por ID)

**Introducción:**
Recupera los datos de un usuario específico por su ID.

**Entradas:**
- `userId`.

**Proceso:**
1. `UserService.GetUserById(userId)` → `UserRepository.GetById(id)`.
2. Ejecuta `SELECT * FROM Users WHERE UserId=@Id`.

**Salida:**
- Objeto `User` o `null`.

---

### UC-08: GetUserRoles (Obtener roles de un usuario)

**Introducción:**
Recupera los roles asignados a un usuario específico.

**Entradas:**
- `userId`.

**Proceso:**
1. `UserService.GetUserRoles(userId)` → `UserRepository.GetUserRoles(userId)`.
2. Ejecuta `SELECT r.* FROM Roles r JOIN UserRoles ur ON r.RoleId=ur.RoleId WHERE ur.UserId=@UserId AND r.IsActive=1`.

**Salida:**
- Lista de objetos `Role` asignados al usuario.

---

## 9. Módulo: Roles

### UC-01: CreateRole (Crear rol)

**Introducción:**
Permite registrar un nuevo rol en el sistema, al cual se le podrán asignar permisos posteriormente.

**Precondición:**
- El nombre del rol no debe existir previamente.

**Entradas:**
- Nombre del rol (único), descripción.

**Proceso:**
1. Validación del formulario.
2. `RoleService.CreateRole(role)`.
3. Verificación de unicidad del nombre con `RoleRepository.GetByName(name)`.
4. Inserción con `RoleRepository.Insert(role)`.

**Salida:**
- Nuevo `roleId` retornado, lista de roles recargada, mensaje de éxito.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: CreateRole:**

1. El administrador completa el formulario de nuevo rol.
2. El formulario ejecuta `ValidateInputs()`.
3. Si la validación falla, se muestra un error.
4. Si pasa, se llama a `RoleService.CreateRole(role)`.
5. El servicio ejecuta `ValidateRole(role)`.
6. Se consulta `RoleRepository.GetByName(name)` para verificar unicidad.
7. Si el nombre es único, se llama a `RoleRepository.Insert(role)`, que ejecuta `INSERT INTO Roles (RoleName, Description, ...) VALUES (...)`.
8. El repositorio retorna el nuevo `roleId`.
9. El formulario ejecuta `LoadRoles()` y muestra mensaje de éxito.

---

### UC-02: DeleteRole (Eliminar rol)

**Introducción:**
Desactiva un rol del sistema (borrado lógico).

**Entradas:**
- `roleId`, `deletedBy`.

**Proceso:**
1. Diálogo de confirmación.
2. `RoleService.DeleteRole(roleId, currentUserId)`.
3. `RoleRepository.SoftDelete(id, deletedBy)` ejecuta `UPDATE Roles SET IsActive=0`.

**Salida:**
- Rol desactivado, lista recargada, mensaje de éxito.

---

### UC-03: GetAllRoles / GetActiveRoles (Obtener roles)

**Introducción:**
Recupera todos los roles o solo los activos según el contexto de uso.

**Proceso:**
1. `RoleService.GetAllRoles()` / `RoleService.GetActiveRoles()`.
2. `RoleRepository.GetAll()` / `RoleRepository.GetAllActive()`.

**Salida:**
- Lista de roles.

---

### UC-04: GetRolePermissions (Obtener permisos de un rol)

**Introducción:**
Recupera los permisos asignados a un rol específico para su gestión o verificación.

**Entradas:**
- `roleId`.

**Proceso:**
1. `RoleService.GetRolePermissions(roleId)` → `RoleRepository.GetRolePermissions(roleId)`.
2. Ejecuta `SELECT p.* FROM Permissions p JOIN RolePermissions rp ON p.PermissionId=rp.PermissionId WHERE rp.RoleId=@RoleId`.

**Salida:**
- Lista de objetos `Permission` asignados al rol.

---

### UC-05: AssignPermission / RemovePermission (Gestionar permisos de un rol)

**Introducción:**
Permite agregar o quitar permisos específicos de un rol.

**Entradas:**
- `roleId`, `permissionId`, `assignedBy` (para asignar).

**Proceso:**
- **Asignar**: `RoleService.AssignPermission(roleId, permissionId, assignedBy)` → `INSERT INTO RolePermissions`.
- **Quitar**: `RoleService.RemovePermission(roleId, permissionId)` → `DELETE FROM RolePermissions`.

**Salida:**
- Permiso asignado o removido del rol, lista actualizada.

---

## 10. Módulo: Permisos y Autorización

### UC-01: GetUserPermissions (Obtener permisos de un usuario)

**Introducción:**
Recupera la lista de códigos de permisos efectivos de un usuario, calculados a través de los roles que tiene asignados.

**Precondición:**
- El usuario debe existir y tener al menos un rol con permisos activos.

**Entradas:**
- `userId`.

**Proceso:**
1. `AuthorizationService.GetUserPermissions(userId)` → `PermissionRepository.GetUserPermissions(userId)`.
2. El repositorio ejecuta un `SELECT` a través de `UserRoles` y `RolePermissions` para obtener todos los códigos de permiso activos del usuario.

**Salida:**
- Lista de cadenas con los códigos de permisos del usuario.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: GetUserPermissions:**

1. Un componente del sistema (formulario o servicio) llama a `AuthorizationService.GetUserPermissions(userId)`.
2. El servicio llama a `PermissionRepository.GetUserPermissions(userId)`.
3. El repositorio obtiene conexión y ejecuta un `SELECT` que une `Users`, `UserRoles`, `RolePermissions` y `Permissions`.
4. El repositorio retorna la lista de códigos de permisos activos.
5. El servicio retorna la lista al componente solicitante.
6. El componente utiliza la lista para habilitar o deshabilitar funcionalidades de la interfaz.

---

### UC-02: HasPermission (Verificar permiso)

**Introducción:**
Verifica si un usuario tiene un permiso específico, utilizado para controlar el acceso a funcionalidades del sistema.

**Entradas:**
- `userId`, `permissionCode` (código del permiso a verificar).

**Proceso:**
1. `AuthorizationService.HasPermission(userId, permissionCode)`.
2. Obtiene la lista de permisos del usuario y verifica si el código está incluido.

**Salida:**
- `true` si el usuario tiene el permiso; `false` en caso contrario.

---

### UC-03: HasAnyPermission / HasAllPermissions

**Introducción:**
Variantes de verificación de permisos: `HasAnyPermission` verifica si el usuario tiene al menos uno de los permisos indicados; `HasAllPermissions` verifica si tiene todos.

**Entradas:**
- `userId`, lista de `permissionCodes`.

**Proceso:**
1. Se obtienen los permisos del usuario.
2. Se evalúa la condición (ANY o ALL) sobre la lista proporcionada.

**Salida:**
- `bool` indicando si se cumple la condición.

---

## 11. Módulo: Localización

### UC-01: LoadAllTranslations (Cargar todas las traducciones)

**Introducción:**
Carga todas las traducciones disponibles (en inglés y español) desde archivos JSON al iniciar la aplicación, para que estén disponibles sin necesidad de leer archivos durante la ejecución normal.

**Precondición:**
- Los archivos de traducción (`en.json`, `es.json`) deben existir en la carpeta `translations/`.

**Entradas:**
- Ninguna (proceso automático al arrancar la aplicación).

**Proceso:**
1. La aplicación llama a `LocalizationService.LoadAllTranslations()`.
2. Para cada idioma disponible, se verifica que el archivo exista y se lee su contenido JSON.
3. Se deserializa y almacena en el diccionario interno de traducciones.

**Salida:**
- Diccionario de traducciones cargado en memoria, listo para su uso.

---

**Descripción paso a paso del diagrama de secuencia — UC-01: LoadAllTranslations:**

1. La aplicación inicia y llama a `LocalizationService.LoadAllTranslations()`.
2. El servicio verifica si existe `translations/en.json`.
3. Si existe, lo lee con `File.ReadAllText("translations/en.json")`.
4. Se deserializa el contenido JSON y se almacena en `_translations["en"]`.
5. El servicio verifica si existe `translations/es.json`.
6. Si existe, lo lee con `File.ReadAllText("translations/es.json")`.
7. Se deserializa y se almacena en `_translations["es"]`.
8. El servicio retorna `void` a la aplicación; todas las traducciones están disponibles en memoria.

---

### UC-02: SetLanguage (Cambiar idioma)

**Introducción:**
Permite al usuario cambiar el idioma de la interfaz en tiempo de ejecución, actualizando todos los textos visibles de la aplicación.

**Precondición:**
- El idioma solicitado debe estar disponible en el sistema.

**Entradas:**
- `languageCode` (ej.: "es", "en").

**Proceso:**
1. `LocalizationService.SetLanguage(languageCode)`.
2. Se actualiza el idioma activo.
3. Se publica el evento `LanguageChanged`.
4. Todos los formularios suscritos llaman a `ApplyLocalization()` para actualizar sus textos.

**Salida:**
- Interfaz mostrada en el nuevo idioma.

---

### UC-03: GetString (Obtener texto localizado)

**Introducción:**
Recupera la traducción de una clave específica en el idioma activo de la aplicación.

**Entradas:**
- `key` (clave de traducción, ej.: "Common.Login").

**Proceso:**
1. `LocalizationService.GetString(key)`.
2. Se busca la clave en el diccionario del idioma activo.
3. Si no se encuentra, se retorna la clave como fallback.

**Salida:**
- Cadena de texto traducida al idioma activo.

---

*Fin del documento.*
