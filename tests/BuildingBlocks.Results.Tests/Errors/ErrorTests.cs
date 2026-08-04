using BuildingBlocks.Results.Defaults;
using BuildingBlocks.Results.Errors;
using FluentAssertions;

namespace BuildingBlocks.Results.Tests.Errors;

public class ErrorTests
{
	public static IEnumerable<object[]> ErrorTestCases =>
	[
		[new NotFoundError(), ErrorType.NotFound, DefaultErrorCodes.NotFound, DefaultMessage.NotFound],
		[
			new BusinessRuleError(), ErrorType.BusinessRule, DefaultErrorCodes.BusinessRule,
			DefaultMessage.BusinessRule
		],
		[new ConflictError(), ErrorType.Conflict, DefaultErrorCodes.Conflict, DefaultMessage.Conflict],
		[new FailureError(), ErrorType.Failure, DefaultErrorCodes.Failure, DefaultMessage.Failure],
		[new ForbiddenError(), ErrorType.Forbidden, DefaultErrorCodes.Forbidden, DefaultMessage.Forbidden],
		[new TimeoutError(), ErrorType.Timeout, DefaultErrorCodes.Timeout, DefaultMessage.Timeout],
		[
			new TooManyRequestsError(), ErrorType.TooManyRequests, DefaultErrorCodes.TooManyRequests,
			DefaultMessage.TooManyRequests
		],
		[
			new UnauthorizedError(), ErrorType.Unauthorized, DefaultErrorCodes.Unauthorized,
			DefaultMessage.Unauthorized
		],
		[new UnavailableError(), ErrorType.Unavailable, DefaultErrorCodes.Unavailable, DefaultMessage.Unavailable],
		[new ValidationError(), ErrorType.Validation, DefaultErrorCodes.Validation, DefaultMessage.Validation]
	];

	public static IEnumerable<object[]> CustomErrorTestCases =>
	[
		[new NotFoundError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new BusinessRuleError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new ConflictError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new FailureError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new ForbiddenError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new TimeoutError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new TooManyRequestsError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new UnauthorizedError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new UnavailableError("Custom message", "Custom.Code"), "Custom.Code", "Custom message"],
		[new ValidationError(message: "Custom message", code: "Custom.Code"), "Custom.Code", "Custom message"]
	];

	[Theory]
	[MemberData(nameof(ErrorTestCases))]
	public void Error_should_have_correct_default_values(Error error,
														 ErrorType expectedErrorType,
														 string expectedCode,
														 string expectedMessage)
	{
		error.Type.Should().Be(expectedErrorType);
		error.Code.Should().Be(expectedCode);
		error.Message.Should().Be(expectedMessage);
	}

	[Theory]
	[MemberData(nameof(CustomErrorTestCases))]
	public void Error_should_have_accept_custom_message_and_code(Error error,
																 string expectedCode,
																 string expectedMessage)
	{
		error.Code.Should().Be(expectedCode);
		error.Message.Should().Be(expectedMessage);
	}
}