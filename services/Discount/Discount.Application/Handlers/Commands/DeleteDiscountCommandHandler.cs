using Discount.Application.Commands;
using Discount.Core.Repositories;
using FluentResults;
using MediatR;

namespace Discount.Application.Handlers.Commands;

public sealed class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, Result>
{
    private readonly IDiscountRepository _discountRepository;

    public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<Result> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var deleted = await _discountRepository.DeleteDiscount(request.ProductName);

        if (!deleted)
            return Result.Fail("Failed to delete discount.");

        return Result.Ok();
    }
}
