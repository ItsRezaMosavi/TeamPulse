using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Context;
using BuildingBlocks.Infrastructure.Identifiers;
using BuildingBlocks.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;
using DateTime = BuildingBlocks.Infrastructure.Time.DateTime;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTime>();
        services.AddSingleton<IGuidGenerator, DefaultGuidGenerator>();

        return services;
    }

    public static IServiceCollection AddSequentialGuidGenerator(this IServiceCollection services)
    {
        services.AddSingleton<IGuidGenerator, SequentialGuidGenerator>();
        return services;
    }
}