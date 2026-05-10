using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class FailureError(string message = DefaultMessage.Failure, int code = (int)DefaultCode.Failure) :
    Error(ErrorType.Failure, code, message);