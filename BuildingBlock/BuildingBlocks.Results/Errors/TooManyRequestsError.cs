using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class TooManyRequestsError(
    string message = DefaultMessage.TooManyRequests,
    int code = (int)DefaultCode.TooManyRequests)
    : Error(ErrorType.TooManyRequests, code, message);