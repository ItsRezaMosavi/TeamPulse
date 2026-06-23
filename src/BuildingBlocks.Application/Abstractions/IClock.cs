namespace BuildingBlocks.Application.Abstractions;

public interface IClock
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}