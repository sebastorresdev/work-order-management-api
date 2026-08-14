using WorkOrderManagement.Application.Features.Branches.Commands.CreateBranch;
using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Application.Features.Branches.Commands.UpdateBranch;

public class UpdateBranchCommandHandler(IBranchRepository branchRepository) : ICommandHandler<UpdateBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetEntityByIdAsync(command.BranchId, cancellationToken: cancellationToken);

        if (branch is null)
        {
            return BranchErrors.NotFound;
        }

        var cleanNormalizedCode = command.Code.Trim().ToUpperInvariant();

        if (await branchRepository.ExistsByCodeAsync(cleanNormalizedCode, command.BranchId, cancellationToken))
            return BranchErrors.DuplicateBranch(command.Name);

        var updateResult = branch.Update(cleanNormalizedCode, command.Name, command.Address);
        if (updateResult.IsError) return updateResult.Errors;

        await branchRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
