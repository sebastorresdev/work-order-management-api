using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Security;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

public interface IWorkOrderReadService
{
    Task<PaginatedResponse<WorkOrderResponse>> GetPageAsync(
        GetWorkOrdersQuery query,
        WorkOrderAccessScope accessScope,
        CancellationToken cancellationToken = default);
}
