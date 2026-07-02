namespace BuildingBlocks.Specification.Contracts;

/// <summary>
    /// Evaluates specifications against IQueryable sources to apply filtering, ordering, and other query configurations.
    /// </summary>
public interface ISpecificationEvaluator
{
    /// <summary>
    /// Evaluates specifications against IQueryable sources to apply filtering, ordering, and other query configurations.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="specification"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IQueryable<T> GetQuery<T>(IQueryable<T> query, IQuerySpecification<T> specification) where T : class;
}