using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Security;

/// <summary>
/// Define los modos de acceso a las órdenes de trabajo según los roles y sedes del usuario.
/// </summary>
public enum WorkOrderAccessMode
{
    /// <summary>
    /// Acceso global a todas las órdenes (Administradores).
    /// </summary>
    All,

    /// <summary>
    /// Acceso restringido por sede o lista de sedes asignadas (Backoffice / Gestores).
    /// </summary>
    ByBranch,

    /// <summary>
    /// Acceso al equipo de trabajo (Supervisores y sus subordinados).
    /// </summary>
    ByTeam,

    /// <summary>
    /// Acceso restringido exclusivamente a las órdenes creadas por el propio usuario (Vendedores).
    /// </summary>
    ByUser
}

/// <summary>
/// Representa el alcance o ámbito de acceso resuelto para la ejecución de una consulta de órdenes de trabajo.
/// </summary>
/// <param name="Mode">Modo de acceso determinado.</param>
/// <param name="BranchIds">Colección de identificadores de sedes autorizadas para la consulta.</param>
/// <param name="UserIds">Colección de identificadores de usuarios creadores autorizados para la consulta.</param>
public sealed record WorkOrderAccessScope(
    WorkOrderAccessMode Mode,
    IReadOnlyCollection<Guid>? BranchIds = null,
    IReadOnlyCollection<Guid>? UserIds = null)
{
    /// <summary>
    /// Resuelve el ámbito de visibilidad evaluando los roles del usuario, sus sedes asignadas y subordinados.
    /// </summary>
    public static WorkOrderAccessScope ResolveScope(
        GetWorkOrdersQuery query,
        IReadOnlyCollection<Guid> userBranchIds,
        IReadOnlyCollection<Guid> subordinateIds)
    {
        if (query.CurrentUserId is null || query.UserRoles is null || query.UserRoles.Count == 0)
        {
            return new WorkOrderAccessScope(WorkOrderAccessMode.All);
        }

        var isSystemAdmin = query.UserRoles.Contains("Admin", StringComparer.OrdinalIgnoreCase)
            || query.UserRoles.Contains("Administrador", StringComparer.OrdinalIgnoreCase);

        var isBackoffice = query.UserRoles.Contains("Backoffice", StringComparer.OrdinalIgnoreCase);
        var isSupervisor = query.UserRoles.Contains("Supervisor", StringComparer.OrdinalIgnoreCase);

        // Administradores: Acceso total o filtrado por una sede seleccionada
        if (isSystemAdmin)
        {
            return query.BranchId.HasValue
                ? new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, BranchIds: [query.BranchId.Value])
                : new WorkOrderAccessScope(WorkOrderAccessMode.All);
        }

        // Backoffice: Acceso a todas las solicitudes de sus sedes asignadas
        if (isBackoffice)
        {
            if (query.BranchId.HasValue)
            {
                if (userBranchIds.Count == 0 || userBranchIds.Contains(query.BranchId.Value))
                {
                    return new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, BranchIds: [query.BranchId.Value]);
                }
            }

            if (userBranchIds.Count > 0)
            {
                return new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, BranchIds: userBranchIds);
            }

            return new WorkOrderAccessScope(WorkOrderAccessMode.All);
        }

        // Supervisores: Acceso a las solicitudes creadas por ellos y por su equipo
        if (isSupervisor)
        {
            var userIds = subordinateIds
                .Append(query.CurrentUserId.Value)
                .Distinct()
                .ToList();

            var branchFilter = query.BranchId.HasValue && (userBranchIds.Count == 0 || userBranchIds.Contains(query.BranchId.Value))
                ? (IReadOnlyCollection<Guid>)[query.BranchId.Value]
                : (userBranchIds.Count > 0 ? userBranchIds : null);

            return new WorkOrderAccessScope(WorkOrderAccessMode.ByTeam, BranchIds: branchFilter, UserIds: userIds);
        }

        // Vendedores (Rol Estándar): Acceso exclusivamente a sus propias solicitudes en sus sedes autorizadas
        var userBranchFilter = query.BranchId.HasValue && (userBranchIds.Count == 0 || userBranchIds.Contains(query.BranchId.Value))
            ? (IReadOnlyCollection<Guid>)[query.BranchId.Value]
            : (userBranchIds.Count > 0 ? userBranchIds : null);

        return new WorkOrderAccessScope(
            WorkOrderAccessMode.ByUser,
            BranchIds: userBranchFilter,
            UserIds: [query.CurrentUserId.Value]);
    }
}

/// <summary>
/// Política de seguridad para la resolución del ámbito de visibilidad de órdenes de trabajo.
/// </summary>
public static class WorkOrderAccessPolicy
{
    /// <summary>
    /// Resuelve las reglas de acceso de la consulta según las características del usuario actual.
    /// </summary>
    public static WorkOrderAccessScope ResolveScope(
        GetWorkOrdersQuery query,
        IReadOnlyCollection<Guid> userBranchIds,
        IReadOnlyCollection<Guid> subordinateIds)
        => WorkOrderAccessScope.ResolveScope(query, userBranchIds, subordinateIds);
}
