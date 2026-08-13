using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Roles.Commands.SetRolePermissions;

[HasPermission(Permission.Role.Edit)]
public record SetRolePermissionsCommand(Guid RoleId, List<string> PermissionKeys) : ICommand<ErrorOr<Success>>;
