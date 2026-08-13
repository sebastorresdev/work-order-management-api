using WorkOrderManagement.Domain.Branches;
using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Application.Features.Branches.Commands.ArchiveBranch;

public class ArchiveBranchCommandHandler(IApplicationDbContext dbContext, ICurrentUserProvider currentUserProvider)
    : ICommandHandler<ArchiveBranchCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ArchiveBranchCommand command, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches.FirstOrDefaultAsync(b => b.Id == command.BranchId, cancellationToken);

        if (branch is null)
            return BranchErrors.NotFound;

        Guid? userId = null;
        try
        {
            var currentUser = currentUserProvider.GetCurrentUser();
            userId = currentUser?.Id;
        }
        catch (InvalidOperationException) { }

        branch.Archive(userId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
