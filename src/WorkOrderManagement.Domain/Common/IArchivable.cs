namespace WorkOrderManagement.Domain.Common;

/// <summary>
/// Interfaz para entidades que soportan marcado y gestión de archivado/borrado lógico.
/// </summary>
public interface IArchivable
{
    /// <summary>
    /// Indica si el registro se encuentra archivado.
    /// </summary>
    bool IsArchived { get; set; }

    /// <summary>
    /// Marca de tiempo UTC indicando cuándo se archivó el registro.
    /// </summary>
    DateTimeOffset? ArchivedAt { get; set; }

    /// <summary>
    /// Identificador del usuario que ejecutó la acción de archivar.
    /// </summary>
    Guid? ArchivedBy { get; set; }

    /// <summary>
    /// Archiva lógicamente la entidad estableciendo los estados y marcas de tiempo correspondientes.
    /// </summary>
    /// <param name="userId">Identificador opcional del usuario que realiza la acción.</param>
    void Archive(Guid? userId = null)
    {
        IsArchived = true;
        ArchivedAt = DateTimeOffset.UtcNow;
        ArchivedBy = userId;
    }

    /// <summary>
    /// Desarchiva la entidad restableciendo el estado a activo.
    /// </summary>
    void Unarchive()
    {
        IsArchived = false;
        ArchivedAt = null;
        ArchivedBy = null;
    }
}
