using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI.BackgroundServices
{
    public class EmployeeEmailBackgroundService : BackgroundService
    {
        private readonly ILogger<EmployeeEmailBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

        public EmployeeEmailBackgroundService(
            ILogger<EmployeeEmailBackgroundService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Employee Email Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Employee Email Background Service is running at {time}", DateTimeOffset.Now);

                try
                {
                    await SendEmployeeReportAsync();
                    _logger.LogInformation("Employee report was sent successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending the employee report");
                }

                _logger.LogInformation("Employee Email Background Service is waiting for the next interval at {time}", DateTimeOffset.Now);
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task SendEmployeeReportAsync()
        {
            // Create a new scope to resolve scoped services
            using var scope = _serviceScopeFactory.CreateScope();
            
            // Resolve the employee report service from the scope
            var employeeReportService = scope.ServiceProvider.GetRequiredService<EmployeeReportService>();
            
            // Generate and send the report
            await employeeReportService.GenerateAndSendEmployeeReportAsync();
        }
    }
} 