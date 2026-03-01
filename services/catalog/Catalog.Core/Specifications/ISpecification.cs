using System.Linq.Expressions;
using MongoDB.Driver;

namespace Catalog.Core.Specifications;

public interface ISpecification<T> where T : class
{
    FilterDefinition<T> FilterDefinition { get; }
    Expression<Func<T, object>>? OrderByExpression { get; }
    Expression<Func<T, object>>? OrderByDescendingExpression { get; }
    int Skip { get; }
    int Take { get; }
    bool IsPagingEnabled { get; }
}
