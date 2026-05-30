namespace BuildingBlocks.Domain.Entities;

/// <summary>
/// Represents a domain entity with a generic identifier type.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier.</typeparam>
/// <remarks>
/// An entity is an object defined by its identity rather than its attributes.
/// This base class provides:
/// <list type="bullet">
/// <item><description>Identity-based equality comparison</description></item>
/// <item><description>Operator overloads for equality checks</description></item>
/// <item><description>Protection against comparison of entities with default (unassigned) IDs</description></item>
/// </list>
/// 
/// Entities with default/unassigned IDs are never considered equal to any other entity,
/// including themselves, to prevent incorrect matches before persistence.
/// </remarks>
public abstract class Entity<TId>
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    /// <value>The entity's identifier of type <typeparamref name="TId"/>.</value>
    /// <remarks>
    /// The protected setter ensures that only derived classes can modify the ID,
    /// typically during construction or when assigned by a repository.
    /// </remarks>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Determines whether the specified object is equal to this entity.
    /// </summary>
    /// <param name="obj">The object to compare with this entity.</param>
    /// <returns>
    /// <c>true</c> if the specified object is an entity of the same type with the same ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Equality is determined by:
    /// <list type="bullet">
    /// <item><description>Both objects must be non-null and of the exact same type</description></item>
    /// <item><description>Reference equality returns true immediately</description></item>
    /// <item><description>Entities with default IDs are never equal (prevents false matches before persistence)</description></item>
    /// <item><description>Non-default IDs are compared using the default equality comparer</description></item>
    /// </list>
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (EqualityComparer<TId>.Default.Equals(Id, default) ||
            EqualityComparer<TId>.Default.Equals(other.Id, default))
            return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// Returns a hash code for this entity based on its ID.
    /// </summary>
    /// <returns>A hash code for the current entity.</returns>
    /// <remarks>
    /// Warning: The hash code will change when the ID is assigned after construction.
    /// Use caution when storing entities with unassigned IDs in hash-based collections.
    /// </remarks>
    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id!);
    }

    /// <summary>
    /// Determines whether two entity instances are equal.
    /// </summary>
    /// <param name="left">The first entity to compare, or null.</param>
    /// <param name="right">The second entity to compare, or null.</param>
    /// <returns>
    /// <c>true</c> if both entities are null, or if they have the same type and ID;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two entity instances are not equal.
    /// </summary>
    /// <param name="left">The first entity to compare, or null.</param>
    /// <param name="right">The second entity to compare, or null.</param>
    /// <returns>
    /// <c>true</c> if one entity is null and the other is not, or if they have different types or IDs;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}
