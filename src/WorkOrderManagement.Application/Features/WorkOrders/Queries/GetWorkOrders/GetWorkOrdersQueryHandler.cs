using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersQueryHandler(
    IWorkOrderRepository workOrderRepository)
    : IQueryHandler<GetWorkOrdersQuery, ErrorOr<PaginatedResponse<WorkOrderResponse>>>
{
    public async Task<ErrorOr<PaginatedResponse<WorkOrderResponse>>> HandleAsync(GetWorkOrdersQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Guid> userBranchIds = [];
        IReadOnlyCollection<Guid> subordinateIds = [];

        if (query.CurrentUserId.HasValue)
        {
            userBranchIds = await workOrderRepository.GetUserBranchIdsAsync(query.CurrentUserId.Value, cancellationToken);

            if (query.UserRoles != null && query.UserRoles.Count > 0)
            {
                var isSupervisor = query.UserRoles.Contains("Supervisor", StringComparer.OrdinalIgnoreCase);

                if (isSupervisor)
                {
                    subordinateIds = await workOrderRepository.GetSubordinateUserIdsAsync(query.CurrentUserId.Value, cancellationToken);
                }
            }
        }

        var accessScope = WorkOrderAccessPolicy.ResolveScope(query, userBranchIds, subordinateIds);

        return await workOrderRepository.GetPageAsync(query, accessScope, cancellationToken);
    }
}
