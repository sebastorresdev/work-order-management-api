using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Branches.Commands.DeleteBranch;

[HasPermission(Permission.Branch.Delete)]
public record DeleteBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;
