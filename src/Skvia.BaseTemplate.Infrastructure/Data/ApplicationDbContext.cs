using System.Reflection;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Branches;
using Skvia.BaseTemplate.Domain.Employees;
using Skvia.BaseTemplate.Domain.Identity;

namespace Skvia.BaseTemplate.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>(options), IApplicationDbContext
{
    public DbSet<BranchUser> BranchUsers => Set<BranchUser>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<ApplicationUserRole> ApplicationUserRole => Set<ApplicationUserRole>();
    public override DatabaseFacade Database => base.Database;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

