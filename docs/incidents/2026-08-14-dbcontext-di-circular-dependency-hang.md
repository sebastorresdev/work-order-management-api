# Incidente: La API se cuelga en el arranque (timeout de 20s en todos los endpoints, Scalar nunca carga)

## Resumen ejecutivo

La API compilaba y "arrancaba" (el proceso quedaba vivo, sin excepciones, sin crashear), pero:
- Todos los endpoints devolvían `The request was cancelled due to the configured timeout of 20 second(s) elapsing.`
- La página de Scalar (`/scalar`) nunca cargaba.
- No aparecía ningún error ni en la consola de la API ni en el dashboard de .NET Aspire.

**Causa raíz real:** una **dependencia circular en el contenedor de Inyección de Dependencias (DI)** de ASP.NET Core, que provoca que la resolución de `ApplicationDbContext` quede bloqueada indefinidamente (deadlock silencioso), sin lanzar ninguna excepción. Como esto ocurre durante `WebApplication.Build()`/inicialización, antes de que Kestrel empiece a escuchar, el proceso nunca llega a levantar el servidor HTTP, y cualquier request queda esperando hasta el timeout configurado.

Esto **no fue causado por "crear repositorios"** en el sentido de patrón Repository (`WorkOrderRepository`, `BranchRepository`, `EmployeeRepository`, etc.) — esos inyectan `IApplicationDbContext` de forma normal y correcta, y no forman parte del ciclo. La causa fue específica de un servicio (`CurrentUserProvider`) que es consumido, indirectamente, por los `SaveChangesInterceptor` que se resuelven **dentro de la misma fábrica que construye el `ApplicationDbContext`**.

---

## 1. Síntomas observados

- `dotnet build` exitoso, sin errores de compilación.
- Al ejecutar el proyecto (standalone o vía `WorkOrderManagement.AppHost`), el proceso quedaba "corriendo" pero nunca respondía.
- Cualquier request HTTP (incluido `/scalar`, `/api/health/live`, cualquier endpoint) fallaba con:
  ```
  The request was cancelled due to the configured timeout of 20 second(s) elapsing.
  ```
- El dashboard de Aspire no mostraba ningún error para el recurso de la API.
- La consola de la API (incluso con Serilog configurado) no mostraba ninguna excepción ni stack trace.
- El log de arranque se detenía justo después de:
  ```
  [HH:mm:ss INF] Iniciando el servidor web de la API...
  ```
  y no avanzaba nunca a `Now listening on: ...` ni a ningún log posterior.

## 2. Hipótesis descartadas (en orden de investigación)

Antes de llegar a la causa raíz, se descartaron — con evidencia — las siguientes hipótesis, todas razonables dado el contexto:

1. **PostgreSQL no estaba levantado** → Se verificó con `docker ps` que el contenedor sí estaba `Up`.
2. **Nombre de base de datos incorrecto** → Se detectó que `appsettings.json`/`appsettings.Development.json` apuntaban a `Database=WorkOrderManagement`, pero Aspire (`AppHost.cs`, `postgresServer.AddDatabase("skvia-base-template-db")`) crea la base con nombre `skvia-base-template-db`. Se corrigió, pero el problema persistió.
3. **Puerto de host mal mapeado** → Se detectó que, pese a `.WithHostPort(5433)` en `AppHost.cs`, el contenedor real (por tener `WithLifetime(ContainerLifetime.Persistent)`) seguía usando un puerto dinámico viejo (`docker ps` mostraba `127.0.0.1:56257->5432/tcp`, no `5433`). Se corrigió (se eliminó el contenedor viejo para forzar recreación), pero el problema persistió incluso apuntando directamente al puerto real.
4. **Credenciales/DB inaccesible** → Se verificó conectividad TCP directa (`Test-NetConnection`) y acceso real con `psql`/`PGPASSWORD` al contenedor, confirmando que la base de datos, usuario y password eran correctos y alcanzables.
5. **`EnrichNpgsqlDbContext` (integración Aspire) con reintentos infinitos** → Se deshabilitó temporalmente esta llamada y el hang **persistió exactamente igual**, descartando esta hipótesis.

Ninguna de estas causas explicaba por qué el proceso se colgaba **sin generar ningún log de error**, ni siquiera tras timeouts cortos configurados explícitamente (`Timeout=5;CommandTimeout=5` en la cadena de conexión).

## 3. Metodología para encontrar la causa real

Como no había ningún error visible, se usó **instrumentación manual por bisección** (no había otra forma de "ver" el problema, porque no era un error sino un deadlock):

1. Se ejecutó la API de forma standalone (fuera de Aspire) con `dotnet run`, redirigiendo toda la salida a una terminal controlada, para tener control total y no depender del dashboard de Aspire (que no propaga logs de proceso hijo a su propia consola).
2. Se agregaron `Console.WriteLine("DEBUG: ...")` (no `Serilog`, para evitar problemas de buffering de sinks async) en puntos clave y secuenciales del arranque:
   - Antes/después de `builder.Build()`.
   - Antes/después de `app.AddConfigAsync()`.
   - Antes/después de `app.InitialiseDatabaseAsync()`.
   - Antes/después de `scope.ServiceProvider.CreateScope()`.
   - Antes/después de resolver, uno por uno, cada servicio (`ApplicationDbContext`, `UserManager<T>`, `RoleManager<T>`, `ApplicationDbContextInitialiser`).
