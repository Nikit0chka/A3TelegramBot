using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Events;

/// <inheritdoc />
/// <summary>
///     Событие завершения заявки на обратный звонок
/// </summary>
public sealed class CallBackRequestCompletedEvent(int userId):DomainEventBase
{
    public int UserId { get; } = userId;
}