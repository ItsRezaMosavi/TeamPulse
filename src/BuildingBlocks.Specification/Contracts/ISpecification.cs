using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool IsSatisfiedBy(T entity);
}