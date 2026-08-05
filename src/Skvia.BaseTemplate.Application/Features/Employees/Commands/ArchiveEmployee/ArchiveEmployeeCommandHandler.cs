using Microsoft.EntityFrameworkCore;
using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Common;
using Skvia.BaseTemplate.Domain.Employees;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.ArchiveEmployee;

public class ArchiveEmployeeCommandHandler(IApplicationDbContext dbContext, ICurrentUserProvider currentUserProvider)
    : ICommandHandler<ArchiveEmployeeCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ArchiveEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == command.EmployeeId, cancellationToken);

        if (employee is null)
            return EmployeeErrors.NotFound;

        Guid? userId = null;
        try
        {
            var currentUser = currentUserProvider.GetCurrentUser();
            userId = currentUser?.Id;
        }
        catch (InvalidOperationException) { }

        employee.Archive(userId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
