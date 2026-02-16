# Manual de Uso - Guía Visual

## Estructura del Menú

```
┌─────────────────────────────────────────────────────────────┐
│ Archivo  Administración  Inventario  Operaciones  Configuración  Ayuda │
└─────────────────────────────────────────────────────────────┘
                                                              │
                                                              ▼
                                                    ┌──────────────────┐
                                                    │ Manual de Uso    │
                                                    ├──────────────────┤
                                                    │ Acerca de...     │
                                                    └──────────────────┘
```

## Ventana del Manual de Usuario

```
┌─────────────────────────────────────────────────────────────────────────┐
│ Manual de Usuario                                                    [X]│
├─────────────────────────────────────────────────────────────────────────┤
│ Manual de Usuario - Stock Manager                                       │
├─────────────────────────────────────────────────────────────────────────┤
│ ┌─────────────────────────────────────────────────────────────────────┐ │
│ │ # MANUAL DE USUARIO - STOCK MANAGER                               ▲│ │
│ │                                                                     ││ │
│ │ ## DESCRIPCIÓN GENERAL                                            ││ │
│ │ Stock Manager es un sistema de gestión de inventario...           ││ │
│ │                                                                     ││ │
│ │ ## INICIO DE SESIÓN                                               ││ │
│ │                                                                     ││ │
│ │ 1. Al iniciar la aplicación, ingrese su Usuario y Contraseña     ││ │
│ │ 2. Haga clic en Iniciar Sesión                                    ││ │
│ │ ...                                                                ││ │
│ │                                                                     ││ │
│ │ ## MENÚ PRINCIPAL                                                 ││ │
│ │                                                                     ││ │
│ │ ### ARCHIVO                                                       ││ │
│ │ - Cerrar Sesión: Cierra la sesión actual                         ││ │
│ │ - Salir: Cierra la aplicación                                    ││ │
│ │                                                                     ││ │
│ │ ### ADMINISTRACIÓN                                                ││ │
│ │ - Usuarios: Gestionar usuarios del sistema                       ││ │
│ │ - Roles: Gestionar roles y permisos                              ││ │
│ │                                                                     ││ │
│ │ [... contenido continúa ...]                                      ▼│ │
│ └─────────────────────────────────────────────────────────────────────┘ │
│                                                                  [Cerrar]│
└─────────────────────────────────────────────────────────────────────────┘
```

## Características de la Ventana

### Dimensiones
- Ancho: 884 píxeles
- Alto: 626 píxeles
- Posición: Centrada en la ventana padre

### Controles

1. **lblTitle** (Label)
   - Texto: "Manual de Usuario - Stock Manager"
   - Fuente: Microsoft Sans Serif, 12pt, Bold
   - Posición: Superior izquierda

2. **txtManualContent** (TextBox)
   - Modo: Solo lectura
   - Fuente: Consolas, 9pt
   - ScrollBars: Vertical y Horizontal
   - WordWrap: Desactivado
   - Contenido: Manual completo en formato de texto

3. **btnClose** (Button)
   - Texto: "Cerrar"
   - Posición: Inferior derecha
   - Acción: Cierra la ventana

### Comportamiento

- La ventana se abre como MDI Child dentro de la ventana principal
- El contenido es de solo lectura
- Se puede desplazar vertical y horizontalmente
- Al cerrar, vuelve a la ventana principal

## Acceso al Manual

### Método 1: Menú
1. Clic en **Ayuda** en la barra de menú
2. Clic en **Manual de Uso**

### Método 2: Teclado
1. Presionar **Alt + Y** (abre el menú Ayuda)
2. Presionar **M** (selecciona Manual de Uso)

## Traducción del Menú

### Español
```
Ayuda
  ├─ Manual de Uso
  └─ Acerca de...
```

### English
```
Help
  ├─ User Manual
  └─ About...
```

## Estados de la Ventana

### Estado Normal
- Ventana abierta mostrando el contenido del manual
- Barra de desplazamiento visible
- Botón Cerrar habilitado

### Ventana No Maximizable
- No se puede maximizar (MaximizeBox = false)
- No se puede minimizar (MinimizeBox = false)
- Solo se puede cerrar

## Integración con el Sistema

La ventana del manual se integra perfectamente con:

1. **Sistema de Localización**: El título del menú cambia según el idioma seleccionado
2. **MDI Parent**: Se abre dentro de la ventana principal como ventana hijo
3. **Consistencia Visual**: Utiliza el mismo estilo que otras ventanas del sistema

## Contenido del Manual

El manual incluye información sobre:

- Descripción del sistema
- Instrucciones de inicio de sesión
- Navegación por el menú principal
- Gestión de productos, almacenes y clientes
- Operaciones de stock (entrada, salida, transferencia, ajuste)
- Registro de ventas
- Consulta de stock
- Generación de reportes
- Gestión de usuarios y roles
- Cambio de idioma
- Solución de problemas comunes

---

**Nota**: Esta es una representación visual en texto. En la aplicación real, los elementos se muestran con los controles nativos de Windows Forms.
