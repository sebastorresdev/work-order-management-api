using WorkOrderManagement.Application.Features.Branches.DTOs;

namespace WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;

public record GetBranchesQuery() : IQuery<ErrorOr<List<BranchResponse>>>;
