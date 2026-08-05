using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Employees.Commands.ArchiveEmployee;

[HasPermission(Permission.Employee.Archive)]
public record ArchiveEmployeeCommand(Guid EmployeeId) : ICommand<ErrorOr<Success>>;
