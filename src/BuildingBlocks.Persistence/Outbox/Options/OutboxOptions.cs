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

	/// <summary>
	/// Gets or sets the minimum delay before retrying a failed outbox message.
	/// </summary>
	public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMinutes(1);

	/// <summary>
	/// Gets or sets the maximum amount of time a message may remain in the Processing state
	/// before it is considered abandoned and becomes eligible for reprocessing.
	/// </summary>
	public TimeSpan ProcessingTimeout { get; set; } = TimeSpan.FromMinutes(5);

	/// <summary>
	/// Gets or sets the maximum number of retry attempts before marking a message as failed.
	/// </summary>
	public int MaxRetryAttempts { get; set; } = 5;
}