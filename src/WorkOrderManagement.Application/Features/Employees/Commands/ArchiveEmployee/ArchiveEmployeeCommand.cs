using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Employees.Commands.ArchiveEmployee;

[HasPermission(Permission.Employee.Archive)]
public record ArchiveEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;
