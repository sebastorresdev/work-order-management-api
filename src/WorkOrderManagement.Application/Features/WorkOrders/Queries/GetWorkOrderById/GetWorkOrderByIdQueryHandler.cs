using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrderById;

public class GetWorkOrderByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetWorkOrderByIdQuery, ErrorOr<WorkOrderDetailResponse>>
{
    public async Task<ErrorOr<WorkOrderDetailResponse>> HandleAsync(GetWorkOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var workOrder = await dbContext.WorkOrders
            .AsNoTracking()
            .Include(w => w.Branch)
            .Include(w => w.CreatedByUser)
            .Include(w => w.AssignedTechnician)
            .Include(w => w.StatusHistory)
                .ThenInclude(sh => sh.ChangedByUser)
            .Include(w => w.ScheduleHistory)
                .ThenInclude(sch => sch.AssignedTechnician)
            .Include(w => w.ScheduleHistory)
                .ThenInclude(sch => sch.ScheduledByUser)
            .FirstOrDefaultAsync(w => w.Id == query.Id, cancellationToken);

        if (workOrder == null)
        {
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");
        }

        var statusHistory = workOrder.StatusHistory
            .OrderByDescending(sh => sh.Timestamp)
            .Select(sh => new WorkOrderStatusHistoryResponse(
                sh.Id,
                sh.StatusFrom,
                sh.StatusFrom.ToString(),
                sh.StatusTo,
                sh.StatusTo.ToString(),
                sh.Comments,
                sh.ChangedByUserId,
                sh.ChangedByUser?.DisplayName ?? sh.ChangedByUser?.UserName ?? "Sistema",
                sh.Timestamp))
            .ToList();

        var scheduleHistory = workOrder.ScheduleHistory
            .OrderByDescending(sch => sch.ScheduledAt)
            .Select(sch => new WorkOrderScheduleHistoryResponse(
                sch.Id,
                sch.ScheduledDate,
                sch.ScheduledSlot,
                sch.AssignedTechnicianId,
                sch.AssignedTechnician?.DisplayName ?? sch.AssignedTechnician?.UserName,
                sch.Notes,
                sch.ScheduledByUserId,
                sch.ScheduledByUser?.DisplayName ?? sch.ScheduledByUser?.UserName ?? "Backoffice",
                sch.ScheduledAt))
            .ToList();

        return new WorkOrderDetailResponse(
            workOrder.Id,
            workOrder.TicketNumber,
            workOrder.RequestType,
            workOrder.RequestType.ToString(),
            workOrder.Status,
            workOrder.Status.ToString(),
            workOrder.Priority,
            workOrder.Priority.ToString(),
            workOrder.BranchId,
            workOrder.Branch.Name,
            workOrder.CreatedByUserId,
            workOrder.CreatedByUser.DisplayName ?? workOrder.CreatedByUser.UserName ?? "Vendedor",
            workOrder.ClientCode,
            workOrder.ClientName,
            workOrder.ClientPhone,
            workOrder.ClientSecondaryPhone,
            workOrder.District,
            workOrder.Address,
            workOrder.AddressReference,
            workOrder.Description,
            workOrder.ScheduledDate,
            workOrder.ScheduledSlot,
            workOrder.AssignedTechnicianId,
            workOrder.AssignedTechnician?.DisplayName ?? workOrder.AssignedTechnician?.UserName,
            workOrder.CompletedAt,
            workOrder.CompletionNotes,
            workOrder.ObservationNotes,
            workOrder.RejectionReason,
            workOrder.CancellationReason,
            workOrder.Created,
            statusHistory,
            scheduleHistory);
    }
}
