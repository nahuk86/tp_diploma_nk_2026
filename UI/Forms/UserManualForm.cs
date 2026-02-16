using System;
using System.Drawing;
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
            txtManualContent.Clear();
            
            // Main Title
            AppendTitle("MANUAL DE USUARIO - STOCK MANAGER", 16, true);
            AppendLine();
            
            // Section: DESCRIPCIÓN GENERAL
            AppendHeading("DESCRIPCIÓN GENERAL", 13);
            AppendText("Stock Manager es un sistema de gestión de inventario diseñado para administrar accesorios de celulares (fundas, carcasas, protectores de pantalla, parlantes, etc.).");
            AppendLine();
            AppendLine();
            
            // Section: INICIO DE SESIÓN
            AppendHeading("INICIO DE SESIÓN", 13);
            AppendText("1. Al iniciar la aplicación, ingrese su ");
            AppendBold("Usuario");
            AppendText(" y ");
            AppendBold("Contraseña");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Iniciar Sesión");
            AppendLine();
            AppendText("3. Si es la primera vez que inicia la aplicación, se le pedirá configurar la contraseña del administrador");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Usuario Predeterminado");
            AppendText("• Usuario: admin");
            AppendLine();
            AppendText("• Contraseña: La configurada en el primer inicio");
            AppendLine();
            AppendLine();
            
            // Section: MENÚ PRINCIPAL
            AppendHeading("MENÚ PRINCIPAL", 13);
            AppendLine();
            
            AppendSubHeading("ARCHIVO");
            AppendText("• ");
            AppendBold("Cerrar Sesión");
            AppendText(": Cierra la sesión actual");
            AppendLine();
            AppendText("• ");
            AppendBold("Salir");
            AppendText(": Cierra la aplicación");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("ADMINISTRACIÓN");
            AppendText("• ");
            AppendBold("Usuarios");
            AppendText(": Gestionar usuarios del sistema");
            AppendLine();
            AppendText("• ");
            AppendBold("Roles");
            AppendText(": Gestionar roles y permisos");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("INVENTARIO");
            AppendText("• ");
            AppendBold("Productos");
            AppendText(": Gestionar catálogo de productos");
            AppendLine();
            AppendText("• ");
            AppendBold("Almacenes");
            AppendText(": Gestionar almacenes");
            AppendLine();
            AppendText("• ");
            AppendBold("Clientes");
            AppendText(": Gestionar clientes");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("OPERACIONES");
            AppendText("• ");
            AppendBold("Ventas");
            AppendText(": Registrar ventas");
            AppendLine();
            AppendText("• ");
            AppendBold("Movimientos");
            AppendText(": Registrar movimientos de stock");
            AppendLine();
            AppendText("   - Entrada (In): Recepción de mercadería");
            AppendLine();
            AppendText("   - Salida (Out): Retiro de mercadería");
            AppendLine();
            AppendText("   - Transferencia (Transfer): Movimiento entre almacenes");
            AppendLine();
            AppendText("   - Ajuste (Adjustment): Ajustes de inventario");
            AppendLine();
            AppendText("• ");
            AppendBold("Consultar Stock");
            AppendText(": Ver stock disponible por almacén");
            AppendLine();
            AppendText("• ");
            AppendBold("Reportes");
            AppendText(": Ver reportes de ventas y stock");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("CONFIGURACIÓN");
            AppendText("• ");
            AppendBold("Idioma");
            AppendText(": Cambiar entre Español e Inglés");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("AYUDA");
            AppendText("• ");
            AppendBold("Manual de Uso");
            AppendText(": Esta pantalla");
            AppendLine();
            AppendText("• ");
            AppendBold("Acerca de...");
            AppendText(": Información del sistema");
            AppendLine();
            AppendLine();
            
            // Section: GESTIÓN DE PRODUCTOS
            AppendHeading("GESTIÓN DE PRODUCTOS", 13);
            AppendLine();
            
            AppendSubHeading("Crear Producto");
            AppendText("1. Ir a ");
            AppendBold("Inventario > Productos");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Completar campos:");
            AppendLine();
            AppendText("   - SKU: Código único del producto");
            AppendLine();
            AppendText("   - Nombre: Nombre del producto");
            AppendLine();
            AppendText("   - Descripción: Descripción detallada");
            AppendLine();
            AppendText("   - Categoría: Categoría del producto");
            AppendLine();
            AppendText("   - Precio: Precio de venta");
            AppendLine();
            AppendText("   - Stock Mínimo: Cantidad mínima en stock");
            AppendLine();
            AppendText("4. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Editar Producto");
            AppendText("1. Seleccionar producto de la lista");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Editar");
            AppendLine();
            AppendText("3. Modificar campos necesarios");
            AppendLine();
            AppendText("4. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Eliminar Producto");
            AppendText("1. Seleccionar producto de la lista");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Eliminar");
            AppendLine();
            AppendText("3. Confirmar eliminación");
            AppendLine();
            AppendLine();
            
            // Section: GESTIÓN DE ALMACENES
            AppendHeading("GESTIÓN DE ALMACENES", 13);
            AppendLine();
            
            AppendSubHeading("Crear Almacén");
            AppendText("1. Ir a ");
            AppendBold("Inventario > Almacenes");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Completar campos:");
            AppendLine();
            AppendText("   - Código: Código único del almacén");
            AppendLine();
            AppendText("   - Nombre: Nombre del almacén");
            AppendLine();
            AppendText("   - Dirección: Dirección física");
            AppendLine();
            AppendText("4. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            // Section: MOVIMIENTOS DE STOCK
            AppendHeading("MOVIMIENTOS DE STOCK", 13);
            AppendLine();
            
            AppendSubHeading("Entrada de Stock (Recepción)");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Movimientos");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Seleccionar ");
            AppendBold("Tipo: In");
            AppendLine();
            AppendText("4. Seleccionar ");
            AppendBold("Almacén Destino");
            AppendLine();
            AppendText("5. Agregar líneas de productos:");
            AppendLine();
            AppendText("   - Producto");
            AppendLine();
            AppendText("   - Cantidad");
            AppendLine();
            AppendText("   - Precio Unitario");
            AppendLine();
            AppendText("6. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Transferencia entre Almacenes");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Movimientos");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Seleccionar ");
            AppendBold("Tipo: Transfer");
            AppendLine();
            AppendText("4. Seleccionar ");
            AppendBold("Almacén Origen");
            AppendLine();
            AppendText("5. Seleccionar ");
            AppendBold("Almacén Destino");
            AppendText(" (diferente al origen)");
            AppendLine();
            AppendText("6. Agregar líneas de productos");
            AppendLine();
            AppendText("7. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Salida de Stock");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Movimientos");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Seleccionar ");
            AppendBold("Tipo: Out");
            AppendLine();
            AppendText("4. Seleccionar ");
            AppendBold("Almacén Origen");
            AppendLine();
            AppendText("5. Agregar líneas de productos");
            AppendLine();
            AppendText("6. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Ajuste de Stock");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Movimientos");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Seleccionar ");
            AppendBold("Tipo: Adjustment");
            AppendLine();
            AppendText("4. Seleccionar almacén");
            AppendLine();
            AppendText("5. Agregar líneas de productos (cantidad puede ser negativa)");
            AppendLine();
            AppendText("6. Especificar motivo del ajuste");
            AppendLine();
            AppendText("7. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            // Section: VENTAS
            AppendHeading("VENTAS", 13);
            AppendLine();
            
            AppendSubHeading("Registrar Venta");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Ventas");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Completar:");
            AppendLine();
            AppendText("   - Vendedor: Nombre del vendedor");
            AppendLine();
            AppendText("   - Cliente: Seleccionar cliente");
            AppendLine();
            AppendText("   - Productos: Agregar líneas de productos");
            AppendLine();
            AppendText("   - Cantidad y precio");
            AppendLine();
            AppendText("4. Verificar que hay stock disponible");
            AppendLine();
            AppendText("5. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            // Section: CONSULTA DE STOCK
            AppendHeading("CONSULTA DE STOCK", 13);
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Consultar Stock");
            AppendLine();
            AppendText("2. Filtros disponibles:");
            AppendLine();
            AppendText("   - Por producto");
            AppendLine();
            AppendText("   - Por almacén");
            AppendLine();
            AppendText("   - Por categoría");
            AppendLine();
            AppendText("3. Ver stock disponible en cada almacén");
            AppendLine();
            AppendText("4. Exportar resultados si es necesario");
            AppendLine();
            AppendLine();
            
            // Section: REPORTES
            AppendHeading("REPORTES", 13);
            AppendLine();
            
            AppendSubHeading("Reporte de Ventas");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Reportes");
            AppendLine();
            AppendText("2. Seleccionar pestaña ");
            AppendBold("Ventas");
            AppendLine();
            AppendText("3. Filtrar por rango de fechas");
            AppendLine();
            AppendText("4. Ver totales y detalles");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Reporte de Stock");
            AppendText("1. Ir a ");
            AppendBold("Operaciones > Reportes");
            AppendLine();
            AppendText("2. Seleccionar pestaña ");
            AppendBold("Stock");
            AppendLine();
            AppendText("3. Ver stock actual por almacén");
            AppendLine();
            AppendText("4. Identificar productos con stock bajo");
            AppendLine();
            AppendLine();
            
            // Section: GESTIÓN DE USUARIOS Y ROLES
            AppendHeading("GESTIÓN DE USUARIOS Y ROLES", 13);
            AppendLine();
            
            AppendSubHeading("Crear Usuario");
            AppendText("1. Ir a ");
            AppendBold("Administración > Usuarios");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Nuevo");
            AppendLine();
            AppendText("3. Completar datos del usuario");
            AppendLine();
            AppendText("4. Asignar contraseña inicial");
            AppendLine();
            AppendText("5. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Asignar Roles");
            AppendText("1. Seleccionar usuario");
            AppendLine();
            AppendText("2. Clic en ");
            AppendBold("Asignar Roles");
            AppendLine();
            AppendText("3. Seleccionar los roles necesarios");
            AppendLine();
            AppendText("4. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Gestionar Permisos de Rol");
            AppendText("1. Ir a ");
            AppendBold("Administración > Roles");
            AppendLine();
            AppendText("2. Seleccionar rol");
            AppendLine();
            AppendText("3. Clic en ");
            AppendBold("Gestionar Permisos");
            AppendLine();
            AppendText("4. Marcar/desmarcar permisos");
            AppendLine();
            AppendText("5. Clic en ");
            AppendBold("Guardar");
            AppendLine();
            AppendLine();
            
            // Section: CAMBIO DE IDIOMA
            AppendHeading("CAMBIO DE IDIOMA", 13);
            AppendText("1. Ir a ");
            AppendBold("Configuración > Idioma");
            AppendLine();
            AppendText("2. Seleccionar:");
            AppendLine();
            AppendText("   - Español");
            AppendLine();
            AppendText("   - English");
            AppendLine();
            AppendText("3. La interfaz cambia inmediatamente");
            AppendLine();
            AppendLine();
            
            // Section: SOLUCIÓN DE PROBLEMAS
            AppendHeading("SOLUCIÓN DE PROBLEMAS", 13);
            AppendLine();
            
            AppendSubHeading("No puedo ver un menú");
            AppendText("• Verificar que su usuario tiene los permisos necesarios");
            AppendLine();
            AppendText("• Contactar al administrador del sistema");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Stock insuficiente");
            AppendText("• Verificar stock disponible antes de registrar venta/salida");
            AppendLine();
            AppendText("• Registrar entrada de stock si es necesario");
            AppendLine();
            AppendLine();
            
            AppendSubHeading("Error al guardar");
            AppendText("• Verificar que todos los campos requeridos están completos");
            AppendLine();
            AppendText("• Verificar que los datos ingresados son válidos");
            AppendLine();
            AppendText("• Contactar a soporte si el problema persiste");
            AppendLine();
            AppendLine();
            
            // Section: CONTACTO Y SOPORTE
            AppendHeading("CONTACTO Y SOPORTE", 13);
            AppendText("Para soporte técnico o consultas:");
            AppendLine();
            AppendText("• Contactar al administrador del sistema");
            AppendLine();
            AppendText("• Ver documentación adicional en la carpeta del proyecto");
            AppendLine();
            AppendLine();
            
            // Footer
            AppendText("───────────────────────────────────────────────");
            AppendLine();
            AppendText("© 2026 - Stock Manager - Sistema Académico");
            
            // Scroll to top
            txtManualContent.SelectionStart = 0;
            txtManualContent.SelectionLength = 0;
            txtManualContent.ScrollToCaret();
        }

        private void AppendTitle(string text, int fontSize, bool bold)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", fontSize, bold ? FontStyle.Bold : FontStyle.Regular);
            txtManualContent.SelectionColor = Color.DarkBlue;
            txtManualContent.Select(txtManualContent.TextLength, 0);
        }

        private void AppendHeading(string text, int fontSize)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", fontSize, FontStyle.Bold);
            txtManualContent.SelectionColor = Color.DarkBlue;
            txtManualContent.Select(txtManualContent.TextLength, 0);
            AppendLine();
        }

        private void AppendSubHeading(string text)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", 11, FontStyle.Bold);
            txtManualContent.SelectionColor = Color.DarkSlateGray;
            txtManualContent.Select(txtManualContent.TextLength, 0);
            AppendLine();
        }

        private void AppendBold(string text)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtManualContent.SelectionColor = Color.Black;
            txtManualContent.Select(txtManualContent.TextLength, 0);
        }

        private void AppendText(string text)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            txtManualContent.SelectionColor = Color.Black;
            txtManualContent.Select(txtManualContent.TextLength, 0);
        }

        private void AppendLine()
        {
            txtManualContent.AppendText(Environment.NewLine);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
