using Microsoft.EntityFrameworkCore.Infrastructure;

using Skvia.BaseTemplate.Domain.Auditing;
using Skvia.BaseTemplate.Domain.Branches;
using Skvia.BaseTemplate.Domain.Employees;
using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

/// <summary>
/// Interfaz para el contexto de base de datos de la aplicación que expone los conjuntos de datos (DbSet) y métodos de persistencia.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Conjunto de datos para la entidad de unión entre sedes y usuarios.
    /// </summary>
    DbSet<BranchUser> BranchUsers { get; }

    /// <summary>
    /// Conjunto de datos para la entidad de sedes.
    /// </summary>
    DbSet<Branch> Branches { get; }

    /// <summary>
    /// Conjunto de datos para la entidad de empleados.
    /// </summary>
    DbSet<Employee> Employees { get; }

    /// <summary>
    /// Conjunto de datos para los registros de auditoría.
    /// </summary>
    DbSet<AuditLog> AuditLogs { get; }

    /// <summary>
    /// Conjunto de datos para los roles asignados a usuarios.
    /// </summary>
    DbSet<ApplicationUserRole> ApplicationUserRole { get; }

    /// <summary>
    /// Fachada de la base de datos para administración de transacciones y ejecuciones directas.
    /// </summary>
    DatabaseFacade Database { get; }

    /// <summary>
    /// Guarda asincrónicamente todos los cambios realizados en el contexto.
    /// </summary>
    /// <param name="cancellationToken">Token para la cancelación de la tarea asíncrona.</param>
    /// <returns>Número de registros de estado afectados en la base de datos.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

