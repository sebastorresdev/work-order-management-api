using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;

public class MarkAllNotificationsAsReadCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<MarkAllNotificationsAsReadCommand, ErrorOr<Success>>
{
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
