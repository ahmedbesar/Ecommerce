using Basket.Application.Responses;
using FluentResults;
using MediatR;

namespace Basket.Application.Commands;

public sealed record CreateBasketCommand : IRequest<Result<ShoppingCartResponseDto>>
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItemDto> Items { get; set; } = new();
}

public sealed record ShoppingCartItemDto
{
    public string ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageFile { get; set; } = default!;
}
