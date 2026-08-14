using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ScheduleWorkOrder;

public class ScheduleWorkOrderCommandHandler(IWorkOrderRepository workOrderRepository)
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

        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
