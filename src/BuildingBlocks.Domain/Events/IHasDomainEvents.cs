namespace BuildingBlocks.Domain.Events;

/// <summary>
/// Interface for entities that can raise and hold domain events.
/// </summary>
/// <remarks>
/// This interface is implemented by aggregate roots to enable them to:
/// <list type="bullet">
/// <item><description>Collect domain events as state changes occur</description></item>
/// <item><description>Expose pending events for dispatch after persistence</description></item>
/// <item><description>Clear events after they have been successfully processed</description></item>
/// </list>
/// The pattern supports eventual consistency by allowing domain events to be
/// dispatched asynchronously after the aggregate state is persisted.
/// </remarks>
public interface IHasDomainEvents
{
    /// <summary>
    /// Gets the read-only collection of domain events raised on this entity.
    /// </summary>
    /// <value>A collection of pending <see cref="IDomainEvent"/> instances.</value>
    /// <remarks>
    /// These events represent state changes that have occurred but have not yet
    /// been dispatched to event handlers. They should be cleared after successful dispatch.
    /// </remarks>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    
    /// <summary>
    /// Clears all domain events from the entity's event collection.
    /// </summary>
    /// <remarks>
    /// This method should be called after domain events have been successfully
    /// dispatched to their handlers, typically by infrastructure code after
    /// persisting the aggregate's state changes.
    /// </remarks>
    void ClearDomainEvents();
}