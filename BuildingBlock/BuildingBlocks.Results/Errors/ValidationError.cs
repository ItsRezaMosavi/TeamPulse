using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class ValidationError(string message = DefaultMessage.Validation, int code = (int)DefaultCode.Validation)
    : Error(ErrorType.Validation, code, message);