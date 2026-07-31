using BuildingBlocks.Domain.Entities.Entities;
using BuildingBlocks.Persistence.Outbox.Enums;
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

		Status = OutboxStatus.Pending;
	}


	public static OutboxMessage Create(SerializedIntegrationEvent integrationEvent)
	{
		return new OutboxMessage(integrationEvent.Type,
								 integrationEvent.Payload,
								 integrationEvent.OccuredOnUtc,
								 integrationEvent.EventVersion);
	}


	public string Type { get; private set; }

	public string Payload { get; private set; }

	public string EventVersion { get; private set; }

	public DateTime OccurredOnUtc { get; private set; }


	public OutboxStatus Status { get; private set; }


	public DateTime? ProcessingStartedOnUtc { get; private set; }

	public Guid? LockedBy { get; private set; }


	public DateTime? ProcessedOnUtc { get; private set; }


	public int AttemptCount { get; private set; }

	public string? LastError { get; private set; }

	public DateTime? LastAttemptOnUtc { get; private set; }


	public byte[] RowVersion { get; private set; } = [];


	public bool IsCompleted => Status == OutboxStatus.Completed;

	public bool IsFinal => Status is OutboxStatus.Completed or OutboxStatus.Failed;


	public void StartProcessing(Guid workerId, DateTime startedOnUtc)
	{
		ArgumentOutOfRangeException.ThrowIfEqual(workerId, Guid.Empty);
		ArgumentOutOfRangeException.ThrowIfEqual(startedOnUtc, default);


		if (IsFinal) return;


		Status = OutboxStatus.Processing;
		LockedBy = workerId;
		ProcessingStartedOnUtc = startedOnUtc;
	}


	public void Complete(DateTime processedOnUtc)
	{
		ArgumentOutOfRangeException.ThrowIfEqual(processedOnUtc, default);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(processedOnUtc, OccurredOnUtc);
		
		if (IsFinal) return;
		
		Status = OutboxStatus.Completed;

		ProcessedOnUtc = processedOnUtc;
		LastError = null;

		ReleaseLock();
	}


	public void Fail(string error, DateTime attemptOnUtc, int maxRetryAttempts)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(error);
		ArgumentOutOfRangeException.ThrowIfEqual(attemptOnUtc, default);
		ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(
														   attemptOnUtc,
														   OccurredOnUtc);

		if (IsFinal) return;

		AttemptCount++;

		LastError = error;
		LastAttemptOnUtc = attemptOnUtc;


		Status = AttemptCount >= maxRetryAttempts ? OutboxStatus.Failed : OutboxStatus.Pending;

		ReleaseLock();
	}


	private void ReleaseLock()
	{
		LockedBy = null;
		ProcessingStartedOnUtc = null;
	}


	public bool IsProcessingExpired(DateTime now, TimeSpan timeout)
	{
		if (Status != OutboxStatus.Processing) return false;


		if (ProcessingStartedOnUtc is null) return true;


		return ProcessingStartedOnUtc.Value.Add(timeout) <= now;
	}
}