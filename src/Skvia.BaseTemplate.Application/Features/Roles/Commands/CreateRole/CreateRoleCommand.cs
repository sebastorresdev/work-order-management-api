using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Roles.Commands.CreateRole;

[HasPermission(Permission.Role.Create)]
public record CreateRoleCommand(string Name, string? Description) : ICommand<ErrorOr<Guid>>;
