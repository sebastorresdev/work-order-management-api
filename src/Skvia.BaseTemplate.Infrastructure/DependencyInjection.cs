using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Common;
using Skvia.BaseTemplate.Domain.Identity;
using Skvia.BaseTemplate.Infrastructure.Data;
using Skvia.BaseTemplate.Infrastructure.Data.Interceptors;
using Skvia.BaseTemplate.Infrastructure.Security.CurrentUserProvider;
using Skvia.BaseTemplate.Infrastructure.Services;

namespace Skvia.BaseTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        // Auditorias e Interceptores
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditTrailInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddSingleton<IClock, SystemClock>();
        builder.Services.AddSingleton<ITimeZoneProvider, SystemTimeZoneProvider>();
        builder.Services.Configure<SeedOptions>(builder.Configuration.GetSection(SeedOptions.SectionName));

        // 2. Registro clásico adaptado con las convenciones necesarias
        builder.Services.AddDbContext<ApplicationDbContext>((sp, opt) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            // Nota: El connectionString real ya lo manejará automáticamente el orquestador a nivel de infraestructura
            string? connectionString = builder.Configuration.GetConnectionString("skvia-base-template-db");

            opt.UseNpgsql(connectionString).AddInterceptors(interceptors);
            opt.UseSnakeCaseNamingConvention();
        });

        // Aspire
        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        // Database
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        // Security
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        // Authentication
        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        // Authorization
        builder.Services.AddAuthorizationBuilder();

        // Identity
        builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            // Password policy
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            // Lockout policy
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            options.Lockout.MaxFailedAccessAttempts = 5;

            // User settings
            options.User.RequireUniqueEmail = true;

            // Sign-in settings
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<ApplicationRole>()
        .AddSignInManager()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Services
        builder.Services.AddScoped<IUserPermissionService, UserPermissionService>();
        builder.Services.AddScoped<IUserAccountService, IdentityUserAccountService>();
        builder.Services.AddScoped<IRoleService, IdentityRoleService>();

        return builder;

    }
}

