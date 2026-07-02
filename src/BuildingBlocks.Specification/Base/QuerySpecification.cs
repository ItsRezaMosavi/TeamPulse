using System.Linq.Expressions;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Specification.Base;

public class QuerySpecification<T> : IQuerySpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = [];

    /// <summary>
    /// Initializes a new instance of the QuerySpecification class.
    /// </summary>
    protected QuerySpecification()
    {
    }

    /// <summary>
    /// Gets the LINQ expression that defines the filtering criteria for the query.
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    /// <summary>
    /// Gets the LINQ expression that defines the ordering of results.
    /// </summary>
    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether the ordering is ascending.
    /// </summary>
    public bool IsAscending { get; protected set; }

    /// <summary>
    /// Gets the collection of include expressions for eager loading related entities.
    /// </summary>
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

    /// <summary>
    /// Gets the number of elements to skip in the result set for paging.
    /// </summary>
    public int Skip { get; protected set; }

    /// <summary>
    /// Gets the number of elements to take in the result set for paging.
    /// </summary>
    public int Take { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether paging is enabled for this specification.
    /// </summary>
    public bool IsPagingEnabled { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether split queries should be used for loading related entities.
    /// </summary>
    public bool IsSplitQuery { get; protected set; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void ApplyPaging(int skip, int take)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(skip, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(take, 0);


        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orderBy"></param>
    /// <param name="isAscending"></param>
    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy, bool isAscending = true)
    {
        OrderBy = orderBy;
        IsAscending = isAscending;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="includeExpression"></param>
    protected void ApplyInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }

    /// <summary>
    /// Enables split query mode for loading related entities.
    /// </summary>
    protected void ApplySplitQuery()
    {
        IsSplitQuery = true;
    }
}