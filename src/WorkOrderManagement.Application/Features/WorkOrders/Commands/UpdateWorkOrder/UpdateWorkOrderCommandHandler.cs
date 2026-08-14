using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.UpdateWorkOrder;

public class UpdateWorkOrderCommandHandler(IWorkOrderRepository workOrderRepository)
    : ICommandHandler<UpdateWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
        {
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");
        }

        var updateResult = workOrder.UpdateInfo(
            command.RequestType,
            command.Priority,
            command.ClientCode,
            command.ClientName,
            command.ClientPhone,
            command.District,
            command.Address,
            command.Description,
            command.ClientSecondaryPhone,
            command.AddressReference,
            command.UpdatedByUserId);

        if (updateResult.IsError) return updateResult.Errors;

        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
