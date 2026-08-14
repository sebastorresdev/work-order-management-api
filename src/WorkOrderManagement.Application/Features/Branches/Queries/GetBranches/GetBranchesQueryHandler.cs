using WorkOrderManagement.Application.Features.Branches.DTOs;

namespace WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;

public class GetBranchesQueryHandler(IBranchRepository branchRepository) : IQueryHandler<GetBranchesQuery, ErrorOr<List<BranchResponse>>>
{
    public async Task<ErrorOr<List<BranchResponse>>> HandleAsync(GetBranchesQuery query, CancellationToken cancellationToken)
    {
        return await branchRepository.GetBranchesAsync(cancellationToken);
    }
}

