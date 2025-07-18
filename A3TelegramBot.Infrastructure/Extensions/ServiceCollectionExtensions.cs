using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Infrastructure.Data;
using A3TelegramBot.Infrastructure.HttpClients.A3Api;
using A3TelegramBot.Infrastructure.HttpClients.Abstractions;
using A3TelegramBot.Infrastructure.HttpClients.SdaiLomRfApi;
using A3TelegramBot.Infrastructure.Services;
using A3TelegramBot.Infrastructure.Services.MessageText;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using Telegram.Bot.Polling;

namespace A3TelegramBot.Infrastructure.Extensions;

/// <summary>
///     Методы расширения для коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Добавляет infrastructure сервисы в ioc контейнер
    /// </summary>
    /// <param name="serviceCollection"> Ioc контейнер </param>
    /// <param name="configurationManager"> Конфигурационный менеджер </param>
    /// <param name="logger"> Логер </param>
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection, IConfigurationManager configurationManager, ILogger logger)
    {
        logger.LogInformation("Добавления infrastructure сервисов...");

        var dbConnectionString = configurationManager.GetConnectionString("DefaultConnection");

        serviceCollection.AddDbContext<DbContext, Context>
            (options => options.UseSqlServer(Guard.Against.NullOrEmpty(dbConnectionString,
                                                                       nameof(dbConnectionString),
                                                                       "Database connection string was null or empty.")));

        serviceCollection.Configure<MessageTextOptions>(configurationManager.GetSection(MessageTextOptions.SectionName));
        serviceCollection.AddScoped<MessageTextOptions>();
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        serviceCollection.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

        serviceCollection.AddScoped<IMessageTextProvider, MessageTextProvider>();
        serviceCollection.AddScoped<IKeyboardFactory, KeyboardFactory>();
        serviceCollection.AddScoped<ITelegramResponseService, TelegramResponseService>();
        serviceCollection.AddScoped<IUpdateHandler, TelegramUpdateHandler>();

        serviceCollection.AddSingleton<ITelegramCommandConfigurator, TelegramCommandConfigurator>();

        AddApiClients(serviceCollection, configurationManager);

        logger.LogInformation("Infrastructure сервисы добавлены");
    }

    private static void AddApiClients(this IServiceCollection serviceCollection, IConfigurationManager configurationManager)
    {
        serviceCollection.Configure<A3ApiOptions>(configurationManager.GetSection(A3ApiOptions.SectionName));
        serviceCollection.Configure<SdaiLomRfOptions>(configurationManager.GetSection(SdaiLomRfOptions.SectionName));

        var a3ApiOptions = configurationManager.GetSection(A3ApiOptions.SectionName).Get<A3ApiOptions>();
        var sdaiLomRfOptions = configurationManager.GetSection(SdaiLomRfOptions.SectionName).Get<SdaiLomRfOptions>();

        serviceCollection.AddRefitClient<IA3ApiClient>()
            .ConfigureHttpClient(client => { client.BaseAddress = new(a3ApiOptions!.DefaultConnection); });
        serviceCollection.AddRefitClient<ISdaiLomRfApiClient>()
            .ConfigureHttpClient(client => { client.BaseAddress = new(sdaiLomRfOptions!.DefaultConnection); }).AddHttpMessageHandler(provider => new LoggingHandler()); ;

        serviceCollection.AddScoped<IA3ApiService, A3ApiService>();
        serviceCollection.AddScoped<ISdaiLomRfApiService, SdaiLomRfService>();
    }
}

public class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Request:");
        Console.WriteLine(request.ToString());
        if (request.Content != null)
        {
            Console.WriteLine(await request.Content.ReadAsStringAsync());
        }

        var response = await base.SendAsync(request, cancellationToken);

        Console.WriteLine("Response:");
        Console.WriteLine(response.ToString());
        if (response.Content != null)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        return response;
    }
}