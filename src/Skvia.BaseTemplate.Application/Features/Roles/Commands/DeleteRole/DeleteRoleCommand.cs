using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Roles.Commands.DeleteRole;

[HasPermission(Permission.Role.Delete)]
public record DeleteRoleCommand(List<Guid> RoleIds) : ICommand<ErrorOr<Success>>;
