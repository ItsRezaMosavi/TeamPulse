using BuildingBlocks.Results;

namespace BuildingBlocks.Application.CQRS.Behaviors;

public delegate Task<Result<TResult>> RequestHandlerDelegate<TResult>();