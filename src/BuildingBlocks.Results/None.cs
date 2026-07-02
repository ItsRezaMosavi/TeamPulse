namespace BuildingBlocks.Results;

/// <summary>
/// Represents a unit type indicating the absence of a value.
/// </summary>
/// <remarks>
/// This struct is used as a placeholder return type for operations that don't return a value
/// but need to use the <see cref="Result{T}"/> pattern. It's equivalent to void in a Result context.
/// 
/// Usage example:
/// <code>
/// Result&lt;None&gt; result = Result&lt;None&gt;.Success(None.Value);
/// </code>
/// </remarks>
public readonly record struct None
{
    /// <summary>
    /// Gets the singleton instance of <see cref="None"/>.
    /// </summary>
    public static readonly None Value = default;
}