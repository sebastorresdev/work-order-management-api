using Skvia.BaseTemplate.Domain.Common;

namespace Skvia.BaseTemplate.Infrastructure.Services;

public class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

