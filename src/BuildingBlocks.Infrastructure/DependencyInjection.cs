using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IGuidGenerator, DefaultGuidGenerator>();

        return services;
    }

    public static IServiceCollection AddSequentialGuidGenerator(this IServiceCollection services)
    {
        services.AddSingleton<IGuidGenerator, SequentialGuidGenerator>();
        return services;
    }
}