namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

/// <summary>
///     Константы заявки на обратный звонок
/// </summary>
public static class CallBackRequestConstants
{
    /// <summary>
    ///     Максимальная длина имени
    /// </summary>
    public const int NameMaxLength = 124;

    /// <summary>
    ///     Минимальная длина имени
    /// </summary>
    internal const int NameMinLength = 1;
}