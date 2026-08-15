# 📋 PROJECT CONTEXT: Sistema de Gestión de Órdenes de Trabajo (Work Order Management)

## 🎯 Visión General del Proyecto
Sistema web interno para la gestión, asignación, seguimiento y control de órdenes de trabajo (Work Orders), diseñado con arquitectura empresarial en .NET 10 y Angular, preparado arquitectónicamente para escalar como una plataforma multi-sede y comercializable (SaaS / Multi-tenant).

---

## 🛠️ Stack Tecnológico
- **Backend:** C# (.NET 10) usando **Minimal APIs**, **CQRS (MediatR)**, **FluentValidation** y **ASP.NET Core Identity**.
- **Base de Datos:** **PostgreSQL** mediante **Entity Framework Core** (Npgsql con convención SnakeCase).
- **Frontend:** **Angular** (Standalone Components) + **NG-ZORRO** (Ant Design UI Library) + **TypeScript** + **Vitest**.
- **Orquestación & Observabilidad:** **.NET Aspire** (AppHost + ServiceDefaults + OpenTelemetry).
- **Autenticación:** Tokens Bearer con endpoint estandarizado `/api/auth/me` para la resolución de permisos dinámicos, roles y datos del usuario.

---

## 🏗️ Decisiones de Arquitectura y Negocio

### 1. Modelo de Negocio e Identidad
- **Sin Auto-Registro Público:** Es un sistema cerrado de gestión administrativa. La alta de empleados y asignación de credenciales la realiza exclusivamente un Administrador / Recursos Humanos.
- **Estructura Actual:** **Multi-sede / Multi-sucursal (`Branch`)**. Un usuario o empleado pertenece a una Sucursal/Sede específica de la organización.
- **Preparado para Futuro Multi-tenant:** Diseñado de manera que en el futuro se pueda agregar la columna `EmpresaId` (Tenant) en las tablas principales y aplicar *Global Query Filters* en EF Core.

### 2. Autenticación y Autorización
- El login recibe credenciales locales (`userName`, `password`), valida la cuenta activa y emite el token Bearer.
- El frontend (Angular) consulta `/api/auth/me` inmediatamente después de autenticarse para almacenar en un servicio global los datos del usuario, sus roles (`Admin`, `Supervisor`, `Empleado`) y permisos dinámicos.
- Angular utiliza un `HttpInterceptor` para enviar el Bearer Token en cada solicitud y `Functional Guards` para proteger las rutas.

---

## 🗄️ Modelo de Datos Básico (PostgreSQL / EF Core)

1. **`Branches` / Sedes** (`Id`, `Code`, `Name`, `Address`, `IsActive`)
2. **`ApplicationUser`** (Hereda de `IdentityUser`):
   - `FirstName`, `LastName`, `Dni`, `IsActive`, `BranchId`
3. **`Employees`** (`Id`, `FirstName`, `LastName`, `DocumentIdentifier` [Value Object: `Type`, `Number`], `Email` [Value Object], `Phone` [Value Object], `BranchId`)
4. **`WorkOrders`** (`Id`, `OrderNumber`, `Title`, `Description`, `Status` [`Draft`, `Assigned`, `InProgress`, `Completed`, `Cancelled`], `Priority` [`Low`, `Medium`, `High`, `Urgent`], `Type`, `AssignedEmployeeId`, `BranchId`, `Historiales` [`StatusHistory`, `ScheduleHistory`])

---

## 🚀 Guía de Estilo de Código para la IA
Cuando generes código o me ayudes a refactorizar dentro de este IDE, sigue estrictamente estas pautas:
- **Backend:** Usa sintaxis moderna de C# (Minimal APIs, `MapGroup`, instanciación de servicios con `inject` o DI de Minimal APIs, Types explícitos o fuertemente tipados).
- **Frontend:** Usa Angular moderno (Standalone Components, `inject()`, Signals para manejo de estado si aplica) y componentes nativos de **NG-ZORRO** (`nz-table`, `nz-form`, `nz-modal`, `nz-notification`).
- **Consultas EF Core:** Mantén las consultas optimizadas (`AsNoTracking()` para lecturas, filtrado directo por `SedeId`).

