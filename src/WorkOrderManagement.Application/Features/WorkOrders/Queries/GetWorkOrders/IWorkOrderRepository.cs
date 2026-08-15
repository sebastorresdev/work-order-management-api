using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

public interface IWorkOrderRepository
{
    Task<IReadOnlyCollection<Guid>> GetUserBranchIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Guid>> GetSubordinateUserIdsAsync(Guid supervisorId, CancellationToken cancellationToken = default);

    Task<int> CountTicketNumbersAsync(string ticketPrefix, CancellationToken cancellationToken = default);

    Task<WorkOrder?> GetByIdAsync(Guid workOrderId, CancellationToken cancellationToken = default);

    Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<PaginatedResponse<WorkOrderResponse>> GetPageAsync(
        GetWorkOrdersQuery query,
        WorkOrderAccessScope accessScope,
        CancellationToken cancellationToken = default);
}
