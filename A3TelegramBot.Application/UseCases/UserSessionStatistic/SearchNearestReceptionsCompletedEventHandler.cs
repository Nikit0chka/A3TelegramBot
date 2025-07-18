using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Events;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic;

/// <inheritdoc />
/// <summary>
///     Обработчик события завершения заявки на обратный звонок
/// </summary>
public class SearchNearestReceptionsCompletedEventHandler(
    IRepository<NearestReceptionSearchCompletedRecord> nearestReceptionSearchCompletedRecordRepository,
    ILogger<SearchNearestReceptionsCompletedEventHandler> logger):INotificationHandler<SearchNearestReceptionsCompletedEvent>
{
    public async Task Handle(SearchNearestReceptionsCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка события {event}", nameof(SearchNearestReceptionsCompletedEvent));

        var nearestReceptionSearchCompletedRecord = new NearestReceptionSearchCompletedRecord(notification.UserId);

        await nearestReceptionSearchCompletedRecordRepository.AddAsync(nearestReceptionSearchCompletedRecord, cancellationToken);

        logger.LogInformation("Событие {event} обработано. Статистика обновлена", nameof(SearchNearestReceptionsCompletedEvent));
    }
}