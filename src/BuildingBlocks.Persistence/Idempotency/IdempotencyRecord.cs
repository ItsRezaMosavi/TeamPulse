using BuildingBlocks.Application.Idempotency.Enums;
using BuildingBlocks.Domain.Entities.Entities;

namespace BuildingBlocks.Persistence.Idempotency;

public sealed class IdempotencyRecord : Entity
{
	private IdempotencyRecord()
	{
	}

	public static IdempotencyRecord Create(string key,
										   IdempotencyStatus status,
										   DateTime createdAtUtc,
										   DateTime expiresAtUtc,
										   string? serializedResponse = null,
										   DateTime? completedAtUtc = null)
	{
		return new IdempotencyRecord
		{
			Key = key,
			Status = status,
			CreatedAtUtc = createdAtUtc,
			CompletedAtUtc = completedAtUtc,
			ExpiresAtUtc = expiresAtUtc,
			SerializedResponse = serializedResponse
		};
	}

	public string Key { get; private set; } = default!;

	public IdempotencyStatus Status { get; private set; }

	public string? SerializedResponse { get; private set; }

	public DateTime CreatedAtUtc { get; private set; }

	public DateTime? CompletedAtUtc { get; private set; }

	public DateTime ExpiresAtUtc { get; private set; }

	public byte[] RowVersion { get; private set; } = default!;


	public bool IsExpired(DateTime now) => ExpiresAtUtc <= now;


	public void MarkAsComplete(string serializedResponse, DateTime completedAtUtc)
	{
		if (Status == IdempotencyStatus.Completed) return;
		SerializedResponse = serializedResponse;
		Status = IdempotencyStatus.Completed;
		CompletedAtUtc = completedAtUtc;
	}

	public void Reacquire(DateTime now, DateTime expiresAtUtc)
	{
		Status = IdempotencyStatus.InProgress;
		CreatedAtUtc = now;
		ExpiresAtUtc = expiresAtUtc;
		SerializedResponse = null;
		CompletedAtUtc = null;
	}
}