namespace WorkOrderManagement.Domain.Common;

/// <summary>
/// Clase base abstracta para entidades que requieren registro de metadatos de auditoría (creación y modificación).
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Fecha y hora en UTC cuando se creó el registro.
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    /// Identificador del usuario que creó el registro (opcional).
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Fecha y hora en UTC de la última modificación realizada en el registro.
    /// </summary>
    public DateTimeOffset LastModified { get; set; }

    /// <summary>
    /// Identificador del último usuario que modificó el registro (opcional).
    /// </summary>
    public Guid? LastModifiedBy { get; set; }
}

