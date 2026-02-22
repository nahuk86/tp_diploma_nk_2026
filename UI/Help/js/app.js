/**
 * Stock Manager — Guía de Usuario Interactiva
 * app.js — Navegación, buscador y acordeones (100% local, sin CDN)
 */

(function () {
    "use strict";

    // -------------------------------------------------------
    // Datos de búsqueda: cada sección tiene su id, título y
    // palabras clave para el buscador simple.
    // -------------------------------------------------------
    var SECTIONS = [
        {
            id: "sec-intro",
            title: "Introducción",
            keywords: "introduccion stock manager sistema gestion inventario accesorios celulares descripcion general"
        },
        {
            id: "sec-requirements",
            title: "Requisitos del Sistema",
            keywords: "requisitos sistema windows net framework hardware software instalacion webview2 edge runtime"
        },
        {
            id: "sec-getting-started",
            title: "Primeros Pasos",
            keywords: "primeros pasos inicio sesion login usuario contraseña admin primer inicio configuracion"
        },
        {
            id: "sec-screens",
            title: "Pantallas Principales",
            keywords: "pantallas principales flujo formularios productos almacenes clientes ventas movimientos stock reportes"
        },
        {
            id: "sec-faq",
            title: "Preguntas Frecuentes (FAQ)",
            keywords: "faq preguntas frecuentes no puedo ver menu permisos stock insuficiente error guardar idioma"
        },
        {
            id: "sec-troubleshooting",
            title: "Solución de Problemas",
            keywords: "solucion problemas error base datos conexion contraseña olvidada permisos soporte tecnico"
        },
        {
            id: "sec-shortcuts",
            title: "Atajos de Teclado",
            keywords: "atajos teclado shortcuts F1 ayuda ctrl s guardar escape cerrar navegacion accesibilidad"
        },
        {
            id: "sec-contact",
            title: "Contacto / Soporte",
            keywords: "contacto soporte administrador sistema ayuda tecnica"
        }
    ];

    // -------------------------------------------------------
    // DOM refs
    // -------------------------------------------------------
    var navLinks       = null;
    var allSections    = null;
    var searchInput    = null;
    var resultsOverlay = null;
    var mainContent    = null;

    // -------------------------------------------------------
    // Navigation
    // -------------------------------------------------------
    function showSection(sectionId) {
        // Hide search overlay
        resultsOverlay.classList.remove("visible");
        mainContent.style.display = "";

        // Update sections visibility
        allSections.forEach(function (sec) {
            if (sec.id === sectionId) {
                sec.classList.add("visible");
            } else {
                sec.classList.remove("visible");
            }
        });

        // Update active link
        navLinks.forEach(function (a) {
            if (a.getAttribute("data-section") === sectionId) {
                a.classList.add("active");
            } else {
                a.classList.remove("active");
            }
        });

        // Scroll main content to top
        var mc = document.getElementById("main-content");
        if (mc) mc.scrollTop = 0;

        // Update URL hash without page reload
        history.replaceState(null, "", "#" + sectionId);
    }

    // -------------------------------------------------------
    // Accordion
    // -------------------------------------------------------
    function initAccordions() {
        var headers = document.querySelectorAll(".accordion-header");
        headers.forEach(function (btn) {
            btn.addEventListener("click", function () {
                var isExpanded = btn.getAttribute("aria-expanded") === "true";
                var body = btn.nextElementSibling;

                if (isExpanded) {
                    btn.setAttribute("aria-expanded", "false");
                    body.classList.remove("open");
                } else {
                    btn.setAttribute("aria-expanded", "true");
                    body.classList.add("open");
                }
            });
        });
    }

    // -------------------------------------------------------
    // Search
    // -------------------------------------------------------
    function escapeHtml(str) {
        return str.replace(/[&<>"']/g, function (c) {
            return {"&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#39;"}[c];
        });
    }

    function highlight(text, query) {
        if (!query) return escapeHtml(text);
        var re = new RegExp("(" + query.replace(/[.*+?^${}()|[\]\\]/g, "\\$&") + ")", "gi");
        return escapeHtml(text).replace(re, "<mark>$1</mark>");
    }

    function doSearch(raw) {
        var query = raw.trim().toLowerCase();

        if (query.length < 2) {
            resultsOverlay.classList.remove("visible");
            mainContent.style.display = "";
            return;
        }

        // Hide main content
        mainContent.style.display = "none";
        resultsOverlay.classList.add("visible");

        var container = document.getElementById("search-results-list");
        container.innerHTML = "";

        var matches = SECTIONS.filter(function (s) {
            return s.title.toLowerCase().indexOf(query) !== -1 ||
                   s.keywords.toLowerCase().indexOf(query) !== -1;
        });

        if (matches.length === 0) {
            container.innerHTML = '<p id="no-results">No se encontraron resultados para &ldquo;' +
                escapeHtml(raw) + '&rdquo;.</p>';
            return;
        }

        matches.forEach(function (s) {
            var div = document.createElement("div");
            div.className = "search-result-item";
            div.setAttribute("tabindex", "0");
            div.setAttribute("role", "button");
            div.setAttribute("aria-label", "Ir a sección: " + s.title);
            div.innerHTML =
                "<h3>" + highlight(s.title, raw) + "</h3>" +
                "<p>Haz clic para ir a esta sección.</p>";

            div.addEventListener("click", function () {
                searchInput.value = "";
                showSection(s.id);
            });
            div.addEventListener("keydown", function (e) {
                if (e.key === "Enter" || e.key === " ") {
                    e.preventDefault();
                    searchInput.value = "";
                    showSection(s.id);
                }
            });

            container.appendChild(div);
        });
    }

    // -------------------------------------------------------
    // Init
    // -------------------------------------------------------
    function init() {
        navLinks       = Array.prototype.slice.call(document.querySelectorAll("#nav-list a"));
        allSections    = Array.prototype.slice.call(document.querySelectorAll(".help-section"));
        searchInput    = document.getElementById("search-input");
        resultsOverlay = document.getElementById("search-results-overlay");
        mainContent    = document.getElementById("main-content");

        // Wire nav links
        navLinks.forEach(function (a) {
            a.addEventListener("click", function (e) {
                e.preventDefault();
                showSection(a.getAttribute("data-section"));
            });
        });

        // Wire search
        searchInput.addEventListener("input", function () {
            doSearch(searchInput.value);
        });

        searchInput.addEventListener("keydown", function (e) {
            if (e.key === "Escape") {
                searchInput.value = "";
                resultsOverlay.classList.remove("visible");
                mainContent.style.display = "";
            }
        });

        // Accordions
        initAccordions();

        // Load section from hash or default
        var hash = window.location.hash.replace("#", "");
        var initial = (hash && document.getElementById(hash)) ? hash : "sec-intro";
        showSection(initial);
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }
})();
