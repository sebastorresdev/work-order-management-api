using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.CompleteWorkOrder;

public record CompleteWorkOrderCommand(
    Guid WorkOrderId,
    string? CompletionNotes,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
