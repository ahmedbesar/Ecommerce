using Basket.Application.Responses;
using FluentResults;
using MediatR;

namespace Basket.Application.Commands;

public sealed record UpdateBasketCommand : IRequest<Result<ShoppingCartResponseDto>>
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItemDto> Items { get; set; } = new();
}
