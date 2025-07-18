using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

namespace A3TelegramBot.Application.Abstractions;

/// <summary>
///     Обработчик состояния заявки на обратный звонок
/// </summary>
internal interface ICallbackStateHandler
{
    /// <summary>
    ///     Текущий статус заявки
    /// </summary>
    CallBackRequestStatus Status { get; }

    /// <summary>
    ///     Выполняется при переходе в это состояние
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="context"> Контекст состояния заявки на обратный звонок </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task EnterAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken);

    /// <summary>
    ///     Обработка сообщения
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="message"> Сообщение для обработки </param>
    /// <param name="context"> Контекст состояния заявки на обратный звонок </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task HandleMessageAsync(long chatId, string message, CallBackRequestStateContext context, CancellationToken cancellationToken);
}