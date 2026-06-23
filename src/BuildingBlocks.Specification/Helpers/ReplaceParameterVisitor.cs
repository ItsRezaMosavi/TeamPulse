using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Helpers;

public class ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == oldParameter ? newParameter : base.VisitParameter(node);
    }
}