using Skvia.BaseTemplate.Domain.Common;

namespace Skvia.BaseTemplate.Domain.Auditing;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public required string Action { get; set; }
    public required string EntityName { get; set; }
    public string? EntityId { get; set; }
    public string? OldValuesJson { get; set; }
    public string? NewValuesJson { get; set; }
    public string? AffectedColumnsJson { get; set; }
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public string? IpAddress { get; set; }
}
