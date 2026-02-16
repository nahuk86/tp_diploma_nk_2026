using System;
using System.Windows.Forms;

namespace UI.Forms
{
    public partial class UserManualForm : Form
    {
        public UserManualForm()
        {
            InitializeComponent();
            LoadManualContent();
        }

        private void LoadManualContent()
        {
            // User manual content in Spanish
            txtManualContent.Text = @"# MANUAL DE USUARIO - STOCK MANAGER

## DESCRIPCIÓN GENERAL
Stock Manager es un sistema de gestión de inventario diseñado para administrar accesorios de celulares (fundas, carcasas, protectores de pantalla, parlantes, etc.).

## INICIO DE SESIÓN

1. Al iniciar la aplicación, ingrese su **Usuario** y **Contraseña**
2. Haga clic en **Iniciar Sesión**
3. Si es la primera vez que inicia la aplicación, se le pedirá configurar la contraseña del administrador

### Usuario Predeterminado
- Usuario: admin
- Contraseña: La configurada en el primer inicio

## MENÚ PRINCIPAL

### ARCHIVO
- **Cerrar Sesión**: Cierra la sesión actual
- **Salir**: Cierra la aplicación

### ADMINISTRACIÓN
- **Usuarios**: Gestionar usuarios del sistema
- **Roles**: Gestionar roles y permisos

### INVENTARIO
- **Productos**: Gestionar catálogo de productos
- **Almacenes**: Gestionar almacenes
- **Clientes**: Gestionar clientes

### OPERACIONES
- **Ventas**: Registrar ventas
- **Movimientos**: Registrar movimientos de stock
  - Entrada (In): Recepción de mercadería
  - Salida (Out): Retiro de mercadería
  - Transferencia (Transfer): Movimiento entre almacenes
  - Ajuste (Adjustment): Ajustes de inventario
- **Consultar Stock**: Ver stock disponible por almacén
- **Reportes**: Ver reportes de ventas y stock

### CONFIGURACIÓN
- **Idioma**: Cambiar entre Español e Inglés

### AYUDA
- **Manual de Uso**: Esta pantalla
- **Acerca de...**: Información del sistema

## GESTIÓN DE PRODUCTOS

### Crear Producto
1. Ir a **Inventario > Productos**
2. Clic en **Nuevo**
3. Completar campos:
   - SKU: Código único del producto
   - Nombre: Nombre del producto
   - Descripción: Descripción detallada
   - Categoría: Categoría del producto
   - Precio: Precio de venta
   - Stock Mínimo: Cantidad mínima en stock
4. Clic en **Guardar**

### Editar Producto
1. Seleccionar producto de la lista
2. Clic en **Editar**
3. Modificar campos necesarios
4. Clic en **Guardar**

### Eliminar Producto
1. Seleccionar producto de la lista
2. Clic en **Eliminar**
3. Confirmar eliminación

## GESTIÓN DE ALMACENES

### Crear Almacén
1. Ir a **Inventario > Almacenes**
2. Clic en **Nuevo**
3. Completar campos:
   - Código: Código único del almacén
   - Nombre: Nombre del almacén
   - Dirección: Dirección física
4. Clic en **Guardar**

## MOVIMIENTOS DE STOCK

### Entrada de Stock (Recepción)
1. Ir a **Operaciones > Movimientos**
2. Clic en **Nuevo**
3. Seleccionar **Tipo: In**
4. Seleccionar **Almacén Destino**
5. Agregar líneas de productos:
   - Producto
   - Cantidad
   - Precio Unitario
6. Clic en **Guardar**

### Transferencia entre Almacenes
1. Ir a **Operaciones > Movimientos**
2. Clic en **Nuevo**
3. Seleccionar **Tipo: Transfer**
4. Seleccionar **Almacén Origen**
5. Seleccionar **Almacén Destino** (diferente al origen)
6. Agregar líneas de productos
7. Clic en **Guardar**

### Salida de Stock
1. Ir a **Operaciones > Movimientos**
2. Clic en **Nuevo**
3. Seleccionar **Tipo: Out**
4. Seleccionar **Almacén Origen**
5. Agregar líneas de productos
6. Clic en **Guardar**

### Ajuste de Stock
1. Ir a **Operaciones > Movimientos**
2. Clic en **Nuevo**
3. Seleccionar **Tipo: Adjustment**
4. Seleccionar almacén
5. Agregar líneas de productos (cantidad puede ser negativa)
6. Especificar motivo del ajuste
7. Clic en **Guardar**

## VENTAS

### Registrar Venta
1. Ir a **Operaciones > Ventas**
2. Clic en **Nuevo**
3. Completar:
   - Vendedor: Nombre del vendedor
   - Cliente: Seleccionar cliente
   - Productos: Agregar líneas de productos
   - Cantidad y precio
4. Verificar que hay stock disponible
5. Clic en **Guardar**

## CONSULTA DE STOCK

1. Ir a **Operaciones > Consultar Stock**
2. Filtros disponibles:
   - Por producto
   - Por almacén
   - Por categoría
3. Ver stock disponible en cada almacén
4. Exportar resultados si es necesario

## REPORTES

### Reporte de Ventas
1. Ir a **Operaciones > Reportes**
2. Seleccionar pestaña **Ventas**
3. Filtrar por rango de fechas
4. Ver totales y detalles

### Reporte de Stock
1. Ir a **Operaciones > Reportes**
2. Seleccionar pestaña **Stock**
3. Ver stock actual por almacén
4. Identificar productos con stock bajo

## GESTIÓN DE USUARIOS Y ROLES

### Crear Usuario
1. Ir a **Administración > Usuarios**
2. Clic en **Nuevo**
3. Completar datos del usuario
4. Asignar contraseña inicial
5. Clic en **Guardar**

### Asignar Roles
1. Seleccionar usuario
2. Clic en **Asignar Roles**
3. Seleccionar los roles necesarios
4. Clic en **Guardar**

### Gestionar Permisos de Rol
1. Ir a **Administración > Roles**
2. Seleccionar rol
3. Clic en **Gestionar Permisos**
4. Marcar/desmarcar permisos
5. Clic en **Guardar**

## CAMBIO DE IDIOMA

1. Ir a **Configuración > Idioma**
2. Seleccionar:
   - Español
   - English
3. La interfaz cambia inmediatamente

## SOLUCIÓN DE PROBLEMAS

### No puedo ver un menú
- Verificar que su usuario tiene los permisos necesarios
- Contactar al administrador del sistema

### Stock insuficiente
- Verificar stock disponible antes de registrar venta/salida
- Registrar entrada de stock si es necesario

### Error al guardar
- Verificar que todos los campos requeridos están completos
- Verificar que los datos ingresados son válidos
- Contactar a soporte si el problema persiste

## CONTACTO Y SOPORTE

Para soporte técnico o consultas:
- Contactar al administrador del sistema
- Ver documentación adicional en la carpeta del proyecto

---
© 2026 - Stock Manager - Sistema Académico";

            txtManualContent.SelectionStart = 0;
            txtManualContent.SelectionLength = 0;
            txtManualContent.ScrollToCaret();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
