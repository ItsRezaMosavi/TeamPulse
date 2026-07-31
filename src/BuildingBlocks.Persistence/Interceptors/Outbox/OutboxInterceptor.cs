using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.Outbox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Interceptors.Outbox;

/// <summary>
/// Intercepts EF Core save operations and persists pending integration events
/// as outbox messages within the current transaction.
/// </summary>
/// <remarks>
/// This interceptor executes before changes are committed, ensuring integration events
/// are stored atomically with the application's data.
/// </remarks>
public class OutboxInterceptor(IOutboxEventCollector collector, IOutboxSerializer serializer)
	: SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
																		  InterceptionResult<int> result,
																		  CancellationToken cancellationToken = new())
	{
		if (eventData.Context is not null) AddOutboxMessages(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		if (eventData.Context is not null) AddOutboxMessages(eventData.Context);

		return base.SavingChanges(eventData, result);
	}

	private void AddOutboxMessages(DbContext context)
	{
		var integrationEvents = collector.GetEvents();

		if (integrationEvents.Count == 0) return;

		var serializedEvents = integrationEvents.Select(serializer.Serialize).ToList();

		var outboxMessages = serializedEvents.Select(OutboxMessage.Create).ToList();

		context.Set<OutboxMessage>().AddRange(outboxMessages);
	}

	public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
	{
		collector.Clear();

		return base.SavedChanges(eventData, result);
	}

	public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
													 int result,
													 CancellationToken cancellationToken = new())
	{
		collector.Clear();

		return base.SavedChangesAsync(eventData, result, cancellationToken);
	}
}