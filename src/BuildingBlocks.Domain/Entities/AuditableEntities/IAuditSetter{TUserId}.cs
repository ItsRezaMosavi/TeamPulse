namespace BuildingBlocks.Domain.Entities.AuditableEntities;

/// <summary>
/// Interface for setting audit information on auditable entities.
/// </summary>
/// <typeparam name="TUserId">The type of the user identifier.</typeparam>
/// <remarks>
/// This interface is implemented by auditable entity base classes to allow
/// infrastructure components (like EF Core interceptors) to set audit information
/// without exposing public setters on the audit properties.
/// </remarks>
public interface IAuditSetter<in TUserId>
{
    /// <summary>
    /// Sets the creation audit information for the entity.
    /// </summary>
    /// <param name="userId">The ID of the user creating the entity.</param>
    /// <param name="createdAt">The UTC date and time when the entity was created.</param>
    void SetCreated(TUserId? userId, DateTime createdAt);
    
    /// <summary>
    /// Sets the update audit information for the entity.
    /// </summary>
    /// <param name="userId">The ID of the user updating the entity.</param>
    /// <param name="updatedAt">The UTC date and time when the entity was updated.</param>
    void SetUpdated(TUserId? userId, DateTime updatedAt);
}