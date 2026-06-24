using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

public interface IQuerySpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }

    Expression<Func<T, object>>? OrderBy { get; }

    public bool IsAscending { get; }

    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

    int Skip { get; }

    int Take { get; }

    bool IsPagingEnabled { get; }
}