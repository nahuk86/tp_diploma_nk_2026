# Guía de Activación: Transferencias y Entradas de Stock

## Problema Resuelto

**Antes**: Los usuarios no podían realizar transferencias de materiales entre almacenes ni hacer entradas de stock.

**Después**: Los usuarios con rol WarehouseOperator ahora pueden realizar todas las operaciones de stock (entradas, salidas, transferencias y ajustes).

## Cambios Realizados

### 1. Permisos Actualizados en la Base de Datos

Se agregó el permiso `Stock.Adjust` al rol **WarehouseOperator** en el archivo de datos semilla.

**Permisos completos del WarehouseOperator:**
- ✅ `Stock.View` - Ver inventario
- ✅ `Stock.Receive` - Recibir stock (entradas)
- ✅ `Stock.Issue` - Emitir stock (salidas)
- ✅ `Stock.Transfer` - Transferir entre almacenes
- ✅ `Stock.Adjust` - Ajustar inventario

### 2. Menú Principal Actualizado

El menú "Movimientos" ahora se habilita si el usuario tiene **cualquier** permiso de operación de stock, no solo `Stock.View`.

### 3. Validación de Acceso Mejorada

La verificación de permisos al abrir el formulario ahora acepta cualquier permiso de operación, permitiendo que los usuarios accedan a las funciones que les corresponden.

## Instrucciones para Aplicar los Cambios

### Paso 1: Actualizar la Base de Datos

Si ya tiene una base de datos existente, ejecute el siguiente script SQL para agregar el permiso faltante:

```sql
USE StockManagerDB;
GO

-- Agregar permiso Stock.Adjust al rol WarehouseOperator
DECLARE @OperatorRoleId INT;
DECLARE @AdjustPermissionId INT;

SELECT @OperatorRoleId = RoleId FROM [dbo].[Roles] WHERE [RoleName] = 'WarehouseOperator';
SELECT @AdjustPermissionId = PermissionId FROM [dbo].[Permissions] WHERE [PermissionCode] = 'Stock.Adjust';

-- Solo insertar si no existe ya
IF NOT EXISTS (
    SELECT 1 FROM [dbo].[RolePermissions] 
    WHERE [RoleId] = @OperatorRoleId AND [PermissionId] = @AdjustPermissionId
)
BEGIN
    INSERT INTO [dbo].[RolePermissions] ([RoleId], [PermissionId])
    VALUES (@OperatorRoleId, @AdjustPermissionId);
    
    PRINT 'Permiso Stock.Adjust agregado al rol WarehouseOperator.';
END
ELSE
BEGIN
    PRINT 'El permiso Stock.Adjust ya existe para WarehouseOperator.';
END
GO
```

**Alternativamente**, si está creando una base de datos nueva, simplemente ejecute los scripts en orden:
1. `Database/01_CreateSchema.sql`
2. `Database/02_SeedData.sql` (ahora incluye el permiso Stock.Adjust)

### Paso 2: Recompilar la Aplicación

```bash
# En Visual Studio:
# Build > Rebuild Solution
```

O desde la línea de comandos:
```bash
msbuild tp_diploma_nk_2026.sln /t:Rebuild /p:Configuration=Release
```

### Paso 3: Reiniciar la Aplicación

Cierre la aplicación si está corriendo y vuelva a iniciarla para que los cambios surtan efecto.

## Cómo Usar las Nuevas Funcionalidades

### Realizar una Entrada de Stock (IN)

1. Inicie sesión con un usuario que tenga rol **WarehouseOperator** o superior
2. Navegue al menú: **Operaciones > Movimientos**
3. Haga clic en el botón **Nuevo**
4. Configure el movimiento:
   - **Tipo**: Seleccione "In"
   - **Fecha**: Seleccione la fecha del movimiento
   - **Almacén Destino**: Seleccione el almacén donde ingresará el stock
   - **Motivo**: (Opcional) Describa el motivo
   - **Notas**: (Opcional) Agregue notas adicionales
