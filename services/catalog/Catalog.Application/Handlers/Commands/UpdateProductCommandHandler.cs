using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IProductRepository _repository;
    private readonly ProductMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository repository, ProductMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetProductByIdAsync(request.Id, cancellationToken);

        if (existing == null)
            return Result.Fail($"Product with ID '{request.Id}' was not found");

        var product = _mapper.ToEntity(request);
        var success = await _repository.UpdateProductAsync(product, cancellationToken);

        return success ? Result.Ok() : Result.Fail("Failed to update product");
    }
}
