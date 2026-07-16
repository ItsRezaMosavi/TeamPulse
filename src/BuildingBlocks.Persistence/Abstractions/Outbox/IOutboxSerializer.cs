using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Persistence.Outbox.Serialization;

namespace BuildingBlocks.Persistence.Abstractions.Outbox;

public interface IOutboxSerializer
{
    SerializedIntegrationEvent Serialize(IIntegrationEvent integrationEvent);

    IIntegrationEvent Deserialize(SerializedIntegrationEvent serializedEvent);
}