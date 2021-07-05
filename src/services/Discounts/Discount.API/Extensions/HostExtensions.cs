using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            using (var scrope = host.Services.CreateScope())
            {
                var services = scrope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    logger.LogInformation("Migrating Postresql database.");
                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(ID SERIAL PRIMARY KEY NOT NULL,
                                                              ProductName VARCHAR(50) NOT NULL,
                                                              Description TEXT,
                                                              Amount INT
                                                            )";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Sumsung 10', 'Sumsung Discount', 100);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated Postresql database.");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");
                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }
            return host;
        }
    }
}