---

## ♻️ Refactorización de Entidades y Objetos de Valor (Realizado por Gemini)

Se ha llevado a cabo una refactorización significativa en la entidad `Employee` para mejorar la encapsulación, la validación y la consistencia del dominio mediante la introducción de objetos de valor.

**Cambios Clave:**

1.  **Entidad `Employee` (Dominio):**
    *   Se reemplazaron las propiedades `DocumentType` (int) y `DocumentNumber` (string) por el objeto de valor `DocumentIdentifier`.
    *   Se reemplazaron las propiedades `Email` (string) y `Phone` (string) por los objetos de valor `Email` y `Phone` respectivamente.
    *   Los métodos `Create` y `Update` de la entidad `Employee` ahora aceptan y utilizan estos nuevos objetos de valor, delegando la lógica de validación intrínseca a los mismos.

2.  **Objetos de Valor Creados (Dominio):**
    *   `DocumentIdentifier` (record struct): Encapsula el tipo y número de documento, con validación de longitud.
    *   `Email` (record struct): Encapsula la dirección de correo electrónico, con validación de formato y longitud.
    *   `Phone` (record struct): Encapsula el número de teléfono, con validación de formato y longitud.

3.  **Capa de Aplicación (`WorkOrderManagement.Application`):**
    *   **Comandos (`CreateEmployeeCommand`, `UpdateEmployeeCommand`):** Se actualizaron para usar el `enum DocumentType` del dominio y para aceptar `string?` para `Email` y `Phone`. La conversión a los objetos de valor se realiza en los `CommandHandlers`.
    *   **Manejadores de Comandos (`CreateEmployeeCommandHandler`, `UpdateEmployeeCommandHandler`):** Se modificaron para construir los objetos de valor `DocumentIdentifier`, `Email` y `Phone` a partir de los datos del comando antes de interactuar con la entidad `Employee`. Se actualizó la lógica de verificación de unicidad para `DocumentIdentifier`.
    *   **Validadores de Comandos (`CreateEmployeeCommandValidator`, `UpdateEmployeeCommandValidator`):** Se ajustaron para delegar las validaciones de longitud y formato específicas a los objetos de valor, manteniendo las validaciones de presencia y formato general a nivel de comando.
    *   **DTOs (`EmployeeResponse`, `EmployeeDetailResponse`):** Se actualizaron para incluir las propiedades `DocumentType`, `DocumentNumber`, `Email` y `Phone` de los objetos de valor, exponiéndolos como tipos primitivos (ej. `string?` para Email/Phone) para simplificar el consumo por parte del cliente.
    *   **Manejadores de Consultas (`GetEmployeesQueryHandler`, `GetEmployeeByIdQueryHandler`):** Se modificaron las proyecciones para extraer correctamente los valores de los objetos de valor (`DocumentIdentifier.Type`, `DocumentIdentifier.Number`, `Email.Value`, `Phone.Value`) al construir los DTOs de respuesta.

4.  **Capa de Infraestructura (`WorkOrderManagement.Infrastructure`):**
    *   **Configuración de Entidad `Employee` (`EmployeeConfiguration.cs`):**
        *   `DocumentIdentifier` se configuró como una entidad poseída (`builder.OwnsOne`), mapeando `Type` y `Number` a columnas separadas (`DocumentType` y `DocumentNumber`) en la tabla `employees`. Se estableció un índice único combinado para `DocumentType` y `DocumentNumber`.
        *   `Email` y `Phone` se configuraron con `ValueConverter` para mapear sus propiedades `Value` a columnas de tipo `string` en la base de datos, manejando la nulabilidad y las longitudes máximas.

Estos cambios mejoran la robustez del modelo de dominio, centralizan la lógica de validación y preparan la aplicación para un manejo de errores más consistente.

