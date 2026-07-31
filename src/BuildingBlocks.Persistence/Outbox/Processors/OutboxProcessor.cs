using BuildingBlocks.Application.Context;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Extensions;
using BuildingBlocks.Persistence.Outbox.Entities;
using BuildingBlocks.Persistence.Outbox.Extensions;
using BuildingBlocks.Persistence.Outbox.Options;
using BuildingBlocks.Persistence.Outbox.QuerySpecifications;
using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Persistence.Outbox.Processors;

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
	OutboxWorkerIdentity workerIdentity,
	ISpecificationEvaluator specificationEvaluator,
	IOptions<OutboxOptions> options)
{
	private readonly OutboxOptions _options = options.Value;

	public async Task ProcessAsync(CancellationToken cancellationToken = default)
	{
		var messages = await AcquireBatchAsync(cancellationToken);

		foreach (var message in messages)
		{
			try
			{
				var integrationEvent = outboxSerializer.Deserialize(message.ToSerializedIntegrationEvent());

				await externalEventBus.PublishAsync(integrationEvent, cancellationToken);

				message.Complete(dateTimeProvider.UtcNow);
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception ex)
			{
				message.Fail(ex.ToString(), dateTimeProvider.UtcNow, _options.MaxRetryAttempts);
			}
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private async Task<List<OutboxMessage>> AcquireBatchAsync(CancellationToken cancellationToken = default)
	{
		var now = dateTimeProvider.UtcNow;

		var specification = new OutboxMessageQuery(now,
																_options.ProcessingTimeout,
																_options.BatchSize,
																_options.RetryDelay);

		var messages = await dbContext.Set<OutboxMessage>()
									  .WithSpecification(specification, specificationEvaluator)
									  .ToListAsync(cancellationToken);

		messages.ForEach(m => m.StartProcessing(workerIdentity.Id, now));

		try
		{
			await dbContext.SaveChangesAsync(cancellationToken);
		}
		catch (DbUpdateConcurrencyException)
		{
			return [];
		}

		return messages;
	}
}