namespace BuildingBlocks.Application.Outbox;

/// <summary>
/// Collects integration events that will be persisted to the Outbox
/// as part of the current transaction.
/// </summary>
public interface IIntegrationEventPublisher
{
    IReadOnlyCollection<IIntegrationEvent> GetEvents();

    void Publish(IIntegrationEvent integrationEvent);

    void Clear();
}