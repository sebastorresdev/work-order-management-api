namespace Skvia.BaseTemplate.Domain.Common;

/// <summary>
/// Proporciona métodos de extensión para simplificar el proceso de archivado y desarchivado de objetos que implementan <see cref="IArchivable"/>.
/// </summary>
public static class ArchivableExtensions
{
    /// <summary>
    /// Método de extensión para archivar lógicamente una entidad.
    /// </summary>
    /// <param name="archivable">Entidad objetivo a archivar.</param>
    /// <param name="userId">Identificador opcional del usuario que realiza la acción.</param>
    public static void Archive(this IArchivable archivable, Guid? userId = null)
    {
        archivable.IsArchived = true;
        archivable.ArchivedAt = DateTimeOffset.UtcNow;
        archivable.ArchivedBy = userId;
    }

    /// <summary>
    /// Método de extensión para desarchivar lógicamente una entidad.
    /// </summary>
    /// <param name="archivable">Entidad objetivo a desarchivar.</param>
    public static void Unarchive(this IArchivable archivable)
    {
        archivable.IsArchived = false;
        archivable.ArchivedAt = null;
        archivable.ArchivedBy = null;
    }
}
