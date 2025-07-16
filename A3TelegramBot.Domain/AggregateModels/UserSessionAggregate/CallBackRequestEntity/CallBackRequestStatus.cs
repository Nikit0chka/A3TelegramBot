namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

/// <summary>
///     Статус заявки на обратный звонок
/// </summary>
public enum CallBackRequestStatus
{
    /// <summary>
    ///     Ожидание телефона
    /// </summary>
    AwaitingPhone,

    /// <summary>
    ///     Ожидание имени
    /// </summary>
    AwaitingName,

    /// <summary>
    ///     Ожидание принятия политики обработки персональных данных
    /// </summary>
    AwaitingPersonalDataProcessingPolicyAgreement,

    /// <summary>
    ///     Завершена
    /// </summary>
    Completed
}