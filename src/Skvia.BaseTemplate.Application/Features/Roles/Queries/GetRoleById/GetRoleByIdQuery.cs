using Skvia.BaseTemplate.Application.Features.Roles.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<ErrorOr<RoleResponse>>;

