using WorkOrderManagement.Application.Common.DTOs;
using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Roles.Queries.GetRolePermissions;

[HasPermission(Permission.Role.View)]
public record GetRolePermissionsQuery(Guid RoleId) : IQuery<ErrorOr<List<PermissionGroupResponse>>>;
