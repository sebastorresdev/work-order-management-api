namespace Skvia.BaseTemplate.Application.Features.Roles.DTOs;

public record RoleResponse(Guid Id, string Name, string? Description, DateTime LastModifiedAt);

