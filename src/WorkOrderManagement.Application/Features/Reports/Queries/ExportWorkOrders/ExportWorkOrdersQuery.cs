using ErrorOr;
using WorkOrderManagement.Application.Common.Messaging;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.Features.Reports.Queries.ExportWorkOrders;

/// <summary>
/// DTO con el contenido del archivo de reporte exportado (CSV).
/// </summary>
/// <param name="FileName">Nombre sugerido para el archivo descargado.</param>
/// <param name="ContentType">Tipo MIME del contenido (text/csv).</param>
/// <param name="FileContents">Arreglo de bytes con el contenido codificado en UTF-8.</param>
public record ExportFileResponse(
    string FileName,
    string ContentType,
    byte[] FileContents);

/// <summary>
/// Consulta CQRS para exportar el listado filtrado de órdenes de trabajo en formato CSV para análisis en Excel.
/// </summary>
public record ExportWorkOrdersQuery(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    Guid? BranchId = null,
    WorkOrderType? RequestType = null,
    WorkOrderStatus? Status = null,
    Guid? CurrentUserId = null,
    IReadOnlyCollection<string>? UserRoles = null) : IQuery<ErrorOr<ExportFileResponse>>;
