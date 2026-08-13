using WorkOrderManagement.Application.Common.DTOs;
using WorkOrderManagement.Application.Common.Security;

namespace WorkOrderManagement.Application.Features.Users.Queries.GetUserPermissions;

[HasPermission(Permission.User.View)]
public record GetUserPermissionsQuery(Guid UserId) : IQuery<ErrorOr<List<PermissionGroupResponse>>>;
