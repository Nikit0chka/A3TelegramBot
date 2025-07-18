using System.Text.RegularExpressions;
using A3TelegramBot.Domain.Extensions;
using Ardalis.GuardClauses;
using ErrorOr;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity.ValueObjects.PhoneNumberValueObject;

/// <inheritdoc />
/// <summary>
///     Value Object, представляющий валидный номер телефона.
///     Обеспечивает проверку формата и длины номера.
/// </summary>
public readonly partial record struct PhoneNumber
{
    private PhoneNumber(string value) { Value = value; }

    /// <summary>
    ///     Строковое значение номера телефона
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Создает новый экземпляр PhoneNumber с валидацией
    /// </summary>
    /// <param name="phoneNumber"> Номер телефона для валидации </param>
    /// <returns>
    ///     Результат с PhoneNumber или список ошибок валидации
    /// </returns>
    public static ErrorOr<PhoneNumber> Create(string phoneNumber)
    {
        var validationResult = Validate(phoneNumber);

        if (validationResult.IsError)
            return validationResult.Errors;

        return new PhoneNumber(phoneNumber);
    }

    /// <summary>
    ///     Проверяет валидность номера телефона
    /// </summary>
    /// <param name="phoneNumber"> Номер для проверки </param>
    /// <returns>
    ///     Success при валидном номере или ошибки валидации
    /// </returns>
    private static ErrorOr<Success> Validate(string phoneNumber)
    {
        try
        {
            // Проверка длины номера
            Guard.Against.StringLengthOutOfRange(
                                                 phoneNumber,
                                                 PhoneNumberConstants.PhoneNumberMinLength,
                                                 PhoneNumberConstants.PhoneNumberMaxLength,
                                                 nameof(phoneNumber),
                                                 PhoneNumberValidationMessages.PhoneNumberIsOutOfRange);

            // Проверка формата регулярным выражением
            if (!PhoneNumberRegex().IsMatch(phoneNumber))
                return Error.Validation(description: PhoneNumberValidationMessages.PhoneNumberNotValid);

            return Result.Success;
        }
        catch (ArgumentException ex)
        {
            return Error.Validation(description: ex.Message);
        }
    }

    /// <summary>
    ///     Регулярное выражение для проверки формата номера
    /// </summary>
    /// <remarks>
    ///     Допускает:
    ///     - необязательный + в начале
    ///     - цифры от 10 до 15 символов
    /// </remarks>
    [GeneratedRegex(@"^\+?[0-9]{10,15}$")]
    private static partial Regex PhoneNumberRegex();
}