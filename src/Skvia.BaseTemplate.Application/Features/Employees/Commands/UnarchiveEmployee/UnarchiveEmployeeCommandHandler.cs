using Microsoft.EntityFrameworkCore;
using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Common;
using Skvia.BaseTemplate.Domain.Employees;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.UnarchiveEmployee;

public class UnarchiveEmployeeCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UnarchiveEmployeeCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UnarchiveEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == command.EmployeeId, cancellationToken);

        if (employee is null)
            return EmployeeErrors.NotFound;

        employee.Unarchive();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
