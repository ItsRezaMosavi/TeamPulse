namespace BuildingBlocks.Domain.Aggregates;

/// <summary>
/// Represents an aggregate root with a default <see cref="Guid"/> identifier.
/// </summary>
/// <remarks>
/// This is a convenience class that inherits from <see cref="AggregateRoot{Guid}"/>
/// to provide a standard aggregate root implementation using Guid as the identifier type.
/// Aggregate roots are the entry points for business operations and maintain consistency
/// boundaries within the domain model.
/// </remarks>
public abstract class AggregateRoot : AggregateRoot<Guid>
{
}
