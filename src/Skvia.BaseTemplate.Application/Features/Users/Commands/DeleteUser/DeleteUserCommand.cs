using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Commands.DeleteUser;

[HasPermission(Permission.User.Delete)]
public record DeleteUserCommand(List<Guid> UserIds) : ICommand<ErrorOr<Success>>;
