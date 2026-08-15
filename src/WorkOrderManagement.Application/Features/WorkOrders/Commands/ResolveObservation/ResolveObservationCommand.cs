using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ResolveObservation;

public record ResolveObservationCommand(
    Guid WorkOrderId,
    string ResolutionNotes,
    Guid UpdatedByUserId) : ICommand<ErrorOr<Success>>;
