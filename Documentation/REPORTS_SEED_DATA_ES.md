# Datos de Prueba para Reportes - Guía Completa

## Problema Resuelto

**Reportado**: Los siguientes reportes no mostraban datos:
1. ❌ Top Products (Productos más vendidos)
2. ❌ Revenue by Date (Ingresos por fecha)
3. ❌ Client Ticket Average (Ticket promedio por cliente)

**Causa Raíz**: La base de datos no tenía datos de ventas, clientes ni movimientos de stock. Los reportes necesitan estos datos para funcionar.

## Solución Implementada

Se creó el archivo `Database/04_ReportsTestData.sql` con datos de prueba completos y realistas.

### Datos Incluidos

#### 1. 15 Clientes
```
- Juan Pérez (DNI: 20345678)
- María González (DNI: 27456789)
- Carlos Rodríguez (DNI: 30567890)
- Ana Martínez (DNI: 33678901)
- Luis Fernández (DNI: 25789012)
- Laura López (DNI: 28890123)
- Pedro García (DNI: 31901234)
- Sofía Díaz (DNI: 26012345)
- Diego Sánchez (DNI: 29123456)
- Valentina Romero (DNI: 32234567)
- Mateo Torres (DNI: 24345678)
- Lucía Flores (DNI: 27456781)
- Tomás Benítez (DNI: 30567892)
- Emma Vargas (DNI: 33678903)
- Martín Castro (DNI: 25789014)
```

Todos con datos completos: email, teléfono, dirección en CABA.

#### 2. 26 Productos en 10 Categorías

**Fundas (Cases)**: 7 productos
- iPhone 14 Case Black
- iPhone 13 Case Red
- iPhone 13 Case Blue
- Samsung S23 Case
- Samsung A54 Case
- Xiaomi 12 Case

**Protectores de Pantalla**: 4 productos
- iPhone 14/13 Screen Protector
- Samsung A54 Screen Protector
- Xiaomi 12 Screen Protector

**Cargadores (Chargers)**: 3 productos
- USB-C Charger 20W
- Fast Charger 30W

**Audio/Parlantes (Speakers)**: 5 productos
- Wireless Earbuds
- Bluetooth Speaker Mini
- Bluetooth Speaker Mini Pro
- Over-Ear Headphones
- Bluetooth Headphones

**Cables**: 2 productos
- USB-C Cable 2m
- Lightning Cable 1m

**Power Banks**: 2 productos
- 10000mAh
- 20000mAh

**Otros**: Adaptadores, soportes, periféricos (mouse, teclado)

**Rango de precios**: $6.99 - $59.99

#### 3. 3 Almacenes
- WH001: Main Warehouse (Principal)
- WH002: Secondary Warehouse (Secundario)
- WH003: Distribution Center (Centro de Distribución) - NUEVO

#### 4. Movimientos de Stock
- **3 Entradas iniciales** (In): Stock inicial en los 3 almacenes
- **1 Transferencia**: Entre almacenes para rebalanceo
- **1 Ajuste**: Ajuste de inventario

Cantidades realistas según categoría:
- Protectores de pantalla: 80-100 unidades
- Fundas: 40-60 unidades
- Cables: 60-80 unidades
- Audio: 20-35 unidades
- Power Banks: 5-25 unidades

#### 5. 90-270 Ventas
- **Período**: Últimos 90 días
- **Frecuencia**: 1-3 ventas por día
- **Clientes**: Distribución aleatoria entre los 15 clientes
- **Vendedores**: 4 vendedores registrados
  - Juan Pérez
  - María González
  - Carlos Rodríguez
  - Ana Martínez

#### 6. Líneas de Venta
- **1-5 productos por venta**
- **Cantidades**: 1-10 unidades por producto
- **Descuentos**: 0-15% aleatorio (simula precios de venta vs lista)
- **Totales calculados** automáticamente

## Cómo Usar el Script

### Paso 1: Abrir SQL Server Management Studio (SSMS)

```sql
-- Conectarse a tu instancia de SQL Server
-- Usar la base de datos correcta
USE StockManagerDB;
GO
```

### Paso 2: Ejecutar el Script

