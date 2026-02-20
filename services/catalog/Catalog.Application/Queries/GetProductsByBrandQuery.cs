using Catalog.Application.Responses;
using FluentResults;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetProductsByBrandQuery() : IRequest<Result<IEnumerable<ProductResponseDto>>>
{
    public string BrandName { get; set; }
}
