using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Branches;

namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.UnarchiveBranch;

public class UnarchiveBranchCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<UnarchiveBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UnarchiveBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches.FindAsync([command.BranchId], cancellationToken);

        if (branch is null)
            return BranchErrors.NotFound;

        // TODO: Corregir implementacion
        // branch.Unarchive();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

