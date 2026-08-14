using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;
using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Application.Features.Branches.Commands.UnarchiveBranch;

public class UnarchiveBranchCommandHandler(IBranchRepository branchRepository)
    : ICommandHandler<UnarchiveBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UnarchiveBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetEntityByIdAsync(command.BranchId, includeArchived: true, cancellationToken: cancellationToken);

        if (branch is null)
            return BranchErrors.NotFound;

        ((IArchivable)branch).Unarchive();

        await branchRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
