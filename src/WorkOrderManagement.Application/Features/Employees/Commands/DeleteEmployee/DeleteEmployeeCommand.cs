using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Employees.Commands.DeleteEmployee;

[HasPermission(Permission.Employee.Delete)]
public record DeleteEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;
