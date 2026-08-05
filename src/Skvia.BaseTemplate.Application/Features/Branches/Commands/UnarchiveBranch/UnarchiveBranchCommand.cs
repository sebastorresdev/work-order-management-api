using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.UnarchiveBranch;

[HasPermission(Permission.Branch.Archive)]
public record UnarchiveBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;
