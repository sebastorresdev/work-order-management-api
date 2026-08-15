using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

public static class WorkOrderQueryBuilder
{
    public static IQueryable<WorkOrder> ApplyScopeAndFilters(
        IQueryable<WorkOrder> queryable,
        GetWorkOrdersQuery query,
        WorkOrderAccessScope accessScope)
    {
        var scopedQuery = accessScope.Mode switch
        {
            WorkOrderAccessMode.ByBranch when accessScope.BranchIds is { Count: > 0 } =>
                queryable.Where(w => accessScope.BranchIds.Contains(w.BranchId)),

            WorkOrderAccessMode.ByTeam when accessScope.UserIds is { Count: > 0 } =>
                accessScope.BranchIds is { Count: > 0 }
                    ? queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId) && accessScope.BranchIds.Contains(w.BranchId))
                    : queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId)),

            WorkOrderAccessMode.ByUser when accessScope.UserIds is { Count: > 0 } =>
                accessScope.BranchIds is { Count: > 0 }
                    ? queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId) && accessScope.BranchIds.Contains(w.BranchId))
                    : queryable.Where(w => accessScope.UserIds.Contains(w.CreatedByUserId)),

            _ => accessScope.BranchIds is { Count: > 0 }
                ? queryable.Where(w => accessScope.BranchIds.Contains(w.BranchId))
                : queryable
        };

        if (query.Status.HasValue)
        {
            scopedQuery = scopedQuery.Where(w => w.Status == query.Status.Value);
        }

        if (query.RequestType.HasValue)
        {
            scopedQuery = scopedQuery.Where(w => w.RequestType == query.RequestType.Value);
        }

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
