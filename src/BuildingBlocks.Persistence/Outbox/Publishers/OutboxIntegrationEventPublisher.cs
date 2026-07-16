using BuildingBlocks.Application.Outbox;

namespace BuildingBlocks.Persistence.Outbox.Publishers;

public class OutboxIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly List<IIntegrationEvent> _integrationEvents = [];

    public void Publish(IIntegrationEvent integrationEvent) => _integrationEvents.Add(integrationEvent);

    public IReadOnlyCollection<IIntegrationEvent> GetEvents() => _integrationEvents.AsReadOnly();

    public void Clear() => _integrationEvents.Clear();
}