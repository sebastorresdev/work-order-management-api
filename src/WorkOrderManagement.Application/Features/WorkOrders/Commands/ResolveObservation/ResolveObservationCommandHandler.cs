using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.Notifications;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ResolveObservation;

/// <summary>
/// Manejador de la lógica de negocio para levantar una observación en una orden de trabajo.
/// Cambia el estado de Observado a Pendiente y envía notificaciones únicamente al personal de Backoffice de la sede correspondiente.
/// </summary>
public class ResolveObservationCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IApplicationDbContext dbContext)
    : ICommandHandler<ResolveObservationCommand, ErrorOr<Success>>
{
    /// <summary>
    /// Procesa el levantamiento de la observación y la generación de alertas para el equipo de gestión.
    /// </summary>
    public async Task<ErrorOr<Success>> HandleAsync(ResolveObservationCommand command, CancellationToken cancellationToken)
    {
        // 1. Obtener la solicitud existente por su ID
        var workOrder = await workOrderRepository.GetByIdAsync(command.WorkOrderId, cancellationToken);

        if (workOrder == null)
            return Error.NotFound("WorkOrder.NotFound", "La orden de trabajo no existe.");

        // 2. Ejecutar el método de dominio para subsanar la observación
        var result = workOrder.ResolveObservation(command.ResolutionNotes, command.UpdatedByUserId);
        if (result.IsError) return result.Errors;

        // 3. Notificar únicamente al personal de Backoffice y Administradores que pertenecen a la sede de la solicitud
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

        // 4. Persistir cambios en la base de datos
        await dbContext.SaveChangesAsync(cancellationToken);
        await workOrderRepository.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
