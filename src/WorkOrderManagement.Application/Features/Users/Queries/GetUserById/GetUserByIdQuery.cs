using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetUserById;

[HasPermission(Permission.User.View)]
public record GetUserByIdQuery(Guid UserId) : IQuery<ErrorOr<UserDetailResponse>>;
