namespace BuildingBlocks.Results;

public class Error
{
    internal Error(ErrorType type, int code, string message)
    {
        Type = type;
        Message = message;
        Code = code;
    }

    public int Code { get; }
    public ErrorType Type { get; }
    public string Message { get; }
}