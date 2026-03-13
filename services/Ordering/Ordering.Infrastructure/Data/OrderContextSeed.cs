using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeded database associated with context {DbContextName}", nameof(OrderContext));
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "ahmed",
                    FirstName = "Ahmed",
                    LastName = "Besar",
                    EmailAddress = "ahmed@example.com",
                    AddressLine = "Cairo",
                    Country = "Egypt",
                    State = "Cairo",
                    ZipCode = "11511",
                    TotalPrice = 750,
                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    Expiration = "12/28",
                    Cvv = "123",
                    PaymentMethod = 1
                }
            };
        }
    }
}
