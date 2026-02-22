# Build Troubleshooting Guide

## Error: Cannot Copy DLL Files During Build (MSB3027/MSB3021)

### Síntomas / Symptoms
Al intentar compilar el proyecto UI, aparecen errores como:
```
error MSB3027: No se pudo copiar "...\BLL.dll" en "bin\Debug\BLL.dll"
error MSB3021: El proceso no puede obtener acceso al archivo 'bin\Debug\BLL.dll' 
porque está siendo utilizado en otro proceso.
```

When trying to build the UI project, errors appear like:
```
error MSB3027: Could not copy "...\BLL.dll" to "bin\Debug\BLL.dll"
error MSB3021: The process cannot access the file 'bin\Debug\BLL.dll' 
because it is being used by another process.
```

### Causa / Cause
Los archivos DLL están bloqueados porque la aplicación UI.exe todavía está en ejecución.
The DLL files are locked because the UI.exe application is still running.

### Soluciones / Solutions

#### Solución 1: Cerrar la Aplicación / Close the Application
1. Cierre todas las ventanas de la aplicación Stock Manager
2. Close all Stock Manager application windows

#### Solución 2: Detener Depuración / Stop Debugging
1. En Visual Studio, presione `Shift + F5` para detener la depuración
2. In Visual Studio, press `Shift + F5` to stop debugging

#### Solución 3: Finalizar Proceso Manualmente / End Process Manually
1. Abra el Administrador de Tareas (Ctrl + Shift + Esc)
2. Busque el proceso "UI.exe" o "UI"
3. Haga clic derecho y seleccione "Finalizar tarea"

Or:
1. Open Task Manager (Ctrl + Shift + Esc)
2. Look for "UI.exe" or "UI" process
3. Right-click and select "End task"

#### Solución 4: Limpiar y Reconstruir / Clean and Rebuild
Si el problema persiste / If the problem persists:
1. En Visual Studio: Build → Clean Solution
2. Espere a que termine / Wait for it to complete
3. Build → Rebuild Solution

### Prevención / Prevention

#### Mejores Prácticas / Best Practices
1. **Siempre cierre la aplicación antes de recompilar**
   Always close the application before rebuilding

2. **Use "Stop Debugging" en lugar de cerrar la ventana**
   Use "Stop Debugging" instead of closing the window

3. **Configure Visual Studio para detener automáticamente al editar**
   Configure Visual Studio to stop automatically when editing:
   - Tools → Options → Debugging
   - Enable "Stop debugging when closing debugger"

4. **Verifique procesos antes de compilar**
   Check processes before building:
   ```
   tasklist | findstr UI.exe
   ```

### Problemas Relacionados / Related Issues

#### La aplicación no se cierra completamente
The application doesn't close completely

**Posibles causas / Possible causes:**
- Hilos en segundo plano sin terminar / Background threads not finishing
- Recursos no liberados / Resources not released
- Conexiones de base de datos abiertas / Open database connections

**Verificación / Verification:**
- Revise que todos los formularios implementen Dispose correctamente
- Check that all forms implement Dispose properly
- Asegúrese de que las conexiones a BD se cierren
- Ensure database connections are closed
- Verifique que no haya timers o tareas asíncronas ejecutándose
- Verify there are no timers or async tasks running

### Contacto / Contact
Si el problema persiste después de seguir estos pasos, por favor:
- Revise los logs de la aplicación en la carpeta `Logs/`
- Documente el error exacto y los pasos para reproducirlo
- Contacte al equipo de desarrollo

If the problem persists after following these steps, please:
- Check application logs in `Logs/` folder
- Document the exact error and steps to reproduce
- Contact the development team
