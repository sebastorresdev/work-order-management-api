using ErrorOr;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.Notifications;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ScheduleWorkOrder;

public class ScheduleWorkOrderCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IApplicationDbContext dbContext)
    : ICommandHandler<ScheduleWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ScheduleWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
        {
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");
        }

        var result = workOrder.Schedule(
            command.ScheduledDate,
            command.ScheduledSlot,
            command.AssignedTechnicianId,
            command.Notes,
            command.ScheduledByUserId);

        if (result.IsError) return result.Errors;

        // Notificar al Vendedor creador de la solicitud
        var notification = Notification.Create(
            workOrder.CreatedByUserId,
            "Solicitud Agendada",
            $"Tu solicitud {workOrder.TicketNumber} fue agendada para el {command.ScheduledDate:dd/MM/yyyy} ({command.ScheduledSlot}).",
            workOrder.Id,
            "WorkOrderScheduled");

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);

        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
