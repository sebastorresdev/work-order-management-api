using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.ToggleUserStatus;

[HasPermission(Permission.User.Edit)]
public record ToggleUserStatusCommand(
    Guid UserId,
    bool IsActive) : ICommand<ErrorOr<Success>>;
