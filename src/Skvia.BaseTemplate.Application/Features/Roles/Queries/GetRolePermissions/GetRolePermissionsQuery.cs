using Skvia.BaseTemplate.Application.Common.DTOs;
using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRolePermissions;

[HasPermission(Permission.Role.View)]
public record GetRolePermissionsQuery(Guid RoleId) : IQuery<ErrorOr<List<PermissionGroupResponse>>>;
