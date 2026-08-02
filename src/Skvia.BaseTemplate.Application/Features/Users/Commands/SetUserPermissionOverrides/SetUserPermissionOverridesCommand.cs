namespace Skvia.BaseTemplate.Application.Features.Users.Commands.SetUserPermissionOverrides;

public record SetUserPermissionOverridesCommand(
    Guid UserId,
    List<string> PermissionKeys
) : ICommand<ErrorOr<Success>>;

