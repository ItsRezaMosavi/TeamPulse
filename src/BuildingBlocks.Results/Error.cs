using BuildingBlocks.Results.Errors;

namespace BuildingBlocks.Results;

/// <summary>
/// Represents an error that occurred during an operation.
/// </summary>
/// <remarks>
/// Errors are immutable objects that encapsulate:
/// <list type="bullet">
/// <item><description><see cref="Type"/> - The category of error (e.g., NotFound, ValidationError)</description></item>
/// <item><description><see cref="Code"/> A stable string identifier for the error.</description></item>
/// <item><description><see cref="Message"/> - A human-readable description</description></item>
/// </list>
/// 
/// Errors are typically created using the factory methods in specific error classes
/// like <see cref="NotFoundError"/>, <see cref="ValidationError"/>, etc.
/// </remarks>
public class Error
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Error"/> class.
	/// </summary>
	/// <param name="type">The type/category of the error.</param>
	/// <param name="code">A stable string identifier for the error.</param>
	/// <param name="message">A human-readable message describing the error.</param>
	/// <remarks>
	/// This constructor is internal to ensure errors are created through
	/// the appropriate factory methods in derived classes.
	/// </remarks>
	internal Error(ErrorType type, string code, string message)
	{
		Type = type;
		Message = message;
		Code = code;
	}

	/// <summary>
	/// Gets the unique error code.
	/// </summary>
	/// <value>
	/// A stable string identifier that can be used for
	/// programmatic error handling, logging, localization,
	/// and client-side error mapping.
	/// </value>
	public string Code { get; }

	/// <summary>
	/// Gets the type or category of this error.
	/// </summary>
	/// <value>An <see cref="ErrorType"/> value indicating the error category.</value>
	public ErrorType Type { get; }

	/// <summary>
	/// Gets the human-readable message describing the error.
	/// </summary>
	/// <value>A descriptive message suitable for logging or display.</value>
	public string Message { get; }
}