using BuildingBlocks.Domain.Entities.Entities;
using BuildingBlocks.Persistence.Outbox.Serialization;

namespace BuildingBlocks.Persistence.Outbox.Entities;

public class OutboxMessage : Entity
{
	private OutboxMessage(string type,
						  string payload,
						  DateTime occurredOnUtc,
						  string eventVersion = "1")
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(type);
		ArgumentException.ThrowIfNullOrWhiteSpace(payload);
		ArgumentOutOfRangeException.ThrowIfEqual(occurredOnUtc, default);


		Type = type;
		Payload = payload;
		OccurredOnUtc = occurredOnUtc;
		EventVersion = eventVersion;
	}

	public static OutboxMessage Create(SerializedIntegrationEvent integrationEvent)
	{
		return new OutboxMessage(integrationEvent.Type, integrationEvent.Payload, integrationEvent.OccuredOnUtc,
								 integrationEvent.EventVersion);
	}

	public string Type { get; private set; }

	public string Payload { get; private set; }

	public string EventVersion { get; private set; }

	public DateTime OccurredOnUtc { get; private set; }

	public DateTime? ProcessedOnUtc { get; private set; }

	public int AttemptCount { get; private set; }

	public string? LastError { get; private set; }

	public DateTime? LastAttemptOnUtc { get; private set; }


	public bool IsProcessed => ProcessedOnUtc.HasValue;

	public void Complete(DateTime processedOnUtc)
	{
		ArgumentOutOfRangeException.ThrowIfEqual(processedOnUtc, default);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(processedOnUtc, OccurredOnUtc);

		if (IsProcessed) return;

		ProcessedOnUtc = processedOnUtc;
		LastError = null;
	}

	public void Fail(string error, DateTime attemptOnUtc)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(error);
		ArgumentOutOfRangeException.ThrowIfEqual(attemptOnUtc, default);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(attemptOnUtc, OccurredOnUtc);

		if (IsProcessed) return;

		AttemptCount++;
		LastError = error;
		LastAttemptOnUtc = attemptOnUtc;
	}

	public SerializedIntegrationEvent ToSerializedIntegrationEvent()
	{
		return new SerializedIntegrationEvent(Type, Payload, OccurredOnUtc, EventVersion);
	}
}