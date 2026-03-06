using StackExchange.Redis;

namespace Basket.Infrastructure.Data.Contexts;

public class BasketContext
{
    private readonly IConnectionMultiplexer _redis;

    public BasketContext(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public IDatabase Database => _redis.GetDatabase();
}
