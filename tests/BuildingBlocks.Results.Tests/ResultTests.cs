using BuildingBlocks.Results.Errors;
using FluentAssertions;

namespace BuildingBlocks.Results.Tests;

public class ResultTests
{
	[Fact]
	public void Success_should_create_successful_result()
	{
		var result = Result.Success();

		result.IsSuccess.Should().BeTrue();
		result.IsFailure.Should().BeFalse();
		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Failure_should_throw_when_no_errors_are_provided()
	{
		var action = () => Result.Failure();

		action.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void Failure_should_create_failure_result_with_errors()
	{
		Error[] errors = [new NotFoundError(), new FailureError()];

		var result = Result.Failure(errors);

		result.IsSuccess.Should().BeFalse();
		result.IsFailure.Should().BeTrue();
		result.Errors.Should().HaveCount(errors.Length).And.BeEqualTo(errors);
	}


	[Fact]
	public void Error_should_implicitly_convert_to_failed_result()
	{
		var error = new NotFoundError();

		Result result = error;

		result.IsSuccess.Should().BeFalse();
		result.IsFailure.Should().BeTrue();
		result.Errors.Should().ContainSingle().Which.Should().Be(error);
	}

	[Fact]
	public void Errors_should_implicitly_convert_to_failed_result()
	{
		Error[] errors = [new NotFoundError(), new FailureError()];

		Result result = errors;

		result.IsSuccess.Should().BeFalse();
		result.IsFailure.Should().BeTrue();
		result.Errors.Should().BeEqualTo(errors);
	}
}