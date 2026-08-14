using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Application.Features.Branches.Commands.DeleteBranch;

public class DeleteBranchCommandHandler(IBranchRepository branchRepository) : ICommandHandler<DeleteBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetEntityByIdAsync(command.BranchId, includeArchived: true, cancellationToken: cancellationToken);

        if (branch is null)
        {
            return BranchErrors.NotFound;
        }

        var hasUsers = await branchRepository.HasUsersAsync(command.BranchId, cancellationToken);

        if (hasUsers)
        {
            return Error.Conflict(
                "Branch.HasDependencies",
                "No se puede eliminar la sede porque tiene usuarios asociados. Debe archivar la sede en su lugar.");
        }

        await branchRepository.DeleteAsync(branch, cancellationToken);
        await branchRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
