using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Persistence.Abstractions;
using BuildingBlocks.Persistence.Abstractions.Repositories;
using BuildingBlocks.Persistence.DbContexts;
using BuildingBlocks.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence;

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

        return services;
    }
}