using Catalog.Application.Responses;
using FluentResults;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetProductByIdQuery() : IRequest<Result<ProductResponseDto>>
{
    public string Id { get; set; }
}
