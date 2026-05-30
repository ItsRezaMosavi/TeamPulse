using BuildingBlocks.Application.Abstractions;

namespace BuildingBlocks.Infrastructure;

public sealed class DefaultGuidGenerator : IGuidGenerator
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}