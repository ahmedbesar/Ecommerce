using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Basket.Application.Handlers.Commands;

public sealed class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Result<ShoppingCartResponseDto>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly BasketMapper _mapper;

    public CreateBasketCommandHandler(IBasketRepository basketRepository, BasketMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    public async Task<Result<ShoppingCartResponseDto>> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = _mapper.ToEntity(request);

        var createdBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket, cancellationToken);

        if (createdBasket == null)
            return Result.Fail<ShoppingCartResponseDto>("Failed to create basket");

        return Result.Ok(_mapper.ToResponseDto(createdBasket));
    }
}
