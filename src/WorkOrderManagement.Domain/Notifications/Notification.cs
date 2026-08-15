using WorkOrderManagement.Domain.Common;
using WorkOrderManagement.Domain.Identity;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Domain.Notifications;

public class Notification : BaseEntity
{
    public Guid UserId { get; private set; }
    public virtual ApplicationUser User { get; private set; } = null!;

    public string Title { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public Guid? WorkOrderId { get; private set; }
    public virtual WorkOrder? WorkOrder { get; private set; }

    public string Type { get; private set; } = null!;
    public bool IsRead { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Notification() { }

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

    public void MarkAsRead()
    {
        IsRead = true;
    }
}
