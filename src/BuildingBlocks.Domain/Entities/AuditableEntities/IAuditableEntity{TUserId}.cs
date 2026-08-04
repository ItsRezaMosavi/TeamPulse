namespace BuildingBlocks.Domain.Entities.AuditableEntities;

/// <summary>
///     Interface for entities that track creation and modification audit information.
/// </summary>
/// <typeparam name="TUserId">The type of the user identifier for tracking who created or updated the entity.</typeparam>
/// <remarks>
///     Auditable entities provide automatic tracking of:
///     <list type="bullet">
///         <item>
///             <description>When an entity was created and by whom</description>
///         </item>
///         <item>
///             <description>When an entity was last modified and by whom</description>
///         </item>
///     </list>
///     This interface is typically implemented by base entity classes and populated automatically
///     by EF Core interceptors during save operations.
/// </remarks>
public interface IAuditableEntity<out TUserId>
{
	/// <summary>
	///     Gets the UTC date and time when the entity was created.
	/// </summary>
	public DateTime CreatedAt { get; }

	/// <summary>
	///     Gets the ID of the user who created the entity.
	/// </summary>
	public TUserId? CreatedBy { get; }

	/// <summary>
	///     Gets the UTC date and time when the entity was last updated.
	/// </summary>
	public DateTime? UpdatedAt { get; }

	/// <summary>
	///     Gets the ID of the user who last updated the entity.
	/// </summary>
	public TUserId? UpdatedBy { get; }
}