using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class BusinessRuleError(string message = DefaultMessage.BusinessRule, int code = (int)DefaultCode.BusinessRule)
    : Error(ErrorType.BusinessRule, code, message);