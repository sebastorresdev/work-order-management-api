using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Branches.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Branches.Queries.GetBranches;

[HasPermission(Permission.Branch.View)]
public record GetBranchesQuery() : IQuery<ErrorOr<List<BranchResponse>>>;
