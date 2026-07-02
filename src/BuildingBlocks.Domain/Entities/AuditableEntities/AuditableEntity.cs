namespace BuildingBlocks.Domain.Entities.AuditableEntities;

/// <summary>
/// Represents an auditable entity with a default <see cref="Guid"/> user identifier.
/// </summary>
/// <remarks>
/// This is a convenience class that inherits from <see cref="AuditableEntity{Guid, Guid}"/>
/// to provide standard audit tracking using Guid as both the entity ID and user ID type.
/// </remarks>
public abstract class AuditableEntity : AuditableEntity<Guid, Guid>
{
}