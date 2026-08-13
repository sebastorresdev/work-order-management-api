using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public record CreateWorkOrderCommand(
    WorkOrderType RequestType,
    WorkOrderPriority Priority,
    Guid BranchId,
    Guid CreatedByUserId,
    string ClientCode,
    string ClientName,
    string ClientPhone,
    string? ClientSecondaryPhone,
    string District,
    string Address,
    string? AddressReference,
    string Description) : ICommand<ErrorOr<WorkOrderResponse>>;
