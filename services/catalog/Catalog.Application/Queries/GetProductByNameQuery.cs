using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public sealed record GetProductByNameQuery(string Name) : IRequest<IEnumerable<ProductResponseDto>>;
}
