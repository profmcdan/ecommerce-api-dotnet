using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using Npgsql.Replication.PgOutput.Messages;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = 0;
            if(retry != null)
                retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating postgresql database...");
                using var connection =
                    new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                connection.Open();
                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };
                command.CommandText = "DROP TABLE IF EXISTS Coupon;";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT);";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsung 11', 'Samsunng Discount', 100);";
                command.ExecuteNonQuery();
                logger.LogInformation("Migrated successfully");

            }
            catch (NpgsqlException e)
            {
                logger.LogError(e, "An error occurred while migrating data");
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    System.Threading.Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, retryForAvailability);
                }
            }

            return host;
        }
    }
}