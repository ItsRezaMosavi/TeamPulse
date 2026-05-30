using BuildingBlocks.Domain.Events;

namespace BuildingBlocks.Application;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}