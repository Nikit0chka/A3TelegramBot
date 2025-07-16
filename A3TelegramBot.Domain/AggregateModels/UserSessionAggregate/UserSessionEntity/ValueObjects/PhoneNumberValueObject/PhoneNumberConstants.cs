namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity.ValueObjects.PhoneNumberValueObject;

/// <summary>
///     Константы номера телефона
/// </summary>
public static class PhoneNumberConstants
{
    /// <summary>
    ///     Максимальная длина номера
    /// </summary>
    public const int PhoneNumberMaxLength = 15;

    /// <summary>
    ///     Минимальная длина номера
    /// </summary>
    internal const int PhoneNumberMinLength = 1;
}