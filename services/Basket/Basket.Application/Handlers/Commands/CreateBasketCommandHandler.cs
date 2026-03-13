using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.GrpcServices;
using Basket.Application.Responses;
using Basket.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Basket.Application.Handlers.Commands;

public sealed class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Result<ShoppingCartResponseDto>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly BasketMapper _mapper;
    private readonly DiscountGrpcService _discountGrpcService;

    public CreateBasketCommandHandler(IBasketRepository basketRepository, BasketMapper mapper, DiscountGrpcService discountGrpcService)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
        _discountGrpcService = discountGrpcService;
    }

    public async Task<Result<ShoppingCartResponseDto>> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = _mapper.ToEntity(request);

        foreach (var item in basket.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            if (coupon != null && coupon.Amount > 0)
            {
                item.Price -= coupon.Amount;
            }
        }

        var createdBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket, cancellationToken);

        if (createdBasket == null)
            return Result.Fail<ShoppingCartResponseDto>("Failed to create basket");

        return Result.Ok(_mapper.ToResponseDto(createdBasket));
    }
}
