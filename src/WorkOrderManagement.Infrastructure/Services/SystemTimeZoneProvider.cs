using WorkOrderManagement.Domain.Common;

namespace WorkOrderManagement.Infrastructure.Services;

public class SystemTimeZoneProvider : ITimeZoneProvider
{
    public TimeZoneInfo GetTimeZone(string timeZoneId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(timeZoneId);
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}

