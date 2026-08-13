using WorkOrderManagement.Application.Features.Users.DTOs;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserAccountService userAccountService)
    : IQueryHandler<GetUserByIdQuery, ErrorOr<UserDetailResponse>>
{
    public Task<ErrorOr<UserDetailResponse>> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken)
        => userAccountService.GetUserByIdAsync(query.UserId, cancellationToken);
}

