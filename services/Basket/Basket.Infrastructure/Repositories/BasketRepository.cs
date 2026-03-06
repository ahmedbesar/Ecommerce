using Basket.Core.Entities;
using Basket.Core.Interfaces;
using Basket.Infrastructure.Data.Contexts;
using System.Text.Json;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly BasketContext _context;

    public BasketRepository(BasketContext context)
    {
        _context = context;
    }

    public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var basket = await _context.Database.StringGetAsync(userName);

        if (basket.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<ShoppingCart>(basket.ToString());
    }

    public async Task<ShoppingCart?> CreateOrUpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var serialized = JsonSerializer.Serialize(basket);
        var created = await _context.Database.StringSetAsync(basket.UserName, serialized);

        if (!created)
            return null;

        return await GetBasketAsync(basket.UserName, cancellationToken);
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _context.Database.KeyDeleteAsync(userName);
    }
}
