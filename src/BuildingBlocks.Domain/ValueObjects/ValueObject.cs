namespace BuildingBlocks.Domain.ValueObjects;

/// <summary>
/// Base class for all value objects in the domain layer.
/// </summary>
/// <remarks>
/// Value objects are immutable objects that represent descriptive aspects of the domain
/// with no conceptual identity. They are defined by their attributes rather than an ID.
/// 
/// Key characteristics:
/// <list type="bullet">
/// <item><description>Equality is based on all property values, not reference equality</description></item>
/// <item><description>Should be immutable - once created, their state cannot change</description></item>
/// <item><description>Can be used interchangeably when all attributes match</description></item>
/// </list>
/// 
/// To create a value object, inherit from this class and implement <see cref="GetEqualityComponents"/>
/// to return all properties that participate in equality comparison.
/// </remarks>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the sequence of components that define this value object's equality.
    /// </summary>
    /// <returns>An enumerable of objects that should be compared for equality.</returns>
    /// <remarks>
    /// Derived classes must override this method to return all properties/fields that
    /// define the value object's identity. The order of components matters for comparison.
    /// 
    /// Example:
    /// <code>
    /// protected override IEnumerable&lt;object?&gt; GetEqualityComponents()
    /// {
    ///     yield return Street;
    ///     yield return City;
    ///     yield return PostalCode;
    /// }
    /// </code>
    /// </remarks>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object equals this value object.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified object is a value object of the same type
    /// and all equality components match; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other)
            return false;

        if (GetType() != other.GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Returns a hash code for this value object based on its equality components.
    /// </summary>
    /// <returns>A hash code computed from all equality components.</returns>
    /// <remarks>
    /// The hash code is computed using <see cref="HashCode"/> which provides
    /// good distribution and handles null values automatically.
    /// </remarks>
    public override int GetHashCode()
    {
       var hash = new HashCode();
       
       foreach (var component in GetEqualityComponents())
           hash.Add(component);
       
       return hash.ToHashCode();
    }

    /// <summary>
    /// Determines whether two value objects are equal.
    /// </summary>
    /// <param name="left">The first value object to compare, or null.</param>
    /// <param name="right">The second value object to compare, or null.</param>
    /// <returns>
    /// <c>true</c> if both are null, or if they have the same type and all equality components match;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two value objects are not equal.
    /// </summary>
    /// <param name="left">The first value object to compare, or null.</param>
    /// <param name="right">The second value object to compare, or null.</param>
    /// <returns>
    /// <c>true</c> if one is null and the other is not, or if they differ in type or any equality component;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}