using Skvia.BaseTemplate.Application.Common.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUserPermissions;

public class GetUserPermissionsQueryHandler(IUserAccountService userAccountService)
    : IQueryHandler<GetUserPermissionsQuery, ErrorOr<List<PermissionGroupResponse>>>
{
    public Task<ErrorOr<List<PermissionGroupResponse>>> HandleAsync(GetUserPermissionsQuery query, CancellationToken cancellationToken)
        => userAccountService.GetUserPermissionsAsync(query.UserId, cancellationToken);
}

