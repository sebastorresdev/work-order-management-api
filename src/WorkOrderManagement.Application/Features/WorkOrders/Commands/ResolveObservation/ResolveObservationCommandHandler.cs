using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.Notifications;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ResolveObservation;

public class ResolveObservationCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IApplicationDbContext dbContext)
    : ICommandHandler<ResolveObservationCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ResolveObservationCommand command, CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");

        var result = workOrder.ResolveObservation(command.ResolutionNotes, command.UpdatedByUserId);
        if (result.IsError) return result.Errors;

        var backofficeUserIds = await dbContext.ApplicationUsers
            .Where(u => u.IsActive &&
                (u.BranchId == workOrder.BranchId || u.BranchUsers.Any(bu => bu.BranchId == workOrder.BranchId)) &&
                u.UserRoles.Any(ur => ur.Role.Name == "Backoffice" || ur.Role.Name == "Administrator" || ur.Role.Name == "Admin"))
            .Select(u => u.Id)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var userId in backofficeUserIds)
        {
            var notification = Notification.Create(
                userId,
                "Observación Subsanada",
                $"El vendedor subsanó la observación de la solicitud {workOrder.TicketNumber}: {command.ResolutionNotes.Trim()}",
                workOrder.Id,
                "ObservationResolved");

            dbContext.Notifications.Add(notification);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
