namespace BuildingBlocks.Persistence.Outbox.Serialization;

public sealed record SerializedIntegrationEvent(
    string Type,
    string Payload,
    DateTime OccuredOnUtc,
    string EventVersion);