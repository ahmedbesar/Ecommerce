using Catalog.Application.Responses;
using FluentResults;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetProductByNameQuery() : IRequest<Result<IEnumerable<ProductResponseDto>>>
{
    public string Name { get; set; }
}
