using Skvia.BaseTemplate.Application.Common.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRolePermissions;

public record GetRolePermissionsQuery(Guid RoleId) : IQuery<ErrorOr<List<PermissionGroupResponse>>>;

