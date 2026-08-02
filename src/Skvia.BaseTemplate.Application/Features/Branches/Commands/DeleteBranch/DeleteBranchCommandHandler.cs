using Skvia.BaseTemplate.Application.Common.Interfaces;
using Skvia.BaseTemplate.Domain.Branches;

namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.DeleteBranch;

public class DeleteBranchCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteBranchCommand command, CancellationToken cancellationToken)
    {
        var affectedRows = await dbContext.Branches
            .Where(u => u.Id == command.BranchId)
            .ExecuteDeleteAsync(cancellationToken);

        if (affectedRows == 0)
        {
            return BranchErrors.NotFound;
        }

        return Result.Success;
    }
}

