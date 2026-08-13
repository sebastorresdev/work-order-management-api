using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Roles.DTOs;

namespace WorkOrderManagement.Application.Features.Roles.Queries.GetRoles;

[HasPermission(Permission.Role.View)]
public record GetRolesQuery() : IQuery<ErrorOr<List<RoleResponse>>>;
