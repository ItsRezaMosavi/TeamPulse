namespace BuildingBlocks.Results;

public sealed class Result<T> : ResultBase
{
    public Result(bool isSuccess, Error[] errors, T? value) : base(isSuccess, errors)
    {
        Value = value;
    }

    public T? Value { get; private set; }

    public static Result<T> Success(T value) => new Result<T>(true, [], value);
    public static Result<T> Failure(Error[] errors) => new(false, errors, default);

    public static implicit operator Result<T>(Error error) => Failure([error]);

    public static implicit operator Result<T>(Error[] errors) => Failure(errors);

    public static implicit operator Result<T>(T value) => Success(value);
}