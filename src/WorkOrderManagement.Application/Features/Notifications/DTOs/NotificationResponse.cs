namespace WorkOrderManagement.Application.Features.Notifications.DTOs;

/// <summary>
/// DTO de respuesta que representa los datos de una notificación enviada al usuario.
/// </summary>
/// <param name="Id">Identificador único de la notificación.</param>
/// <param name="Title">Título corto o encabezado de la alerta.</param>
/// <param name="Message">Cuerpo detallado del mensaje explicativo.</param>
/// <param name="WorkOrderId">Identificador opcional de la orden de trabajo relacionada.</param>
/// <param name="Type">Tipo de notificación (WorkOrderObserved, ObservationResolved, WorkOrderScheduled, WorkOrderCreated).</param>
/// <param name="IsRead">Indica si la notificación ya fue vista o leída por el usuario.</param>
/// <param name="CreatedAt">Fecha y hora de creación de la notificación.</param>
public record NotificationResponse(
    Guid Id,
    string Title,
    string Message,
    Guid? WorkOrderId,
    string Type,
    bool IsRead,
    DateTimeOffset CreatedAt);
