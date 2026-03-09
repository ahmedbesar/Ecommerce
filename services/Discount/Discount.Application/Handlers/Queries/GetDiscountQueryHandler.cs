using Discount.Application.Mappers;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Discount.Core.Repositories;
using FluentResults;
using MediatR;

namespace Discount.Application.Handlers.Queries;

public sealed class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, Result<CouponModel>>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly DiscountMapper _mapper;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository, DiscountMapper mapper)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async Task<Result<CouponModel>> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        var dto = _mapper.ToModel(coupon);
        return Result.Ok(dto);
    }
}
