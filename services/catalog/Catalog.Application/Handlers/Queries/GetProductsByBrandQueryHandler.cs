using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetProductsByBrandQueryHandler : IRequestHandler<GetProductsByBrandQuery, Result<IEnumerable<ProductResponseDto>>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;

    public GetProductsByBrandQueryHandler(IProductRepository repository, ProductMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllProductsByBrandAsync(request.BrandName, cancellationToken);
        var response = _mapper.ToResponseListDto(products);
        return Result.Ok(response);
    }
}
