namespace Skvia.BaseTemplate.Domain.Common;

public static class ArchivableExtensions
{
    public static void Archive(this IArchivable archivable, Guid? userId = null)
    {
        archivable.IsArchived = true;
        archivable.ArchivedAt = DateTimeOffset.UtcNow;
        archivable.ArchivedBy = userId;
    }

    public static void Unarchive(this IArchivable archivable)
    {
        archivable.IsArchived = false;
        archivable.ArchivedAt = null;
        archivable.ArchivedBy = null;
    }
}
