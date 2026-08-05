namespace Skvia.BaseTemplate.Domain.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn => DateTimeOffset.UtcNow;
}
