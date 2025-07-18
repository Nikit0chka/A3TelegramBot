namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity.ValueObjects.PhoneNumberValueObject;

/// <summary>
///     Валидационные сообщения номера телефона
/// </summary>
internal static class PhoneNumberValidationMessages
{
    /// <summary>
    ///     Не верный формат номера
    /// </summary>
    public const string PhoneNumberNotValid = "Номер телефона в неверном формате";

    /// <summary>
    ///     Длина номера телефона вне допустимого диапазона
    /// </summary>
    public readonly static string PhoneNumberIsOutOfRange = $"Длина номера телефона должен быть между {PhoneNumberConstants.PhoneNumberMinLength} и {PhoneNumberConstants.PhoneNumberMaxLength} символов";
}