namespace Skvia.BaseTemplate.Domain.Common;

public interface IArchivable
{
    bool IsArchived { get; set; }
    DateTimeOffset? ArchivedAt { get; set; }
    Guid? ArchivedBy { get; set; }

    void Archive(Guid? userId = null)
    {
        IsArchived = true;
        ArchivedAt = DateTimeOffset.UtcNow;
        ArchivedBy = userId;
    }

    void Unarchive()
    {
        IsArchived = false;
        ArchivedAt = null;
        ArchivedBy = null;
    }
}
