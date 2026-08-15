using System.Text;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.WorkOrders.Queries.GetWorkOrders;
using WorkOrderManagement.Application.Features.WorkOrders.Security;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.Reports.Queries.ExportWorkOrders;

/// <summary>
/// Manejador para generar el reporte descargable CSV de órdenes de trabajo.
/// </summary>
public class ExportWorkOrdersQueryHandler(
    IApplicationDbContext dbContext,
    IWorkOrderRepository workOrderRepository)
    : IQueryHandler<ExportWorkOrdersQuery, ErrorOr<ExportFileResponse>>
{
    /// <summary>
    /// Construye y codifica el archivo CSV con soporte para caracteres especiales (UTF-8 con BOM).
    /// </summary>
    public async Task<ErrorOr<ExportFileResponse>> HandleAsync(ExportWorkOrdersQuery query, CancellationToken cancellationToken)
    {
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

        var dummyGetQuery = new GetWorkOrdersQuery(
            CurrentUserId: query.CurrentUserId,
            UserRoles: query.UserRoles?.ToList(),
            BranchId: query.BranchId,
            RequestType: query.RequestType,
            Status: query.Status);

        var accessScope = WorkOrderAccessPolicy.ResolveScope(dummyGetQuery, userBranchIds, subordinateIds);

        var baseQuery = dbContext.WorkOrders
            .AsNoTracking()
            .Include(w => w.Branch)
            .Include(w => w.CreatedByUser)
            .Include(w => w.AssignedTechnician)
            .AsQueryable();

        baseQuery = WorkOrderQueryBuilder.ApplyScopeAndFilters(baseQuery, dummyGetQuery, accessScope);

        if (query.StartDate.HasValue)
        {
            baseQuery = baseQuery.Where(w => w.Created >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            var endOfDay = query.EndDate.Value.Date.AddDays(1).AddTicks(-1);
            baseQuery = baseQuery.Where(w => w.Created <= endOfDay);
        }

        var list = await baseQuery.OrderByDescending(w => w.Created).ToListAsync(cancellationToken);

        var sb = new StringBuilder();
        // Encabezados en español
        sb.AppendLine("Ticket;Tipo;Estado;Prioridad;Sede;Vendedor;Codigo_Cliente;Nombre_Cliente;Telefono;Distrito;Direccion;Fecha_Registro;Fecha_Agendada;Tecnico");

        foreach (var item in list)
        {
            sb.AppendLine($"\"{EscapeCsv(item.TicketNumber)}\";" +
                          $"\"{EscapeCsv(item.RequestType.ToString())}\";" +
                          $"\"{EscapeCsv(item.Status.ToString())}\";" +
                          $"\"{EscapeCsv(item.Priority.ToString())}\";" +
                          $"\"{EscapeCsv(item.Branch?.Name ?? "")}\";" +
                          $"\"{EscapeCsv(item.CreatedByUser?.DisplayName ?? item.CreatedByUser?.UserName ?? "")}\";" +
                          $"\"{EscapeCsv(item.ClientCode)}\";" +
                          $"\"{EscapeCsv(item.ClientName)}\";" +
                          $"\"{EscapeCsv(item.ClientPhone)}\";" +
                          $"\"{EscapeCsv(item.District)}\";" +
                          $"\"{EscapeCsv(item.Address)}\";" +
                          $"\"{item.Created:yyyy-MM-dd HH:mm}\";" +
                          $"\"{(item.ScheduledDate.HasValue ? item.ScheduledDate.Value.ToString("yyyy-MM-dd") : "")}\";" +
                          $"\"{EscapeCsv(item.AssignedTechnician?.DisplayName ?? item.AssignedTechnician?.UserName ?? "")}\"");
        }

        // UTF-8 BOM para apertura directa sin garabatos en MS Excel
        byte[] preamble = Encoding.UTF8.GetPreamble();
        byte[] contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
        byte[] fileBytes = new byte[preamble.Length + contentBytes.Length];

        Buffer.BlockCopy(preamble, 0, fileBytes, 0, preamble.Length);
        Buffer.BlockCopy(contentBytes, 0, fileBytes, preamble.Length, contentBytes.Length);

        var fileName = $"Reporte_Ordenes_Trabajo_{DateTime.UtcNow:yyyyMMdd_HHmm}.csv";
        return new ExportFileResponse(fileName, "text/csv; charset=utf-8", fileBytes);
    }

    private static string EscapeCsv(string? input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return input.Replace("\"", "\"\"");
    }
}
