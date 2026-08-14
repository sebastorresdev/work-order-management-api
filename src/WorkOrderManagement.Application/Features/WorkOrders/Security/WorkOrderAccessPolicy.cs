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
    Guid? BranchId = null,
    IReadOnlyCollection<Guid>? UserIds = null)
{
    public static WorkOrderAccessScope ResolveScope(
        GetWorkOrdersQuery query,
        Guid? currentUserBranchId,
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
                ? new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, query.BranchId.Value)
                : new WorkOrderAccessScope(WorkOrderAccessMode.All);
        }

        if (isBackoffice)
        {
            if (query.BranchId.HasValue)
            {
                return new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, query.BranchId.Value);
            }

            if (currentUserBranchId.HasValue)
            {
                return new WorkOrderAccessScope(WorkOrderAccessMode.ByBranch, currentUserBranchId.Value);
            }

            return new WorkOrderAccessScope(WorkOrderAccessMode.All);
        }

        if (isSupervisor)
        {
            var userIds = subordinateIds
                .Append(query.CurrentUserId.Value)
                .Distinct()
                .ToList();

            return new WorkOrderAccessScope(WorkOrderAccessMode.ByTeam, null, userIds);
        }

        return new WorkOrderAccessScope(
            WorkOrderAccessMode.ByUser,
            null,
            [query.CurrentUserId.Value]);
    }
}

public static class WorkOrderAccessPolicy
{
    public static WorkOrderAccessScope ResolveScope(
        GetWorkOrdersQuery query,
        Guid? currentUserBranchId,
        IReadOnlyCollection<Guid> subordinateIds)
        => WorkOrderAccessScope.ResolveScope(query, currentUserBranchId, subordinateIds);
}
