using BuildingBlocks.Domain.Events;

namespace BuildingBlocks.Application.Events;

public interface IEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default);
}