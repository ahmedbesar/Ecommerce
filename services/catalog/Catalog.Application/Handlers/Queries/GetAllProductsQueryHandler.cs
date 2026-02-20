using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<IEnumerable<ProductResponseDto>>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository repository, ProductMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ProductResponseDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllProductsAsync(cancellationToken);
        var response = _mapper.ToResponseListDto(products);
        return Result.Ok(response);
    }
}
