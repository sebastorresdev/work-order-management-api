using Skvia.BaseTemplate.Application.Features.Users.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserAccountService userAccountService)
    : IQueryHandler<GetUserByIdQuery, ErrorOr<UserDetailResponse>>
{
    public Task<ErrorOr<UserDetailResponse>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        => userAccountService.GetUserByIdAsync(query.UserId, cancellationToken);
}

