using Skvia.BaseTemplate.Application.Features.Roles.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Roles.Queries.GetRoles;

public class GetRolesQueryHandler(IRoleService identityRoleService) : IQueryHandler<GetRolesQuery, ErrorOr<List<RoleResponse>>>
{
    public async Task<ErrorOr<List<RoleResponse>>> HandleAsync(GetRolesQuery query, CancellationToken cancellationToken)
        => await identityRoleService.GetRolesAsync(cancellationToken);
}

