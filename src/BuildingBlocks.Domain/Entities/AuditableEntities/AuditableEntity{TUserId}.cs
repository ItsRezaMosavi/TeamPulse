namespace BuildingBlocks.Domain.Entities.AuditableEntities;

/// <summary>
/// Represents an auditable entity with a generic user identifier type and default <see cref="Guid"/> entity ID.
/// </summary>
/// <typeparam name="TUserId">The type of the user identifier for audit tracking.</typeparam>
/// <remarks>
/// This is a convenience class that inherits from <see cref="AuditableEntity{TId, TUserId}"/>
/// to provide standard audit tracking using Guid as the entity ID while allowing
/// flexibility in the user ID type.
/// </remarks>
public abstract class AuditableEntity<TUserId> : AuditableEntity<Guid,TUserId>
{
    
}