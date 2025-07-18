namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;

/// <summary>
///     Состояние сессии пользователя
/// </summary>
public enum UserSessionState
{
    /// <summary>
    ///     Простой
    /// </summary>
    Idle,

    /// <summary>
    ///     В процессе оформления заявки на обратный звонок
    /// </summary>
    InCallbackRequest,

    /// <summary>
    ///     В процессе поиска ближайших приемных пунктов
    /// </summary>
    InFindingNearestReceptions
}