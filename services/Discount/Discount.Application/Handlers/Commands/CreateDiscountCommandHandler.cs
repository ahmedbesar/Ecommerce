using Discount.Application.Commands;
using Discount.Application.Mappers;
using Discount.Grpc.Protos;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using FluentResults;
using MediatR;

namespace Discount.Application.Handlers.Commands;

public sealed class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, Result<CouponModel>>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly DiscountMapper _mapper;

    public CreateDiscountCommandHandler(IDiscountRepository discountRepository, DiscountMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<Result<CouponModel>> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
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

        var dto = _mapper.ToModel(coupon);
        return Result.Ok(dto);
    }
}
