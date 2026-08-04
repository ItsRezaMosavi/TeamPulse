using BuildingBlocks.Application.Context;
using BuildingBlocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Interceptors.SoftDelete;

/// <summary>
/// Intercepts EF Core save operations to implement soft delete behavior.
/// </summary>
/// <typeparam name="TUserId">The type of the user identifier for tracking who deleted entities.</typeparam>
/// <remarks>
/// This interceptor automatically converts hard deletes into soft deletes for entities
/// implementing <see cref="ISoftDeletable{TUserId}"/>. Instead of physically removing
/// deleted entities from the database, it:
/// 
/// <list type="bullet">
/// <item><description>Sets the <see cref="ISoftDeletable{TUserId}.IsDeleted"/> flag to true</description></item>
/// <item><description>Records the deletion timestamp and user ID</description></item>
/// <item><description>Changes the entity state from Deleted to Modified</description></item>
/// </list>
/// 
/// The interceptor runs during <see cref="DbContext.SaveChangesAsync"/> and processes
/// all entries marked for deletion before the changes are persisted.
/// </remarks>
public class SoftDeleteInterceptor<TUserId>(IDateTimeProvider dateTimeProvider, ICurrentUser<TUserId> currentUser)
	: SaveChangesInterceptor
{
	/// <summary>
	///     Intercepts the save operation to convert hard deletes into soft deletes.
	/// </summary>
	/// <param name="eventData">Event data containing the DbContext context.</param>
	/// <param name="result">The current interception result.</param>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>A task containing the interception result.</returns>
	/// <remarks>
	///     For each entity marked as Deleted that implements ISoftDeletable, this method:
	///     <list type="number">
	///         <item>
	///             <description>Calls the Delete method with current user ID and timestamp</description>
	///         </item>
	///         <item>
	///             <description>Changes the entity state to Modified so it updates instead of deletes</description>
	///         </item>
	///     </list>
	///     If no context is available or no deletable entities are found, the operation proceeds normally.
	/// </remarks>
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
																		  InterceptionResult<int> result,
																		  CancellationToken cancellationToken = default)
	{
		var now = dateTimeProvider.UtcNow;
		var userId = currentUser.UserId;
		var context = eventData.Context;

		if (context is null) return ValueTask.FromResult(result);

		var entries = context.ChangeTracker.Entries()
							 .Where(e => e.State is EntityState.Deleted &&
										 e.Entity is ISoftDeletable<TUserId>);

		foreach (var entry in entries)
		{
			var entity = (ISoftDeletable<TUserId>)entry.Entity;
			entity.Delete(userId, now);
			entry.State = EntityState.Modified;
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}