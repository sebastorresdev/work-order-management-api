using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;

/// <summary>
/// Manejador para marcar masivamente como leídas las notificaciones de un usuario.
/// </summary>
public class MarkAllNotificationsAsReadCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<MarkAllNotificationsAsReadCommand, ErrorOr<Success>>
{
    /// <summary>
    /// Actualiza el estado de todas las notificaciones no leídas del usuario.
    /// </summary>
    public async Task<ErrorOr<Success>> HandleAsync(MarkAllNotificationsAsReadCommand command, CancellationToken cancellationToken)
    {
        var unreadNotifications = await dbContext.Notifications
            .Where(n => n.UserId == command.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.MarkAsRead();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
