using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Domain.Auditing;

/// <summary>
/// Representa un registro de auditoría del sistema para rastrear cambios realizados en las entidades.
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// Identificador único del usuario que realizó la acción de auditoría (opcional).
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Nombre de usuario que ejecutó la acción auditada (opcional).
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Tipo de acción o de operación ejecutada (por ejemplo: Create, Update, Delete).
    /// </summary>
    public required string Action { get; set; }

    /// <summary>
    /// Nombre de la entidad del dominio sobre la cual se ejecutó la acción.
    /// </summary>
    public required string EntityName { get; set; }

    /// <summary>
    /// Identificador primario de la entidad modificada en formato de cadena (opcional).
    /// </summary>
    public string? EntityId { get; set; }

    /// <summary>
    /// Representación en formato JSON de los valores previos a la modificación (opcional).
    /// </summary>
    public string? OldValuesJson { get; set; }

    /// <summary>
    /// Representación en formato JSON de los nuevos valores asignados tras la modificación (opcional).
    /// </summary>
    public string? NewValuesJson { get; set; }

    /// <summary>
    /// Lista en formato JSON de los nombres de columnas o campos afectados por la modificación (opcional).
    /// </summary>
    public string? AffectedColumnsJson { get; set; }

    /// <summary>
    /// Marca de tiempo precisa en UTC indicando cuándo se registró la auditoría.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Dirección IP del cliente u origen que ejecutó la solicitud (opcional).
    /// </summary>
    public string? IpAddress { get; set; }
}
