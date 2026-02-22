# 🚀 Guía Rápida: Habilitar Transferencias y Entradas de Stock

## ⚡ Solución en 3 Pasos

### 1️⃣ Ejecutar Script SQL
```sql
-- Abrir SQL Server Management Studio
-- Conectar a su servidor
-- Abrir y ejecutar: Database/03_UpdatePermissions.sql
```

### 2️⃣ Reiniciar Aplicación
```
Cerrar la aplicación completamente
Volver a abrirla
```

### 3️⃣ Cerrar y Abrir Sesión
```
Cerrar sesión en la aplicación
Iniciar sesión nuevamente
```

## ✅ Verificación Rápida

### ¿Funciona?
1. Iniciar sesión con usuario **WarehouseOperator**
2. Ir a: **Operaciones > Movimientos**
3. El menú debe estar **habilitado** (no gris)
4. Hacer clic en **Nuevo**
5. El botón debe estar **habilitado** (no gris)

### Si NO funciona:
- ¿Ejecutó el script SQL? → Volver al paso 1
- ¿Reinició la aplicación? → Volver al paso 2
- ¿Cerró y abrió sesión? → Volver al paso 3

## 📝 Cómo Usar

### Entrada de Stock (Recepción)
```
Operaciones > Movimientos > Nuevo
Tipo: In
Almacén Destino: [Seleccionar]
Productos: [Agregar líneas]
Guardar
```

### Transferencia entre Almacenes
```
Operaciones > Movimientos > Nuevo
Tipo: Transfer
Almacén Origen: [Seleccionar]
Almacén Destino: [Seleccionar diferente]
Productos: [Agregar líneas]
Guardar
```

## 🔧 Archivos Importantes

| Archivo | Para Qué |
|---------|----------|
| `Database/03_UpdatePermissions.sql` | Script para actualizar BD existente |
| `ACTIVATION_GUIDE_ES.md` | Guía completa en español |
| `FIX_SUMMARY_ES.md` | Resumen del fix implementado |

## 🆘 Ayuda

### Problema: "Botón Nuevo deshabilitado"
**Solución**: Cerrar sesión y volver a iniciar sesión

### Problema: "No veo el menú Movimientos"
**Solución**: Verificar que el usuario tiene rol WarehouseOperator o superior

### Problema: "Stock insuficiente"
**Solución**: Primero hacer una entrada de stock, luego la transferencia

## 📞 Más Información

Para detalles completos, ver: **ACTIVATION_GUIDE_ES.md**

---

**¿Todo listo?** → Comience con una **Entrada de Stock** de prueba para verificar que funciona! 🎉
