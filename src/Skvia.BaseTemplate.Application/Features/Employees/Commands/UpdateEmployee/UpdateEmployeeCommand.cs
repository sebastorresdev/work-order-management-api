using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Domain.Employees;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.UpdateEmployee;

[HasPermission(Permission.Employee.Update)]
public record UpdateEmployeeCommand(
    Guid Id,
    string Code,
    string FirstName,
    string LastName,
    DocumentType DocumentType,
    string DocumentNumber,
    DateTimeOffset HireDate,
    string? Email = null,
    string? Phone = null,
    string? Position = null,
    string? Department = null,
    string? PhotoUrl = null) : ICommand<ErrorOr<Success>>;