3. Con esto se identificó que el `Console.WriteLine` **inmediatamente anterior** a `scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()` se imprimía, pero el que le seguía **nunca se imprimía**. Es decir: el hang ocurría exactamente durante la resolución (construcción) del `ApplicationDbContext` por el contenedor de DI, no durante ninguna operación de red/EF Core explícita (`CanConnectAsync`, `MigrateAsync`, etc.), que ni siquiera llegaban a ejecutarse.
4. Con el punto de fallo aislado a "la construcción del `DbContext` en sí", se revisó qué se resuelve *dentro* de la fábrica de registro de `AddDbContext<ApplicationDbContext>`:
   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>((sp, opt) =>
   {
       var interceptors = sp.GetServices<ISaveChangesInterceptor>(); // <- aquí
       ...
   });
   ```
5. Se inspeccionaron los tres interceptores registrados (`AuditableEntityInterceptor`, `AuditTrailInterceptor`, `DispatchDomainEventsInterceptor`) y sus dependencias. Dos de ellos dependían de `ICurrentUserProvider`:
   ```csharp
   public class AuditTrailInterceptor(ICurrentUserProvider currentUserProvider) : SaveChangesInterceptor
   public class AuditableEntityInterceptor(ICurrentUserProvider currentUserProvider) : SaveChangesInterceptor
   ```
6. Se inspeccionó la implementación de `ICurrentUserProvider` (`CurrentUserProvider`) y se encontró que dependía, por constructor, de `IApplicationDbContext`:
   ```csharp
   public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor, IApplicationDbContext _dbContext) : ICurrentUserProvider
   ```
7. Y `IApplicationDbContext` está registrado así:
   ```csharp
   builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
   ```

Con esto quedó reconstruido el ciclo completo (ver diagrama abajo).

## 4. Causa raíz exacta (el ciclo)

```
1. Algo pide ApplicationDbContext (p.ej. ApplicationDbContextInitialiser, un repositorio, etc.)
   └─> El contenedor de DI empieza a CONSTRUIR ApplicationDbContext (aún no terminó)

2. La fábrica de registro de ApplicationDbContext ejecuta:
       sp.GetServices<ISaveChangesInterceptor>()
   └─> Esto resuelve AuditTrailInterceptor y AuditableEntityInterceptor

3. Ambos interceptores requieren ICurrentUserProvider
   └─> El contenedor construye CurrentUserProvider

4. CurrentUserProvider requiere IApplicationDbContext por constructor
   └─> El contenedor intenta resolver IApplicationDbContext
       └─> que está registrado como: provider.GetRequiredService<ApplicationDbContext>()
           └─> ¡PIDE DE NUEVO ApplicationDbContext, EN EL MISMO SCOPE, MIENTRAS AÚN SE ESTÁ CONSTRUYENDO! (paso 1)
```

Esto es una dependencia circular real: `ApplicationDbContext → (vía interceptores) → CurrentUserProvider → IApplicationDbContext → ApplicationDbContext`.

### ¿Por qué no lanza una excepción de "circular dependency detected"?

El detector de dependencias circulares de `Microsoft.Extensions.DependencyInjection` identifica ciclos comparando el **mismo tipo de servicio** en la misma cadena de resolución (`CallSiteChain`). Aquí el ciclo pasa por **dos tipos distintos** (`ApplicationDbContext` y `IApplicationDbContext`), aunque en la práctica resuelven la misma instancia. El motor de DI no siempre detecta esto como ciclo directo, y en su lugar, dado el mecanismo interno de sincronización por scope (bloqueo/caché de instancias en construcción), la segunda solicitud queda esperando indefinidamente a que la primera termine — pero la primera nunca termina porque está esperando a la segunda. Es un **deadlock silencioso**, no una excepción.

Esto explica exactamente el síntoma reportado: **"no puedo ver ningún error en ninguna parte"** — porque no hay ningún error; hay un bloqueo mutuo a nivel de framework, que no genera logs ni excepciones, solo hace que el `Task` de construcción nunca se complete.

## 5. Por qué es fácil de introducir sin darse cuenta

Este bug es especialmente insidioso porque:
- Compila perfectamente (el compilador de C# no detecta ciclos de DI en tiempo de compilación).
- No falla en el primer segundo — el proceso "arranca" (el `Main` sigue vivo).
- No genera logs de error, porque no es una excepción: es una espera infinita a nivel del `ServiceProvider`.
- Cualquier nuevo servicio que:
  - dependa de `ICurrentUserProvider` (o cualquier otro servicio que dependa de `IApplicationDbContext`), **y**
  - sea consumido, directa o indirectamente, por algo que se resuelve **dentro** de la fábrica de `AddDbContext<ApplicationDbContext>` (como los `ISaveChangesInterceptor`)

  puede reintroducir este mismo ciclo sin que nada lo avise hasta que alguien intente arrancar la app y todo se cuelgue.

## 6. Fix aplicado

Se rompió el ciclo evitando que `CurrentUserProvider` dependa de `IApplicationDbContext` **por constructor**. En su lugar, se inyecta `IServiceProvider` y se resuelve `IApplicationDbContext` de forma perezosa (lazy), solo cuando el método `GetCurrentUser()` es invocado en tiempo de ejecución (momento en el cual el `ApplicationDbContext` ya terminó de construirse y está cacheado en el scope, por lo que no hay reentrancia):

```csharp
// Antes (rompía el arranque):
public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor, IApplicationDbContext _dbContext) : ICurrentUserProvider
{
    public CurrentUserResponse GetCurrentUser()
    {
        // ...
        var user = _dbContext.ApplicationUsers.AsNoTracking()...
    }
}

