using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.Notifications.Commands.MarkNotificationAsRead;

/// <summary>
/// Manejador para cambiar el estado de lectura de una notificación a verdadero.
/// </summary>
public class MarkNotificationAsReadCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<MarkNotificationAsReadCommand, ErrorOr<Success>>
{
    /// <summary>
    /// Marca la notificación especificada como leída.
    /// </summary>
    public async Task<ErrorOr<Success>> HandleAsync(MarkNotificationAsReadCommand command, CancellationToken cancellationToken)
    {
        var notification = await dbContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId && n.UserId == command.UserId, cancellationToken);

        if (notification == null)
            return Error.NotFound("Notification.NotFound", "La notificación no fue encontrada.");

        notification.MarkAsRead();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
