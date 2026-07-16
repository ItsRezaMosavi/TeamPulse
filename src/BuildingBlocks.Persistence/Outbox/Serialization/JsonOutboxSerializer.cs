using System.Text.Json;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Abstractions.Outbox;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Persistence.Outbox.Serialization;

public sealed class JsonOutboxSerializer(IOptions<JsonSerializerOptions> options) : IOutboxSerializer
{
    private readonly JsonSerializerOptions _options = options.Value;

    public SerializedIntegrationEvent Serialize(IIntegrationEvent integrationEvent)
    {
        var payload = JsonSerializer.Serialize(integrationEvent, _options);
        var type = integrationEvent.GetType().AssemblyQualifiedName;

        ArgumentNullException.ThrowIfNull(type);

        return new SerializedIntegrationEvent(type, payload, integrationEvent.OccurredOnUtc, integrationEvent.Version);
    }

    public IIntegrationEvent Deserialize(SerializedIntegrationEvent serializedEvent)
    {
        var eventType = Type.GetType(serializedEvent.Type);

        if (eventType == null)
            throw new InvalidOperationException($"Could not resolve integration event type '{serializedEvent.Type}'.");

        var result = JsonSerializer.Deserialize(serializedEvent.Payload, eventType, _options) as IIntegrationEvent;

        return result ?? throw new InvalidOperationException($"Failed to deserialize integration event '{serializedEvent.Type}'.");
    }
}