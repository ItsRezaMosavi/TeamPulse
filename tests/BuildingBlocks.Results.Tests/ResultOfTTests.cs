using BuildingBlocks.Results.Errors;
using FluentAssertions;

namespace BuildingBlocks.Results.Tests;

public class ResultOfTTests
{
	[Fact]
	public void Success_should_create_successful_result_with_value()
	{
		const int value = 42;

		var result = Result<int>.Success(value);


		result.IsSuccess.Should().BeTrue();
		result.IsFailure.Should().BeFalse();
		result.Value.Should().Be(value);
		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Failure_should_create_failure_result_with_errors()
	{
		var error = new NotFoundError();

		var result = Result<int>.Failure(error);

		result.IsSuccess.Should().BeFalse();
		result.IsFailure.Should().BeTrue();
		result.Value.Should().Be(default);
		result.Errors.Should().ContainSingle().Which.Should().Be(error);
	}


	[Fact]
	public void Failure_should_throw_when_no_errors_are_provided()
	{
		var action = () => Result<int>.Failure();

		action.Should().Throw<ArgumentException>();
	}

	[Fact]
	public void Error_should_implicitly_convert_to_failed_result()
	{
		var error = new NotFoundError();

		Result<int> result = error;

		result.IsSuccess.Should().BeFalse();
		result.IsFailure.Should().BeTrue();
		result.Value.Should().Be(default);
		result.Errors.Should().ContainSingle().Which.Should().Be(error);
	}

	[Fact]
	public void Errors_should_implicitly_convert_to_failed_result()
	{
		Error[] errors = [new NotFoundError(), new ConflictError()];

		Result<int> result = errors;

		result.IsSuccess.Should().BeFalse();
		result.IsFailure.Should().BeTrue();
		result.Value.Should().Be(default);
		result.Errors.Should().HaveCount(errors.Length).And.BeEqualTo(errors);
	}


	[Fact]
	public void Value_should_implicitly_convert_to_successful_result()
	{
		const int value = 42;

		Result<int> result = value;

		result.IsSuccess.Should().BeTrue();
		result.IsFailure.Should().BeFalse();
		result.Value.Should().Be(value);
		result.Errors.Should().BeEmpty();
	}
}