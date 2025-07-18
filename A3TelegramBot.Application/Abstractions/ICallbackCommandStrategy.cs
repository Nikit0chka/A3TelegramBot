using A3TelegramBot.Application.Commands;
using A3TelegramBot.Application.Services.UserState.CallBackRequestState.CommandStrategies;

namespace A3TelegramBot.Application.Abstractions;

/// <summary>
///     Контракт стратегии для обработки команд
///     при заявке на обратный звонок
/// </summary>
internal interface ICallbackCommandStrategy
{
    /// <summary>
    ///     Можно ли обработать команду
    /// </summary>
    /// <param name="command"> Команда </param>
    bool CanHandle(TelegramBotCommand command);

    /// <summary>
    ///     Вызов логики обработки
    /// </summary>
    /// <param name="chatId"> Id чата телеграм бота </param>
    /// <param name="context"> Контекст состояния заявки на обратный звонок </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    Task ExecuteAsync(long chatId, CallBackRequestStateContext context, CancellationToken cancellationToken);
}