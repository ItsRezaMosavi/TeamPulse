using BuildingBlocks.Application.Context;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Outbox.Entities;
using BuildingBlocks.Persistence.Outbox.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Persistence.Outbox.Processing;
/// <summary>
/// Processes pending outbox messages and publishes them to the configured external event bus.
/// </summary>
/// <remarks>
/// This processor retrieves unprocessed messages in batches, deserializes the stored
/// integration events, publishes them, and updates the processing state.
/// Failed messages remain in the Outbox and can be retried later.
/// </remarks>
public class OutboxProcessor(
	BuildingBlocksDbContext dbContext,
	IOutboxSerializer outboxSerializer,
	IExternalEventBus externalEventBus,
	IDateTimeProvider dateTimeProvider,
	IOptions<OutboxOptions> options)
{
	private readonly OutboxOptions _options = options.Value;

	public async Task ProcessAsync(CancellationToken cancellationToken = default)
	{
		var messages = await GetMessages(cancellationToken);

		foreach (var message in messages)
		{
			try
			{
				var integrationEvent = outboxSerializer.Deserialize(message.ToSerializedIntegrationEvent());

				await externalEventBus.PublishAsync(integrationEvent, cancellationToken);

				message.Complete(dateTimeProvider.UtcNow);
			}
			catch (Exception ex)
			{
				message.Fail(ex.ToString(), dateTimeProvider.UtcNow);
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private async Task<List<OutboxMessage>> GetMessages(CancellationToken cancellationToken = default)
	{
		return await dbContext.Set<OutboxMessage>()
							  .Where(m => !m.IsProcessed)
							  .OrderBy(m => m.OccurredOnUtc)
							  .Take(_options.BatchSize)
							  .ToListAsync(cancellationToken);
	}
}