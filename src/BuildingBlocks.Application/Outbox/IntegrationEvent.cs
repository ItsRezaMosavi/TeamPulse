namespace BuildingBlocks.Application.Outbox;

public abstract record IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent(string version = "1")
    {
        OccurredOnUtc = DateTime.UtcNow;
        Version = version;
    }

    public DateTime OccurredOnUtc { get; }
    public string Version { get; }
}