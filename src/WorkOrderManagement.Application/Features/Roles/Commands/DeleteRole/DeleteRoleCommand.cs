using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Roles.Commands.DeleteRole;

[HasPermission(Permission.Role.Delete)]
public record DeleteRoleCommand(List<Guid> RoleIds) : ICommand<ErrorOr<Success>>;
