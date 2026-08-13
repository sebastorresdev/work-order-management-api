using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Auditing;
using Skvia.BaseTemplate.Domain.Branches;
using Skvia.BaseTemplate.Domain.Common;
using Skvia.BaseTemplate.Domain.Employees;
using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Infrastructure.Data;

/// <summary>
/// Contexto principal de Entity Framework Core de la aplicación para el acceso a datos y la gestión de identidades.
/// </summary>
/// <param name="options">Opciones de configuración del DbContext.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>(options), IApplicationDbContext
{
    /// <summary>
    /// Conjunto de datos para relaciones entre sedes y usuarios.
    /// </summary>
    public DbSet<BranchUser> BranchUsers => Set<BranchUser>();

    /// <summary>
    /// Conjunto de datos para la entidad de sedes.
    /// </summary>
    public DbSet<Branch> Branches => Set<Branch>();

    /// <summary>
    /// Conjunto de datos para los empleados.
    /// </summary>
    public DbSet<Employee> Employees => Set<Employee>();

    /// <summary>
    /// Conjunto de datos para registros de auditoría.
    /// </summary>
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    /// <summary>
    /// Conjunto de datos para roles de usuario.
    /// </summary>
    public DbSet<ApplicationUserRole> ApplicationUserRole => Set<ApplicationUserRole>();

    /// <summary>
    /// Fachada de la base de datos subyacente.
    /// </summary>
    public override DatabaseFacade Database => base.Database;

    /// <summary>
    /// Configura el modelo de datos de EF Core e instala filtros globales de consulta (Global Query Filters).
    /// </summary>
    /// <param name="builder">Constructor de modelos de Entity Framework.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Aplica todas las configuraciones IEntityTypeConfiguration presentes en la infraestructura
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Global Query Filter para el patrón de Archivado (IArchivable)
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(IArchivable).IsAssignableFrom(entityType.ClrType))
            {
                // Expresión lambda e => e.IsArchived == false
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var property = System.Linq.Expressions.Expression.Property(parameter, nameof(IArchivable.IsArchived));
                var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                var lambda = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Equal(property, falseConstant), parameter);

                builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}
