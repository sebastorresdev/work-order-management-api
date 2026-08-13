using WorkOrderManagement.Application.Common.DTOs;
using WorkOrderManagement.Application.Common.Interfaces;

namespace WorkOrderManagement.Application.Features.Roles.Queries.GetRolePermissions;

public class GetRolePermissionsQueryHandler(IRoleService roleService)
    : IQueryHandler<GetRolePermissionsQuery, ErrorOr<List<PermissionGroupResponse>>>
{
    public Task<ErrorOr<List<PermissionGroupResponse>>> HandleAsync(GetRolePermissionsQuery query, CancellationToken cancellationToken)
        => roleService.GetRolePermissionsAsync(query.RoleId, cancellationToken);
}

