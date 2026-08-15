using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorkOrderManagement.Api.Models;
using WorkOrderManagement.Application.Common.Interfaces;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Application.Features.Reports.DTOs;
using WorkOrderManagement.Application.Features.Reports.Queries.ExportWorkOrders;
using WorkOrderManagement.Application.Features.Reports.Queries.GetDashboardReport;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Api.Endpoints.Reports;

/// <summary>
/// Mapeo de endpoints de la API HTTP para reportes y analítica de gestión.
/// </summary>
public sealed class ReportEndpoints : IEndpoint
{
    /// <summary>
    /// Registra las rutas asociadas a reportes en el enrutador de ASP.NET Core.
    /// </summary>
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/dashboard", GetDashboardReport)
            .WithName("GetDashboardReport")
            .WithSummary("Obtener métricas y KPIs para el dashboard de analítica")
            .Produces<DashboardReportResponse>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet("/export", ExportWorkOrders)
            .WithName("ExportWorkOrders")
            .WithSummary("Exportar listado de órdenes de trabajo a formato CSV/Excel")
            .Produces<FileContentResult>(StatusCodes.Status200OK)
            .Produces<ApiProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetDashboardReport(
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate,
        [FromQuery] Guid? branchId,
        [FromQuery] WorkOrderType? requestType,
        ClaimsPrincipal userClaims,
        ICurrentUserProvider currentUserProvider,
        IQueryHandler<GetDashboardReportQuery, ErrorOr<DashboardReportResponse>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var userRoles = userClaims.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct()
            .ToList();

        var query = new GetDashboardReportQuery(
            startDate,
            endDate,
            branchId,
            requestType,
            currentUser.Id,
            userRoles);

        var result = await handler.HandleAsync(query, cancellationToken);
        return result.Match(TypedResults.Ok, errors => errors.ToProblem());
    }

    private static async Task<IResult> ExportWorkOrders(
        [FromQuery] DateTimeOffset? startDate,
        [FromQuery] DateTimeOffset? endDate,
        [FromQuery] Guid? branchId,
        [FromQuery] WorkOrderType? requestType,
        [FromQuery] WorkOrderStatus? status,
        ClaimsPrincipal userClaims,
        ICurrentUserProvider currentUserProvider,
        IQueryHandler<ExportWorkOrdersQuery, ErrorOr<ExportFileResponse>> handler,
        CancellationToken cancellationToken)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        var userRoles = userClaims.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct()
            .ToList();

        var query = new ExportWorkOrdersQuery(
            startDate,
            endDate,
            branchId,
            requestType,
            status,
            currentUser.Id,
            userRoles);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result.Match(
            file => TypedResults.File(file.FileContents, file.ContentType, file.FileName),
            errors => errors.ToProblem());
    }
}
