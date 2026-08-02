using Skvia.BaseTemplate.Domain.Common;

namespace Skvia.BaseTemplate.Infrastructure.Services;

public class SystemTimeZoneProvider : ITimeZoneProvider
{
    public TimeZoneInfo GetTimeZone(string timeZoneId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(timeZoneId);
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}

