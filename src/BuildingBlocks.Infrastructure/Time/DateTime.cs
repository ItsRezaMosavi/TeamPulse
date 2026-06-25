using BuildingBlocks.Application.Context;

namespace BuildingBlocks.Infrastructure.Time;

public class DateTime : IDateTimeProvider
{
    public System.DateTime UtcNow => System.DateTime.UtcNow;
    public TimeZoneInfo TimeZoneInfo => TimeZoneInfo.Local;
}