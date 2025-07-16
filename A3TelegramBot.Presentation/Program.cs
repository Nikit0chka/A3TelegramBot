using A3TelegramBot.Application.Extensions;
using A3TelegramBot.Infrastructure.Extensions;
using A3TelegramBot.Presentation.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Config/appsettings.presentation.json")
    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "Config/appsettings.infrastructure.json"))
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(static loggingBuilder => { loggingBuilder.AddSerilog(); });
var logger = loggerFactory.CreateLogger<Program>();

builder.Services.AddPresentationServices(builder.Configuration, logger);
builder.Services.AddInfrastructureServices(builder.Configuration, logger);
builder.Services.AddApplicationServices(logger);

var app = builder.Build();

app.Run();