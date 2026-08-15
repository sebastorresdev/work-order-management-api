using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Identity;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Domain.Notifications;

/// <summary>
/// Entidad de dominio que representa una notificación o alerta del sistema para un usuario específico.
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    /// Identificador del usuario destinatario de la notificación.
    /// </summary>
    public Guid UserId { get; private set; }
    public virtual ApplicationUser User { get; private set; } = null!;

    /// <summary>
    /// Título descriptivo de la notificación.
    /// </summary>
    public string Title { get; private set; } = null!;

    /// <summary>
    /// Mensaje o contenido explicativo de la alerta.
    /// </summary>
    public string Message { get; private set; } = null!;

    /// <summary>
    /// Identificador de la orden de trabajo asociada, si aplica.
    /// </summary>
    public Guid? WorkOrderId { get; private set; }
    public virtual WorkOrder? WorkOrder { get; private set; }

    /// <summary>
    /// Tipo técnico de notificación (ej: WorkOrderObserved, ObservationResolved).
    /// </summary>
    public string Type { get; private set; } = null!;

    /// <summary>
    /// Estado de lectura de la notificación.
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// Fecha y hora exacta de registro.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    private Notification() { }

    /// <summary>
    /// Fábrica para la creación de instancias de Notificación de dominio.
    /// </summary>
    public static Notification Create(
        Guid userId,
        string title,
        string message,
        Guid? workOrderId,
        string type)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title.Trim(),
            Message = message.Trim(),
            WorkOrderId = workOrderId,
            Type = type.Trim(),
            IsRead = false,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Cambia el estado de la notificación a leída.
    /// </summary>
    public void MarkAsRead()
    {
        IsRead = true;
    }
}
