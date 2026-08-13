using WorkOrderManagement.Application.Common.DTOs;
using WorkOrderManagement.Application.Features.Roles.Commands.CreateRole;
using WorkOrderManagement.Application.Features.Roles.Commands.DeleteRole;
using WorkOrderManagement.Application.Features.Roles.Commands.UpdateRole;
using WorkOrderManagement.Application.Features.Roles.Commands.SetRolePermissions;
using WorkOrderManagement.Application.Features.Roles.DTOs;

namespace WorkOrderManagement.Application.Common.Interfaces;

/// <summary>
/// Servicio de aplicación para la gestión de roles de usuario y asignación de permisos.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Crea un nuevo rol en el sistema.
    /// </summary>
    /// <param name="command">Comando con la información para la creación del rol.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Identificador de la instancia de rol creada o errores.</returns>
    Task<ErrorOr<Guid>> CreateRoleAsync(CreateRoleCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Actualiza un rol existente.
    /// </summary>
    /// <param name="command">Comando con los datos a actualizar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> UpdateRoleAsync(UpdateRoleCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Elimina un rol del sistema.
    /// </summary>
    /// <param name="command">Comando con la orden de eliminación del rol.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> DeleteRoleAsync(DeleteRoleCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene la información detallada de un rol por su identificador.
    /// </summary>
    /// <param name="roleId">Identificador único del rol.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>DTO con los datos del rol o errores.</returns>
    Task<ErrorOr<RoleResponse>> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene la lista completa de roles registrados.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de roles o errores.</returns>
    Task<ErrorOr<List<RoleResponse>>> GetRolesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Obtiene los permisos asignados a un rol agrupados por categoría.
    /// </summary>
    /// <param name="roleId">Identificador del rol.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Grupos de permisos asignados al rol o errores.</returns>
    Task<ErrorOr<List<PermissionGroupResponse>>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken);

    /// <summary>
    /// Configura los permisos asociados a un rol específico.
    /// </summary>
    /// <param name="command">Comando con la asignación de permisos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Éxito de la operación o errores.</returns>
    Task<ErrorOr<Success>> SetRolePermissionsAsync(SetRolePermissionsCommand command, CancellationToken cancellationToken);
}

