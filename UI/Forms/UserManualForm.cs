using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Forms
{
    public partial class UserManualForm : Form
    {
        /// <summary>
        /// Inicializa una nueva instancia del formulario de manual de usuario
        /// </summary>
        public UserManualForm()
        {
            InitializeComponent();
            LoadManualContent();
        }

        /// <summary>
        /// Carga el contenido completo del manual de usuario en el RichTextBox
        /// </summary>
        private void LoadManualContent()
        {
            txtManualContent.Clear();

            // Main Title
            AppendTitle("MANUAL DE USUARIO — STOCK MANAGER", 16, true);
            AppendLine();
            AppendText("Sistema de Gestión de Inventario y Ventas — Versión 2026");
            AppendLine();
            AppendLine();

            // ── ÍNDICE ────────────────────────────────────────────────────────
            AppendHeading("ÍNDICE", 13);
            AppendText("  1.  Descripción General");
            AppendLine();
            AppendText("  2.  Inicio de Sesión");
            AppendLine();
            AppendText("  3.  Menú Principal");
            AppendLine();
            AppendText("  4.  Gestión de Productos");
            AppendLine();
            AppendText("  5.  Gestión de Almacenes");
            AppendLine();
            AppendText("  6.  Gestión de Clientes");
            AppendLine();
            AppendText("  7.  Movimientos de Stock");
            AppendLine();
            AppendText("        7.1  Entrada de Mercadería");
            AppendLine();
            AppendText("        7.2  Salida de Mercadería");
            AppendLine();
            AppendText("        7.3  Transferencia entre Almacenes");
            AppendLine();
            AppendText("        7.4  Ajuste de Inventario");
            AppendLine();
            AppendText("  8.  Registro de Ventas");
            AppendLine();
            AppendText("  9.  Consulta de Stock");
            AppendLine();
            AppendText("  10. Reportes");
            AppendLine();
            AppendText("        10.1 Productos Más Vendidos");
            AppendLine();
            AppendText("        10.2 Compras por Cliente");
            AppendLine();
            AppendText("        10.3 Variación de Precios");
            AppendLine();
            AppendText("        10.4 Ventas por Vendedor");
            AppendLine();
            AppendText("        10.5 Ventas por Categoría");
            AppendLine();
            AppendText("        10.6 Ranking Clientes-Productos");
            AppendLine();
            AppendText("        10.7 Exportación de Reportes a CSV");
            AppendLine();
            AppendText("  11. Gestión de Usuarios y Roles");
            AppendLine();
            AppendText("        11.1 Usuarios");
            AppendLine();
            AppendText("        11.2 Roles y Permisos");
            AppendLine();
            AppendText("  12. Cambio de Idioma");
            AppendLine();
            AppendText("  13. Solución de Problemas");
            AppendLine();
            AppendText("  14. Contacto y Soporte");
            AppendLine();
            AppendLine();

            // ── 1. DESCRIPCIÓN GENERAL ────────────────────────────────────────
            AppendHeading("1.  DESCRIPCIÓN GENERAL", 13);
            AppendText("Stock Manager es un sistema integral de gestión de inventario y ventas diseñado para administrar accesorios de telefonía móvil (fundas, carcasas, protectores de pantalla, parlantes Bluetooth, entre otros artículos). La solución está desarrollada en .NET Framework 4.8 con Windows Forms y admite múltiples idiomas.");
            AppendLine();
            AppendLine();
            AppendText("Las principales capacidades del sistema incluyen:");
            AppendLine();
            AppendText("  • Control completo del catálogo de productos con SKU, categoría y precio.");
            AppendLine();
            AppendText("  • Gestión de almacenes múltiples con seguimiento de stock individualizado.");
            AppendLine();
            AppendText("  • Registro de movimientos de stock: entradas, salidas, transferencias y ajustes.");
            AppendLine();
            AppendText("  • Gestión de clientes y registro de ventas con detalle de líneas.");
            AppendLine();
            AppendText("  • Módulo de reportes analíticos con seis tipos de informes y exportación a CSV.");
            AppendLine();
            AppendText("  • Control de acceso basado en roles (RBAC) con permisos granulares.");
            AppendLine();
            AppendText("  • Interfaz en español e inglés con cambio de idioma en tiempo real.");
            AppendLine();
            AppendLine();

            // ── 2. INICIO DE SESIÓN ───────────────────────────────────────────
            AppendHeading("2.  INICIO DE SESIÓN", 13);
            AppendText("1. Al iniciar la aplicación, ingrese su ");
            AppendBold("Usuario");
            AppendText(" y su ");
            AppendBold("Contraseña");
            AppendText(" en los campos correspondientes.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Iniciar Sesión");
            AppendText(".");
            AppendLine();
            AppendText("3. Si es el primer inicio de la aplicación, el sistema solicitará configurar la contraseña del administrador antes de continuar.");
            AppendLine();
            AppendLine();
            AppendSubHeading("Credenciales predeterminadas");
            AppendText("  • Usuario: ");
            AppendBold("admin");
            AppendLine();
            AppendText("  • Contraseña: la definida durante la configuración inicial.");
            AppendLine();
            AppendLine();
            AppendText("Nota: Cada usuario solo tiene acceso a los módulos habilitados por los permisos asignados a sus roles.");
            AppendLine();
            AppendLine();

            // ── 3. MENÚ PRINCIPAL ─────────────────────────────────────────────
            AppendHeading("3.  MENÚ PRINCIPAL", 13);
            AppendText("El menú principal se organiza en las siguientes secciones. Las opciones visibles dependen de los permisos del usuario autenticado.");
            AppendLine();
            AppendLine();
            AppendSubHeading("ARCHIVO");
            AppendText("  • ");
            AppendBold("Cerrar Sesión");
            AppendText(": Cierra la sesión actual y regresa a la pantalla de inicio de sesión.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Salir");
            AppendText(": Cierra la aplicación por completo.");
            AppendLine();
            AppendLine();
            AppendSubHeading("ADMINISTRACIÓN");
            AppendText("  • ");
            AppendBold("Usuarios");
            AppendText(": Alta, baja, modificación y asignación de roles a usuarios del sistema.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Roles");
            AppendText(": Alta, baja, modificación y gestión de permisos asociados a cada rol.");
            AppendLine();
            AppendLine();
            AppendSubHeading("INVENTARIO");
            AppendText("  • ");
            AppendBold("Productos");
            AppendText(": Gestión del catálogo de productos.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Almacenes");
            AppendText(": Gestión de los depósitos o puntos de almacenamiento.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Clientes");
            AppendText(": Gestión de la cartera de clientes.");
            AppendLine();
            AppendLine();
            AppendSubHeading("OPERACIONES");
            AppendText("  • ");
            AppendBold("Ventas");
            AppendText(": Registro y consulta de operaciones de venta.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Movimientos");
            AppendText(": Entradas, salidas, transferencias y ajustes de stock.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Consultar Stock");
            AppendText(": Consulta del stock disponible con filtros por producto, almacén y categoría.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Reportes");
            AppendText(": Acceso al módulo analítico de informes.");
            AppendLine();
            AppendLine();
            AppendSubHeading("CONFIGURACIÓN");
            AppendText("  • ");
            AppendBold("Idioma");
            AppendText(": Permite alternar la interfaz entre Español e Inglés.");
            AppendLine();
            AppendLine();
            AppendSubHeading("AYUDA");
            AppendText("  • ");
            AppendBold("Manual de Uso");
            AppendText(": Abre este documento.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Acerca de...");
            AppendText(": Muestra información de la versión y créditos del sistema.");
            AppendLine();
            AppendLine();

            // ── 4. GESTIÓN DE PRODUCTOS ───────────────────────────────────────
            AppendHeading("4.  GESTIÓN DE PRODUCTOS", 13);
            AppendText("Acceso: ");
            AppendBold("Inventario > Productos");
            AppendLine();
            AppendLine();
            AppendSubHeading("Crear un producto");
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Complete los siguientes campos:");
            AppendLine();
            AppendText("     • SKU: Código único de identificación del producto (obligatorio).");
            AppendLine();
            AppendText("     • Nombre: Denominación comercial del producto (obligatorio).");
            AppendLine();
            AppendText("     • Descripción: Descripción detallada del artículo.");
            AppendLine();
            AppendText("     • Categoría: Clasificación del producto (p. ej., Fundas, Protectores).");
            AppendLine();
            AppendText("     • Precio: Precio de venta unitario (obligatorio).");
            AppendLine();
            AppendText("     • Stock Mínimo: Cantidad mínima deseada en inventario para alertas.");
            AppendLine();
            AppendText("3. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("Editar un producto");
            AppendText("1. Seleccione el producto en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Editar");
            AppendText(".");
            AppendLine();
            AppendText("3. Modifique los campos requeridos.");
            AppendLine();
            AppendText("4. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("Eliminar un producto");
            AppendText("1. Seleccione el producto en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Eliminar");
            AppendText(".");
            AppendLine();
            AppendText("3. Confirme la operación en el cuadro de diálogo.");
            AppendLine();
            AppendLine();

            // ── 5. GESTIÓN DE ALMACENES ───────────────────────────────────────
            AppendHeading("5.  GESTIÓN DE ALMACENES", 13);
            AppendText("Acceso: ");
            AppendBold("Inventario > Almacenes");
            AppendLine();
            AppendLine();
            AppendSubHeading("Crear un almacén");
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Complete los siguientes campos:");
            AppendLine();
            AppendText("     • Código: Identificador único del almacén (obligatorio).");
            AppendLine();
            AppendText("     • Nombre: Nombre descriptivo del almacén (obligatorio).");
            AppendLine();
            AppendText("     • Dirección: Ubicación física del almacén.");
            AppendLine();
            AppendText("3. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("Editar un almacén");
            AppendText("1. Seleccione el almacén en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Editar");
            AppendText(", realice las modificaciones y haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("Eliminar un almacén");
            AppendText("1. Seleccione el almacén en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Eliminar");
            AppendText(" y confirme la operación.");
            AppendLine();
            AppendLine();

            // ── 6. GESTIÓN DE CLIENTES ────────────────────────────────────────
            AppendHeading("6.  GESTIÓN DE CLIENTES", 13);
            AppendText("Acceso: ");
            AppendBold("Inventario > Clientes");
            AppendLine();
            AppendLine();
            AppendSubHeading("Crear un cliente");
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Complete los siguientes campos:");
            AppendLine();
            AppendText("     • Nombre: Nombre o razón social del cliente (obligatorio).");
            AppendLine();
            AppendText("     • Email: Dirección de correo electrónico de contacto.");
            AppendLine();
            AppendText("     • Teléfono: Número de contacto.");
            AppendLine();
            AppendText("     • Dirección: Domicilio del cliente.");
            AppendLine();
            AppendText("3. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("Editar un cliente");
            AppendText("1. Seleccione el cliente en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Editar");
            AppendText(", actualice los datos y haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("Eliminar un cliente");
            AppendText("1. Seleccione el cliente en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Eliminar");
            AppendText(" y confirme la operación.");
            AppendLine();
            AppendLine();
            AppendText("Nota: También es posible dar de alta un cliente de forma rápida desde el formulario de ventas mediante el botón ");
            AppendBold("Nuevo Cliente");
            AppendText(".");
            AppendLine();
            AppendLine();

            // ── 7. MOVIMIENTOS DE STOCK ───────────────────────────────────────
            AppendHeading("7.  MOVIMIENTOS DE STOCK", 13);
            AppendText("Acceso: ");
            AppendBold("Operaciones > Movimientos");
            AppendLine();
            AppendLine();
            AppendText("El módulo de movimientos permite registrar cualquier variación en el inventario. Cada movimiento genera un comprobante numerado con fecha, almacenes involucrados, productos, cantidades y notas.");
            AppendLine();
            AppendLine();
            AppendSubHeading("7.1  Entrada de Mercadería (Recepción)");
            AppendText("Registra el ingreso de productos a un almacén.");
            AppendLine();
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Seleccione ");
            AppendBold("Tipo: Entrada (In)");
            AppendText(".");
            AppendLine();
            AppendText("3. Seleccione el ");
            AppendBold("Almacén Destino");
            AppendText(".");
            AppendLine();
            AppendText("4. Agregue líneas de detalle con los productos recibidos:");
            AppendLine();
            AppendText("     • Producto, Cantidad, Precio Unitario.");
            AppendLine();
            AppendText("5. Ingrese notas u observaciones si corresponde.");
            AppendLine();
            AppendText("6. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("7.2  Salida de Mercadería");
            AppendText("Registra el retiro de productos de un almacén (p. ej., merma, devolución a proveedor).");
            AppendLine();
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Seleccione ");
            AppendBold("Tipo: Salida (Out)");
            AppendText(".");
            AppendLine();
            AppendText("3. Seleccione el ");
            AppendBold("Almacén Origen");
            AppendText(".");
            AppendLine();
            AppendText("4. Agregue las líneas de productos y cantidades a retirar.");
            AppendLine();
            AppendText("5. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("7.3  Transferencia entre Almacenes");
            AppendText("Permite mover stock de un almacén a otro.");
            AppendLine();
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Seleccione ");
            AppendBold("Tipo: Transferencia (Transfer)");
            AppendText(".");
            AppendLine();
            AppendText("3. Seleccione el ");
            AppendBold("Almacén Origen");
            AppendText(".");
            AppendLine();
            AppendText("4. Seleccione el ");
            AppendBold("Almacén Destino");
            AppendText(" (debe ser diferente al origen).");
            AppendLine();
            AppendText("5. Agregue las líneas de productos y cantidades a transferir.");
            AppendLine();
            AppendText("6. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendSubHeading("7.4  Ajuste de Inventario");
            AppendText("Permite corregir diferencias de inventario detectadas en recuentos físicos.");
            AppendLine();
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Seleccione ");
            AppendBold("Tipo: Ajuste (Adjustment)");
            AppendText(".");
            AppendLine();
            AppendText("3. Seleccione el almacén a ajustar.");
            AppendLine();
            AppendText("4. Agregue los productos con la cantidad de ajuste (puede ser negativa para reducir stock).");
            AppendLine();
            AppendText("5. Indique el motivo del ajuste en el campo de notas.");
            AppendLine();
            AppendText("6. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();

            // ── 8. REGISTRO DE VENTAS ─────────────────────────────────────────
            AppendHeading("8.  REGISTRO DE VENTAS", 13);
            AppendText("Acceso: ");
            AppendBold("Operaciones > Ventas");
            AppendLine();
            AppendLine();
            AppendSubHeading("Registrar una venta");
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Complete los datos del encabezado:");
            AppendLine();
            AppendText("     • Vendedor: Nombre del responsable de la venta (obligatorio).");
            AppendLine();
            AppendText("     • Cliente: Seleccione el cliente de la lista desplegable.");
            AppendLine();
            AppendText("     • Fecha: Se asigna automáticamente; puede modificarse si es necesario.");
            AppendLine();
            AppendText("     • Notas: Observaciones adicionales.");
            AppendLine();
            AppendText("3. Agregue líneas de detalle con ");
            AppendBold("Agregar Línea");
            AppendText(":");
            AppendLine();
            AppendText("     • Seleccione el Producto, ingrese la Cantidad y el Precio Unitario.");
            AppendLine();
            AppendText("     • El total de la línea se calcula automáticamente.");
            AppendLine();
            AppendText("4. Verifique que existe stock suficiente en el almacén correspondiente.");
            AppendLine();
            AppendText("5. Haga clic en ");
            AppendBold("Guardar");
            AppendText(". El sistema descuenta el stock automáticamente.");
            AppendLine();
            AppendLine();
            AppendSubHeading("Consultar detalles de una venta");
            AppendText("1. Seleccione la venta en la grilla.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Ver Detalles");
            AppendText(" para visualizar el comprobante completo con todas las líneas.");
            AppendLine();
            AppendLine();

            // ── 9. CONSULTA DE STOCK ──────────────────────────────────────────
            AppendHeading("9.  CONSULTA DE STOCK", 13);
            AppendText("Acceso: ");
            AppendBold("Operaciones > Consultar Stock");
            AppendLine();
            AppendLine();
            AppendText("Permite visualizar el stock disponible con los siguientes filtros:");
            AppendLine();
            AppendText("  • ");
            AppendBold("Por producto");
            AppendText(": Muestra el stock de un artículo específico en todos los almacenes.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Por almacén");
            AppendText(": Muestra todos los productos almacenados en un depósito.");
            AppendLine();
            AppendText("  • ");
            AppendBold("Por categoría");
            AppendText(": Filtra los productos por su clasificación.");
            AppendLine();
            AppendLine();
            AppendText("Los resultados muestran la cantidad actual por almacén e identifican los productos que se encuentran por debajo del stock mínimo configurado.");
            AppendLine();
            AppendLine();

            // ── 10. REPORTES ──────────────────────────────────────────────────
            AppendHeading("10. REPORTES", 13);
            AppendText("Acceso: ");
            AppendBold("Operaciones > Reportes");
            AppendLine();
            AppendLine();
            AppendText("El módulo de reportes provee seis informes analíticos, cada uno con filtros propios y la posibilidad de exportar los resultados a un archivo CSV. Los reportes se organizan en pestañas independientes.");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.1  Productos Más Vendidos");
            AppendText("Muestra el ranking de los productos con mayor volumen de ventas en un período.");
            AppendLine();
            AppendText("Filtros disponibles:");
            AppendLine();
            AppendText("  • Rango de fechas (Desde / Hasta).");
            AppendLine();
            AppendText("  • Categoría de producto (opcional).");
            AppendLine();
            AppendText("  • Cantidad de resultados a mostrar (Top N).");
            AppendLine();
            AppendText("  • Criterio de ordenamiento (por cantidad o por monto).");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.2  Compras por Cliente");
            AppendText("Detalla el historial de compras de cada cliente con totales acumulados.");
            AppendLine();
            AppendText("Filtros disponibles:");
            AppendLine();
            AppendText("  • Rango de fechas.");
            AppendLine();
            AppendText("  • Cliente específico (opcional; sin selección muestra todos).");
            AppendLine();
            AppendText("  • Top N clientes por monto de compras.");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.3  Variación de Precios");
            AppendText("Analiza la evolución histórica del precio de venta de los productos.");
            AppendLine();
            AppendText("Filtros disponibles:");
            AppendLine();
            AppendText("  • Rango de fechas.");
            AppendLine();
            AppendText("  • Producto específico (opcional).");
            AppendLine();
            AppendText("  • Categoría (opcional).");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.4  Ventas por Vendedor");
            AppendText("Evalúa el desempeño de cada vendedor en términos de cantidad y monto de ventas.");
            AppendLine();
            AppendText("Filtros disponibles:");
            AppendLine();
            AppendText("  • Rango de fechas.");
            AppendLine();
            AppendText("  • Nombre del vendedor (opcional).");
            AppendLine();
            AppendText("  • Categoría de producto (opcional).");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.5  Ventas por Categoría");
            AppendText("Muestra el total de ventas agrupado por categoría de producto.");
            AppendLine();
            AppendText("Filtros disponibles:");
            AppendLine();
            AppendText("  • Rango de fechas.");
            AppendLine();
            AppendText("  • Categoría específica (opcional).");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.6  Ranking Clientes-Productos");
            AppendText("Identifica qué productos son los más adquiridos por cada cliente.");
            AppendLine();
            AppendText("Filtros disponibles:");
            AppendLine();
            AppendText("  • Rango de fechas.");
            AppendLine();
            AppendText("  • Producto específico (opcional).");
            AppendLine();
            AppendText("  • Categoría (opcional).");
            AppendLine();
            AppendText("  • Top N registros.");
            AppendLine();
            AppendLine();
            AppendSubHeading("10.7  Exportación de Reportes a CSV");
            AppendText("Todos los reportes pueden exportarse a un archivo de valores separados por comas (CSV) para su análisis en hojas de cálculo externas.");
            AppendLine();
            AppendText("1. Genere el reporte con los filtros deseados.");
            AppendLine();
            AppendText("2. Haga clic en el botón ");
            AppendBold("Exportar CSV");
            AppendText(" de la pestaña correspondiente.");
            AppendLine();
            AppendText("3. Seleccione la carpeta y el nombre del archivo en el cuadro de diálogo.");
            AppendLine();
            AppendText("4. Haga clic en ");
            AppendBold("Guardar");
            AppendText(". El sistema confirmará la exportación exitosa.");
            AppendLine();
            AppendLine();

            // ── 11. GESTIÓN DE USUARIOS Y ROLES ──────────────────────────────
            AppendHeading("11. GESTIÓN DE USUARIOS Y ROLES", 13);
            AppendLine();
            AppendSubHeading("11.1  Usuarios");
            AppendText("Acceso: ");
            AppendBold("Administración > Usuarios");
            AppendLine();
            AppendLine();
            AppendText("Crear un usuario:");
            AppendLine();
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Complete los datos del usuario (nombre, nombre de usuario, email).");
            AppendLine();
            AppendText("3. Asigne una contraseña inicial.");
            AppendLine();
            AppendText("4. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendText("Editar un usuario:");
            AppendLine();
            AppendText("1. Seleccione el usuario en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Editar");
            AppendText(", modifique los datos y haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendText("Cambiar contraseña:");
            AppendLine();
            AppendText("1. Seleccione el usuario en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Cambiar Contraseña");
            AppendText(".");
            AppendLine();
            AppendText("3. Ingrese la nueva contraseña y confírmela.");
            AppendLine();
            AppendText("4. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendText("Asignar roles a un usuario:");
            AppendLine();
            AppendText("1. Seleccione el usuario en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Asignar Roles");
            AppendText(".");
            AppendLine();
            AppendText("3. Marque los roles que correspondan.");
            AppendLine();
            AppendText("4. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendText("Eliminar un usuario:");
            AppendLine();
            AppendText("1. Seleccione el usuario en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Eliminar");
            AppendText(" y confirme la operación.");
            AppendLine();
            AppendLine();
            AppendSubHeading("11.2  Roles y Permisos");
            AppendText("Acceso: ");
            AppendBold("Administración > Roles");
            AppendLine();
            AppendLine();
            AppendText("Crear un rol:");
            AppendLine();
            AppendText("1. Haga clic en ");
            AppendBold("Nuevo");
            AppendText(".");
            AppendLine();
            AppendText("2. Ingrese el nombre y la descripción del rol.");
            AppendLine();
            AppendText("3. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendText("Gestionar permisos de un rol:");
            AppendLine();
            AppendText("1. Seleccione el rol en la lista.");
            AppendLine();
            AppendText("2. Haga clic en ");
            AppendBold("Gestionar Permisos");
            AppendText(".");
            AppendLine();
            AppendText("3. Marque o desmarque los permisos según corresponda.");
            AppendLine();
            AppendText("4. Haga clic en ");
            AppendBold("Guardar");
            AppendText(".");
            AppendLine();
            AppendLine();
            AppendText("Los permisos disponibles se agrupan por módulo:");
            AppendLine();
            AppendText("  • Productos: Ver, Crear, Editar, Eliminar.");
            AppendLine();
            AppendText("  • Almacenes: Ver, Crear, Editar, Eliminar.");
            AppendLine();
            AppendText("  • Clientes: Ver, Crear, Editar, Eliminar.");
            AppendLine();
            AppendText("  • Ventas: Ver, Crear.");
            AppendLine();
            AppendText("  • Stock: Ver, Recibir, Emitir, Transferir, Ajustar.");
            AppendLine();
            AppendText("  • Reportes: Ver.");
            AppendLine();
            AppendText("  • Usuarios: Ver, Crear, Editar, Eliminar.");
            AppendLine();
            AppendText("  • Roles: Ver, Crear, Editar, Eliminar.");
            AppendLine();
            AppendLine();

            // ── 12. CAMBIO DE IDIOMA ──────────────────────────────────────────
            AppendHeading("12. CAMBIO DE IDIOMA", 13);
            AppendText("Acceso: ");
            AppendBold("Configuración > Idioma");
            AppendLine();
            AppendLine();
            AppendText("1. Seleccione ");
            AppendBold("Español");
            AppendText(" o ");
            AppendBold("English");
            AppendText(".");
            AppendLine();
            AppendText("2. La interfaz completa, incluyendo todos los formularios abiertos, se actualiza de inmediato sin necesidad de reiniciar la aplicación.");
            AppendLine();
            AppendLine();

            // ── 13. SOLUCIÓN DE PROBLEMAS ─────────────────────────────────────
            AppendHeading("13. SOLUCIÓN DE PROBLEMAS", 13);
            AppendLine();
            AppendSubHeading("El menú o módulo no está disponible");
            AppendText("  • Verifique que su cuenta tiene asignado un rol con los permisos necesarios.");
            AppendLine();
            AppendText("  • Contacte al administrador del sistema para la revisión de permisos.");
            AppendLine();
            AppendLine();
            AppendSubHeading("Stock insuficiente al registrar venta o salida");
            AppendText("  • Consulte el stock disponible en ");
            AppendBold("Operaciones > Consultar Stock");
            AppendText(".");
            AppendLine();
            AppendText("  • Registre una entrada de mercadería si el stock es insuficiente.");
            AppendLine();
            AppendLine();
            AppendSubHeading("Error al guardar un registro");
            AppendText("  • Verifique que todos los campos obligatorios estén completos.");
            AppendLine();
            AppendText("  • Asegúrese de que los valores ingresados sean válidos (p. ej., precio mayor a cero).");
            AppendLine();
            AppendText("  • Si el problema persiste, contacte al soporte técnico.");
            AppendLine();
            AppendLine();
            AppendSubHeading("La exportación a CSV no genera el archivo");
            AppendText("  • Verifique que la carpeta de destino seleccionada existe y tiene permisos de escritura.");
            AppendLine();
            AppendText("  • Asegúrese de que el reporte contiene datos antes de exportar.");
            AppendLine();
            AppendLine();
            AppendSubHeading("El cambio de idioma no se aplica correctamente");
            AppendText("  • Cierre y vuelva a abrir el formulario afectado.");
            AppendLine();
            AppendText("  • Si el problema continúa, cierre sesión y vuelva a ingresar.");
            AppendLine();
            AppendLine();

            // ── 14. CONTACTO Y SOPORTE ────────────────────────────────────────
            AppendHeading("14. CONTACTO Y SOPORTE", 13);
            AppendText("Para soporte técnico, consultas o reporte de incidencias:");
            AppendLine();
            AppendText("  • Comuníquese con el administrador del sistema.");
            AppendLine();
            AppendText("  • Consulte la documentación técnica disponible en la carpeta ");
            AppendBold("Documentation");
            AppendText(" del proyecto.");
            AppendLine();
            AppendLine();

            // Footer
            AppendText("───────────────────────────────────────────────────────────────");
            AppendLine();
            AppendText("© 2026 — Stock Manager — Sistema Académico");

            // Scroll to top
            txtManualContent.SelectionStart = 0;
            txtManualContent.SelectionLength = 0;
            txtManualContent.ScrollToCaret();
        }

        /// <summary>
        /// Agrega un título formateado al contenido del manual
        /// </summary>
        /// <param name="text">Texto del título</param>
        /// <param name="fontSize">Tamaño de fuente en puntos</param>
        /// <param name="bold">Indica si el texto debe ser negrita</param>
        private void AppendTitle(string text, int fontSize, bool bold)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", fontSize, bold ? FontStyle.Bold : FontStyle.Regular);
            txtManualContent.SelectionColor = Color.DarkBlue;
            txtManualContent.Select(txtManualContent.TextLength, 0);
        }

        /// <summary>
        /// Agrega un encabezado de sección formateado al contenido del manual
        /// </summary>
        /// <param name="text">Texto del encabezado</param>
        /// <param name="fontSize">Tamaño de fuente en puntos</param>
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

        /// <summary>
        /// Agrega un subencabezado formateado al contenido del manual
        /// </summary>
        /// <param name="text">Texto del subencabezado</param>
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

        /// <summary>
        /// Agrega texto en negrita al contenido del manual
        /// </summary>
        /// <param name="text">Texto a agregar en negrita</param>
        private void AppendBold(string text)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            txtManualContent.SelectionColor = Color.Black;
            txtManualContent.Select(txtManualContent.TextLength, 0);
        }

        /// <summary>
        /// Agrega texto normal al contenido del manual
        /// </summary>
        /// <param name="text">Texto a agregar</param>
        private void AppendText(string text)
        {
            int start = txtManualContent.TextLength;
            txtManualContent.AppendText(text);
            txtManualContent.Select(start, text.Length);
            txtManualContent.SelectionFont = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            txtManualContent.SelectionColor = Color.Black;
            txtManualContent.Select(txtManualContent.TextLength, 0);
        }

        /// <summary>
        /// Agrega un salto de línea al contenido del manual
        /// </summary>
        private void AppendLine()
        {
            txtManualContent.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Maneja el evento Click del botón Cerrar para cerrar el formulario
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
