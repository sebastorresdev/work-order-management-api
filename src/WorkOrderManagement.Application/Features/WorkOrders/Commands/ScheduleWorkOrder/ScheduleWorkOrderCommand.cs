using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.ScheduleWorkOrder;

public record ScheduleWorkOrderCommand(
    Guid WorkOrderId,
    DateOnly ScheduledDate,
    string ScheduledSlot,
    Guid? AssignedTechnicianId,
    string? Notes,
    Guid ScheduledByUserId) : ICommand<ErrorOr<Success>>;
