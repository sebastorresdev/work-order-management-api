using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Notifications.DTOs;

namespace WorkOrderManagement.Application.Features.Notifications.Queries.GetNotifications;

/// <summary>
/// Manejador para obtener las notificaciones ordenadas cronológicamente para el usuario.
/// </summary>
public class GetNotificationsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetNotificationsQuery, ErrorOr<List<NotificationResponse>>>
{
    /// <summary>
    /// Recupera las últimas 30 notificaciones asociadas al usuario autenticado.
    /// </summary>
    public async Task<ErrorOr<List<NotificationResponse>>> HandleAsync(GetNotificationsQuery query, CancellationToken cancellationToken)
    {
        var notifications = await dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == query.UserId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(30)
            .Select(n => new NotificationResponse(
                n.Id,
                n.Title,
                n.Message,
                n.WorkOrderId,
                n.Type,
                n.IsRead,
                n.CreatedAt))
            .ToListAsync(cancellationToken);

        return notifications;
    }
}
