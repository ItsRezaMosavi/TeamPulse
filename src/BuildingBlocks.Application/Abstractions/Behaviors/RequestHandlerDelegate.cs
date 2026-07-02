using BuildingBlocks.Results;

namespace BuildingBlocks.Application.Abstractions.Behaviors;

public delegate Task<Result<TResult>> RequestHandlerDelegate<TResult>();