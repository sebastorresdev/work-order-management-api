using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.SetUserPermissionOverrides;

[HasPermission(Permission.User.Edit)]
public record SetUserPermissionOverridesCommand(
    Guid UserId,
    List<string> PermissionKeys
) : ICommand<ErrorOr<Success>>;
