using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Outbox.Interceptors;

namespace BuildingBlocks.Persistence.Outbox.Publishers;
/// <summary>
/// Collects integration events during the current scope until they are persisted
/// to the Outbox.
/// </summary>
/// <remarks>
/// Events are temporarily stored in memory and are converted to outbox messages
/// by the <see cref="OutboxInterceptor"/> during SaveChanges.
/// </remarks>
public class OutboxIntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly List<IIntegrationEvent> _integrationEvents = [];

    public void Publish(IIntegrationEvent integrationEvent) => _integrationEvents.Add(integrationEvent);

    public IReadOnlyCollection<IIntegrationEvent> GetEvents() => _integrationEvents.AsReadOnly();

    public void Clear() => _integrationEvents.Clear();
}