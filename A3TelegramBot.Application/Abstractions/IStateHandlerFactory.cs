using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;

namespace A3TelegramBot.Application.Abstractions;

/// <summary>
///     Фабрика создания обработчиков состояния
/// </summary>
internal interface IStateHandlerFactory
{
    /// <summary>
    ///     Получить обработчик по состоянию
    /// </summary>
    /// <param name="state"> Состояние сессии </param>
    IStateHandler GetHandler(UserSessionState state);
}