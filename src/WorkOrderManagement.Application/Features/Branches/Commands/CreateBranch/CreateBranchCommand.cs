using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Branches.Commands.CreateBranch;

[HasPermission(Permission.Branch.Create)]
public record CreateBranchCommand(string Code, string Name, string? Address) : ICommand<ErrorOr<Guid>>;
