using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Application.Features.WorkOrders.Security;

namespace WorkOrderManagement.Infrastructure.Services;

public class WorkOrderReadService(IApplicationDbContext dbContext) : IWorkOrderReadService
{
    public async Task<PaginatedResponse<WorkOrderResponse>> GetPageAsync(
        GetWorkOrdersQuery query,
        WorkOrderAccessScope accessScope,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var queryable = dbContext.WorkOrders
            .AsNoTracking()
            .AsQueryable();

        queryable = WorkOrderQueryBuilder.ApplyScopeAndFilters(queryable, query, accessScope);

        var totalCount = await queryable.CountAsync(cancellationToken);

        var items = await queryable
            .OrderByDescending(w => w.Created)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WorkOrderResponse(
                w.Id,
                w.TicketNumber,
                w.RequestType,
                w.RequestType.ToString(),
                w.Status,
                w.Status.ToString(),
                w.Priority,
                w.Priority.ToString(),
                w.BranchId,
                w.Branch != null ? w.Branch.Name : string.Empty,
                w.CreatedByUserId,
                w.CreatedByUser != null ? (w.CreatedByUser.DisplayName ?? w.CreatedByUser.UserName ?? "Vendedor") : "Vendedor",
                w.ClientCode,
                w.ClientName,
                w.ClientPhone,
                w.District,
                w.Address,
                w.Description,
                w.ScheduledDate,
                w.ScheduledSlot,
                w.AssignedTechnicianId,
                w.AssignedTechnician != null ? (w.AssignedTechnician.DisplayName ?? w.AssignedTechnician.UserName) : null,
                w.Created,
                w.CompletedAt))
            .ToListAsync(cancellationToken);

        return PaginatedResponse<WorkOrderResponse>.Create(items, totalCount, pageNumber, pageSize);
    }
}
