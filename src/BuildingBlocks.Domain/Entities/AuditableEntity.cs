namespace BuildingBlocks.Domain.Entities;

/// <summary>
/// Represents an auditable domain entity with default <see cref="Guid"/> identifier and user ID types.
/// </summary>
/// <remarks>
/// This is a convenience class that inherits from <see cref="AuditableEntity{Guid, Guid}"/>
/// to provide a standard auditable entity implementation using Guid for both entity ID and user ID.
/// Auditable entities track creation and modification timestamps along with the users responsible.
/// </remarks>
public abstract class AuditableEntity : AuditableEntity<Guid,Guid>
{
    
}