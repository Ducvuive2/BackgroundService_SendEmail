using Application;
using Infrastructure;
using WebAPI.BackgroundServices;
using Serilog;
using Serilog.Events;

// Configure Serilog first, before any host building
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog to the logging pipeline
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Add Application layer services
    builder.Services.AddApplication();

    // Add Infrastructure layer services
    builder.Services.AddInfrastructure(builder.Configuration);

    // Add the background service for sending employee reports
    builder.Services.AddHostedService<EmployeeEmailBackgroundService>();

    // Add the background service for sending purchasing manager reports
    builder.Services.AddHostedService<PurchasingManagerEmailBackgroundService>();

    // Add Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
