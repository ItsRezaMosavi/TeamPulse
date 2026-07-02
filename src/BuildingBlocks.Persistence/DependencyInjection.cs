using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence;

/// <summary>
/// 
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureDbContext"></param>
    /// <returns></returns>
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