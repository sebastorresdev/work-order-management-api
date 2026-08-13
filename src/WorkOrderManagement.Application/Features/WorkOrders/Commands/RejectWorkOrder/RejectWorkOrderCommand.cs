using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.RejectWorkOrder;

public record RejectWorkOrderCommand(
    Guid WorkOrderId,
    string Reason,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
