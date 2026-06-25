namespace BuildingBlocks.Results;

/// <summary>
/// Base class for all result types providing common success/failure state and error information.
/// </summary>
/// <remarks>
/// This abstract class serves as the foundation for both <see cref="Result"/> and <see cref="Result{T}"/>.
/// It encapsulates:
/// <list type="bullet">
/// <item><description>Success/failure state via <see cref="IsSuccess"/> and <see cref="IsFailure"/></description></item>
/// <item><description>Error collection when operations fail</description></item>
/// </list>
/// 
/// The Result pattern provides a type-safe way to handle operation outcomes without relying
/// on exceptions for control flow, making error handling explicit and predictable.
/// </remarks>
public abstract class ResultBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultBase"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="errors">The collection of errors if the operation failed.</param>
    protected ResultBase(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    /// <value><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <value><c>true</c> if the operation failed; otherwise, <c>false</c>.</value>
    /// <remarks>This is the inverse of <see cref="IsSuccess"/>.</remarks>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the collection of errors that occurred during the operation.
    /// </summary>
    /// <value>An array of <see cref="Error"/> objects describing failures, or null if successful.</value>
    /// <remarks>
    /// When <see cref="IsSuccess"/> is true, this property may be null or empty.
    /// When <see cref="IsFailure"/> is true, this array contains one or more errors.
    /// </remarks>
    public IReadOnlyCollection<Error> Errors { get; }
}