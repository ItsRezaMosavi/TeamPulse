namespace BuildingBlocks.Domain.Entities;

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
public abstract class AuditableEntity<TId, TUserId> : Entity<TId>
{
    /// <summary>
    /// Gets the UTC date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }
    
    /// <summary>
    /// Gets the ID of the user who created the entity.
    /// </summary>
    public TUserId? CreatedBy { get; protected set; }
    
    /// <summary>
    /// Gets the UTC date and time when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; protected set; }
    
    /// <summary>
    /// Gets the ID of the user who last updated the entity.
    /// </summary>
    public TUserId? UpdatedBy { get; protected set; }

    /// <summary>
    /// Sets the creation audit information.
    /// </summary>
    /// <param name="userId">The ID of the user creating the entity.</param>
    /// <remarks>
    /// This method should be called when a new entity is being created to record
    /// the creation timestamp and the creator's user ID.
    /// </remarks>
    protected void SetCreated(TUserId? userId)
    {
        CreatedAt = DateTime.UtcNow;
        CreatedBy = userId;
    }

    /// <summary>
    /// Sets the update audit information.
    /// </summary>
    /// <param name="userId">The ID of the user updating the entity.</param>
    /// <remarks>
    /// This method should be called whenever an existing entity is modified to record
    /// the update timestamp and the updater's user ID.
    /// </remarks>
    protected void SetUpdated(TUserId? userId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userId;
    }
}