using Catalog.Application.Responses;
using Catalog.Core.Specifications;
using Catalog.Core.Specifications.Products;
using FluentResults;
using MediatR;

namespace Catalog.Application.Queries;

public sealed record GetAllProductsQuery(ProductSpecificationParams SpecParams)
    : IRequest<Result<Pagination<ProductResponseDto>>>;
