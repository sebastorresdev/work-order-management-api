using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Identity;
using WorkOrderManagement.Infrastructure.Data;
using WorkOrderManagement.Infrastructure.Data.Interceptors;
using WorkOrderManagement.Infrastructure.Security.CurrentUserProvider;
using WorkOrderManagement.Infrastructure.Services;

namespace WorkOrderManagement.Infrastructure;

/// <summary>
/// Proporciona métodos de extensión para el registro de servicios de la capa de Infraestructura en el contenedor de dependencias.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de base de datos PostgreSQL, interceptores, ASP.NET Core Identity y servicios de infraestructura.
    /// </summary>
    /// <param name="builder">Constructor de la aplicación host.</param>
    /// <returns>El mismo <see cref="IHostApplicationBuilder"/> con la infraestructura configurada.</returns>
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        // Habilitar compatibilidad con timestamps y zonas horarias en Npgsql/PostgreSQL
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Auditorías e Interceptores
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditTrailInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddSingleton<IClock, SystemClock>();
        builder.Services.AddSingleton<ITimeZoneProvider, SystemTimeZoneProvider>();
        builder.Services.Configure<SeedOptions>(builder.Configuration.GetSection(SeedOptions.SectionName));

        // Registro de DbContext de Entity Framework con PostgreSQL y convención Npgsql SnakeCase
        builder.Services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            string? connectionString = builder.Configuration.GetConnectionString("work-order-management-db");

            opt.UseNpgsql(connectionString).AddInterceptors(interceptors);
            opt.UseSnakeCaseNamingConvention();
        });

        // Enriquecimiento para integración con .NET Aspire
        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        // Registro del contexto e inicializador de semillas de base de datos
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        // Contexto de seguridad y accesor HTTP
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        // Esquema de autenticación Bearer Token
        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        // Constructor de servicios de autorización
        builder.Services.AddAuthorizationBuilder();

        // Configuración de políticas de seguridad e identidad (ASP.NET Core Identity)
        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            // Políticas de complejidad de contraseñas
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            // Políticas de bloqueo temporal por intentos fallidos
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;

            // Ajustes de usuario
            options.User.RequireUniqueEmail = true;

            // Ajustes de inicio de sesión
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<ApplicationRole>()
        .AddSignInManager()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Registro de servicios de aplicación de identidad y permisos
        builder.Services.AddScoped<IUserPermissionService, UserPermissionService>();
        builder.Services.AddScoped<IUserAccountService, IdentityUserAccountService>();
        builder.Services.AddScoped<IRoleService, IdentityRoleService>();
        builder.Services.AddScoped<WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders.IWorkOrderRepository, WorkOrderManagement.Infrastructure.Repositories.WorkOrderRepository>();
        builder.Services.AddScoped<WorkOrderManagement.Application.Features.Branches.Queries.GetBranches.IBranchRepository, WorkOrderManagement.Infrastructure.Repositories.BranchRepository>();
        builder.Services.AddScoped<WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees.IEmployeeRepository, WorkOrderManagement.Infrastructure.Repositories.EmployeeRepository>();

        return builder;
    }
}

