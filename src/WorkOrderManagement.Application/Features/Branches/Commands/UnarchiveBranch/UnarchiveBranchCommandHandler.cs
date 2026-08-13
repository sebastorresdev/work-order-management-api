using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Domain.Branches;
using WorkOrderManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace WorkOrderManagement.Application.Features.Branches.Commands.UnarchiveBranch;

public class UnarchiveBranchCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UnarchiveBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UnarchiveBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id == command.BranchId, cancellationToken);

        if (branch is null)
            return BranchErrors.NotFound;

        branch.Unarchive();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
