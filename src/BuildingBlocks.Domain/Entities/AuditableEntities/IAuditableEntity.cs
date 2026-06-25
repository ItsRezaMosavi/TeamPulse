namespace BuildingBlocks.Domain.Entities.AuditableEntities;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TUserId"></typeparam>
public interface IAuditableEntity<out TUserId>
{
    /// <summary>
    /// Gets the UTC date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the ID of the user who created the entity.
    /// </summary>
    public TUserId? CreatedBy { get; }

    /// <summary>
    /// Gets the UTC date and time when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; }

    /// <summary>
    /// Gets the ID of the user who last updated the entity.
    /// </summary>
    public TUserId? UpdatedBy { get; }
}