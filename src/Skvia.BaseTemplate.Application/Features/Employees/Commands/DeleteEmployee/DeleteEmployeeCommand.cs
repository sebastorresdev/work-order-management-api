using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.DeleteEmployee;

[HasPermission(Permission.Employee.Delete)]
public record DeleteEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;
