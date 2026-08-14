using WorkOrderManagement.Application.Features.Branches.DTOs;
using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Application.Features.Branches.Queries.GetBranchById;

public class GetBranchByIdQueryHandler(IBranchRepository branchRepository) : IQueryHandler<GetBranchByIdQuery, ErrorOr<BranchDetailResponse>>
{
    public async Task<ErrorOr<BranchDetailResponse>> HandleAsync(GetBranchByIdQuery query, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(query.BranchId, cancellationToken);

        return branch is not null
            ? branch
            : BranchErrors.NotFound;
    }
}

