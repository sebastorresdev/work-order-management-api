using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Notifications.DTOs;

namespace WorkOrderManagement.Application.Features.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(Guid UserId) : IQuery<ErrorOr<List<NotificationResponse>>>;
