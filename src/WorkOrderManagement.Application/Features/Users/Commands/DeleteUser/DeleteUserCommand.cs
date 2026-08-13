using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Commands.DeleteUser;

[HasPermission(Permission.User.Delete)]
public record DeleteUserCommand(List<Guid> UserIds) : ICommand<ErrorOr<Success>>;
