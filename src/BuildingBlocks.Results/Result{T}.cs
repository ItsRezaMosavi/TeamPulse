namespace BuildingBlocks.Results;

/// <summary>
/// Represents the result of an operation that returns a value.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation.</typeparam>
/// <remarks>
/// The Result pattern is used to encapsulate operation outcomes without using exceptions
/// for control flow. A result can be either successful (with a value) or failed (with errors).
/// 
/// Usage examples:
/// <code>
/// // Success case with value
/// var result = Result&lt;User&gt;.Success(new User { Id = 1, Name = "John" });
/// 
/// // Failure case
/// var result = Result&lt;User&gt;.Failure(new NotFoundError("User not found"));
/// 
/// // Using implicit conversion from value
/// Result&lt;int&gt; result = 42;
/// 
/// // Accessing the value
/// if (result.IsSuccess) { var value = result.Value; }
/// </code>
/// </remarks>
public sealed class Result<T> : ResultBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="errors">The collection of errors if the operation failed.</param>
    /// <param name="value">The value returned if the operation succeeded.</param>
    public Result(bool isSuccess, Error[] errors, T? value) : base(isSuccess, errors)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value returned by the operation, or default if failed.
    /// </summary>
    /// <value>The operation's result value if successful; otherwise, default(T).</value>
    /// <remarks>
    /// Accessing Value when IsSuccess is false will return the default value for the type.
    /// Always check IsSuccess before using the Value property.
    /// </remarks>
    public T? Value { get; private set; }

    /// <summary>
    /// Creates a successful result instance with the specified value.
    /// </summary>
    /// <param name="value">The value to include in the successful result.</param>
    /// <returns>A <see cref="Result{T}"/> indicating success with the provided value.</returns>
    public static Result<T> Success(T value) => new(true, [], value);

    /// <summary>
    /// Creates a failed result instance with the specified errors.
    /// </summary>
    /// <param name="errors">One or more errors describing the failure.</param>
    /// <returns>A <see cref="Result{T}"/> indicating failure.</returns>
    public static Result<T> Failure(params Error[] errors) => new(false, errors, default);

    /// <summary>
    /// Implicitly converts a single error to a failed result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the error.</returns>
    public static implicit operator Result<T>(Error error) => Failure([error]);

    /// <summary>
    /// Implicitly converts an array of errors to a failed result.
    /// </summary>
    /// <param name="errors">The errors to convert.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the errors.</returns>
    public static implicit operator Result<T>(Error[] errors) => Failure(errors);

    /// <summary>
    /// Implicitly converts a value to a successful result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the value.</returns>
    public static implicit operator Result<T>(T value) => Success(value);
}