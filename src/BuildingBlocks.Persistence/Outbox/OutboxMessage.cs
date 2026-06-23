namespace BuildingBlocks.Persistence.OutBox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public string Type { get; init; } = null!;
    public string Content { get; init; } = null!;

    public DateTime? ProcessedOnUtc { get; set; }
    public int AttemptCount { get; set; } = 0;
    public string? LastError { get; set; }
    
}