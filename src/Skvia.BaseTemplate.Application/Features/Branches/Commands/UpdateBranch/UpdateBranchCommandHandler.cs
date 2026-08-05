using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Branches;

namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.UpdateBranch;

public class UpdateBranchCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches.FindAsync([command.BranchId], cancellationToken);

        if (branch is null)
        {
            return BranchErrors.NotFound;
        }

        var cleanNormalizedCode = command.Code.Trim().ToUpperInvariant();

        if (await dbContext.Branches
               .AnyAsync(b => b.Code == cleanNormalizedCode && b.Id != command.BranchId, cancellationToken))
            return BranchErrors.DuplicateBranch(command.Name);

        var updateResult = branch.Update(cleanNormalizedCode, command.Name, command.Address);
        if (updateResult.IsError) return updateResult.Errors;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
