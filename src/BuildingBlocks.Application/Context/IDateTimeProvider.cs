namespace BuildingBlocks.Application.Context;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    TimeZoneInfo TimeZoneInfo { get; }
}