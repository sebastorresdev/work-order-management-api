using Skvia.BaseTemplate.Application.Features.Roles.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery() : IQuery<ErrorOr<List<RoleResponse>>>;

