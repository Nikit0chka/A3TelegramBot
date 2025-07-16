using A3TelegramBot.Application.Commands;

namespace A3TelegramBot.Application.Abstractions;

/// <summary>
///     Обработчик конкретного состояния пользовательской сессии
/// </summary>
internal interface IStateHandler
{
    /// <summary>
    ///     Выполняется при переходе в это состояние
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task EnterStateAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика обработки текстовой команды
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="command"> Команда </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика обработки текстового сообщения
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="message"> Текст сообщения </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика обработки контакта пользователя (shared contact)
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="phoneNumber"> Номер телефона пользователя </param>
    /// <param name="userName"> Фио пользователя </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken);

    /// <summary>
    ///     Логика обработки геолокации пользователя (shared location)
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="latitude"> Географическая широта </param>
    /// <param name="longitude"> Географическая долгота </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken);
}