using BuildingBlocks.Application.Context;
using BuildingBlocks.Infrastructure.Identifiers;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTime.DateTime>();
        services.AddSingleton<IGuidGenerator, DefaultGuidGenerator>();

        return services;
    }

    public static IServiceCollection AddSequentialGuidGenerator(this IServiceCollection services)
    {
        services.AddSingleton<IGuidGenerator, SequentialGuidGenerator>();
        return services;
    }
}