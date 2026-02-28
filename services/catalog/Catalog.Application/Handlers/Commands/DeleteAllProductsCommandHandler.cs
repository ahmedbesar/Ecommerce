using Catalog.Application.Commands;
using Catalog.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public sealed class DeleteAllProductsCommandHandler : IRequestHandler<DeleteAllProductsCommand, Result>
{
    private readonly IProductRepository _repository;

    public DeleteAllProductsCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteAllProductsCommand request, CancellationToken cancellationToken)
    {
        var success = await _repository.DeleteAllProductsAsync(cancellationToken);
        return success ? Result.Ok() : Result.Fail("Failed to delete all products");
    }
}
