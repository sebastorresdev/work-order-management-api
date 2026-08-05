using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Roles.Commands.UpdateRole;

[HasPermission(Permission.Role.Edit)]
public record UpdateRoleCommand(Guid Id, string Name, string? Description) : ICommand<ErrorOr<Success>>;
