using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Basket.Application.Handlers.Commands;

public sealed class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, Result<ShoppingCartResponseDto>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly BasketMapper _mapper;

    public UpdateBasketCommandHandler(IBasketRepository basketRepository, BasketMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    public async Task<Result<ShoppingCartResponseDto>> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = _mapper.ToEntity(request);
        var updatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket, cancellationToken);

        if (updatedBasket == null)
            return Result.Fail<ShoppingCartResponseDto>("Failed to update basket");

        return Result.Ok(_mapper.ToResponseDto(updatedBasket));
    }
}
