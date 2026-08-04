using BuildingBlocks.Results.Errors;
using FluentAssertions;

namespace BuildingBlocks.Results.Tests.Errors;

public class ValidationErrorTests
{
	[Fact]
	public void ValidationError_should_have_null_property_and_validation_code_by_default()
	{
		var validationError = new ValidationError();

		validationError.Property.Should().BeNull();
		validationError.ValidationCode.Should().BeNull();
	}

	[Fact]
	public void ValidationError_should_set_property_and_validation_code()
	{
		var property = "TestProperty";
		var code = "TestCode";

		var validationError = new ValidationError(property, validationCode: code);

		validationError.Property.Should().Be(code);
		validationError.ValidationCode.Should().Be(code);
	}

	[Fact]
	public void ValidationError_should_allow_null_property_and_validation_code()
	{
		var validationError = new ValidationError(null, validationCode: null);

		validationError.Property.Should().BeNull();
		validationError.ValidationCode.Should().BeNull();
	}
}