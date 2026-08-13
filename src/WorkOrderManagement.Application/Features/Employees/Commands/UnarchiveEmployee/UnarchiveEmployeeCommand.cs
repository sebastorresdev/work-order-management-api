using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Employees.Commands.UnarchiveEmployee;

[HasPermission(Permission.Employee.Archive)]
public record UnarchiveEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;
