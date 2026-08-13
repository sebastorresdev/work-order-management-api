using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Branches.DTOs;

namespace WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;

[HasPermission(Permission.Branch.View)]
public record GetBranchesQuery() : IQuery<ErrorOr<List<BranchResponse>>>;
