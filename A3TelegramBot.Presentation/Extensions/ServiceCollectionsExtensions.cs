using System.Reflection;
using A3TelegramBot.Presentation.BackgroundServices;
using A3TelegramBot.Presentation.Security.Authorization.ApiKey;
using Ardalis.GuardClauses;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using NSwag;
using Telegram.Bot;

namespace A3TelegramBot.Presentation.Extensions;

/// <summary>
///     Методы расширения service collection
/// </summary>
internal static class ServiceCollectionsExtensions
{
    /// <summary>
    ///     Добавление сервисов presentation слоя
    /// </summary>
    /// <param name="serviceCollection"> Коллекция сервисов </param>
    /// <param name="configurationManager"> Конфиг </param>
    /// <param name="logger"> Логер </param>
    public static void AddPresentationServices(this IServiceCollection serviceCollection, IConfigurationManager configurationManager, ILogger logger)
    {
        logger.LogInformation("Добавление presentation сервисов...");

        var telegramBotToken = configurationManager["TelegramBot:Token"];

        serviceCollection.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(Guard.Against.NullOrEmpty(telegramBotToken, nameof(telegramBotToken), "Telegram bot token was null ro empty")));
        serviceCollection.AddHostedService<BotBackgroundService>();

        serviceCollection.Configure<ApiKeyAuthOptions>(configurationManager.GetSection(ApiKeyAuthOptions.SectionName));

        serviceCollection.AddFastEndpoints()
            .AddAuthorization()
            .AddAuthentication(ApikeyAuthorization.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, ApikeyAuthorization>(ApikeyAuthorization.SchemeName, null);

        serviceCollection.SwaggerDocument(static documentOptions =>
        {
            documentOptions.EnableJWTBearerAuth = false;

            documentOptions.DocumentSettings = static settings =>
            {
                settings.AddAuth(ApikeyAuthorization.SchemeName,
                                 new()
                                 {
                                     Name = ApikeyAuthorization.HeaderName,
                                     In = OpenApiSecurityApiKeyLocation.Header,
                                     Type = OpenApiSecuritySchemeType.ApiKey
                                 });
            };
        });

        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        serviceCollection.AddOpenApi();

        logger.LogInformation("Presentation сервисы добавлены");
    }
}