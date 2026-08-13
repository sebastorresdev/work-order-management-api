using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Roles.Commands.CreateRole;

[HasPermission(Permission.Role.Create)]
public record CreateRoleCommand(string Name, string? Description) : ICommand<ErrorOr<Guid>>;
