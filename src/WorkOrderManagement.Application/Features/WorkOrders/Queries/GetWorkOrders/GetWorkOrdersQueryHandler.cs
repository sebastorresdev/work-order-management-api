using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Common.Models;
using WorkOrderManagement.Application.Features.WorkOrders.DTOs;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetWorkOrdersQuery, ErrorOr<PaginatedResponse<WorkOrderResponse>>>
{
    public async Task<ErrorOr<PaginatedResponse<WorkOrderResponse>>> HandleAsync(GetWorkOrdersQuery query, CancellationToken cancellationToken)
    {
        var queryable = dbContext.WorkOrders
            .AsNoTracking()
            .Include(w => w.Branch)
            .Include(w => w.CreatedByUser)
            .Include(w => w.AssignedTechnician)
            .AsQueryable();

        // 1. Filtrado por Rol / Alcance
        if (query.CurrentUserId.HasValue && query.UserRoles != null && query.UserRoles.Count > 0)
        {
            var isSystemAdmin = query.UserRoles.Contains("Admin", StringComparer.OrdinalIgnoreCase) ||
                                query.UserRoles.Contains("Administrador", StringComparer.OrdinalIgnoreCase);
            var isBackoffice = query.UserRoles.Contains("Backoffice", StringComparer.OrdinalIgnoreCase);
            var isSupervisor = query.UserRoles.Contains("Supervisor", StringComparer.OrdinalIgnoreCase);

            if (!isSystemAdmin)
            {
                if (isBackoffice)
                {
                    // Backoffice ve solicitudes de su sede asignada
                    if (query.BranchId.HasValue)
                    {
                        queryable = queryable.Where(w => w.BranchId == query.BranchId.Value);
                    }
                    else
                    {
                        var currentUserBranchId = await dbContext.ApplicationUsers
                            .Where(u => u.Id == query.CurrentUserId.Value)
                            .Select(u => u.BranchId)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (currentUserBranchId.HasValue)
                        {
                            queryable = queryable.Where(w => w.BranchId == currentUserBranchId.Value);
                        }
                    }
                }
                else if (isSupervisor)
                {
                    // Supervisor ve sus solicitudes + las de su personal a cargo
                    var subordinateIds = await dbContext.ApplicationUsers
                        .Where(u => u.SupervisorId == query.CurrentUserId.Value)
                        .Select(u => u.Id)
                        .ToListAsync(cancellationToken);

                    var teamUserIds = subordinateIds.Concat([query.CurrentUserId.Value]).ToList();
                    queryable = queryable.Where(w => teamUserIds.Contains(w.CreatedByUserId));
                }
                else
                {
                    // Vendedor o usuario común: solo ve sus propias solicitudes
                    queryable = queryable.Where(w => w.CreatedByUserId == query.CurrentUserId.Value);
                }
            }
            else if (query.BranchId.HasValue)
            {
                // Admin filtrando opcionalmente por sede
                queryable = queryable.Where(w => w.BranchId == query.BranchId.Value);
            }
        }

        // 2. Filtros opcionales
        if (query.Status.HasValue)
        {
            queryable = queryable.Where(w => w.Status == query.Status.Value);
        }

        if (query.RequestType.HasValue)
        {
            queryable = queryable.Where(w => w.RequestType == query.RequestType.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.Trim().ToLower();
            queryable = queryable.Where(w =>
                w.TicketNumber.ToLower().Contains(term) ||
                w.ClientCode.ToLower().Contains(term) ||
                w.ClientName.ToLower().Contains(term) ||
                w.District.ToLower().Contains(term) ||
                w.Address.ToLower().Contains(term));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var items = await queryable
            .OrderByDescending(w => w.Created)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new WorkOrderResponse(
                w.Id,
                w.TicketNumber,
                w.RequestType,
                w.RequestType.ToString(),
                w.Status,
                w.Status.ToString(),
                w.Priority,
                w.Priority.ToString(),
                w.BranchId,
                w.Branch.Name,
                w.CreatedByUserId,
                w.CreatedByUser.DisplayName ?? w.CreatedByUser.UserName ?? "Vendedor",
                w.ClientCode,
                w.ClientName,
                w.ClientPhone,
                w.District,
                w.Address,
                w.Description,
                w.ScheduledDate,
                w.ScheduledSlot,
                w.AssignedTechnicianId,
                w.AssignedTechnician != null ? (w.AssignedTechnician.DisplayName ?? w.AssignedTechnician.UserName) : null,
                w.Created,
                w.CompletedAt))
            .ToListAsync(cancellationToken);

        return PaginatedResponse<WorkOrderResponse>.Create(items, totalCount, pageNumber, pageSize);
    }
}
