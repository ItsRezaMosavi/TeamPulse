using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Persistence;
using BuildingBlocks.Domain.Events;
using BuildingBlocks.Persistence.DbContexts;

namespace BuildingBlocks.Persistence;

public sealed class UnitOfWork(ApplicationDbContext dbContext, IEventDispatcher eventDispatcher)
    : IUnitOfWork
{
    public ValueTask DisposeAsync()
    {
        return dbContext.DisposeAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = GetDomainEvents(dbContext);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            await eventDispatcher.DispatchAsync(domainEvents, cancellationToken);
        }
        finally
        {
            ClearDomainEvents(dbContext);
        }

        return result;
    }

    private static List<IDomainEvent> GetDomainEvents(ApplicationDbContext dbContext)
    {
        return dbContext.ChangeTracker
                        .Entries<IHasDomainEvents>()
                        .Where(e => e.Entity.DomainEvents.Count != 0)
                        .Select(e => e.Entity)
                        .SelectMany(e => e.DomainEvents)
                        .ToList();
    }

    private static void ClearDomainEvents(ApplicationDbContext dbContext)
    {
        var aggregates = dbContext.ChangeTracker
                                  .Entries<IHasDomainEvents>()
                                  .Select(e => e.Entity)
                                  .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }
    }
}