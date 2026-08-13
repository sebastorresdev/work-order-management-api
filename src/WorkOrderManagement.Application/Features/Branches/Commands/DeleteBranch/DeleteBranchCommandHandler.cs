using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Domain.Branches;
using Microsoft.EntityFrameworkCore;

namespace WorkOrderManagement.Application.Features.Branches.Commands.DeleteBranch;

public class DeleteBranchCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(DeleteBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.Id == command.BranchId, cancellationToken);

        if (branch is null)
        {
            return BranchErrors.NotFound;
        }

        var hasUsers = await dbContext.BranchUsers.AnyAsync(bu => bu.BranchId == command.BranchId, cancellationToken);

        if (hasUsers)
        {
            return Error.Conflict(
                "Branch.HasDependencies",
                "No se puede eliminar la sede porque tiene usuarios asociados. Debe archivar la sede en su lugar.");
        }

        dbContext.Branches.Remove(branch);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
