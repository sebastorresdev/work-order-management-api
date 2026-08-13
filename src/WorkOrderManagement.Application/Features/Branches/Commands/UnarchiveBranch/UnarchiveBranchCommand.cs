using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Branches.Commands.UnarchiveBranch;

[HasPermission(Permission.Branch.Archive)]
public record UnarchiveBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;
