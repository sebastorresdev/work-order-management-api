namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.DeleteBranch;

public record DeleteBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;

