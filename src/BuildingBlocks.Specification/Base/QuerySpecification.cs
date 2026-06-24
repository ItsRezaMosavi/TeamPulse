using System.Linq.Expressions;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Specification.Base;

public class QuerySpecification<T> : IQuerySpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = [];

    protected QuerySpecification()
    {
    }

    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    public bool IsAscending { get; protected set; }

    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;
    
    public int Skip { get; protected set; }
    public int Take { get; protected set; }
    public bool IsPagingEnabled { get; protected set; }


    protected void ApplyPaging(int skip, int take)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(skip, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(take, 0);
        
        
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy, bool isAscending = true)
    {
        OrderBy = orderBy;
        IsAscending = isAscending;
    }

    protected void ApplyInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }
}