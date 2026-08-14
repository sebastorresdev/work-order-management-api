using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Features.Branches.DTOs;
using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Infrastructure.Repositories;

public class BranchRepository(IApplicationDbContext dbContext) : IBranchRepository
{
    public async Task<List<BranchResponse>> GetBranchesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Branches
            .AsNoTracking()
            .Select(b => new BranchResponse(b.Id, b.Code, b.Name, b.Address))
            .ToListAsync(cancellationToken);
    }

    public async Task<BranchDetailResponse?> GetByIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Branches
            .AsNoTracking()
            .Where(b => b.Id == branchId)
            .Select(b => new BranchDetailResponse(b.Id, b.Code, b.Name, b.Address))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Branch?> GetEntityByIdAsync(Guid branchId, bool includeArchived = false, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Branches.AsQueryable();

        if (includeArchived)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query
            .FirstOrDefaultAsync(b => b.Id == branchId, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await dbContext.Branches
            .AnyAsync(b => b.Code == normalizedCode && (!excludeId.HasValue || b.Id != excludeId.Value), cancellationToken);
    }

    public async Task<bool> HasUsersAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await dbContext.BranchUsers
            .AnyAsync(bu => bu.BranchId == branchId, cancellationToken);
    }

    public async Task AddAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        dbContext.Branches.Add(branch);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        dbContext.Branches.Remove(branch);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
