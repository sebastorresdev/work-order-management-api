using Skvia.BaseTemplate.Application.Features.Auth.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IQuery<ErrorOr<CurrentUserResponse>>;

