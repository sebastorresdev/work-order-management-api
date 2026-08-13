using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Identity;

namespace WorkOrderManagement.Domain.WorkOrders;

/// <summary>
/// Bitácora de auditoría para registrar cada cambio de estado de la orden de trabajo.
/// </summary>
public class WorkOrderStatusHistory : BaseEntity
{
    public Guid WorkOrderId { get; private set; }
    public virtual WorkOrder WorkOrder { get; private set; } = null!;

    public WorkOrderStatus StatusFrom { get; private set; }
    public WorkOrderStatus StatusTo { get; private set; }

    public string? Comments { get; private set; }

    public Guid ChangedByUserId { get; private set; }
    public virtual ApplicationUser ChangedByUser { get; private set; } = null!;

    public DateTimeOffset Timestamp { get; private set; }

    private WorkOrderStatusHistory() { }

    public static WorkOrderStatusHistory Create(
        Guid workOrderId,
        WorkOrderStatus statusFrom,
        WorkOrderStatus statusTo,
        string? comments,
        Guid changedByUserId)
    {
        return new WorkOrderStatusHistory
        {
            Id = Guid.Empty,
            WorkOrderId = workOrderId,
            StatusFrom = statusFrom,
            StatusTo = statusTo,
            Comments = comments?.Trim(),
            ChangedByUserId = changedByUserId,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}
