using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Interfaces;
using FluentResults;
using MediatR;

namespace Basket.Application.Handlers.Queries;

public sealed class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<ShoppingCartResponseDto>>
{
    private readonly IBasketRepository _basketRepository;
    private readonly BasketMapper _mapper;

    public GetBasketQueryHandler(IBasketRepository basketRepository, BasketMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    public async Task<Result<ShoppingCartResponseDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasketAsync(request.UserName, cancellationToken);

        if (basket == null)
            return Result.Ok(_mapper.ToResponseDto(new ShoppingCart(request.UserName)));

        return Result.Ok(_mapper.ToResponseDto(basket));
    }
}
