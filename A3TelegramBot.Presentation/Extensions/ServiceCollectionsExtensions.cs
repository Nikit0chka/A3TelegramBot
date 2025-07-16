using A3TelegramBot.Presentation.Services;
using Ardalis.GuardClauses;
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

        serviceCollection.AddOpenApi();

        logger.LogInformation("Presentation сервисы добавлены");
    }
}