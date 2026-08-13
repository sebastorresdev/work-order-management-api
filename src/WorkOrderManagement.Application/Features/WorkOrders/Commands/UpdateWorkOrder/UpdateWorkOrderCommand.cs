using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.UpdateWorkOrder;

public record UpdateWorkOrderCommand(
    Guid WorkOrderId,
    WorkOrderType RequestType,
    WorkOrderPriority Priority,
    string ClientCode,
    string ClientName,
    string ClientPhone,
    string? ClientSecondaryPhone,
    string District,
    string Address,
    string? AddressReference,
    string Description,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
