namespace BuildingBlocks.Specification.Contracts;

/// <summary>
/// 
/// </summary>
public interface ISpecificationEvaluator
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    /// <param name="specification"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> query, IQuerySpecification<T> specification) where T : class;
}