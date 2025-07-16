namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

/// <summary>
///     Сообщения валидации для обратной заявки
/// </summary>
internal static class CallBackRequestValidationMessages
{
    /// <summary>
    ///     Имя обязательно
    /// </summary>
    public const string NameRequired = "Имя обязательно";

    /// <summary>
    ///     Длина имени вне доступного диапазона
    /// </summary>
    public readonly static string NameIsOutOfRange = $"Длина имени должна быть между {CallBackRequestConstants.NameMinLength} и {CallBackRequestConstants.NameMaxLength} символов";
}