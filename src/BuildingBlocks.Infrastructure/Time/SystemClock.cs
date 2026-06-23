using BuildingBlocks.Application.Abstractions;

namespace BuildingBlocks.Infrastructure.Time;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}