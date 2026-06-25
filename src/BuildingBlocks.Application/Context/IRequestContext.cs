namespace BuildingBlocks.Application.Context;

public interface IRequestContext
{
    Guid CorrelationId { get; }
    Guid RequestId { get; }
}