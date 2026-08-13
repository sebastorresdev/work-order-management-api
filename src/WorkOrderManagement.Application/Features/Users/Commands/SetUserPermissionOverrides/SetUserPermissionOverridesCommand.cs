using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Commands.SetUserPermissionOverrides;

[HasPermission(Permission.User.Edit)]
public record SetUserPermissionOverridesCommand(
    Guid UserId,
    List<string> PermissionKeys
) : ICommand<ErrorOr<Success>>;
