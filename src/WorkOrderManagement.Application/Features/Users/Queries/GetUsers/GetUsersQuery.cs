using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery() : IQuery<ErrorOr<List<UserResponse>>>;
