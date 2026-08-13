using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.Commands.UnarchiveEmployee;

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
