using ErrorOr;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.Notifications;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ObserveWorkOrder;

public class ObserveWorkOrderCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IApplicationDbContext dbContext)
    : ICommandHandler<ObserveWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ObserveWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");

        var result = workOrder.Observe(command.Reason, command.UpdatedByUserId);
        if (result.IsError) return result.Errors;

        // Notificar al Vendedor creador de la solicitud
        var notification = Notification.Create(
            workOrder.CreatedByUserId,
            "Solicitud Observada",
            $"Tu solicitud {workOrder.TicketNumber} fue observada por Backoffice: {command.Reason.Trim()}",
            workOrder.Id,
            "WorkOrderObserved");

        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);

        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
