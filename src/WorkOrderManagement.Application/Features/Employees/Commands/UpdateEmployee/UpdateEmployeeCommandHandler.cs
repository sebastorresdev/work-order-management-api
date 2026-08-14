using WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository) : ICommandHandler<UpdateEmployeeCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetEntityByIdAsync(command.Id, cancellationToken);

        if (employee is null)
        {
            return EmployeeErrors.NotFound;
        }

        var docResult = DocumentIdentifier.Create(command.DocumentType, command.DocumentNumber);
        if (docResult.IsError) return docResult.Errors;

        var documentIdentifier = docResult.Value;

        if (employee.DocumentIdentifier.Type != documentIdentifier.Type || employee.DocumentIdentifier.Number != documentIdentifier.Number)
        {
            if (await employeeRepository.ExistsByDocumentAsync(documentIdentifier, command.Id, cancellationToken))
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

        await employeeRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
