namespace BuildingBlocks.Persistence.Outbox.Processors;

public sealed class OutboxWorkerIdentity
{
	public Guid Id { get; } = Guid.NewGuid();
}