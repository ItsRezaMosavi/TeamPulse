using BuildingBlocks.Results.Contracts;

namespace BuildingBlocks.Results;

public sealed class Result : ResultBase
{
    public Result(bool isSuccess, Error[] errors) : base(isSuccess, errors)
    {
    }

    public static Result Success() => new Result(true, []);
    public static Result Failure(params Error[] errors) => new Result(false, errors);

    public static implicit operator Result(Error[] errors) => Failure(errors);
    public static implicit operator Result(Error error) => Failure(error);
}