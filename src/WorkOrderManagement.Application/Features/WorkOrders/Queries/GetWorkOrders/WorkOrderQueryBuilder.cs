using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

/// <summary>
/// Constructor de consultas IQueryable de Entity Framework Core para aplicar filtros de seguridad por roles y filtros dinámicos.
/// </summary>
public static class WorkOrderQueryBuilder
{
    /// <summary>
    /// Aplica las cláusulas Where de visibilidad según el ámbito de acceso resuelto (Scope) y los filtros adicionales (Estado, Tipo, Término de búsqueda).
    /// </summary>
    public static IQueryable<WorkOrder> ApplyScopeAndFilters(
        IQueryable<WorkOrder> queryable,
        GetWorkOrdersQuery query,
        WorkOrderAccessScope accessScope)
    {
        // 1. Aplicar filtro de visibilidad por rol y sede
        var scopedQuery = accessScope.Mode switch
        {
            // Backoffice: Filtra por el conjunto de sedes asignadas
            WorkOrderAccessMode.ByBranch when accessScope.BranchIds is { Count: > 0 } =>
                queryable.Where(w => accessScope.BranchIds.Contains(w.BranchId)),

            // Supervisores: Filtra por solicitudes de sus subordinados e históricas dentro de sus sedes
            WorkOrderAccessMode.ByTeam when accessScope.UserIds is { Count: > 0 } =>
                accessScope.BranchIds is { Count: > 0 }
                    ? queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId) && accessScope.BranchIds.Contains(w.BranchId))
                    : queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId)),

            // Vendedores (Rol Estándar): Filtra por sus propias solicitudes Y dentro de sus sedes autorizadas (conjunción &&)
            WorkOrderAccessMode.ByUser when accessScope.UserIds is { Count: > 0 } =>
                accessScope.BranchIds is { Count: > 0 }
                    ? queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId) && accessScope.BranchIds.Contains(w.BranchId))
                    : queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId)),

            // Administradores u otros casos por defecto
            _ => accessScope.BranchIds is { Count: > 0 }
                ? queryable.Where(w => accessScope.BranchIds.Contains(w.BranchId))
                : queryable
        };

        // 2. Filtro por Estado
        if (query.Status.HasValue)
        {
            scopedQuery = scopedQuery.Where(w => w.Status == query.Status.Value);
        }

        // 3. Filtro por Tipo de Solicitud (Instalación, Avería, Encomienda)
        if (query.RequestType.HasValue)
        {
            scopedQuery = scopedQuery.Where(w => w.RequestType == query.RequestType.Value);
        }

        // 4. Filtro por Término de Búsqueda (Ticket, Cliente, Teléfono, Distrito, Dirección)
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var normalizedTerm = query.SearchTerm.Trim();
            var loweredTerm = normalizedTerm.ToLower();

            scopedQuery = scopedQuery.Where(w =>
                w.TicketNumber.ToLower().Contains(loweredTerm) ||
                w.ClientCode.ToLower().Contains(loweredTerm) ||
                w.ClientName.ToLower().Contains(loweredTerm) ||
                w.District.ToLower().Contains(loweredTerm) ||
                w.Address.ToLower().Contains(loweredTerm));
        }

        return scopedQuery;
    }
}
