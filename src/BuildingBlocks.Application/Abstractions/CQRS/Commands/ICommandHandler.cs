using BuildingBlocks.Results;

namespace BuildingBlocks.Application.Abstractions.CQRS.Commands;

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default);
}