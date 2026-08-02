namespace Skvia.BaseTemplate.Application.Features.Branches.Commands.ArchiveBranch;

public record ArchiveBranchCommand(Guid BranchId) : ICommand<ErrorOr<Success>>;

