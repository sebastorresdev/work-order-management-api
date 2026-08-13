using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.DTOs;

/// <summary>
/// DTO con el detalle completo de una orden de trabajo incluyendo historial de estados y agendamientos.
/// </summary>
public record WorkOrderDetailResponse(
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
    string? ClientSecondaryPhone,
    string District,
    string Address,
    string? AddressReference,
    string Description,
    DateOnly? ScheduledDate,
    string? ScheduledSlot,
    Guid? AssignedTechnicianId,
    string? AssignedTechnicianName,
    DateTimeOffset? CompletedAt,
    string? CompletionNotes,
    string? ObservationNotes,
    string? RejectionReason,
    string? CancellationReason,
    DateTimeOffset Created,
    List<WorkOrderStatusHistoryResponse> StatusHistory,
    List<WorkOrderScheduleHistoryResponse> ScheduleHistory);
