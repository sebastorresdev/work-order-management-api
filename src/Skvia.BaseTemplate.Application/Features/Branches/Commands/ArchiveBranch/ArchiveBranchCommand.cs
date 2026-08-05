using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.ArchiveBranch;

[HasPermission(Permission.Branch.Archive)]
public record ArchiveBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;
