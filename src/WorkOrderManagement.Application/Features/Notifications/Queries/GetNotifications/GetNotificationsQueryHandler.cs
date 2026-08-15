using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Notifications.DTOs;

namespace WorkOrderManagement.Application.Features.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetNotificationsQuery, ErrorOr<List<NotificationResponse>>>
{
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
