# Guía de Ayuda Interactiva — Stock Manager

## Tabla de Contenidos

1. [Qué se agregó y por qué](#1-qué-se-agregó-y-por-qué)
2. [Estructura de carpetas /Help](#2-estructura-de-carpetas-help)
3. [Cómo editar la guía y previsualizarla](#3-cómo-editar-la-guía-y-previsualizarla)
4. [Empaquetado offline (CopyToOutputDirectory)](#4-empaquetado-offline-copytooutputdirectory)
5. [Requisitos de WebView2 Runtime](#5-requisitos-de-webview2-runtime)
6. [Instrucciones para Desarrolladores](#6-instrucciones-para-desarrolladores)
7. [Instrucciones para QA — Checklist](#7-instrucciones-para-qa--checklist)
8. [Consideraciones de Accesibilidad](#8-consideraciones-de-accesibilidad)
9. [Notas de Mantenimiento y Extensión](#9-notas-de-mantenimiento-y-extensión)

---

## 1. Qué se agregó y por qué

### Resumen de cambios

| Archivo | Cambio |
|---|---|
| `UI/Forms/UserManualForm.cs` | Reescrito: usa WebView2 en lugar de RichTextBox |
| `UI/Forms/UserManualForm.Designer.cs` | Actualizado: control WebView2 en vez de RichTextBox + Label |
| `UI/Form1.cs` | Abre ayuda como ventana flotante; añade atajo F1 |
| `UI/UI.csproj` | PackageReference a WebView2 + copia de archivos Help/ |
| `UI/Help/index.html` | Nueva guía interactiva HTML |
| `UI/Help/css/styles.css` | Estilos locales (sin CDN) |
| `UI/Help/js/app.js` | Buscador, acordeones, navegación lateral |

### Decisión técnica: WebView2 sobre WebBrowser

Se utilizó **Microsoft Edge WebView2** (`Microsoft.Web.WebView2` NuGet) en lugar del control clásico `WebBrowser` por las siguientes razones:

| Criterio | WebBrowser (IE) | WebView2 (Edge) |
|---|---|---|
| Motor de renderizado | Internet Explorer (Trident) | Chromium (moderno) |
| Soporte CSS/JS moderno | Muy limitado | Completo (ES2022+, Flexbox, Grid…) |
| Seguridad | Obsoleto, sin parches | Actualizado con Edge |
| Modo offline | Sí | Sí |
| Soporte .NET Framework 4.8 | Sí | Sí (WebView2 ≥ 1.0.x) |
| Mantenimiento | Deprecado | Activo (Microsoft) |

> **Decisión**: Se usa WebView2 Evergreen (ver sección 5). El control `WebBrowser` **no** se utiliza en esta implementación.

---

## 2. Estructura de carpetas /Help

```
UI/
└── Help/
    ├── index.html          ← Punto de entrada de la guía
    ├── css/
    │   └── styles.css      ← Todos los estilos (sin CDN externo)
    ├── js/
    │   └── app.js          ← Navegación, buscador, acordeones
    └── assets/             ← Carpeta para imágenes e íconos futuros
```

### Secciones de la guía

| ID de sección | Título |
|---|---|
| `sec-intro` | Introducción |
| `sec-requirements` | Requisitos del Sistema |
| `sec-getting-started` | Primeros Pasos |
| `sec-screens` | Pantallas Principales |
| `sec-faq` | Preguntas Frecuentes (FAQ) |
| `sec-troubleshooting` | Solución de Problemas |
| `sec-shortcuts` | Atajos de Teclado |
| `sec-contact` | Contacto / Soporte |

---

## 3. Cómo editar la guía y previsualizarla

### Editar contenido HTML

1. Abra `UI/Help/index.html` con cualquier editor de texto (VS Code, Notepad++, etc.).
2. Cada sección está marcada con `<section id="sec-XXX" class="help-section">`.
3. Para agregar una nueva sección:
   - Añada el `<section>` con un `id` único.
   - Agregue el link correspondiente en el `<ul id="nav-list">`.
   - Registre la sección en el array `SECTIONS` dentro de `js/app.js` para que el buscador la incluya.

### Editar estilos

Edite `UI/Help/css/styles.css`. Todos los estilos son locales. No hay dependencias externas.

### Previsualización local (sin ejecutar la app)

Abra `UI/Help/index.html` directamente en **Microsoft Edge**. Chrome y Firefox pueden bloquear acceso a archivos locales por defecto; Edge y el modo de archivo de Edge funcionan correctamente.

Alternativamente, use una extensión de servidor local como **Live Server** en VS Code para previsualizar desde `localhost`.

> **Nota**: WebView2 carga los archivos vía `file://` con los mismos permisos que Edge. No hay restricciones de CORS para archivos locales dentro de la misma carpeta.

---

## 4. Empaquetado offline (CopyToOutputDirectory)

Los archivos de `Help/` se copian automáticamente al directorio de salida gracias a esta configuración en `UI/UI.csproj`:

```xml
<ItemGroup>
  <Content Include="Help\**\*">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

- **`PreserveNewest`**: solo copia si el archivo origen es más nuevo que el destino (eficiente).
- **`Always`**: alternativa para forzar la copia en cada build (útil en desarrollo intensivo).

### Resultado en el output

```
bin/Debug/
├── UI.exe
├── Help/
│   ├── index.html
│   ├── css/styles.css
│   ├── js/app.js
│   └── assets/
└── ...
```

### Publicación con instalador (Setup project)

Si se usa un proyecto de Setup (ej. `Sistema_gestion_setup`), asegúrese de que la carpeta `Help/` esté incluida en el installer con la misma estructura relativa al ejecutable.

---

## 5. Requisitos de WebView2 Runtime

### Evergreen Runtime vs. Fixed Version Runtime

| Modo | Descripción | Pros | Contras |
|---|---|---|---|
| **Evergreen** (predeterminado) | El runtime se instala a nivel de sistema y se actualiza automáticamente junto con Microsoft Edge | Siempre actualizado, pequeño instalador | Requiere instalación previa o descarga |
| **Fixed Version** | Se incluye una versión específica del runtime dentro de la carpeta de la aplicación | Sin dependencia de versión externa | Ocupa ~150–200 MB adicionales en el deploy; no recibe actualizaciones automáticas |

### Decisión del proyecto

**Se usa Evergreen Runtime** (opción predeterminada de `EnsureCoreWebView2Async(null)`). En Windows 10/11 con actualizaciones recientes, el runtime ya está instalado como parte de Microsoft Edge.

### Cómo cambiar a Fixed Version Runtime

Si se desea empaquetar la ayuda sin depender de Edge instalado en el equipo:

1. Descargue la versión fija desde [aka.ms/webview2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/).
2. Copie la carpeta del runtime junto al ejecutable (ej. `bin/Debug/WebView2Runtime/`).
3. Pase la ruta al inicializar:

```csharp
var env = await CoreWebView2Environment.CreateAsync(
    browserExecutableFolder: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebView2Runtime"),
    userDataFolder: null,
    options: null);
await webView.EnsureCoreWebView2Async(env);
```

---

## 6. Instrucciones para Desarrolladores

### Ejecutar el proyecto

1. Abra `tp_diploma_nk_2026.sln` en Visual Studio 2019/2022.
2. Asegúrese de que `UI` es el proyecto de inicio.
3. Restaure los paquetes NuGet: **clic derecho en solución → Restore NuGet Packages**.
4. Compile y ejecute (`F5`).

### Restauración del paquete WebView2

El paquete `Microsoft.Web.WebView2` se descarga automáticamente desde NuGet.org al restaurar. No requiere acción manual.

### Depurar el formulario de ayuda

Para inspeccionar el HTML/CSS/JS dentro del WebView2:

1. Abra `UserManualForm` en la app.
2. Haga **clic derecho** dentro del WebView → **Inspeccionar** (si el modo de depuración está habilitado).
3. Para habilitar DevTools en código:

```csharp
// En UserManualForm_Load, después de EnsureCoreWebView2Async:
#if DEBUG
webView.CoreWebView2.OpenDevToolsWindow();
#endif
```

### Agregar nuevas secciones

1. En `index.html`: agregue un `<section id="sec-nueva" class="help-section">` con su contenido.
2. En `index.html`: añada `<li><a href="#sec-nueva" data-section="sec-nueva">Título</a></li>` en el `#nav-list`.
3. En `js/app.js`: agregue al array `SECTIONS`:

```javascript
{ id: "sec-nueva", title: "Nueva Sección", keywords: "palabras clave para búsqueda" }
```

---

## 7. Instrucciones para QA — Checklist

### Verificación básica

- [ ] Al hacer clic en **Ayuda → Guía de usuario**, se abre una ventana flotante (no MDI).
- [ ] La ventana muestra la guía HTML correctamente (sidebar, secciones, colores).
- [ ] Todas las secciones del sidebar (8 total) responden al clic y muestran contenido.
- [ ] El buscador devuelve resultados relevantes para términos como "stock", "usuario", "contraseña".
- [ ] Al limpiar el buscador (o presionar Esc), vuelve la vista principal.
- [ ] Los acordeones (▼) en "Pantallas Principales" y otras secciones se abren/cierran correctamente.
- [ ] El botón **Cerrar** cierra la ventana.
- [ ] Presionar **F1** desde el formulario principal abre la guía.
- [ ] Si la guía ya está abierta y se presiona F1 o se hace clic en el menú, la ventana se trae al frente (no abre duplicada).

### Verificación offline

- [ ] Desconectar la red (modo avión o deshabilitar adaptador).
- [ ] Abrir la guía: debe funcionar igual que con red.
- [ ] Verificar que ningún recurso externo falla (no debe haber intentos de carga de CDN).

### Verificación de errores

- [ ] Renombrar temporalmente la carpeta `Help/` en el directorio de salida → al abrir la guía debe aparecer un `MessageBox` indicando que faltan los archivos.
- [ ] (Si es posible en un equipo de test) Desinstalar WebView2 Runtime → al abrir la guía debe aparecer un `MessageBox` con instrucciones de instalación.

### Verificación del paquete de salida

- [ ] Compilar en modo **Debug** → verificar que `bin/Debug/Help/` contiene `index.html`, `css/styles.css`, `js/app.js`.
- [ ] Compilar en modo **Release** → verificar que `bin/Release/Help/` tiene los mismos archivos.

---

## 8. Consideraciones de Accesibilidad

- **Contraste de colores**: El diseño cumple relaciones de contraste mínimas (WCAG 2.1 AA). El texto principal (`#1a1a2e`) sobre fondo blanco/claro supera 7:1.
- **Tamaño de fuente base**: 16 px (`1rem`), expandible mediante el zoom del navegador (Ctrl + / -).
- **Navegación por teclado**:
  - Todos los elementos interactivos (links del sidebar, botones de acordeón, resultados de búsqueda) son accesibles con `Tab`.
  - Los acordeones tienen atributo `aria-expanded` correctamente actualizado.
  - Los botones de acordeón son `<button>` nativos, activables con `Enter` / `Space`.
  - Los resultados de búsqueda tienen `role="button"` y `tabindex="0"`, activables con `Enter` / `Space`.
- **Atributos ARIA**: Regiones, listas de nav y barra de búsqueda tienen `aria-label` descriptivos. El overlay de búsqueda usa `aria-live="polite"`.
- **Foco visible**: Los estilos incluyen `focus-visible` para indicar foco con teclado.

---

## 9. Notas de Mantenimiento y Puntos de Extensión

### Mantenimiento periódico

- Actualice el contenido de `index.html` cuando se agreguen nuevas funcionalidades a la app.
- Actualice el paquete NuGet `Microsoft.Web.WebView2` según el ciclo de actualizaciones del proyecto.

### Puntos de extensión

| Característica | Cómo implementarla |
|---|---|
| Ayuda contextual por pantalla | Pasar un parámetro `sectionId` al constructor de `UserManualForm` y navegar a esa sección tras la carga (`webView.CoreWebView2.ExecuteScriptAsync("showSection('sec-xxx')")`) |
| Soporte multiidioma en la guía | Crear `Help/index.es.html` y `Help/index.en.html`; en `UserManualForm`, elegir el archivo según `Thread.CurrentUICulture` |
| Imágenes y capturas de pantalla | Colocar en `Help/assets/` y referenciar desde HTML con rutas relativas (`<img src="../assets/captura.png" alt="..."/>`) |
| Impresión | Agregar un botón que llame a `webView.CoreWebView2.ExecuteScriptAsync("window.print()")` |
| PDF export | Usar `webView.CoreWebView2.PrintToPdfAsync(filePath)` disponible en WebView2 1.0.1108+ |
