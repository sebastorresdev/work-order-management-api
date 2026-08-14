using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Domain.Branches;

namespace WorkOrderManagement.Application.Features.Branches.Commands.CreateBranch;

public class CreateBranchCommandHandler(IBranchRepository branchRepository) : ICommandHandler<CreateBranchCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> HandleAsync(CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var cleanCode = command.Code.Trim().ToUpperInvariant();

        var branchExisting = await branchRepository.ExistsByCodeAsync(cleanCode, cancellationToken: cancellationToken);

        if (branchExisting)
            return BranchErrors.DuplicateBranch(command.Code);

        var branchResult = Branch.Create(command.Code, command.Name, command.Address);
        if (branchResult.IsError) return branchResult.Errors;

        var branch = branchResult.Value;

        await branchRepository.AddAsync(branch, cancellationToken);
        await branchRepository.SaveChangesAsync(cancellationToken);

        return branch.Id;
    }
}
