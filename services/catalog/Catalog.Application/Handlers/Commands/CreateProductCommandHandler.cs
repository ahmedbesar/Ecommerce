using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductResponseDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly ProductMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        ITypeRepository typeRepository,
        ProductMapper mapper)
    {
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _typeRepository = typeRepository;
        _mapper = mapper;
    }

    public async Task<Result<ProductResponseDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetBrandByIdAsync(request.BrandId, cancellationToken);
        if (brand == null)
            return Result.Fail<ProductResponseDto>($"Brand with ID '{request.BrandId}' not found");

        var type = await _typeRepository.GetTypeByIdAsync(request.TypeId, cancellationToken);
        if (type == null)
            return Result.Fail<ProductResponseDto>($"Type with ID '{request.TypeId}' not found");

        var product = _mapper.ToEntity(request);
        var createdProduct = await _productRepository.CreateProductAsync(product, cancellationToken);

        if (createdProduct == null)
            return Result.Fail<ProductResponseDto>("Failed to create product");

        return Result.Ok(_mapper.ToResponseDto(createdProduct));
    }
}
