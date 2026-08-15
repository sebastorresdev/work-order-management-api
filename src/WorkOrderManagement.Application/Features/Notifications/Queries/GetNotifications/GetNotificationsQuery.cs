using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Notifications.DTOs;

namespace WorkOrderManagement.Application.Features.Notifications.Queries.GetNotifications;

/// <summary>
/// Consulta CQRS para obtener el listado de notificaciones recibidas por un usuario determinado.
/// </summary>
/// <param name="UserId">Identificador del usuario que realiza la consulta.</param>
public record GetNotificationsQuery(Guid UserId) : IQuery<ErrorOr<List<NotificationResponse>>>;
