using BuildingBlocks.Domain.Entities;
using BuildingBlocks.Domain.Events;
using BuildingBlocks.Domain.Exceptions;
using BuildingBlocks.Domain.Rules;

namespace BuildingBlocks.Domain.Aggregates;

/// <summary>
/// Represents an aggregate root with a generic identifier type.
/// </summary>
/// <typeparam name="TId">The type of the aggregate's identifier.</typeparam>
/// <remarks>
/// An aggregate root is a special entity that serves as the entry point for all
/// operations within its consistency boundary. It manages domain events and ensures
/// that all changes to entities within the aggregate maintain business invariants.
/// 
/// Key features:
/// <list type="bullet">
/// <item><description>Inherits from <see cref="Entity{TId}"/> for base entity functionality</description></item>
/// <item><description>Maintains a collection of domain events for eventual consistency</description></item>
/// <item><description>Provides methods to add and clear domain events</description></item>
/// </list>
/// </remarks>
public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvents
{
    protected static void CheckRule(IDomainRule rule)
    {
        if (rule.IsBroken())
            throw new DomainRuleException(rule);
    }

    protected static void CheckRules(params IDomainRule[] rules)
    {
        foreach (var rule in rules)
            CheckRule(rule);
    }


    /// <summary>
    /// The internal collection of domain events.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Gets the read-only collection of domain events that have been raised on this aggregate.
    /// </summary>
    /// <value>A collection of <see cref="IDomainEvent"/> instances representing pending domain events.</value>
    /// <remarks>
    /// These events should be dispatched after the aggregate changes are persisted,
    /// typically by the unit of work or repository implementation.
    /// </remarks>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    /// <summary>
    /// Adds a domain event to the aggregate's event collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    /// <remarks>
    /// This method is typically called from within entity methods when state changes occur
    /// that other parts of the system need to react to. Events are stored until they are
    /// dispatched after persistence.
    /// </remarks>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the aggregate's event collection.
    /// </summary>
    /// <remarks>
    /// This method should be called after domain events have been successfully dispatched,
    /// typically by the infrastructure layer after persisting changes to the database.
    /// </remarks>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}