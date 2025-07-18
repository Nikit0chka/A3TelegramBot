using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.CallBackRequestEntity;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession.Events;
using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession;

/// <inheritdoc cref="IAggregateRoot" />
/// <summary>
///     Пользовательская сессия, представляющая состояние взаимодействия
///     пользователя с ботом. Является агрегатом доменной модели.
/// </summary>
public sealed class UserSession:EntityBase, IAggregateRoot
{
    /// <summary>
    ///     Конструктор для EF Core
    /// </summary>
    private UserSession() { }

    /// <summary>
    ///     Создает новую пользовательскую сессию
    /// </summary>
    /// <param name="chatId"> Идентификатор чата в Telegram </param>
    public UserSession(long chatId)
    {
        ChatId = chatId;
        CurrentState = UserSessionState.Idle;
        LastActivity = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    ///     Идентификатор чата в Telegram
    /// </summary>
    public long ChatId { get; private set; }

    /// <summary>
    ///     Текущее состояние сессии
    /// </summary>
    public UserSessionState CurrentState { get; private set; }

    /// <summary>
    ///     Время последней активности (UTC)
    /// </summary>
    public DateTime LastActivity { get; private set; }

    /// <summary>
    ///     Дата создания сессии (UTC)
    /// </summary>
    public DateTime CreatedAt { get; private init; }

    /// <summary>
    ///     Запрос на обратный звонок (если есть)
    /// </summary>
    public CallBackRequest? CallBackRequest { get; private set; }

    /// <summary>
    ///     Изменяет состояние сессии
    /// </summary>
    /// <param name="newState"> Новое состояние </param>
    public void ChangeState(UserSessionState newState)
    {
        CurrentState = newState;
        LastActivity = DateTime.UtcNow;
    }

    /// <summary>
    ///     Начинает процесс обратного звонка
    /// </summary>
    /// <param name="callBackRequest"> Данные запроса на звонок </param>
    public void StartCallbackProcess(CallBackRequest callBackRequest)
    {
        CurrentState = UserSessionState.InCallbackRequest;
        CallBackRequest = callBackRequest;
        LastActivity = DateTime.UtcNow;
    }

    /// <summary>
    ///     Начинает процесс поиска ближайших пунктов
    /// </summary>
    public void StartSearchNearestReceptionsProcess()
    {
        CurrentState = UserSessionState.InFindingNearestReceptions;
        LastActivity = DateTime.UtcNow;
    }

    /// <summary>
    ///     Отменяет процесс обратного звонка
    /// </summary>
    public void CancelCallbackProcess()
    {
        CallBackRequest = null;
        CurrentState = UserSessionState.Idle;
        LastActivity = DateTime.UtcNow;
    }

    /// <summary>
    ///     Завершает процесс обратного звонка
    /// </summary>
    public void CompleteCallbackProcess()
    {
        CurrentState = UserSessionState.Idle;
        LastActivity = DateTime.UtcNow;
        RegisterDomainEvent(new CallBackRequestCompletedEvent(Id));
    }

    /// <summary>
    ///     Отменяет процесс поиска пунктов
    /// </summary>
    public void CancelSearchNearestReceptionsProcess()
    {
        CurrentState = UserSessionState.Idle;
        LastActivity = DateTime.UtcNow;
    }

    /// <summary>
    ///     Отменяет процесс поиска пунктов
    /// </summary>
    public void CompleteSearchNearestReceptionsProcess()
    {
        CurrentState = UserSessionState.Idle;
        LastActivity = DateTime.UtcNow;
        RegisterDomainEvent(new SearchNearestReceptionsCompletedEvent(Id));
    }

    /// <summary>
    ///     Продолжает прерванный процесс обратного звонка
    /// </summary>
    public void ContinueRequestCallBackProcess()
    {
        CurrentState = UserSessionState.InCallbackRequest;
        LastActivity = DateTime.UtcNow;
    }
}