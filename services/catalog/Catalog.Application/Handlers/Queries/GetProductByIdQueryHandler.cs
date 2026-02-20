using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository repository, ProductMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductByIdAsync(request.Id, cancellationToken);

        if (product == null)
            return Result.Fail<ProductResponseDto>($"Product with ID '{request.Id}' was not found");

        var response = _mapper.ToResponseDto(product);
        return Result.Ok(response);
    }
}
