using Skvia.BaseTemplate.Application.Common.DTOs;
using Skvia.BaseTemplate.Application.Common.Security;

namespace Skvia.BaseTemplate.Application.Features.Users.Queries.GetUserPermissions;

[HasPermission(Permission.User.View)]
public record GetUserPermissionsQuery(Guid UserId) : IQuery<ErrorOr<List<PermissionGroupResponse>>>;
