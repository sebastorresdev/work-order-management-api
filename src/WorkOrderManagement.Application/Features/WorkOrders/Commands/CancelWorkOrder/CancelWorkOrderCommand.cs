using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.CancelWorkOrder;

public record CancelWorkOrderCommand(
    Guid WorkOrderId,
    string? Reason,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
