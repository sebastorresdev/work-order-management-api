using Skvia.BaseTemplate.Application.Features.Users.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery<ErrorOr<UserDetailResponse>>;

