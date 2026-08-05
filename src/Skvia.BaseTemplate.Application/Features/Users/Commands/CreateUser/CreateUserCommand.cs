using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.CreateUser;

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
