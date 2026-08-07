namespace BuildingBlocks.Domain.Entities;

/// <summary>
///     Represents an entity that supports soft deletion functionality.
/// </summary>
/// <typeparam name="TUserId">The type of the user identifier for tracking who deleted the entity.</typeparam>
/// <remarks>
///     Soft deletable entities are marked as deleted rather than being physically removed from the database.
///     This provides:
///     <list type="bullet">
///         <item>
///             <description>Audit trail of when and by whom entities were deleted</description>
///         </item>
///         <item>
///             <description>Ability to restore accidentally or intentionally deleted data</description>
///         </item>
///         <item>
///             <description>Historical data preservation for compliance and reporting</description>
///         </item>
///     </list>
///     The interface provides default implementations for <see cref="Delete" /> and <see cref="Restore" /> methods.
/// </remarks>
public interface ISoftDeletable<TUserId>
{
	/// <summary>
	///     Gets the UTC date and time when the entity was soft deleted.
	/// </summary>
	public DateTime? DeletedAt { get; }

	/// <summary>
	///     Gets the ID of the user who soft deleted the entity.
	/// </summary>
	public TUserId? DeletedBy { get; }

	/// <summary>
	///     Gets a value indicating whether the entity is currently marked as deleted.
	/// </summary>
	bool IsDeleted { get; }

	/// <summary>
	///     Marks the entity as deleted by setting the deletion timestamp and user ID.
	/// </summary>
	/// <param name="userId">The ID of the user performing the deletion.</param>
	/// <param name="deletedAt"></param>
	/// <remarks>
	///     If the entity is already deleted, this method returns without making changes.
	///     Sets <see cref="IsDeleted" /> to true, records the <see cref="DeletedBy" /> user ID,
	///     and sets <see cref="DeletedAt" /> to the current UTC time.
	/// </remarks>
	void Delete(TUserId userId, DateTime deletedAt);

	/// <summary>
	///     Restores a previously deleted entity by clearing the deletion information.
	/// </summary>
	/// <remarks>
	///     If the entity is not currently deleted, this method returns without making changes.
	///     Sets <see cref="IsDeleted" /> to false, clears <see cref="DeletedBy" />,
	///     and sets <see cref="DeletedAt" /> to null.
	/// </remarks>
	void Restore();
}