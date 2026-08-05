using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.UpdateUser;

[HasPermission(Permission.User.Edit)]
public record UpdateUserCommand(
    Guid UserId,
    string UserName,
    bool IsActive,
    string Email,
    string? DisplayName,
    string? PhoneNumber,
    string? PhotoUrl,
    List<Guid> BranchIds,
    List<Guid> RoleIds) : ICommand<ErrorOr<Success>>;
