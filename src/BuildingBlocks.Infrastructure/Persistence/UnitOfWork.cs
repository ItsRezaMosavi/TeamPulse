using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Persistence;
using BuildingBlocks.Domain.Events;

namespace BuildingBlocks.Infrastructure.Persistence;

public sealed class UnitOfWork(ApplicationDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
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
            await domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
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