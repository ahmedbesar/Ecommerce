using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using Riok.Mapperly.Abstractions;

namespace Basket.Application.Mappers;

[Mapper]
public partial class BasketMapper
{
    public partial ShoppingCartResponseDto ToResponseDto(ShoppingCart cart);
    public partial ShoppingCartItemResponseDto ToItemResponseDto(ShoppingCartItem item);
    public partial List<ShoppingCartItemResponseDto> ToItemResponseListDto(List<ShoppingCartItem> items);

    public partial ShoppingCart ToEntity(CreateBasketCommand command);
    public partial ShoppingCart ToEntity(UpdateBasketCommand command);
    public partial ShoppingCartItem ToItemEntity(ShoppingCartItemDto dto);
    public partial List<ShoppingCartItem> ToItemEntityList(List<ShoppingCartItemDto> dtos);

    public partial BasketCheckoutEvent Map(BasketCheckout checkout);
}
