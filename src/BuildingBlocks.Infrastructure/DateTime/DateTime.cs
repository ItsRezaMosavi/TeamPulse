using BuildingBlocks.Application.Context;

namespace BuildingBlocks.Infrastructure.DateTime;

public class DateTime : IDateTimeProvider
{
    public System.DateTime UtcNow => System.DateTime.UtcNow;
    public TimeZoneInfo TimeZoneInfo => TimeZoneInfo.Local;
}