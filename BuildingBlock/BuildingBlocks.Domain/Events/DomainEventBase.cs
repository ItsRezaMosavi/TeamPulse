namespace BuildingBlocks.Domain.Events;

/// <summary>
/// Base class for all domain events.
/// Provides unique <see cref="EventId"/> and time of occurrence <see cref="OccurredOn"/>
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    protected DomainEventBase(Guid? eventId = null, DateTime? occurredOn = null)
    {
        EventId = eventId ?? Guid.NewGuid();
        OccurredOn = occurredOn ?? DateTime.UtcNow;
    }

    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}