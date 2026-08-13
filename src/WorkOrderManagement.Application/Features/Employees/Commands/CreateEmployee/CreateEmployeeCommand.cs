using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Application.Features.Employees.Commands.CreateEmployee;

[HasPermission(Permission.Employee.Create)]
public record CreateEmployeeCommand(
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
    string? PhotoUrl = null) : ICommand<ErrorOr<Guid>>;

