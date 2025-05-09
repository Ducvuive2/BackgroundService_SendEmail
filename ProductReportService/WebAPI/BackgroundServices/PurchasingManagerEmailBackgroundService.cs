using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI.BackgroundServices
{
    public class PurchasingManagerEmailBackgroundService : BackgroundService
    {
        private readonly ILogger<PurchasingManagerEmailBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24); // Daily report

        public PurchasingManagerEmailBackgroundService(
            ILogger<PurchasingManagerEmailBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Purchasing Manager Email Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Purchasing Manager Email Background Service is running at {time}", DateTimeOffset.Now);

                try
                {
                    await SendPurchasingManagerReportAsync();
                    _logger.LogInformation("Purchasing Manager report was sent successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending the Purchasing Manager report");
                }

                _logger.LogInformation("Purchasing Manager Email Background Service is waiting for the next interval at {time}", DateTimeOffset.Now);
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task SendPurchasingManagerReportAsync()
        {
            // Create a new scope to resolve scoped services
            using var scope = _serviceScopeFactory.CreateScope();
            
            // Resolve the purchasing manager report service from the scope
            var purchasingManagerReportService = scope.ServiceProvider.GetRequiredService<PurchasingManagerReportService>();
            
            // Generate and send the report
            await purchasingManagerReportService.GenerateAndSendPurchasingManagerReportAsync();
        }
    }
} 