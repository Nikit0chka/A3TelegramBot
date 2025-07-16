using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity.Events;

/// <inheritdoc />
/// <summary>
/// Событие завершения заявки на обратный звонок
/// </summary>
public sealed class CallBackRequestCompletedEvent:DomainEventBase;