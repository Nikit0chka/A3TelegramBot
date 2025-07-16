using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate.Specifications;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;

/// <inheritdoc />
/// <summary>
///     Обработчик команды сохранение статистики запроса цен
/// </summary>
internal sealed class PriceCheckRequestCommandHandler(
    IRepository<UserSessionStatistics> userSessionStatisticsRepository,
    ILogger<PriceCheckRequestCommandHandler> logger)
    :ICommandHandler<PriceCheckRequestCommand, Unit>
{
    public async Task<Unit> Handle(
        PriceCheckRequestCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {command}", nameof(PriceCheckRequestCommand));

        var userSessionStatistics = await userSessionStatisticsRepository.FirstOrDefaultAsync(new TheOnlyOneUserSessionStatisticsSpecification(), cancellationToken);

        if (userSessionStatistics == null)
        {
            logger.LogCritical("Пользовательская статистика не найдена");
            return Unit.Value;
        }

        userSessionStatistics.RecordPriceRequest();

        await userSessionStatisticsRepository.UpdateAsync(userSessionStatistics, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, статистика обновлена",
                              nameof(PriceCheckRequestCommand));

        return Unit.Value;
    }
}