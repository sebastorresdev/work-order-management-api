using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Infrastructure.Repositories;

public class WorkOrderRepository(IApplicationDbContext dbContext) : IWorkOrderRepository
{
    public async Task<Guid?> GetUserBranchIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.ApplicationUsers
            .Include(u => u.BranchUsers)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return null;
        }

        if (user.BranchId.HasValue)
        {
            return user.BranchId.Value;
        }

        var assignedBranchId = user.BranchUsers
            .Select(bu => bu.BranchId)
            .FirstOrDefault();

        return assignedBranchId == Guid.Empty ? null : assignedBranchId;
    }

    public async Task<IReadOnlyCollection<Guid>> GetSubordinateUserIdsAsync(Guid supervisorId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ApplicationUsers
            .Where(u => u.SupervisorId == supervisorId)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountTicketNumbersAsync(string ticketPrefix, CancellationToken cancellationToken = default)
    {
        return await dbContext.WorkOrders
            .CountAsync(w => w.TicketNumber.StartsWith(ticketPrefix), cancellationToken);
    }

    public async Task<WorkOrder?> GetByIdAsync(Guid workOrderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.WorkOrders
            .Include(w => w.StatusHistory)
            .Include(w => w.ScheduleHistory)
            .FirstOrDefaultAsync(w => w.Id == workOrderId, cancellationToken);
    }

    public async Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken = default)
    {
        dbContext.WorkOrders.Add(workOrder);
        await Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

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
