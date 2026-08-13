using WorkOrderManagement.Application.Common.Security;
using WorkOrderManagement.Application.Features.Roles.DTOs;

namespace WorkOrderManagement.Application.Features.Roles.Queries.GetRoleById;

[HasPermission(Permission.Role.View)]
public record GetRoleByIdQuery(Guid Id) : IQuery<ErrorOr<RoleResponse>>;
