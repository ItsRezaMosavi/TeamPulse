namespace BuildingBlocks.Domain.Entities.Entities;

/// <summary>
/// Represents a domain entity with a default <see cref="Guid"/> identifier.
/// </summary>
/// <remarks>
/// This is a convenience class that inherits from <see cref="Entity{Guid}"/>
/// to provide a standard entity implementation using Guid as the identifier type.
/// Entities are objects defined by their identity rather than their attributes.
/// </remarks>
public abstract class Entity : Entity<Guid>
{
	protected Entity()
	{
		Id = Guid.NewGuid();
	}
}