5. Agregue productos:
   - Haga clic en **Agregar Línea**
   - Seleccione el **Producto** de la lista desplegable
   - Ingrese la **Cantidad** a recibir
   - (Opcional) Ingrese el **Precio Unitario**
6. Haga clic en **Guardar**

**Resultado**: El stock aumentará en el almacén de destino.

### Realizar una Transferencia entre Almacenes (TRANSFER)

1. Inicie sesión con un usuario que tenga rol **WarehouseOperator** o superior
2. Navegue al menú: **Operaciones > Movimientos**
3. Haga clic en el botón **Nuevo**
4. Configure el movimiento:
   - **Tipo**: Seleccione "Transfer"
   - **Fecha**: Seleccione la fecha del movimiento
   - **Almacén Origen**: Seleccione de dónde saldrá el stock
   - **Almacén Destino**: Seleccione a dónde irá el stock
   - **Motivo**: (Opcional) Describa el motivo de la transferencia
   - **Notas**: (Opcional) Agregue notas adicionales
5. Agregue productos:
   - Haga clic en **Agregar Línea**
   - Seleccione el **Producto** de la lista desplegable
   - Ingrese la **Cantidad** a transferir
6. Haga clic en **Guardar**

**Validaciones automáticas**:
- ✅ El almacén origen debe tener stock suficiente
- ✅ Los almacenes origen y destino deben ser diferentes
- ✅ Si no hay stock suficiente, se mostrará un mensaje de error

**Resultado**: El stock disminuirá en el almacén origen y aumentará en el almacén destino.

### Realizar una Salida de Stock (OUT)

1. Inicie sesión con un usuario que tenga rol **WarehouseOperator** o superior
2. Navegue al menú: **Operaciones > Movimientos**
3. Haga clic en el botón **Nuevo**
4. Configure el movimiento:
   - **Tipo**: Seleccione "Out"
   - **Fecha**: Seleccione la fecha del movimiento
   - **Almacén Origen**: Seleccione de dónde saldrá el stock
   - **Motivo**: (Opcional) Describa el motivo de la salida
   - **Notas**: (Opcional) Agregue notas adicionales
5. Agregue productos con cantidades
6. Haga clic en **Guardar**

**Resultado**: El stock disminuirá en el almacén origen.

### Realizar un Ajuste de Inventario (ADJUSTMENT)

1. Inicie sesión con un usuario que tenga rol **WarehouseOperator** o superior
2. Navegue al menú: **Operaciones > Movimientos**
3. Haga clic en el botón **Nuevo**
4. Configure el movimiento:
   - **Tipo**: Seleccione "Adjustment"
   - **Fecha**: Seleccione la fecha del ajuste
   - **Almacén Destino**: Seleccione el almacén a ajustar
   - **Motivo**: **REQUERIDO** - Describa el motivo del ajuste (ej: "Corrección por inventario físico")
   - **Notas**: (Opcional) Agregue detalles adicionales
5. Agregue productos con cantidades (positivas para aumentar stock)
6. Haga clic en **Guardar**

**Importante**: Para ajustes, el campo "Motivo" es **obligatorio** para mantener trazabilidad.

## Permisos por Rol

### Administrator
- ✅ Todos los permisos de stock
- ✅ Puede realizar cualquier operación

### WarehouseManager
- ✅ Todos los permisos de stock
- ✅ Puede realizar cualquier operación

### WarehouseOperator (ACTUALIZADO)
- ✅ Stock.View - Consultar inventario
- ✅ Stock.Receive - Recibir stock (entradas)
- ✅ Stock.Issue - Emitir stock (salidas)
- ✅ Stock.Transfer - Transferir entre almacenes
- ✅ Stock.Adjust - Ajustar inventario

