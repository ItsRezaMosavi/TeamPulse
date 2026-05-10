namespace BuildingBlocks.Results.Contracts;

public abstract class ResultBase
{
    protected ResultBase(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error[]? Errors { get; }
}