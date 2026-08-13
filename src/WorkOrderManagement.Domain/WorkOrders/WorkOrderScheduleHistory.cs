using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Identity;

namespace WorkOrderManagement.Domain.WorkOrders;

/// <summary>
/// Historial de agendamientos y reprogramaciones de la orden de trabajo.
/// </summary>
public class WorkOrderScheduleHistory : BaseEntity
{
    public Guid WorkOrderId { get; private set; }
    public virtual WorkOrder WorkOrder { get; private set; } = null!;

    public DateOnly ScheduledDate { get; private set; }
    public string ScheduledSlot { get; private set; } = null!;

    public Guid? AssignedTechnicianId { get; private set; }
    public virtual ApplicationUser? AssignedTechnician { get; private set; }

    public string? Notes { get; private set; }

    public Guid ScheduledByUserId { get; private set; }
    public virtual ApplicationUser ScheduledByUser { get; private set; } = null!;

    public DateTimeOffset ScheduledAt { get; private set; }

    private WorkOrderScheduleHistory() { }

    public static WorkOrderScheduleHistory Create(
        Guid workOrderId,
        DateOnly scheduledDate,
        string scheduledSlot,
        Guid? assignedTechnicianId,
        string? notes,
        Guid scheduledByUserId)
    {
        return new WorkOrderScheduleHistory
        {
            Id = Guid.Empty,
            WorkOrderId = workOrderId,
            ScheduledDate = scheduledDate,
            ScheduledSlot = scheduledSlot.Trim(),
            AssignedTechnicianId = assignedTechnicianId,
            Notes = notes?.Trim(),
            ScheduledByUserId = scheduledByUserId,
            ScheduledAt = DateTimeOffset.UtcNow
        };
    }
}
