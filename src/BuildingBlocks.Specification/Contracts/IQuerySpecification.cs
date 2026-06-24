using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

public interface IQuerySpecification<T>
{
    /// <summary>
    /// 
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// 
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAscending { get; }

    /// <summary>
    /// 
    /// </summary>
    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// 
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// 
    /// </summary>
    int Take { get; }

    /// <summary>
    /// 
    /// </summary>
    bool IsPagingEnabled { get; }
    
    /// <summary>
    /// 
    /// </summary>
    bool IsSplitQuery { get; }
}