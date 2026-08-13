using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.DTOs;

/// <summary>
/// DTO con la información resumida de una orden de trabajo.
/// </summary>
public record WorkOrderResponse(
    Guid Id,
    string TicketNumber,
    WorkOrderType RequestType,
    string RequestTypeName,
    WorkOrderStatus Status,
    string StatusName,
    WorkOrderPriority Priority,
    string PriorityName,
    Guid BranchId,
    string BranchName,
    Guid CreatedByUserId,
    string CreatedByUserName,
    string ClientCode,
    string ClientName,
    string ClientPhone,
    string District,
    string Address,
    string Description,
    DateOnly? ScheduledDate,
    string? ScheduledSlot,
    Guid? AssignedTechnicianId,
    string? AssignedTechnicianName,
    DateTimeOffset Created,
    DateTimeOffset? CompletedAt);
