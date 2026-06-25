using BuildingBlocks.Results;

namespace BuildingBlocks.Application.CQRS.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> Handle(TQuery query, CancellationToken cancellationToken = default);
}