using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ObserveWorkOrder;

public record ObserveWorkOrderCommand(
    Guid WorkOrderId,
    string Reason,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
