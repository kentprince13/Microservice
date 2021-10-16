using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Ordering.Infrastructure.Persistence;
using Polly;

namespace Ordering.API.Extensions
{
    public  static class HostExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var logger = service.GetRequiredService<ILogger<TContext>>();
                var context = service.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                    
                    var policy = Policy.Handle<Exception>().WaitAndRetry(5,count=> TimeSpan.FromSeconds(count));
                    policy.Execute(() =>
                    {
                        context?.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
                        context?.Database.Migrate();
                        seeder(context, service);
                    });
                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");

                }
            }
            return host;
        }
    }
}