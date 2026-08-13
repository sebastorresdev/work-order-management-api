using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetUsers;

[HasPermission(Permission.User.View)]
public record GetUsersQuery() : IQuery<ErrorOr<List<UserResponse>>>;
