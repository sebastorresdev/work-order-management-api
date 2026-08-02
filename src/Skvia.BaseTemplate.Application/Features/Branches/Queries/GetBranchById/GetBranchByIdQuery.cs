using Skvia.BaseTemplate.Application.Features.Branches.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Branches.Queries.GetBranchById;

public record GetBranchByIdQuery(Guid BranchId) : IQuery<ErrorOr<BranchDetailResponse>>;

