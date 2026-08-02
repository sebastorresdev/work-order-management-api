namespace Skvia.BaseTemplate.Application.Features.Users.Commands.ToggleUserStatus;

public record ToggleUserStatusCommand(
    Guid UserId,
    bool IsActive) : ICommand<ErrorOr<Success>>;

