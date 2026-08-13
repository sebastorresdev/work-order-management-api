using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Branches.Commands.UpdateBranch;

[HasPermission(Permission.Branch.Update)]
public record UpdateBranchCommand(Guid BranchId, string Code, string Name, string? Address) : ICommand<ErrorOr<Success>>;
