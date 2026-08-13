using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Commands.ToggleUserStatus;

[HasPermission(Permission.User.Edit)]
public record ToggleUserStatusCommand(
    Guid UserId,
    bool IsActive) : ICommand<ErrorOr<Success>>;
