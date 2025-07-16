using A3TelegramBot.Application.Commands;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity;

namespace A3TelegramBot.Application.Abstractions;

/// <summary>
///     Машина состояния пользовательской сессии.
///     Маршрутизирует сообщения между обработчиками состояний <see cref="IStateHandler" />
/// </summary>
internal interface IUserSessionStateMachine
{
    /// <summary>
    ///     Переводит сессию в указанное состояние
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="newState"> Целевое состояние сессии </param>
    /// <param name="command"> Команда, для вызова в новом обработчике состояния </param>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции </param>
    Task TransitionToStateAsync(long chatId, UserSessionState newState, TelegramBotCommand? command, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает контактные данные, предоставленные пользователем.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="phoneNumber"> Номер телефона пользователя </param>
    /// <param name="userName"> Имя пользователя </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleContactAsync(long chatId, string phoneNumber, string userName, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает гео-локацию, предоставленную пользователем.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="latitude"> Географическая широта </param>
    /// <param name="longitude"> Географическая долгота </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleLocationAsync(long chatId, double latitude, double longitude, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает текстовое сообщение от пользователя.
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="message"> Текст сообщения </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleTextMessageAsync(long chatId, string message, CancellationToken cancellationToken);

    /// <summary>
    ///     Обрабатывает текстовую команду
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="command"> Команда для обработки </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleTextCommandAsync(long chatId, TelegramBotCommand? command, CancellationToken cancellationToken);
}