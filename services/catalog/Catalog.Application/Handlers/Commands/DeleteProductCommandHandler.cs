using Catalog.Application.Commands;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetProductByIdAsync(request.Id, cancellationToken);

        if (existing == null)
            return Result.Fail($"Product with ID '{request.Id}' was not found");

        var success = await _repository.DeleteProductAsync(request.Id, cancellationToken);

        return success ? Result.Ok() : Result.Fail("Failed to delete product");
    }
}
