using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Roles.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRoleById;

[HasPermission(Permission.Role.View)]
public record GetRoleByIdQuery(Guid Id) : IQuery<ErrorOr<RoleResponse>>;
