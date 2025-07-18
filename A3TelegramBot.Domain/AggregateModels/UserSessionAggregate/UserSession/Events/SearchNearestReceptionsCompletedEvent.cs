using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession.Events;

/// <inheritdoc />
/// <summary>
///     Событие завершения поиска ближайших приемных пунктов
/// </summary>
public sealed class SearchNearestReceptionsCompletedEvent(int userId):DomainEventBase
{
    public int UserId { get; } = userId;
}