using System.Linq.Expressions;
using Meeting.Domain.Primitives;

namespace Meeting.Persistence.Specifications;

public abstract class Specification<TEntity> where TEntity : Entity
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria) => Criteria = criteria;

    public bool IsSplitQuery { get; protected set; }

    public Expression<Func<TEntity, bool>>? Criteria { get; }

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpressions { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpressions { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        IncludeExpressions.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) =>
        OrderByExpressions = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) =>
        OrderByDescendingExpressions = orderByDescendingExpression;
}