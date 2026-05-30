namespace BuildingBlocks.Application;

public interface IClock
{
    DateTime UtcNow { get; }
}