using Skvia.BaseTemplate.Application.Common.Security;
using Skvia.BaseTemplate.Application.Features.Users.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUserById;

[HasPermission(Permission.User.View)]
public record GetUserByIdQuery(Guid UserId) : IQuery<ErrorOr<UserDetailResponse>>;
