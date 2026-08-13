using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Commands.CreateUser;

[HasPermission(Permission.User.Create)]
public record CreateUserCommand(
    string UserName,
    string Password,
    string Email,
    string? DisplayName,
    string? PhoneNumber,
    string? PhotoUrl,
    List<Guid> BranchIds,
    List<Guid> RoleIds
) : ICommand<ErrorOr<Guid>>;
