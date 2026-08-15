using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Branches.Queries.GetBranches;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Domain.Notifications;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IBranchRepository branchRepository,
    IUserAccountService userAccountService,
    IApplicationDbContext dbContext) 
    : ICommandHandler<CreateWorkOrderCommand, ErrorOr<WorkOrderResponse>>
{
    public async Task<ErrorOr<WorkOrderResponse>> HandleAsync(CreateWorkOrderCommand command, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(command.BranchId, cancellationToken);

        if (branch == null)
        {
            return Error.NotFound("Branch.NotFound", "La sede especificada no existe.");
        }

        var creatorUserResult = await userAccountService.GetUserByIdAsync(command.CreatedByUserId, cancellationToken);

        if (creatorUserResult.IsError)
        {
            return Error.NotFound("User.NotFound", "El usuario solicitante no existe.");
        }

        var creatorUser = creatorUserResult.Value;

        var todayPrefix = $"SOL-{DateTime.UtcNow:yyyyMMdd}-";
        var countToday = await workOrderRepository.CountTicketNumbersAsync(todayPrefix, cancellationToken);

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

        await workOrderRepository.AddAsync(workOrder, cancellationToken);
        await workOrderRepository.SaveChangesAsync(cancellationToken);

        // Notificar únicamente al personal de Backoffice/Admin asignado a esta sede
        var branchBackofficeUserIds = await dbContext.ApplicationUsers
            .Where(u => u.IsActive && u.Id != command.CreatedByUserId &&
                (u.BranchId == command.BranchId || u.BranchUsers.Any(bu => bu.BranchId == command.BranchId)) &&
                u.UserRoles.Any(ur => ur.Role.Name == "Backoffice" || ur.Role.Name == "Administrator" || ur.Role.Name == "Admin"))
            .Select(u => u.Id)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var userId in branchBackofficeUserIds)
        {
            var notification = Notification.Create(
                userId,
                "Nueva Solicitud Registrada",
                $"Se registró la solicitud {workOrder.TicketNumber} en tu sede {branch.Name}.",
                workOrder.Id,
                "WorkOrderCreated");

            dbContext.Notifications.Add(notification);
        }

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
