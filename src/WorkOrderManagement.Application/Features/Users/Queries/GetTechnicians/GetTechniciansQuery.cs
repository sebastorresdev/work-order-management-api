using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetTechnicians;

public record GetTechniciansQuery(Guid? BranchId = null) : IQuery<ErrorOr<List<UserResponse>>>;
