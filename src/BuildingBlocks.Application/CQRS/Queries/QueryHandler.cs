using BuildingBlocks.Application.Abstractions.CQRS.Queries;
using BuildingBlocks.Results;

namespace BuildingBlocks.Application.CQRS.Queries;

public class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    public async Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}