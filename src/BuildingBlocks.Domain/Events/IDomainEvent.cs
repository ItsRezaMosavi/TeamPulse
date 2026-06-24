namespace BuildingBlocks.Domain.Events;

/// <summary>
/// Represents a domain event that occurred within the system.
/// </summary>
/// <remarks>
/// Domain events capture significant occurrences in the domain that other parts of the system
/// may need to react to. They are immutable facts about something that happened in the past.
/// 
/// Key characteristics:
/// <list type="bullet">
/// <item><description>Past tense naming - events describe what already happened (e.g., OrderCreated, PaymentProcessed)</description></item>
/// <item><description>Immutable - once created, their data cannot change</description></item>
/// <item><description>Carry relevant data about the occurrence</description></item>
/// </list>
/// </remarks>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier for this event instance.
    /// </summary>
    /// <value>A GUID that uniquely identifies this specific event occurrence.</value>
    Guid EventId { get; }
    
    /// <summary>
    /// Gets the UTC date and time when the event occurred.
    /// </summary>
    /// <value>The timestamp indicating when the domain event was raised.</value>
    DateTime OccurredOn { get; }
}