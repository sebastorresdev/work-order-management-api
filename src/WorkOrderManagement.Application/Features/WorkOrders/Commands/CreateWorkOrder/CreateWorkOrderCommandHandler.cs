using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderCommandHandler(IApplicationDbContext dbContext) 
    : ICommandHandler<CreateWorkOrderCommand, ErrorOr<WorkOrderResponse>>
{
    public async Task<ErrorOr<WorkOrderResponse>> HandleAsync(CreateWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var branch = await dbContext.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == command.BranchId, cancellationToken);

        if (branch == null)
        {
            return Error.NotFound("Branch.NotFound", "La sede especificada no existe.");
        }

        var creatorUser = await dbContext.ApplicationUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == command.CreatedByUserId, cancellationToken);

        if (creatorUser == null)
        {
            return Error.NotFound("User.NotFound", "El usuario solicitante no existe.");
        }

        // Generar número de ticket único: SOL-YYYYMMDD-XXXX
        var todayPrefix = $"SOL-{DateTime.UtcNow:yyyyMMdd}-";
        var countToday = await dbContext.WorkOrders
            .CountAsync(w => w.TicketNumber.StartsWith(todayPrefix), cancellationToken);

        var ticketNumber = $"{todayPrefix}{(countToday + 1):D4}";

        var workOrderResult = WorkOrder.Create(
            ticketNumber,
            command.RequestType,
            command.Priority,
            command.BranchId,
            command.CreatedByUserId,
            command.ClientCode,
            command.ClientName,
            command.ClientPhone,
            command.District,
            command.Address,
            command.Description,
            command.ClientSecondaryPhone,
            command.AddressReference);

        if (workOrderResult.IsError) return workOrderResult.Errors;

        var workOrder = workOrderResult.Value;

        dbContext.WorkOrders.Add(workOrder);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new WorkOrderResponse(
            workOrder.Id,
            workOrder.TicketNumber,
            workOrder.RequestType,
            workOrder.RequestType.ToString(),
            workOrder.Status,
            workOrder.Status.ToString(),
            workOrder.Priority,
            workOrder.Priority.ToString(),
            workOrder.BranchId,
            branch.Name,
            workOrder.CreatedByUserId,
            creatorUser.DisplayName ?? creatorUser.UserName ?? "Vendedor",
            workOrder.ClientCode,
            workOrder.ClientName,
            workOrder.ClientPhone,
            workOrder.District,
            workOrder.Address,
            workOrder.Description,
            workOrder.ScheduledDate,
            workOrder.ScheduledSlot,
            workOrder.AssignedTechnicianId,
            null,
            workOrder.Created,
            workOrder.CompletedAt);
    }
}
