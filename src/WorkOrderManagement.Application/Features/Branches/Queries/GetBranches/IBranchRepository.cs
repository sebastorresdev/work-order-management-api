using WorkOrderManagement.Application.Features.Branches.DTOs;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;

public interface IBranchRepository
{
    Task<List<BranchResponse>> GetBranchesAsync(CancellationToken cancellationToken = default);
    Task<BranchDetailResponse?> GetByIdAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<Branch?> GetEntityByIdAsync(Guid branchId, bool includeArchived = false, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> HasUsersAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task AddAsync(Branch branch, CancellationToken cancellationToken = default);
    Task DeleteAsync(Branch branch, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
