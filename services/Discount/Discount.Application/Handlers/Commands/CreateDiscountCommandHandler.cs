using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Application.Responses;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using FluentResults;
using MediatR;

namespace Discount.Application.Handlers.Commands;

public sealed class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, Result<CouponResponseDto>>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly DiscountMapper _mapper;

    public CreateDiscountCommandHandler(IDiscountRepository discountRepository, DiscountMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<Result<CouponResponseDto>> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            ProductName = request.ProductName,
            Description = request.Description,
            Amount = request.Amount
        };

        var created = await _discountRepository.CreateDiscount(coupon);

        if (!created)
            return Result.Fail("Failed to create discount.");

        var dto = _mapper.ToDto(coupon);
        return Result.Ok(dto);
    }
}
