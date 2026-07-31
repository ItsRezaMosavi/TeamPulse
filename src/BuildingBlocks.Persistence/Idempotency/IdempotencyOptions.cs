namespace BuildingBlocks.Persistence.Idempotency;

public class IdempotencyOptions
{
	public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(30);
	public bool CacheFailures { get; set; } = false;
}