using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Reports.DTOs;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.Reports.Queries.GetDashboardReport;

/// <summary>
/// Consulta CQRS para obtener el informe analítico de rendimiento y KPIs con agregaciones por fecha y sede.
/// </summary>
/// <param name="StartDate">Fecha inicial de filtrado.</param>
/// <param name="EndDate">Fecha final de filtrado.</param>
/// <param name="BranchId">Filtro opcional por sede específica.</param>
/// <param name="RequestType">Filtro opcional por tipo de servicio.</param>
/// <param name="CurrentUserId">Identificador del usuario que ejecuta la consulta.</param>
/// <param name="UserRoles">Roles asignados al usuario para la resolución de ámbito de seguridad.</param>
public record GetDashboardReportQuery(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    Guid? BranchId = null,
    WorkOrderType? RequestType = null,
    Guid? CurrentUserId = null,
    IReadOnlyCollection<string>? UserRoles = null) : IQuery<ErrorOr<DashboardReportResponse>>;
