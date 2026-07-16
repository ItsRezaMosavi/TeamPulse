namespace BuildingBlocks.Application.Outbox;

public interface IIntegrationEventPublisher
{
    IReadOnlyCollection<IIntegrationEvent> GetEvents();

    void Publish(IIntegrationEvent integrationEvent);

    void Clear();
}