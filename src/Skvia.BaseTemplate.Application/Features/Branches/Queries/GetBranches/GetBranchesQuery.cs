using Skvia.BaseTemplate.Application.Features.Branches.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Branches.Queries.GetBranches;

public record GetBranchesQuery() : IQuery<ErrorOr<List<BranchResponse>>>;

