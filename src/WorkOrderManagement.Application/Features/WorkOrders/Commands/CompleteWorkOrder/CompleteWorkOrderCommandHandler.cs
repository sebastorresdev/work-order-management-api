using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.CompleteWorkOrder;

public class CompleteWorkOrderCommandHandler(IWorkOrderRepository workOrderRepository)
    : ICommandHandler<CompleteWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(CompleteWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");

        var result = workOrder.Complete(command.CompletionNotes, command.UpdatedByUserId);
        if (result.IsError) return result.Errors;

        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
