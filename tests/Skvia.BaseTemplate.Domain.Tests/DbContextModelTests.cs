using Microsoft.EntityFrameworkCore;
using Skvia.BaseTemplate.Infrastructure.Data;

namespace Skvia.BaseTemplate.Domain.Tests;

public class DbContextModelTests
{
    [Fact]
    public void OnModelCreating_BuildsModelWithoutExceptions()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Database=test_db;Username=postgres;Password=postgres")
            .Options;

        using var dbContext = new ApplicationDbContext(options);

        // Accessing dbContext.Model forces OnModelCreating and model validation to run
        var model = dbContext.Model;

        Assert.NotNull(model);
        var employeeEntity = model.FindEntityType(typeof(Domain.Employees.Employee));
        Assert.NotNull(employeeEntity);
    }
}

