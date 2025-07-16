namespace A3TelegramBot.Application.Abstractions;

/// <summary>
///     Основной интерфейс для обработки входящих обновлений от телеграм бота.
///     Определяет абстракцию для обработки различных типов сообщений и команд.
/// </summary>
public interface ITelegramProcessor
{
    /// <summary>
    ///     Обрабатывает команду, полученную от пользователя.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="command"> Текст команды </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task ProcessCommandAsync(long chatId, string command, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает контактные данные, предоставленные пользователем.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="phoneNumber"> Номер телефона пользователя </param>
    /// <param name="userName"> Имя пользователя </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task ProcessContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает гео-локацию, предоставленную пользователем.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="latitude"> Географическая широта </param>
    /// <param name="longitude"> Географическая долгота </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task ProcessLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает текстовое сообщение от пользователя.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="message"> Текст сообщения </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task ProcessTextMessageAsync(long chatId, string message, CancellationToken cancellationToken);
}