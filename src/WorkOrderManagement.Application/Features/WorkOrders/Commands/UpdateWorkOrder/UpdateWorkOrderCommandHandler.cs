using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.UpdateWorkOrder;

public class UpdateWorkOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(UpdateWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await dbContext.WorkOrders
            .Include(w => w.StatusHistory)
            .Include(w => w.ScheduleHistory)
            .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, cancellationToken);

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

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
