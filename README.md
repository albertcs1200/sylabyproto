# SylabyProto

Proyecto ASP.NET Core MVC con Identity y SQLite para gestión de cumplimiento de sílabos, propuestas de actualización y roles director/docente.

## Descripción

Esta aplicación contiene:

- Inicio de sesión con Identity
- Roles `Director` y `Docente`
- Registro y control de cumplimiento de sílabo
- Propuestas de actualización de sílabo
- Control de acceso restringido según rol

## Requisitos

- .NET SDK 8.0 o compatible
- Visual Studio 2022/2023, VS Code o editor compatible
- SQLite (se usa automáticamente desde el proyecto)

## Ejecución

1. Abre una terminal en la raíz del proyecto:

   ```powershell
   cd "c:\Users\Albert\Documents\protipo sprint tareas"
   ```

2. Restaurar paquetes y ejecutar:

   ```powershell
   dotnet restore
   dotnet run
   ```

3. Abre el navegador en la URL que indique la salida, normalmente:
   ```text
   https://localhost:5001
   ```

## Usuarios por defecto

La aplicación crea automáticamente dos usuarios al iniciar por primera vez:

- Director:
  - Email: `director@sprint.com`
  - Contraseña: `Director123!`
- Docente:
  - Email: `docente@sprint.com`
  - Contraseña: `Docente123!`

## Pruebas principales

### 1. Probar login correcto

- Ingresa con `director@sprint.com` / `Director123!`
- Ingresa con `docente@sprint.com` / `Docente123!`
- Verifica que el inicio de sesión se realiza sin errores.

### 2. Probar error de credenciales

- Ingresa con un email o contraseña incorrectos.
- Verifica que se muestra un mensaje de error y no permite el acceso.

### 3. Probar acceso restringido

- Accede a rutas o acciones que requieran rol específico.
- Verifica que un usuario sin el rol adecuado no puede acceder.
- Por ejemplo, sólo el `Director` debe poder enviar propuestas y ver controles administrativos.

## Rutas importantes

- `/Account/Login` — Inicio de sesión
- `/Account/Register` — Registro de usuarios
- `/Syllabus/Index` — Lista de sílabos
- `/Syllabus/AdminPanel` — Panel administrativo (requiere rol)
- `/Syllabus/CreateProposal` — Formulario de propuestas
- `/Syllabus/RegisterCompliance` — Formulario de cumplimiento

## Notas

- La base de datos SQLite se crea automáticamente desde `ApplicationDbContext`.
- No es necesario crear manualmente la base de datos.
- Si se desea cambiar la conexión, revisa `appsettings.json`.
