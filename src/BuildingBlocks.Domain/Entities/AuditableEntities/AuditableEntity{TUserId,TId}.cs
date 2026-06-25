using BuildingBlocks.Domain.Entities.Entities;

namespace BuildingBlocks.Domain.Entities.AuditableEntities;

/// <summary>
/// Represents an auditable domain entity with generic identifier and user ID types.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
/// <typeparam name="TUserId">The type of the user identifier for audit tracking.</typeparam>
/// <remarks>
/// Auditable entities extend basic entities by tracking:
/// <list type="bullet">
/// <item><description><see cref="CreatedAt"/> and <see cref="CreatedBy"/> - When and by whom the entity was created</description></item>
/// <item><description><see cref="UpdatedAt"/> and <see cref="UpdatedBy"/> - When and by whom the entity was last modified</description></item>
/// </list>
/// The protected methods <see cref="SetCreated"/> and <see cref="SetUpdated"/> should be called by derived classes
/// to update audit information when changes occur.
/// </remarks>
public abstract class AuditableEntity<TId, TUserId> : Entity<TId>, IAuditableEntity<TUserId>, IAuditSetter<TUserId>
{
    public DateTime CreatedAt { get; private set; }
    public TUserId? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public TUserId? UpdatedBy { get; private set; }

    /// <summary>
    /// Sets the creation audit information.
    /// </summary>
    /// <param name="userId">The ID of the user creating the entity.</param>
    /// <param name="createdAt"></param>
    /// <remarks>
    /// This method should be called when a new entity is being created to record
    /// the creation timestamp and the creator's user ID.
    /// </remarks>
    void IAuditSetter<TUserId>.SetCreated(TUserId? userId, DateTime createdAt)
    {
        CreatedAt = createdAt;
        CreatedBy = userId;
    }

    /// <summary>
    /// Sets the update audit information.
    /// </summary>
    /// <param name="userId">The ID of the user updating the entity.</param>
    /// <param name="updatedAt"></param>
    /// <remarks>
    /// This method should be called whenever an existing entity is modified to record
    /// the update timestamp and the updater's user ID.
    /// </remarks>
    void IAuditSetter<TUserId>.SetUpdated(TUserId? userId, DateTime updatedAt)
    {
        UpdatedAt = updatedAt;
        UpdatedBy = userId;
    }
}