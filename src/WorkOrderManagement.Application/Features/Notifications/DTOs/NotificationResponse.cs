namespace WorkOrderManagement.Application.Features.Notifications.DTOs;

public record NotificationResponse(
    Guid Id,
    string Title,
    string Message,
    Guid? WorkOrderId,
    string Type,
    bool IsRead,
    DateTimeOffset CreatedAt);
