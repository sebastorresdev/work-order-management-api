IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", "postgres", secret: true);

// 🚀 1. Contenedor de PostgreSQL
var postgresServer = builder.AddPostgres("postgres", password: postgresPassword)
    .WithImage("postgres")
    .WithImageTag("16")
    .WithHostPort(5433)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin();

// 🚀 2. Base de datos específica del dominio
var database = postgresServer.AddDatabase("work-order-management-db");

// 🚀 3. API Backend (.NET 10)
var api = builder.AddProject<Projects.WorkOrderManagement_Api>("work-order-management-api")
    .WithReference(database)
    .WaitFor(database)
    .WithExternalHttpEndpoints()
    .WithUrlForEndpoint("https", url =>
    {
        url.DisplayText = "Scalar API Reference";
        url.Url = "/scalar";
    });

// 🚀 4. Frontend Angular (Ruta absoluta desde AppHostDirectory)
var frontendPath = Path.GetFullPath(Path.Combine(builder.AppHostDirectory, "../../../work-order-management-frontend"));

builder.AddNpmApp("work-order-management-frontend", frontendPath, scriptName: "start")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(port: 4200, targetPort: 4200, isProxied: false)
    .WithExternalHttpEndpoints();

builder.Build().Run();


