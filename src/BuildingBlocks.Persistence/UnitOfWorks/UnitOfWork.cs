using BuildingBlocks.Application.Events;
using BuildingBlocks.Domain.Events;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Persistence.DbContexts;

namespace BuildingBlocks.Persistence.UnitOfWorks;

/// <summary>
/// Implements the Unit of Work pattern with domain event dispatching.
/// </summary>
/// <remarks>
/// This class coordinates database transactions and ensures domain events are dispatched
/// after successful persistence. It implements the following workflow:
/// 
/// <list type="number">
/// <item><description>Collects all pending domain events from tracked aggregates</description></item>
/// <item><description>Saves all changes to the database within a transaction</description></item>
/// <item><description>Dispatches collected domain events to handlers</description></item>
/// <item><description>Clears domain events from aggregates after successful dispatch</description></item>
/// </list>
/// 
/// The domain event dispatch occurs in a try-finally block to ensure events are cleared
/// even if dispatch fails, preventing duplicate event processing on retry scenarios.
/// </remarks>
public sealed class UnitOfWork(BuildingBlocksDbContext dbContext, IEventDispatcher eventDispatcher)
	: IUnitOfWork
{
	/// <summary>
	///     Disposes the underlying database context asynchronously.
	/// </summary>
	/// <returns>A ValueTask representing the asynchronous dispose operation.</returns>
	/// <remarks>
	///     This method releases all resources held by the DbContext, including database
	///     connections and change tracker entries. Call this when the unit of work is
	///     no longer needed, typically at the end of a request or scope.
	/// </remarks>
	public ValueTask DisposeAsync()
	{
		return dbContext.DisposeAsync();
	}

	/// <summary>
	///     Saves all pending changes and dispatches domain events.
	/// </summary>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>The number of state entries written to the database.</returns>
	/// <remarks>
	///     This method executes the complete unit of work transaction:
	///     <list type="number">
	///         <item>
	///             <description>Extracts domain events from all tracked aggregates</description>
	///         </item>
	///         <item>
	///             <description>Persists changes via SaveChangesAsync</description>
	///         </item>
	///         <item>
	///             <description>Dispatches events to registered handlers</description>
	///         </item>
	///         <item>
	///             <description>Clears domain events from aggregates</description>
	///         </item>
	///     </list>
	///     If event dispatch fails, the exception propagates but domain events are still cleared
	///     to prevent infinite retry loops. The caller should handle failed event dispatch appropriately.
	/// </remarks>
	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var domainEvents = GetDomainEvents(dbContext);

		var result = await dbContext.SaveChangesAsync(cancellationToken);

		if (domainEvents.Count == 0) return result;

		try
		{
			await eventDispatcher.DispatchAsync(domainEvents, cancellationToken);
		}
		finally
		{
			ClearDomainEvents(dbContext);
		}

		return result;
	}

	/// <summary>
	///     Extracts all pending domain events from tracked entities in the change tracker.
	/// </summary>
	/// <param name="dbContext">The database context containing tracked entities.</param>
	/// <returns>A list of domain events to be dispatched.</returns>
	/// <remarks>
	///     This method scans the change tracker for entities implementing IHasDomainEvents,
	///     filters those with non-empty event collections, and flattens all events into
	///     a single list for batch dispatch.
	/// </remarks>
	private static List<IDomainEvent> GetDomainEvents(BuildingBlocksDbContext dbContext)
	{
		return dbContext.ChangeTracker
						.Entries<IHasDomainEvents>()
						.Where(e => e.Entity.DomainEvents.Count != 0)
						.Select(e => e.Entity)
						.SelectMany(e => e.DomainEvents)
						.ToList();
	}

	/// <summary>
	///     Clears all domain events from tracked aggregates after successful dispatch.
	/// </summary>
	/// <param name="dbContext">The database context containing tracked entities.</param>
	/// <remarks>
	///     This method iterates through all tracked entities implementing IHasDomainEvents
	///     and calls ClearDomainEvents on each to remove processed events from their
	///     internal collections, preventing duplicate dispatch.
	/// </remarks>
	private static void ClearDomainEvents(BuildingBlocksDbContext dbContext)
	{
		var aggregates = dbContext.ChangeTracker
								  .Entries<IHasDomainEvents>()
								  .Select(e => e.Entity)
								  .ToList();

		foreach (var aggregate in aggregates) aggregate.ClearDomainEvents();
	}
}