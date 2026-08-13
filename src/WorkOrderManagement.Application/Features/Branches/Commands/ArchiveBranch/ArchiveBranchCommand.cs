using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Branches.Commands.ArchiveBranch;

[HasPermission(Permission.Branch.Archive)]
public record ArchiveBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;
