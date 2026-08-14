using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;
using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Application.Features.Branches.Commands.ArchiveBranch;

public class ArchiveBranchCommandHandler(IBranchRepository branchRepository, ICurrentUserProvider currentUserProvider)
    : ICommandHandler<ArchiveBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ArchiveBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetEntityByIdAsync(command.BranchId, cancellationToken: cancellationToken);

        if (branch is null)
            return BranchErrors.NotFound;

        Guid? userId = null;
        try
        {
            var currentUser = currentUserProvider.GetCurrentUser();
            userId = currentUser?.Id;
        }
        catch (InvalidOperationException) { }

        ((IArchivable)branch).Archive(userId);

        await branchRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
