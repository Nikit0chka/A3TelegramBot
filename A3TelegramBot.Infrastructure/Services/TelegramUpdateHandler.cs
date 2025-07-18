using A3TelegramBot.Application.Abstractions;
using A3TelegramBot.Application.Contracts;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace A3TelegramBot.Infrastructure.Services;

/// <inheritdoc />
/// <summary>
///     Обработчик обновлений телеграм бота.
///     Маршрутизирует сообщения в application слой
/// </summary>
internal sealed class TelegramUpdateHandler(
    ITelegramProcessor telegramProcessor,
    ITelegramResponseService telegramResponseService,
    ILogger<IUpdateHandler> logger):IUpdateHandler
{
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            if (update.Message is not { } message)
            {
                logger.LogDebug("Получен update без Message: {UpdateType}", update.Type);
                return;
            }

            var chatId = message.Chat.Id;
            logger.LogInformation("Новое сообщение от ChatId: {ChatId}, Type: {MessageType}", chatId, message.Type);

            // Обработка контакта
            if (message.Contact is { } contact)
            {
                await telegramProcessor.ProcessContactAsync(
                                                            chatId,
                                                            contact.PhoneNumber,
                                                            $"{contact.FirstName} {contact.LastName}",
                                                            cancellationToken);

                return;
            }

            // Обработка локации
            if (message.Location is { } location)
            {
                await telegramProcessor.ProcessLocationAsync(
                                                             chatId,
                                                             location.Latitude,
                                                             location.Longitude,
                                                             cancellationToken);

                return;
            }

            // Обработка текстовых сообщений
            if (message.Text is { } text)
            {
                if (text.StartsWith('/'))
                    await telegramProcessor.ProcessCommandAsync(chatId, text, cancellationToken);
                else
                    await telegramProcessor.ProcessTextMessageAsync(chatId, text, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            var chatId = update.Message?.Chat.Id;

            if (chatId.HasValue)
                await telegramResponseService.SendUnhandledExceptionOccured(chatId.Value, cancellationToken);

            logger.LogError(ex, "Необработанное исключение при обработке update");
            throw;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Необработанная ошибка");
        return Task.CompletedTask;
    }
}