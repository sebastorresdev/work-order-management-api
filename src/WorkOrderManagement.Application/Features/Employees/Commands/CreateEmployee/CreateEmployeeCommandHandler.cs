using WorkOrderManagement.Application.Features.Employees.Queries.GetEmployees;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository) : ICommandHandler<CreateEmployeeCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var normalizedCode = command.Code.Trim().ToUpperInvariant();

        if (await employeeRepository.ExistsByCodeAsync(normalizedCode, cancellationToken))
        {
            return EmployeeErrors.CodeExists(command.Code);
        }

        var docResult = DocumentIdentifier.Create(command.DocumentType, command.DocumentNumber);
        if (docResult.IsError) return docResult.Errors;

        var documentIdentifier = docResult.Value;

        if (await employeeRepository.ExistsByDocumentAsync(documentIdentifier, cancellationToken: cancellationToken))
        {
            return EmployeeErrors.DocumentExists(command.DocumentNumber);
        }

        var employeeResult = Employee.Create(
            code: command.Code,
            firstName: command.FirstName,
            lastName: command.LastName,
            documentIdentifier: documentIdentifier,
            hireDate: command.HireDate,
            email: command.Email,
            phone: command.Phone,
            position: command.Position,
            department: command.Department,
            photoUrl: command.PhotoUrl);

        if (employeeResult.IsError) return employeeResult.Errors;

        var employee = employeeResult.Value;

        await employeeRepository.AddAsync(employee, cancellationToken);
        await employeeRepository.SaveChangesAsync(cancellationToken);

        return employee.Id;
    }
}
