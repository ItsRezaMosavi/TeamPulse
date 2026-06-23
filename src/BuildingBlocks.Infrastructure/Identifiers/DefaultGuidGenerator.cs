using BuildingBlocks.Application.Abstractions;

namespace BuildingBlocks.Infrastructure.Identifiers;

public sealed class DefaultGuidGenerator : IGuidGenerator
{
    public Guid Generate()
    {
        return Guid.NewGuid();
    }
}