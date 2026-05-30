using BuildingBlocks.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.Persistence;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    public ValueTask DisposeAsync()
    {
        return dbContext.DisposeAsync();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}