using Skvia.BaseTemplate.Application.Common.DTOs;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUserPermissions;

public record GetUserPermissionsQuery(Guid UserId) : IQuery<ErrorOr<List<PermissionGroupResponse>>>;


