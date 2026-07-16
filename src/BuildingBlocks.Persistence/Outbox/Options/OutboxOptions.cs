namespace BuildingBlocks.Persistence.Outbox.Options;
/// <summary>
/// Represents configuration options for the Outbox processing infrastructure.
/// </summary>
public sealed class OutboxOptions
{
	
	/// <summary>
	/// Gets or sets the maximum number of outbox messages processed in a single batch.
	/// </summary>
	public int BatchSize { get; set; } = 100;

	/// <summary>
	/// Gets or sets the delay between consecutive polling cycles.
	/// </summary>
	public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(5);

	public int MaxRetryCount { get; set; } = 5;
}