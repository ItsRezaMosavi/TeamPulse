using System.Linq.Expressions;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Specification.Base;

public class QuerySpecification<T> : IQuerySpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = [];

    /// <summary>
    /// 
    /// </summary>
    protected QuerySpecification()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAscending { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

    /// <summary>
    /// 
    /// </summary>
    public int Skip { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public int Take { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsPagingEnabled { get; protected set; }

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    protected void ApplySplitQuery()
    {
        IsSplitQuery = true;
    }
}