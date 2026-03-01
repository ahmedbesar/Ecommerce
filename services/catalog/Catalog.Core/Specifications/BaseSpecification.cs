using System.Linq.Expressions;
using MongoDB.Driver;

namespace Catalog.Core.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T> where T : class
{
    public FilterDefinition<T> FilterDefinition { get; private set; } = Builders<T>.Filter.Empty;
    public Expression<Func<T, object>>? OrderByExpression { get; private set; }
    public Expression<Func<T, object>>? OrderByDescendingExpression { get; private set; }
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void ApplyFilter(FilterDefinition<T> filter)
    {
        FilterDefinition &= filter;
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderByExpression = orderBy;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescendingExpression = orderByDescending;
    }

    protected void ApplyPaging(int skip, int take)
    {
        IsPagingEnabled = true;
        Skip = skip;
        Take = take;
    }
}
