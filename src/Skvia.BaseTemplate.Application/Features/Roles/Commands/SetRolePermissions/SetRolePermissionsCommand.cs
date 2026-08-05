using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Roles.Commands.SetRolePermissions;

[HasPermission(Permission.Role.Edit)]
public record SetRolePermissionsCommand(Guid RoleId, List<string> PermissionKeys) : ICommand<ErrorOr<Success>>;
