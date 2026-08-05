using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.UnarchiveEmployee;

[HasPermission(Permission.Employee.Archive)]
public record UnarchiveEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;
