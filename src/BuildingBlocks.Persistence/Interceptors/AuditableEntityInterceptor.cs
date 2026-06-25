using BuildingBlocks.Application.Context;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Persistence.Interceptors;

public class AuditableEntityInterceptor<TUserId>(IDateTimeProvider dateTimeProvider, ICurrentUser<TUserId> currentUser)
    : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                 InterceptionResult<int> result,
                                                                 CancellationToken cancellationToken =
                                                                     new CancellationToken())
    {
        throw new NotImplementedException();
    }
}