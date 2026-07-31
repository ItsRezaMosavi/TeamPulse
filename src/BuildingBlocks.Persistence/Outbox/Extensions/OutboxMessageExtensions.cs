using BuildingBlocks.Persistence.Outbox.Entities;
using BuildingBlocks.Persistence.Outbox.Serialization;

namespace BuildingBlocks.Persistence.Outbox.Extensions;

public static class OutboxMessageExtensions
{
	public static SerializedIntegrationEvent ToSerializedIntegrationEvent(this OutboxMessage message)
	{
		return new SerializedIntegrationEvent(message.Type, message.Payload, message.OccurredOnUtc,
											  message.EventVersion);
	}
}