using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;

/// <summary>
/// Comando CQRS para marcar todas las notificaciones pendientes de un usuario como leídas.
/// </summary>
/// <param name="UserId">Identificador del usuario.</param>
public record MarkAllNotificationsAsReadCommand(Guid UserId) : ICommand<ErrorOr<Success>>;
