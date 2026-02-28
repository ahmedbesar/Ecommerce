using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly ProductMapper _mapper;

    public UpdateProductCommandHandler(
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

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _productRepository.GetProductByIdAsync(request.Id, cancellationToken);

        if (existing == null)
            return Result.Fail($"Product with ID '{request.Id}' was not found");

        var brand = await _brandRepository.GetBrandByIdAsync(request.BrandId, cancellationToken);
        if (brand == null)
            return Result.Fail($"Brand with ID '{request.BrandId}' not found");

        var type = await _typeRepository.GetTypeByIdAsync(request.TypeId, cancellationToken);
        if (type == null)
            return Result.Fail($"Type with ID '{request.TypeId}' not found");

        var product = _mapper.ToEntity(request);
        var success = await _productRepository.UpdateProductAsync(product, cancellationToken);

        return success ? Result.Ok() : Result.Fail("Failed to update product");
    }
}
