using Microsoft.EntityFrameworkCore.Infrastructure;

using Skvia.BaseTemplate.Domain.Auditing;
using Skvia.BaseTemplate.Domain.Branches;
using Skvia.BaseTemplate.Domain.Employees;
using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<BranchUser> BranchUsers { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Employee> Employees { get; }
    DbSet<AuditLog> AuditLogs { get; }

    DbSet<ApplicationUserRole> ApplicationUserRole { get; }
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

