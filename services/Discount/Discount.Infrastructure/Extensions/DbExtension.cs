using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Discount DB Migration Started");

                await ApplyMigrationsAsync(configuration, logger);

                logger.LogInformation("Discount DB Migration Completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            return host;
        }

        private static async Task ApplyMigrationsAsync(
            IConfiguration configuration,
            ILogger logger)
        {
            var retry = 5;

            while (retry > 0)
            {
                try
                {
                    var connectionString =
                        configuration.GetValue<string>("DatabaseSettings:ConnectionString");

                    await using var connection = new NpgsqlConnection(connectionString);
                    await connection.OpenAsync();

                    await using var cmd = connection.CreateCommand();

                    // Create table if not exists
                    cmd.CommandText = """
                        CREATE TABLE IF NOT EXISTS Coupon
                        (
                            Id SERIAL PRIMARY KEY,
                            ProductName VARCHAR(500) NOT NULL,
                            Description TEXT,
                            Amount INTEGER
                        );
                        """;

                    await cmd.ExecuteNonQueryAsync();

                    // Seed data (only if not exists)
                    cmd.CommandText = """
                        INSERT INTO Coupon(ProductName, Description, Amount)
                        SELECT 'Egypt Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500
                        WHERE NOT EXISTS (
                            SELECT 1 FROM Coupon WHERE ProductName = 'Egypt Adidas Quick Force Indoor Badminton Shoes'
                        );
                        """;

                    await cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = """
                        INSERT INTO Coupon(ProductName, Description, Amount)
                        SELECT 'PowerFit 19 FH Rubber Spike Cricket Shoes', 'Racquet Discount', 700
                        WHERE NOT EXISTS (
                            SELECT 1 FROM Coupon WHERE ProductName = 'PowerFit 19 FH Rubber Spike Cricket Shoes'
                        );
                        """;

                    await cmd.ExecuteNonQueryAsync();

                    break;
                }
                catch (Exception ex)
                {
                    retry--;

                    logger.LogWarning(ex,
                        "Error during database migration. Retrying... {Retry}",
                        retry);

                    if (retry == 0)
                        throw;

                    await Task.Delay(2000);
                }
            }
        }
    }
}