using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Branches.DTOs;

namespace WorkOrderManagement.Application.Features.Branches.Queries.GetBranchById;

[HasPermission(Permission.Branch.View)]
public record GetBranchByIdQuery(Guid BranchId) : IQuery<ErrorOr<BranchDetailResponse>>;
