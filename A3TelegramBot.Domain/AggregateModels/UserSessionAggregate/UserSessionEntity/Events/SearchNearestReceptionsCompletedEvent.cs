using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity.Events;

/// <inheritdoc />
/// <summary>
/// Событие завершения поиска ближайших приемных пунктов
/// </summary>
public sealed class SearchNearestReceptionsCompletedEvent:DomainEventBase;