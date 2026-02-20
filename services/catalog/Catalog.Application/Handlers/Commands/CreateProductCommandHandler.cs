using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductResponseDto>>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;

    public CreateProductCommandHandler(IProductRepository repository, ProductMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ProductResponseDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.ToEntity(request);
        var createdProduct = await _repository.CreateProductAsync(product, cancellationToken);

        if (createdProduct == null)
            return Result.Fail<ProductResponseDto>("Failed to create product");

        return Result.Ok(_mapper.ToResponseDto(createdProduct));
    }
}
