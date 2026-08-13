# 🛠️ Skvia Base Template API (.NET 10 Web API)

Plantilla backend empresarial desarrollada en .NET 10 con Clean Architecture, CQRS, ASP.NET Core Identity, JWT + Refresh Tokens, EF Core Interceptors, ErrorOr y Migraciones.

---

## 🏛️ Arquitectura y Estructura

```text
src/
├── WorkOrderManagement.Domain/        # Entidades, Objetos de Valor e IArchivable
├── WorkOrderManagement.Application/   # CQRS, Permisos, Manejo Funcional de Errores (ErrorOr)
├── WorkOrderManagement.Infrastructure/# EF Core, Interceptores (AuditLog, DomainEvents), Identity
└── WorkOrderManagement.Api/           # Minimal API Endpoints, Middlewares y Swagger/Scalar
```

---

## 🔑 Características Clave

1. **Permisos Dinámicos por Módulo**:
   - Organizados en partial classes: `Application/Features/{Modulo}/Security/Permission.{Modulo}.cs`.
   - Decorador `[HasPermission(Permission.Modulo.Accion)]` en cada comando/query.

2. **Manejo Funcional de Errores con `ErrorOr`**:
   - Sin excepciones para errores de negocio o validación.
   - Acumulación de errores de entrada en `List<Error>` devueltos en formato **RFC 7807 Problem Details (HTTP 400)**.

3. **Archivado Lógico (`IArchivable`) vs Eliminación**:
   - Extensión global `.Archive(userId)` y `.Unarchive()`.
   - Consulta con filtro global `e => !e.IsArchived` e `.IgnoreQueryFilters()` para restauración.
   - Bloqueo de eliminación física (`Error.Conflict`) si la entidad tiene dependencias vivas.

4. **Auditoría e Interceptores**:
   - `AuditTrailInterceptor` registra cambios JSON en `AuditLogs`.
   - `DispatchDomainEventsInterceptor` despacha eventos de dominio.

5. **Migraciones Automáticas**:
   - `await _context.Database.MigrateAsync()` en el arranque del sistema.

---

## 🛠️ Comandos de Desarrollo

- **Compilar**:
  ```bash
  dotnet build WorkOrderManagement.slnx
  ```
- **Pruebas Unitarias**:
  ```bash
  dotnet test WorkOrderManagement.slnx
  ```
- **Nueva Migración de EF Core**:
  ```bash
  dotnet ef migrations add NombreMigracion --project src/WorkOrderManagement.Infrastructure --startup-project src/WorkOrderManagement.Api --output-dir Data/Migrations
  ```
- **Ejecutar API**:
  ```bash
  dotnet run --project src/WorkOrderManagement.Api
  ```
