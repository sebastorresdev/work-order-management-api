using Skvia.BaseTemplate.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateEmployeeCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        if (employee is null)
        {
            return EmployeeErrors.NotFound;
        }

        var docResult = DocumentIdentifier.Create(command.DocumentType, command.DocumentNumber);
        if (docResult.IsError) return docResult.Errors;

        var documentIdentifier = docResult.Value;

        if (employee.DocumentIdentifier.Type != documentIdentifier.Type || employee.DocumentIdentifier.Number != documentIdentifier.Number)
        {
            if (await dbContext.Employees.AnyAsync(e => e.DocumentIdentifier.Type == documentIdentifier.Type && e.DocumentIdentifier.Number == documentIdentifier.Number && e.Id != command.Id, cancellationToken))
            {
                return EmployeeErrors.DocumentExists(command.DocumentNumber);
            }
        }

        var updateResult = employee.Update(
            command.Code,
            command.FirstName,
            command.LastName,
            documentIdentifier,
            command.HireDate,
            command.Email,
            command.Phone,
            command.Position,
            command.Department,
            command.PhotoUrl);

        if (updateResult.IsError) return updateResult.Errors;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
