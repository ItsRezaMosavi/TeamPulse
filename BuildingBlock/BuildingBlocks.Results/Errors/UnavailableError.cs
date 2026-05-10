using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class UnavailableError(string message = DefaultMessage.Unavailable, int code = (int)DefaultCode.Unavailable)
    : Error(ErrorType.Unavailable, code, message);