// Después (fix):
public class CurrentUserProvider(IHttpContextAccessor _httpContextAccessor, IServiceProvider _serviceProvider) : ICurrentUserProvider
{
    public CurrentUserResponse GetCurrentUser()
    {
        // ...
        var dbContext = _serviceProvider.GetRequiredService<IApplicationDbContext>();
        var user = dbContext.ApplicationUsers.AsNoTracking()...
    }
}
```

Archivo modificado: `src/WorkOrderManagement.Infrastructure/Security/CurrentUserProvider/CurrentUserProvider.cs`

### Verificación del fix

Se ejecutó la API de forma aislada con la cadena de conexión real del contenedor de PostgreSQL, confirmando:
1. El log de arranque avanzó correctamente por todas las etapas (`CreateScope`, resolver `ApplicationDbContext`, `InitialiseAsync`, `SeedAsync`), sin colgarse.
2. Un `GET /api/health/live` devolvió `200 { "status": "Alive" }`.

## 7. Cómo evitar que vuelva a pasar

1. **Regla general de diseño:** ningún servicio que sea dependencia (directa o transitiva) de un `ISaveChangesInterceptor` de `ApplicationDbContext` debe depender por constructor de `IApplicationDbContext` (ni de `ApplicationDbContext`). Si necesita acceso a datos, debe:
   - Resolver el `DbContext` de forma perezosa vía `IServiceProvider` (como se hizo en el fix), o
   - Recibir los datos que necesita ya calculados/pasados desde afuera, en lugar de consultarlos él mismo.
2. **Revisar cualquier interceptor nuevo** (`ISaveChangesInterceptor`, `IInterceptor` de EF Core en general) y trazar manualmente sus dependencias transitivas para descartar que alguna termine referenciando de vuelta al propio `DbContext` en el mismo scope.
3. **Al agregar un servicio nuevo a `AddDbContext<T>(...)` (como interceptores, extensiones, `AddInterceptors`, etc.)**, tratar esa fábrica como "zona de alto riesgo de ciclos": cualquier dependencia resuelta ahí debe evitar tocar `IApplicationDbContext`/`ApplicationDbContext`.
4. **Detección temprana:** si el arranque de la app se cuelga sin ningún log ni excepción (a diferencia de un error de conexión, que sí debería loguear algo con los cambios ya aplicados en `ApplicationDbContextInitialiser`), sospechar primero de un ciclo de DI antes que de infraestructura (DB, red, contenedores). Un truco rápido de diagnóstico: agregar temporalmente resoluciones explícitas y secuenciales con `Console.WriteLine` alrededor de cada `GetRequiredService<T>()` sospechoso — si el log se detiene justo antes de resolver un tipo específico y nunca lanza excepción, es un fuerte indicio de ciclo de DI, no de una llamada de red colgada (que normalmente sí lanza `TimeoutException`/`SocketException` tarde o temprano).
5. Opcional (mejora de robustez futura): envolver la inicialización de la base de datos (`InitialiseDatabaseAsync`) con un `CancellationTokenSource` con timeout explícito (p.ej. 30s) que fuerce una excepción clara en vez de un colgado indefinido, para que este tipo de problemas — sea por DI o por infraestructura — sea siempre visible como error en los logs y no como un cuelgue silencioso.

## 8. Cambios adicionales que quedaron aplicados (válidos independientemente de este incidente)

- `src/WorkOrderManagement.Api/appsettings.json` y `appsettings.Development.json`: `Database` corregido a `skvia-base-template-db` (nombre real creado por Aspire) y se agregó `Timeout=5;CommandTimeout=5` a la cadena de conexión para fallar rápido si PostgreSQL no está disponible.
- `src/WorkOrderManagement.Infrastructure/Data/ApplicationDbContextInitialiser.cs`: `InitialiseAsync()` ahora valida `CanConnectAsync()` antes de migrar, y si falla, lanza un `InvalidOperationException` con mensaje explícito indicando host/puerto/base de datos, en vez de dejar el error genérico de EF Core sin contexto.
