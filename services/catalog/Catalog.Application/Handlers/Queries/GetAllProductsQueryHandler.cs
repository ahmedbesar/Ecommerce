using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using Catalog.Core.Specifications;
using Catalog.Core.Specifications.Products;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<Pagination<ProductResponseDto>>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository repository, ProductMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<Pagination<ProductResponseDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductSpecification(request.SpecParams);

        var paginatedProducts = await _repository.GetProductsAsync(spec, cancellationToken);

        var response = _mapper.ToResponseListDto(paginatedProducts.Data);

        var pagination = new Pagination<ProductResponseDto>(
            paginatedProducts.PageIndex,
            paginatedProducts.PageSize,
            paginatedProducts.TotalCount,
            response
        );

        return Result.Ok(pagination);
    }
}
