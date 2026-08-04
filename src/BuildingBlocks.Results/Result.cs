namespace BuildingBlocks.Results;

/// <summary>
/// Represents the result of an operation that does not return a value.
/// </summary>
/// <remarks>
/// The Result pattern is used to encapsulate operation outcomes without using exceptions
/// for control flow. A result can be either successful or failed, with failures containing
/// one or more error objects describing what went wrong.
/// 
/// Usage examples:
/// <code>
/// // Success case
/// var result = Result.Success();
/// 
/// // Failure case with single error
/// var result = Result.Failure(new NotFoundError("Item not found"));
/// 
/// // Using implicit conversion
/// Result result = new NotFoundError("Item not found");
/// 
/// // Checking result
/// if (result.IsSuccess) { /* handle success */ }
/// if (result.IsFailure) { /* handle failure */ }
/// </code>
/// </remarks>
public sealed class Result : ResultBase
{
	/// <summary>
	///     Initializes a new instance of the <see cref="Result" /> class.
	/// </summary>
	/// <param name="isSuccess">Indicates whether the operation was successful.</param>
	/// <param name="errors">The collection of errors if the operation failed.</param>
	public Result(bool isSuccess, Error[] errors) : base(isSuccess, errors)
	{
	}

	/// <summary>
	///     Creates a successful result instance.
	/// </summary>
	/// <returns>A <see cref="Result" /> indicating success.</returns>
	public static Result Success()
	{
		return new Result(true, []);
	}

	/// <summary>
	///     Creates a failed result instance with the specified errors.
	/// </summary>
	/// <param name="errors">One or more errors describing the failure.</param>
	/// <returns>A <see cref="Result" /> indicating failure.</returns>
	public static Result Failure(params Error[] errors)
	{
		ArgumentNullException.ThrowIfNull(errors);

		if (errors.Length == 0) throw new ArgumentException("Errors cannot be empty", nameof(errors));

		return new Result(false, errors);
	}

	/// <summary>
	///     Implicitly converts an array of errors to a failed result.
	/// </summary>
	/// <param name="errors">The errors to convert.</param>
	/// <returns>A failed <see cref="Result" /> containing the errors.</returns>
	public static implicit operator Result(Error[] errors)
	{
		return Failure(errors);
	}

	/// <summary>
	///     Implicitly converts a single error to a failed result.
	/// </summary>
	/// <param name="error">The error to convert.</param>
	/// <returns>A failed <see cref="Result" /> containing the error.</returns>
	public static implicit operator Result(Error error)
	{
		return Failure(error);
	}
}