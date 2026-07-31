using BuildingBlocks.Application.Outbox;

namespace BuildingBlocks.Persistence.Abstractions.Outbox;

/// <summary>
/// Represents an external message bus capable of publishing integration events
/// to other services.
/// </summary>
/// <remarks>
/// Implementations may use RabbitMQ, Kafka, Azure Service Bus,
/// MassTransit, or any other messaging infrastructure.
/// </remarks>
public interface IExternalEventBus
{
	Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}