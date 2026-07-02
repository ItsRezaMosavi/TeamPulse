using BuildingBlocks.Application.Abstractions.CQRS.Commands;
using BuildingBlocks.Results;

namespace BuildingBlocks.Application.CQRS.Commands;

public class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    public async Task<Result<TResult>> Handle(TCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}