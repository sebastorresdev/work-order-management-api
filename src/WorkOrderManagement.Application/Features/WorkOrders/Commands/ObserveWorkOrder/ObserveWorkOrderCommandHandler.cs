using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ObserveWorkOrder;

public class ObserveWorkOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<ObserveWorkOrderCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ObserveWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await dbContext.WorkOrders
            .Include(w => w.StatusHistory)
            .Include(w => w.ScheduleHistory)
            .FirstOrDefaultAsync(w => w.Id == command.WorkOrderId, cancellationToken);

        if (workOrder == null)
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");

        var result = workOrder.Observe(command.Reason, command.UpdatedByUserId);
        if (result.IsError) return result.Errors;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
