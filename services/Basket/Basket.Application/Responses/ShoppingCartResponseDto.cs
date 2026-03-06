namespace Basket.Application.Responses;

public sealed record ShoppingCartResponseDto
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItemResponseDto> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}
