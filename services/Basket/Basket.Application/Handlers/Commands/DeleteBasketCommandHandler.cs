using Basket.Application.Commands;
using Basket.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Basket.Application.Handlers.Commands;

public sealed class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Result>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Result> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _basketRepository.DeleteBasketAsync(request.UserName, cancellationToken);

        if (!deleted)
            return Result.Fail($"Basket for user '{request.UserName}' not found or could not be deleted");

        return Result.Ok();
    }
}
