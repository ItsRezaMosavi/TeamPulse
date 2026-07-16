using BuildingBlocks.Persistence.Outbox.Options;
using BuildingBlocks.Persistence.Outbox.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Persistence.Outbox.Background;

/// <summary>
/// Periodically processes pending outbox messages and publishes them to the external event bus.
/// </summary>
/// <remarks>
/// This background service continuously polls the Outbox table, deserializes pending
/// integration events, publishes them through the configured external event bus,
/// and updates their processing status.
/// </remarks>
public sealed class OutboxBackgroundService(
	IServiceScopeFactory serviceScopeFactory,
	IOptions<OutboxOptions> options,
	ILogger<OutboxBackgroundService> logger)
	: BackgroundService
{
	private readonly OutboxOptions _options = options.Value;

	/// <inheritdoc />
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("Outbox Background Service started.");
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				using var scope = serviceScopeFactory.CreateScope();

				var processor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();

				await processor.ProcessAsync(stoppingToken);
				await Task.Delay(_options.PollingInterval, stoppingToken);
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while processing outbox messages.");
			}
		}

		logger.LogInformation("Outbox Background Service stopped.");
	}
}