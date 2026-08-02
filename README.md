# Skvia Base Template API 🚀

Bienvenido a **Skvia Base Template API**, una plantilla de arquitectura limpia (Clean Architecture) robusta y lista para usar en tus próximos proyectos .NET. 

Esta plantilla incluye los módulos base de **Usuarios, Roles, Empleados y Sucursales** junto con autenticación, manejo de identidades y persistencia, configurada para aprovechar `.NET Aspire` y `Entity Framework Core`.

## 📦 ¿Por qué usar esta plantilla?

- **Arquitectura Limpia**: Separación clara de responsabilidades (Domain, Application, Infrastructure, Api).
- **Módulos Base Incluidos**: Gestión de usuarios, roles, empleados y sucursales ya listos.
- **Motor de Plantillas de .NET**: Usa `dotnet new` para renombrar automáticamente los espacios de nombres (*namespaces*) y configuraciones al instante al momento de crear tu proyecto.
- **Lista para Aspire**: Soporte incorporado para .NET Aspire (`AppHost`, `ServiceDefaults`) facilitando la orquestación y desarrollo local.

---

## ⚙️ Instrucciones de Instalación y Uso

Sigue estos pasos para generar un nuevo proyecto a partir de esta plantilla en cualquier computadora.

### 1. Clonar el repositorio base (Opcional)
Si deseas explorar el código base antes de instalar la plantilla, clona este repositorio en tu máquina local:
```bash
git clone https://github.com/sebastorresdev/skvia-base-template-api.git
cd skvia-base-template-api
```

### 2. Instalar la Plantilla en tu PC Local
Desde la raíz de este repositorio (o dentro de la carpeta clonada), ejecuta el siguiente comando para registrar la plantilla en el motor nativo de `.NET`:
```bash
dotnet new install .
```
> *Nota: Una vez instalado, verás que la plantilla se registra con el nombre corto `skvia-core`.*

### 3. Crear un Nuevo Proyecto
Abre una terminal, navega hacia la carpeta vacía donde quieres crear tu nuevo sistema y ejecuta:

```bash
# Cambia "MiEmpresa.Inventario" por el nombre de tu proyecto
dotnet new skvia-core -n MiEmpresa.Inventario
```

Al hacerlo, el motor de `.NET` copiará toda esta arquitectura limpia en tu carpeta y **renombrará automáticamente** todos los archivos, carpetas y *namespaces* (ej. `Skvia.BaseTemplate` pasará a ser `MiEmpresa.Inventario`).

### 4. Ejecutar las Migraciones
Dado que esta plantilla provee un dominio base limpio, es importante que generes tu propia migración inicial antes de correr el proyecto por primera vez. Entra a tu nuevo proyecto y ejecuta:

```bash
cd MiEmpresa.Inventario/src/MiEmpresa.Inventario.Infrastructure
dotnet ef migrations add InitialCreate
dotnet ef database update
```
*(Asegúrate de configurar tus cadenas de conexión en tu AppHost o appsettings.json previamente).*

## 🧱 Estructura de la Solución

- `Domain/`: Entidades centrales del negocio, Value Objects, interfaces del repositorio y constantes (`BaseAuditableEntity`, `User`, `Role`, `Employee`).
- `Application/`: Casos de uso (Commands & Queries bajo el patrón CQRS), validaciones y DTOs.
- `Infrastructure/`: Implementación de interfaces, Entity Framework DbContext, Identity y servicios externos.
- `Api/`: Controladores o Endpoints (FastEndpoints/Minimal APIs) para exponer los casos de uso hacia el exterior.
- `AppHost/` y `ServiceDefaults/`: Proyectos de orquestación local con **.NET Aspire**.

---

Desarrollado con ❤️ para acelerar la creación de software robusto y escalable.
