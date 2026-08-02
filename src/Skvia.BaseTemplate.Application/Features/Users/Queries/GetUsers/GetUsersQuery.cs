using Skvia.BaseTemplate.Application.Features.Users.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery() : IQuery<ErrorOr<List<UserResponse>>>;

