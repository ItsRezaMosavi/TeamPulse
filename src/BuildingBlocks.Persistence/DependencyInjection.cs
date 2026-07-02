using BuildingBlocks.Application;
using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence;

/// <summary>
    /// Provides extension methods for configuring Building Blocks persistence services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Building Blocks infrastructure services including DbContext, Unit of Work, and repositories to the service collection.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configureDbContext">An action to configure the DbContext options.</param>
    /// <returns>The configured service collection for method chaining.</returns>
    public static IServiceCollection AddBuildingBlocksInfrastructure(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureDbContext)
    {
        services.AddDbContext<ApplicationDbContext>(configureDbContext);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IReadRepository<,>), typeof(ReadRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}