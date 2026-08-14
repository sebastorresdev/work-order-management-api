using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ObserveWorkOrder;

public class ObserveWorkOrderCommandHandler(IWorkOrderRepository workOrderRepository)
    : ICommandHandler<ObserveWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ObserveWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");

        var result = workOrder.Observe(command.Reason, command.UpdatedByUserId);
        if (result.IsError) return result.Errors;

        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
