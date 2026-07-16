using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Outbox.Serialization;

namespace BuildingBlocks.Persistence.Abstractions.Outbox;
/// <summary>
/// Defines methods for serializing and deserializing integration events
/// for storage in the Outbox.
/// </summary>
public interface IOutboxSerializer
{
    SerializedIntegrationEvent Serialize(IIntegrationEvent integrationEvent);

    IIntegrationEvent Deserialize(SerializedIntegrationEvent serializedEvent);
}