using Core.Extentions;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Net.payOS;

namespace Presentation
{
    public static class StartupExtentions
    {
        public static IServiceCollection AddPayOS(this IServiceCollection services, IConfiguration configuration)
        {
            var clientId = configuration ["PayOS:ClientId"]
                ?? throw new ArgumentNullException("PayOS:ClientId");
            var apiKey = configuration ["PayOS:ApiKey"]
                ?? throw new ArgumentNullException("PayOS:ApiKey");
            var checkSum = configuration ["PayOS:CheckSum"]
                ?? throw new ArgumentNullException("PayOS:CheckSum");
            services.AddSingleton(x => new PayOS(clientId, apiKey, checkSum));
            return services;
        }

        public static IApplicationBuilder UsePayOS(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var payOSService = serviceProvider.GetRequiredService<PayOS>();
            var logger = serviceProvider.GetRequiredService<ILogger<PayOS>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = (configuration ["PayOS:WebHookBaseUrl"] ?? string.Empty).Replace("https://", "");

            Task.Run(async () =>
            {
                if (!baseUrl.IsNullOrEmpty())
                {
                    logger.LogInformation("Registing to Webhook {baseUrl}...", baseUrl);
                    logger.LogInformation("https://{baseUrl}/web-hooks/payment-info", baseUrl);
                    var result = await payOSService.confirmWebhook($"https://{baseUrl}/web-hooks/payment-info/");
                    logger.LogInformation("Registed to Webhook {result}", result);
                }
            }).Wait();

            return app;
        }

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TableHubDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TableHubDbContext>>();

            try
            {
                logger.LogInformation("Updating migration to database...");
                db.Database.Migrate();
                logger.LogInformation("Updated migration");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fail to Update migration");
                throw;
            }
        }
    }
}
