using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Roles.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRoles;

[HasPermission(Permission.Role.View)]
public record GetRolesQuery() : IQuery<ErrorOr<List<RoleResponse>>>;