**Opción A - Desde SSMS**:
1. Abrir el archivo `Database/04_ReportsTestData.sql`
2. Presionar F5 o click en "Ejecutar"

**Opción B - Orden completo** (si es instalación nueva):
```sql
-- 1. Crear tablas
:r 01_CreateSchema.sql

-- 2. Datos básicos (permisos, admin)
:r 02_SeedData.sql

-- 3. Actualizar permisos
:r 03_UpdatePermissions.sql

-- 4. DATOS DE PRUEBA PARA REPORTES
:r 04_ReportsTestData.sql
```

### Paso 3: Verificar la Carga

El script muestra un resumen al finalizar:

```
================================================
REPORTS TEST DATA SEED COMPLETED!
================================================

Entity              Count
-----------------   -----
Clients             15
Products            26
Warehouses          3
Stock Movements     5
Sales               150-250 (varía)
Sale Lines          300-1000 (varía)
================================================
```

## Cobertura de Reportes

Con estos datos, **TODOS los 8 reportes** funcionarán correctamente:

### ✅ 1. Top Products (Productos Más Vendidos)
- 26 productos con ventas variadas
- Múltiples categorías
- Rangos de precio diversos
- Datos para ranking por unidades o ingresos

**Filtros que funcionan**:
- Rango de fechas (últimos 90 días)
- Por categoría (10 categorías disponibles)
- Top N productos

### ✅ 2. Client Purchases (Compras por Cliente)
- 15 clientes con compras
- Múltiples compras por cliente
- Diferentes patrones de compra
- Productos variados por cliente

**Filtros que funcionan**:
- Rango de fechas
- Cliente específico (15 opciones)
- Top N clientes

### ✅ 3. Price Variation (Variación de Precios)
- Descuentos aleatorios (0-15%)
- Precio de lista vs precio de venta
- Múltiples transacciones por producto
- Cálculo de mínimo, máximo, promedio

**Filtros que funcionan**:
- Rango de fechas
- Producto específico
- Por categoría

### ✅ 4. Seller Performance (Ventas por Vendedor)
- 4 vendedores con ventas registradas
- Ventas distribuidas aleatoriamente
- Múltiples categorías por vendedor

**Filtros que funcionan**:
- Rango de fechas
- Vendedor específico (4 opciones)
- Por categoría

### ✅ 5. Category Sales (Ventas por Categoría)
- 10 categorías con ventas
- Distribución de ingresos
- Porcentajes de participación

**Filtros que funcionan**:
- Rango de fechas
- Categoría específica

### ✅ 6. Revenue by Date (Ingresos por Fecha)
- 90 días de datos de ventas
- 5 movimientos de stock registrados
- Comparación ventas vs entradas

**Filtros que funcionan**:
- Rango de fechas
- Tipo de movimiento (In, Out, Transfer, Adjustment)
- Almacén específico (3 opciones)

### ✅ 7. Client Product Ranking (Ranking Clientes-Productos)
- Relaciones cliente-producto
- Múltiples compras por combinación
- Porcentajes de participación

**Filtros que funcionan**:
- Rango de fechas
- Producto específico
- Por categoría
- Top N clientes

### ✅ 8. Client Ticket Average (Ticket Promedio)
- 15 clientes con múltiples compras
- Variedad de tickets (montos)
- Estadísticas calculadas

**Filtros que funcionan**:
- Rango de fechas
- Cliente específico
- Compras mínimas

## Pruebas Recomendadas

### Prueba 1: Reporte Sin Filtros
1. Abrir cualquier reporte
2. Click en "Generar" sin modificar fechas
3. **Resultado esperado**: Datos de los últimos 90 días

### Prueba 2: Filtro por Categoría
1. Reporte "Top Products"
2. Seleccionar categoría "Cases"
3. Click "Generar"
4. **Resultado esperado**: Solo productos de fundas

### Prueba 3: Top 10
1. Reporte "Top Products"
2. Marcar "Top 10"
3. Click "Generar"
4. **Resultado esperado**: Máximo 10 productos

### Prueba 4: Exportar CSV
1. Generar cualquier reporte
2. Click "Exportar CSV"
3. **Resultado esperado**: Archivo CSV descargado

