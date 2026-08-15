using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Reports.DTOs;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.Reports.Queries.GetDashboardReport;

/// <summary>
/// Manejador de la consulta de analítica de gestión para construir el informe de rendimiento y KPIs.
/// </summary>
public class GetDashboardReportQueryHandler(
    IApplicationDbContext dbContext,
    IWorkOrderRepository workOrderRepository)
    : IQueryHandler<GetDashboardReportQuery, ErrorOr<DashboardReportResponse>>
{
    /// <summary>
    /// Procesa la agregación de datos respetando las restricciones de seguridad multisede del usuario.
    /// </summary>
    public async Task<ErrorOr<DashboardReportResponse>> HandleAsync(GetDashboardReportQuery query, CancellationToken cancellationToken)
    {
        // 1. Resolver sedes asignadas al usuario para aplicar seguridad multisede
        IReadOnlyCollection<Guid> userBranchIds = [];
        IReadOnlyCollection<Guid> subordinateIds = [];

        if (query.CurrentUserId.HasValue)
        {
            userBranchIds = await workOrderRepository.GetUserBranchIdsAsync(query.CurrentUserId.Value, cancellationToken);
            if (query.UserRoles != null && query.UserRoles.Contains("Supervisor", StringComparer.OrdinalIgnoreCase))
            {
                subordinateIds = await workOrderRepository.GetSubordinateUserIdsAsync(query.CurrentUserId.Value, cancellationToken);
            }
        }

        // Construir la consulta genérica para el Scope
        var dummyGetQuery = new GetWorkOrdersQuery(
            CurrentUserId: query.CurrentUserId,
            UserRoles: query.UserRoles?.ToList(),
            BranchId: query.BranchId,
            RequestType: query.RequestType);

        var accessScope = WorkOrderAccessPolicy.ResolveScope(dummyGetQuery, userBranchIds, subordinateIds);

        // 2. Base IQueryable de órdenes de trabajo con Includes de navegación
        var baseQuery = dbContext.WorkOrders
            .AsNoTracking()
            .Include(w => w.Branch)
            .Include(w => w.CreatedByUser)
            .Include(w => w.AssignedTechnician)
            .AsQueryable();

        baseQuery = WorkOrderQueryBuilder.ApplyScopeAndFilters(baseQuery, dummyGetQuery, accessScope);

        // 3. Aplicar filtros de fecha si se especificaron
        if (query.StartDate.HasValue)
        {
            baseQuery = baseQuery.Where(w => w.Created >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            // Ajustar al final del día
            var endOfDay = query.EndDate.Value.Date.AddDays(1).AddTicks(-1);
            baseQuery = baseQuery.Where(w => w.Created <= endOfDay);
        }

        var workOrders = await baseQuery.ToListAsync(cancellationToken);

        int totalCount = workOrders.Count;

        if (totalCount == 0)
        {
            return new DashboardReportResponse(
                new DashboardKpisDto(0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
                [], [], [], [], []);
        }

        // 4. Calcular KPIs
        int pending = workOrders.Count(w => w.Status == WorkOrderStatus.Pendiente);
        int observed = workOrders.Count(w => w.Status == WorkOrderStatus.Observado);
        int scheduled = workOrders.Count(w => w.Status == WorkOrderStatus.Agendado);
        int completed = workOrders.Count(w => w.Status == WorkOrderStatus.Completado);
        int rejected = workOrders.Count(w => w.Status == WorkOrderStatus.Rechazado);
        int cancelled = workOrders.Count(w => w.Status == WorkOrderStatus.Cancelado);

        double completionRate = Math.Round((double)completed / totalCount * 100, 1);
        double observationRate = Math.Round((double)observed / totalCount * 100, 1);

        var completedOrdersWithTime = workOrders
            .Where(w => w.Status == WorkOrderStatus.Completado && w.CompletedAt.HasValue)
            .ToList();

        double avgDays = 0;
        if (completedOrdersWithTime.Count > 0)
        {
            avgDays = Math.Round(completedOrdersWithTime
                .Average(w => (w.CompletedAt!.Value - w.Created).TotalDays), 1);
        }

        var kpis = new DashboardKpisDto(
            totalCount,
            pending,
            observed,
            scheduled,
            completed,
            rejected,
            cancelled,
            completionRate,
            observationRate,
            avgDays);

        // 5. Distribución por Estado
        var statusDist = workOrders
            .GroupBy(w => w.Status)
            .Select(g => new StatusDistributionDto(
                GetStatusName(g.Key),
                g.Count(),
                Math.Round((double)g.Count() / totalCount * 100, 1)))
            .OrderByDescending(s => s.Count)
            .ToList();

        // 6. Rendimiento por Sede
        var branchPerf = workOrders
            .GroupBy(w => new { w.BranchId, BranchName = w.Branch?.Name ?? "Sin Sede" })
            .Select(g => new BranchPerformanceDto(
                g.Key.BranchId,
                g.Key.BranchName,
                g.Count(),
                g.Count(w => w.Status == WorkOrderStatus.Completado),
                g.Count(w => w.Status == WorkOrderStatus.Observado),
                g.Count(w => w.Status == WorkOrderStatus.Agendado),
                Math.Round((double)g.Count(w => w.Status == WorkOrderStatus.Completado) / g.Count() * 100, 1)))
            .OrderByDescending(b => b.TotalWorkOrders)
            .ToList();

        // 7. Distribución por Tipo de Solicitud
        var typeDist = workOrders
            .GroupBy(w => w.RequestType)
            .Select(g => new TypeDistributionDto(
                GetTypeName(g.Key),
                g.Count(),
                Math.Round((double)g.Count() / totalCount * 100, 1)))
            .OrderByDescending(t => t.Count)
            .ToList();

        // 8. Top Vendedores (Creadores)
        var topCreators = workOrders
            .GroupBy(w => new { w.CreatedByUserId, UserName = w.CreatedByUser?.DisplayName ?? w.CreatedByUser?.UserName ?? "Vendedor" })
            .Select(g => new UserProductivityDto(
                g.Key.CreatedByUserId,
                g.Key.UserName,
                "Vendedor",
                g.Count(),
                g.Count(w => w.Status == WorkOrderStatus.Completado),
                g.Count(w => w.Status == WorkOrderStatus.Observado)))
            .OrderByDescending(u => u.WorkOrdersCount)
            .Take(5)
            .ToList();

        // 9. Top Técnicos
        var topTechnicians = workOrders
            .Where(w => w.AssignedTechnicianId.HasValue)
            .GroupBy(w => new { Id = w.AssignedTechnicianId!.Value, UserName = w.AssignedTechnician?.DisplayName ?? w.AssignedTechnician?.UserName ?? "Técnico" })
            .Select(g => new UserProductivityDto(
                g.Key.Id,
                g.Key.UserName,
                "Técnico",
                g.Count(),
                g.Count(w => w.Status == WorkOrderStatus.Completado),
                g.Count(w => w.Status == WorkOrderStatus.Observado)))
            .OrderByDescending(u => u.WorkOrdersCount)
            .Take(5)
            .ToList();

        return new DashboardReportResponse(
            kpis,
            statusDist,
            branchPerf,
            typeDist,
            topCreators,
            topTechnicians);
    }

    private static string GetStatusName(WorkOrderStatus status) => status switch
    {
        WorkOrderStatus.Pendiente => "Pendiente",
        WorkOrderStatus.Observado => "Observado",
        WorkOrderStatus.Agendado => "Agendado",
        WorkOrderStatus.Completado => "Completado",
        WorkOrderStatus.Rechazado => "Rechazado",
        WorkOrderStatus.Cancelado => "Cancelado",
        _ => status.ToString()
    };

    private static string GetTypeName(WorkOrderType type) => type switch
    {
        WorkOrderType.Instalacion => "Instalación",
        WorkOrderType.Averia => "Avería",
        WorkOrderType.Encomienda => "Encomienda",
        _ => type.ToString()
    };
}
