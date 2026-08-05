using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Users.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUsers;

[HasPermission(Permission.User.View)]
public record GetUsersQuery() : IQuery<ErrorOr<List<UserResponse>>>;
