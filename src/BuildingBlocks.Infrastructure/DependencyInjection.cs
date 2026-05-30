using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Persistence;
using BuildingBlocks.Application.Persistence.Repositories;
using BuildingBlocks.Infrastructure.Persistence;
using BuildingBlocks.Infrastructure.Persistence.Repositories;
using BuildingBlocks.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocksInfrastructure(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDbContext)
    {
        services.AddDbContext<ApplicationDbContext>(configureDbContext);

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

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