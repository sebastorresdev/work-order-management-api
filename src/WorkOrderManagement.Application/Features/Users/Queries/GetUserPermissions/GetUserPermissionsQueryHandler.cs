using WorkOrderManagement.Application.Common.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetUserPermissions;

public class GetUserPermissionsQueryHandler(IUserAccountService userAccountService)
    : IQueryHandler<GetUserPermissionsQuery, ErrorOr<List<PermissionGroupResponse>>>
{
    public Task<ErrorOr<List<PermissionGroupResponse>>> HandleAsync(GetUserPermissionsQuery query, CancellationToken cancellationToken)
        => userAccountService.GetUserPermissionsAsync(query.UserId, cancellationToken);
}

