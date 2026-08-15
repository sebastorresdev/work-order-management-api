using ErrorOr;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetTechnicians;

public class GetTechniciansQueryHandler(IUserAccountService userAccountService)
    : IQueryHandler<GetTechniciansQuery, ErrorOr<List<UserResponse>>>
{
    public Task<ErrorOr<List<UserResponse>>> HandleAsync(GetTechniciansQuery query, CancellationToken cancellationToken)
        => userAccountService.GetTechniciansAsync(query.BranchId, cancellationToken);
}
