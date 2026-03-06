namespace Basket.Core.Entities;

public class ShoppingCartItem
{
    public string ProductId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageFile { get; set; } = default!;
}
