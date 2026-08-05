using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.DeleteBranch;

[HasPermission(Permission.Branch.Delete)]
public record DeleteBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;
