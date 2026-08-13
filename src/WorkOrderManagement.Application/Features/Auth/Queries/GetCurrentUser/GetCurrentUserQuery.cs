using WorkOrderManagement.Application.Features.Auth.DTOs;

namespace WorkOrderManagement.Application.Features.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery() : IQuery<ErrorOr<CurrentUserResponse>>;

