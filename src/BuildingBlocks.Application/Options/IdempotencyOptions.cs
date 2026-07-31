namespace BuildingBlocks.Application.Options;

public class IdempotencyOptions
{
	public bool CacheFailures { get; set; } = false;
	
	public TimeSpan Expiration { get; set; } = TimeSpan.FromHours(24);
}