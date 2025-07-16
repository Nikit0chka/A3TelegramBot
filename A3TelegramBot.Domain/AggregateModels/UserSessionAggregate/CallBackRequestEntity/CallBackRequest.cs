using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity.ValueObjects.PhoneNumberValueObject;
using A3TelegramBot.Domain.Extensions;
using Ardalis.GuardClauses;
using ErrorOr;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;

/// <summary>
///     Сущность запроса на обратный звонок.
///     Содержит данные и состояние процесса оформления запроса.
/// </summary>
public sealed class CallBackRequest
{
    /// <summary>
    ///     Конструктор для EF Core
    /// </summary>
    private CallBackRequest() { }

    /// <summary>
    ///     Создает новый запрос на обратный звонок
    /// </summary>
    /// <param name="userSessionId"> ID пользовательской сессии </param>
    /// <exception cref="ArgumentException">
    ///     Выбрасывается если userSessionId меньше или равен 0
    /// </exception>
    public CallBackRequest(int userSessionId)
    {
        CurrentState = CallBackRequestStatus.AwaitingPhone;
        Name = string.Empty;

        Guard.Against.Negative(userSessionId,
                               nameof(userSessionId),
                               "User session ID must be greater than zero");

        UserSessionId = userSessionId;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    ///     Текущее состояние запроса
    /// </summary>
    public CallBackRequestStatus CurrentState { get; private set; }

    /// <summary>
    ///     Номер телефона пользователя
    /// </summary>
    public PhoneNumber? Phone { get; private set; }

    /// <summary>
    ///     Дата и время создания запроса (UTC)
    /// </summary>
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    /// <summary>
    ///     Имя пользователя
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    ///     ID связанной пользовательской сессии
    /// </summary>
    public int UserSessionId { get; private init; }

    /// <summary>
    ///     Устанавливает номер телефона для запроса
    /// </summary>
    /// <param name="phoneNumber"> Номер телефона в международном формате </param>
    /// <returns>
    ///     Результат операции: Success или список ошибок валидации
    /// </returns>
    public ErrorOr<Success> SetPhone(string phoneNumber)
    {
        var createResult = PhoneNumber.Create(phoneNumber);

        if (createResult.IsError)
            return createResult.Errors;

        Phone = createResult.Value;
        CurrentState = CallBackRequestStatus.AwaitingName;

        return Result.Success;
    }

    /// <summary>
    ///     Устанавливает имя пользователя для запроса
    /// </summary>
    /// <param name="name"> Имя пользователя </param>
    /// <returns>
    ///     Результат операции: Success или ошибку валидации
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Выбрасывается если имя не соответствует требованиям
    /// </exception>
    public ErrorOr<Success> SetName(string name)
    {
        try
        {
            var trimName = name.Trim();

            Guard.Against.NullOrWhiteSpace(trimName,
                                           nameof(name),
                                           CallBackRequestValidationMessages.NameRequired);

            Guard.Against.StringLengthOutOfRange(
                                                 trimName,
                                                 CallBackRequestConstants.NameMinLength,
                                                 CallBackRequestConstants.NameMaxLength,
                                                 nameof(name),
                                                 CallBackRequestValidationMessages.NameIsOutOfRange);

            Name = trimName;
            CurrentState = CallBackRequestStatus.AwaitingPersonalDataProcessingPolicyAgreement;

            return Result.Success;
        }
        catch (ArgumentException ex)
        {
            return Error.Validation(ex.Message);
        }
    }

    /// <summary>
    ///     Подтверждает согласие с политикой обработки данных.
    ///     Переводит запрос в завершенное состояние.
    /// </summary>
    public void AcceptPersonalDataProcessingPolicy() { CurrentState = CallBackRequestStatus.Completed; }
}