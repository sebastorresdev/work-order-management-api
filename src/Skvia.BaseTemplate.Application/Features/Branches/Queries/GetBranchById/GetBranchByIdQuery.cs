using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Branches.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Branches.Queries.GetBranchById;

[HasPermission(Permission.Branch.View)]
public record GetBranchByIdQuery(Guid BranchId) : IQuery<ErrorOr<BranchDetailResponse>>;
