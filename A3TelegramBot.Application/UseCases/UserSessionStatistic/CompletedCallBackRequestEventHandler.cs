using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.UserSessionEntity.Events;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate.Specifications;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic;

/// <inheritdoc />
/// <summary>
/// Обработчик события завершения заявки на обратный звонок
/// </summary>
public class CompletedCallBackRequestEventHandler(
    IRepository<UserSessionStatistics> userSessionStatisticsRepository,
    ILogger<CompletedCallBackRequestEventHandler> logger):INotificationHandler<CallBackRequestCompletedEvent>
{
    public async Task Handle(CallBackRequestCompletedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка события {event}", nameof(CallBackRequestCompletedEvent));

        var userSessionStatistics = await userSessionStatisticsRepository.FirstOrDefaultAsync(new TheOnlyOneUserSessionStatisticsSpecification(), cancellationToken);

        if (userSessionStatistics == null)
        {
            logger.LogCritical("Пользовательская статистика не найдена");
            return;
        }

        userSessionStatistics.RecordCallBackRequestCompleted();

        await userSessionStatisticsRepository.UpdateAsync(userSessionStatistics, cancellationToken);

        logger.LogInformation("Событие {event} обработано. Статистика обновлена", nameof(CallBackRequestCompletedEvent));
    }
}