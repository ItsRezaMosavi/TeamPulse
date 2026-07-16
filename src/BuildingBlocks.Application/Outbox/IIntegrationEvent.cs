namespace BuildingBlocks.Application.Outbox;

public interface IIntegrationEvent
{
    DateTime OccurredOnUtc { get; }
    string Version { get; }
}