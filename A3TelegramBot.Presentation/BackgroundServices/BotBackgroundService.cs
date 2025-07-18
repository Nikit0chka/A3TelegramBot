using A3TelegramBot.Application.Contracts;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace A3TelegramBot.Presentation.BackgroundServices;

/// <inheritdoc />
/// <summary>
///     Сервис polly опроса бота
/// </summary>
/// <param name="telegramBotClient"> Телеграм бот </param>
public sealed class BotBackgroundService(
    ITelegramBotClient telegramBotClient,
    ITelegramCommandConfigurator telegramCommandConfigurator,
    IServiceProvider serviceProvider,
    ILogger<BotBackgroundService> logger):BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await telegramCommandConfigurator.SetCommandsAsync(telegramBotClient, stoppingToken);

        var receiverOptions = new ReceiverOptions
                              {
                                  AllowedUpdates = []
                              };

        telegramBotClient.StartReceiving(
                                         UpdateHandler,
                                         ErrorHandler,
                                         receiverOptions,
                                         stoppingToken
                                        );

        var me = await telegramBotClient.GetMe(stoppingToken);
        logger.LogInformation("Бот {BotName} запущен!", me.Username);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var handler = serviceScope.ServiceProvider.GetRequiredService<IUpdateHandler>();
        await handler.HandleUpdateAsync(client, update, cancellationToken);
    }

    private async Task ErrorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var handler = serviceScope.ServiceProvider.GetRequiredService<IUpdateHandler>();
        await handler.HandleErrorAsync(client, exception, source, cancellationToken);
    }
}