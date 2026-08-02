namespace Skvia.BaseTemplate.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name, string? Description) : ICommand<ErrorOr<Guid>>;