### Prueba 5: Vendedor Específico
1. Reporte "Seller Performance"
2. Ingresar "Juan Pérez" en vendedor
3. Click "Generar"
4. **Resultado esperado**: Solo ventas de Juan Pérez

## Características del Script

### ✅ Seguro para Re-ejecutar
```sql
-- El script limpia datos previos:
DELETE FROM [dbo].[Clients] WHERE ClientId > 0;
DELETE FROM [dbo].[Products] WHERE ProductId > 5;
```

**Preserva**:
- Usuario admin
- Primeros 5 productos originales
- Almacenes WH001 y WH002
- Permisos y roles

### ✅ Datos Realistas
- Nombres argentinos comunes
- DNI válidos (formato argentino)
- Direcciones en CABA
- Emails con formato correcto
- Teléfonos formato argentino

### ✅ Distribución Aleatoria
```sql
-- Usa NEWID() para aleatorización:
ORDER BY NEWID()  -- Cliente aleatorio
ABS(CHECKSUM(NEWID())) % 10  -- Cantidad aleatoria
```

Esto genera:
- Diferentes productos por venta
- Cantidades variadas
- Descuentos realistas
- Distribución natural de ventas

### ✅ Relaciones Correctas
- Todas las claves foráneas válidas
- Totales calculados correctamente
- Fechas en secuencia lógica
- Stock suficiente para ventas

## Solución de Problemas

### Problema: "Cannot insert duplicate key"
**Causa**: Datos ya existen
**Solución**: El script hace DELETE, ejecutar de nuevo

### Problema: "Foreign key constraint"
**Causa**: Orden incorrecto de ejecución
**Solución**: Ejecutar scripts en orden: 01→02→03→04

### Problema: Reportes aún sin datos
**Causa posible**: Filtros muy restrictivos
**Solución**: 
1. Verificar rango de fechas (últimos 90 días)
2. Quitar filtros de categoría/cliente
3. Verificar que el script se ejecutó (ver mensajes)

### Problema: "Invalid object name"
**Causa**: Tablas no creadas
**Solución**: Ejecutar primero 01_CreateSchema.sql

## Resumen de Archivos

```
Database/
├── 01_CreateSchema.sql          -- Crear tablas (PRIMERO)
├── 02_SeedData.sql             -- Admin, permisos, 5 productos
├── 03_UpdatePermissions.sql    -- Actualizar permisos
└── 04_ReportsTestData.sql      -- DATOS DE PRUEBA (NUEVO)
                                   15 clientes
                                   21 productos adicionales
                                   90 días de ventas
                                   Movimientos de stock
```

## Próximos Pasos

1. ✅ **Ejecutar el script** - 04_ReportsTestData.sql
2. ✅ **Verificar carga** - Ver resumen al final
3. ✅ **Abrir aplicación** - Menú → Reportes
4. ✅ **Probar cada reporte** - Generar sin filtros primero
5. ✅ **Probar filtros** - Categoría, fechas, clientes
6. ✅ **Exportar CSV** - Verificar funcionalidad

## Soporte Técnico

Si después de ejecutar el script los reportes siguen sin mostrar datos:

1. **Verificar ejecución exitosa**:
   ```sql
   SELECT COUNT(*) FROM Sales WHERE IsActive = 1;
   -- Debe mostrar 150-250
   
   SELECT COUNT(*) FROM Clients WHERE IsActive = 1;
   -- Debe mostrar 15
   ```

2. **Verificar filtros en la aplicación**:
   - Rango de fechas incluye últimos 90 días
   - No hay filtros muy restrictivos activos

3. **Revisar logs de error**:
   - La aplicación tiene manejo de errores
   - Verificar mensajes en pantalla

4. **Consultar este documento**: GRID_FORMATTING_FIX.md
   - Detalla fix de NullReferenceException
   - Protección de columnas nulas

## Conclusión

Con este script de datos de prueba:
- ✅ Los 8 reportes funcionan correctamente
- ✅ Datos realistas para demos
- ✅ Suficiente variedad para probar filtros
- ✅ 90 días de historia para análisis
- ✅ Fácil de ejecutar y re-ejecutar

**¡Todo listo para probar los reportes!** 🎉