### Viewer
- ✅ Stock.View - Solo consultar, sin modificaciones
- ❌ No puede crear movimientos

## Validaciones y Reglas de Negocio

### Entradas (IN)
- ✔️ Debe seleccionar un almacén de destino
- ✔️ La cantidad debe ser mayor a cero
- ✔️ Los productos deben estar activos

### Salidas (OUT)
- ✔️ Debe seleccionar un almacén de origen
- ✔️ Debe haber stock suficiente en el almacén de origen
- ✔️ La cantidad debe ser mayor a cero
- ✔️ Los productos deben estar activos

### Transferencias (TRANSFER)
- ✔️ Debe seleccionar almacén de origen y destino
- ✔️ Los almacenes deben ser diferentes
- ✔️ Debe haber stock suficiente en el almacén de origen
- ✔️ La cantidad debe ser mayor a cero
- ✔️ Los productos deben estar activos

### Ajustes (ADJUSTMENT)
- ✔️ Debe seleccionar un almacén
- ✔️ El motivo es **obligatorio**
- ✔️ La cantidad debe ser mayor a cero (para ajustes positivos)
- ✔️ Para reducciones de stock, usar salidas (OUT) en su lugar

## Trazabilidad y Auditoría

Todos los movimientos de stock generan:

1. **Número de Movimiento Único**: Formato `{TIPO}{YYYYMMDD}{NNNN}`
   - Ejemplo: `IN202602150001`, `TRA202602150002`
   
2. **Registro de Auditoría**: Incluye:
   - Usuario que creó el movimiento
   - Fecha y hora exacta
   - Tipo de movimiento
   - Productos y cantidades
   - Almacenes involucrados

3. **Actualización Automática de Stock**: 
   - El inventario se actualiza inmediatamente al guardar el movimiento
   - Los cambios son visibles en la consulta de stock

## Solución de Problemas

### El botón "Nuevo" está deshabilitado
**Causa**: El usuario no tiene ningún permiso de operación de stock.
**Solución**: Asegúrese de que el usuario tiene al menos uno de estos permisos: Stock.Receive, Stock.Issue, Stock.Transfer, o Stock.Adjust.

### El menú "Movimientos" no aparece
**Causa**: El usuario no tiene ningún permiso relacionado con stock.
**Solución**: Asigne al menos el permiso Stock.View al rol del usuario.

### Error "Stock insuficiente"
**Causa**: Intentando sacar o transferir más stock del disponible.
**Solución**: 
1. Verifique el stock actual en **Operaciones > Consultar Stock**
2. Ajuste la cantidad en el movimiento
3. O realice primero una entrada de stock si es necesario

### Los cambios en permisos no surten efecto
**Causa**: La sesión actual usa los permisos cargados al iniciar sesión.
**Solución**: Cierre sesión y vuelva a iniciar sesión para que se carguen los nuevos permisos.

## Próximos Pasos Recomendados

1. ✅ **Aplicar el script SQL** para actualizar permisos en bases de datos existentes
2. ✅ **Recompilar** la aplicación
3. ✅ **Probar** con un usuario WarehouseOperator
4. ✅ **Capacitar** a los usuarios en las nuevas funcionalidades
5. ✅ **Documentar** los procesos específicos de su organización

## Resumen de Beneficios

- ✅ **Mayor autonomía**: Los operadores de almacén pueden realizar sus tareas sin intervención de administradores
- ✅ **Mejor trazabilidad**: Todos los movimientos quedan registrados con usuario y fecha
- ✅ **Prevención de errores**: Validaciones automáticas evitan stock negativo y operaciones inválidas
- ✅ **Flexibilidad**: Soporte para los 4 tipos principales de movimientos de inventario
- ✅ **Auditoría completa**: Historial detallado de todos los movimientos de stock

---

Para más información sobre cómo usar el formulario de movimientos, consulte el archivo `STOCK_MOVEMENT_GUIDE.md`.
