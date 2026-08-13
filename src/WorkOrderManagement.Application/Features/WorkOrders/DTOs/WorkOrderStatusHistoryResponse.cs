using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.DTOs;

public record WorkOrderStatusHistoryResponse(
    Guid Id,
    WorkOrderStatus StatusFrom,
    string StatusFromName,
    WorkOrderStatus StatusTo,
    string StatusToName,
    string? Comments,
    Guid ChangedByUserId,
    string ChangedByUserName,
    DateTimeOffset Timestamp);
