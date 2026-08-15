using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.Notifications.Commands.MarkNotificationAsRead;

/// <summary>
/// Comando CQRS para marcar una notificación individual como leída.
/// </summary>
/// <param name="NotificationId">Identificador de la notificación.</param>
/// <param name="UserId">Identificador del usuario propietario de la notificación.</param>
public record MarkNotificationAsReadCommand(Guid NotificationId, Guid UserId) : ICommand<ErrorOr<Success>>;
