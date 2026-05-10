using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class TimeoutError(string message = DefaultMessage.Timeout, int code = (int)DefaultCode.Timeout)
    : Error(ErrorType.Timeout, code, message);