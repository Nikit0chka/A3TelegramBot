using Telegram.Bot;

namespace A3TelegramBot.Application.Contracts;

/// <summary>
///     Конфигуратор команд для телеграм бота
/// </summary>
public interface ITelegramCommandConfigurator
{
    /// <summary>
    ///     Устанавливает команды в телеграм бота
    /// </summary>
    /// <param name="telegramBotClient"> Клиент телеграм бота </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task SetCommandsAsync(ITelegramBotClient telegramBotClient, CancellationToken cancellationToken);
}