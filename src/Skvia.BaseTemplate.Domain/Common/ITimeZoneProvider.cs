namespace Skvia.BaseTemplate.Domain.Common;

public interface ITimeZoneProvider
{
    TimeZoneInfo GetTimeZone(string timeZoneId);
}

