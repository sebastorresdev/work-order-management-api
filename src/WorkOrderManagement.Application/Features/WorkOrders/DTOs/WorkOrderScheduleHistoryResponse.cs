namespace WorkOrderManagement.Application.Features.WorkOrders.DTOs;

public record WorkOrderScheduleHistoryResponse(
    Guid Id,
    DateOnly ScheduledDate,
    string ScheduledSlot,
    Guid? AssignedTechnicianId,
    string? AssignedTechnicianName,
    string? Notes,
    Guid ScheduledByUserId,
    string ScheduledByUserName,
    DateTimeOffset ScheduledAt);
