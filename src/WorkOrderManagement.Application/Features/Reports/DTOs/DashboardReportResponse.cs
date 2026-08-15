namespace WorkOrderManagement.Application.Features.Reports.DTOs;

/// <summary>
/// Indicadores Clave de Rendimiento (KPIs) globales para el rango de fechas y sedes consultadas.
/// </summary>
/// <param name="TotalWorkOrders">Cantidad total de órdenes registradas.</param>
/// <param name="PendingCount">Total de solicitudes pendientes de agendar.</param>
/// <param name="ObservedCount">Total de solicitudes con observación.</param>
/// <param name="ScheduledCount">Total de solicitudes agendadas.</param>
/// <param name="CompletedCount">Total de solicitudes completadas con éxito.</param>
/// <param name="RejectedCount">Total de solicitudes rechazadas.</param>
/// <param name="CancelledCount">Total de solicitudes canceladas.</param>
/// <param name="CompletionRatePercentage">Porcentaje de éxito / finalización.</param>
/// <param name="ObservationRatePercentage">Porcentaje de solicitudes observadas.</param>
/// <param name="AverageResolutionDays">Promedio en días desde creación hasta cierre.</param>
public record DashboardKpisDto(
    int TotalWorkOrders,
    int PendingCount,
    int ObservedCount,
    int ScheduledCount,
    int CompletedCount,
    int RejectedCount,
    int CancelledCount,
    double CompletionRatePercentage,
    double ObservationRatePercentage,
    double AverageResolutionDays);

/// <summary>
/// Distribución de solicitudes por estado operativo.
/// </summary>
public record StatusDistributionDto(
    string StatusName,
    int Count,
    double Percentage);

/// <summary>
/// Rendimiento operativo y volumen de solicitudes agrupado por sede.
/// </summary>
public record BranchPerformanceDto(
    Guid BranchId,
    string BranchName,
    int TotalWorkOrders,
    int CompletedCount,
    int ObservedCount,
    int ScheduledCount,
    double CompletionRatePercentage);

/// <summary>
/// Distribución por tipo de servicio (Instalación, Avería, Encomienda).
/// </summary>
public record TypeDistributionDto(
    string TypeName,
    int Count,
    double Percentage);

/// <summary>
/// Resumen de productividad por usuario (Vendedores / Técnicos).
/// </summary>
public record UserProductivityDto(
    Guid UserId,
    string UserName,
    string RoleName,
    int WorkOrdersCount,
    int CompletedCount,
    int ObservedCount);

/// <summary>
/// Respuesta consolidada con todas las agregaciones y métricas para el Dashboard de Reportes.
/// </summary>
public record DashboardReportResponse(
    DashboardKpisDto Kpis,
    List<StatusDistributionDto> StatusDistribution,
    List<BranchPerformanceDto> BranchPerformance,
    List<TypeDistributionDto> TypeDistribution,
    List<UserProductivityDto> TopCreators,
    List<UserProductivityDto> TopTechnicians);
