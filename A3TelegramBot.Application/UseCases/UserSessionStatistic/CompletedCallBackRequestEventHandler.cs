using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSession.Events;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic;

/// <inheritdoc />
/// <summary>
///     Обработчик события завершения заявки на обратный звонок
/// </summary>
public class CompletedCallBackRequestEventHandler(
    IRepository<CallBackRequestCompletedRecord> callBackRequestCompletedRecordRepository,
    ILogger<CompletedCallBackRequestEventHandler> logger):INotificationHandler<CallBackRequestCompletedEvent>
{
    public async Task Handle(CallBackRequestCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка события {event}", nameof(CallBackRequestCompletedEvent));

        var callBackRequestCompletedRecord = new CallBackRequestCompletedRecord(notification.UserId);

        await callBackRequestCompletedRecordRepository.AddAsync(callBackRequestCompletedRecord, cancellationToken);

        logger.LogInformation("Событие {event} обработано. Статистика обновлена", nameof(CallBackRequestCompletedEvent));
    }
}