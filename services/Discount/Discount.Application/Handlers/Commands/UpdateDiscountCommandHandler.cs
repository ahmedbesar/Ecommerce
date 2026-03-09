using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Grpc.Protos;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using FluentResults;
using MediatR;

namespace Discount.Application.Handlers.Commands;

public sealed class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, Result<CouponModel>>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly DiscountMapper _mapper;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository, DiscountMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<Result<CouponModel>> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            Id = request.Id,
            ProductName = request.ProductName,
            Description = request.Description,
            Amount = request.Amount
        };

        var updated = await _discountRepository.UpdateDiscount(coupon);

        if (!updated)
            return Result.Fail("Failed to update discount.");

        var dto = _mapper.ToModel(coupon);
        return Result.Ok(dto);
    }
}
