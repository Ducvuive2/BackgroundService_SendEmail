using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.EmailConfig;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext - Update to use AdventureWorks database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Register repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPurchasingManagerRepository, PurchasingManagerRepository>();
            
            // Register application services
            services.AddScoped<EmployeeReportService>();
            services.AddScoped<PurchasingManagerReportService>();

            // Register infrastructure services
            services.AddSingleton<IExcelReportGenerator, ExcelReportGenerator>();

            // Configure and register email services
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}