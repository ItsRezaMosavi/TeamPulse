using BuildingBlocks.Application.Events;
using BuildingBlocks.Domain.Events;
using MediatR;

namespace BuildingBlocks.Infrastructure.Events;

public class MediatorEventDispatcher(IMediator mediator) : IEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}