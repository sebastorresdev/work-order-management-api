using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Security;

public enum WorkOrderAccessMode
{
    All,
    ByBranch,
    ByTeam,
    ByUser
}

public sealed record WorkOrderAccessScope(
    WorkOrderAccessMode Mode,
    IReadOnlyCollection<Guid>? BranchIds = null,
    IReadOnlyCollection<Guid>? UserIds = null)
{
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

        if (isSystemAdmin)
        {
            return query.BranchId.HasValue
                ? new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, BranchIds: [query.BranchId.Value])
                : new WorkOrderAccessScope(WorkOrderAccessMode.All);
        }

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

        var userBranchFilter = query.BranchId.HasValue && (userBranchIds.Count == 0 || userBranchIds.Contains(query.BranchId.Value))
            ? (IReadOnlyCollection<Guid>)[query.BranchId.Value]
            : (userBranchIds.Count > 0 ? userBranchIds : null);

        return new WorkOrderAccessScope(
            WorkOrderAccessMode.ByUser,
            BranchIds: userBranchFilter,
            UserIds: [query.CurrentUserId.Value]);
    }
}

public static class WorkOrderAccessPolicy
{
    public static WorkOrderAccessScope ResolveScope(
        GetWorkOrdersQuery query,
        IReadOnlyCollection<Guid> userBranchIds,
        IReadOnlyCollection<Guid> subordinateIds)
        => WorkOrderAccessScope.ResolveScope(query, userBranchIds, subordinateIds);
}
