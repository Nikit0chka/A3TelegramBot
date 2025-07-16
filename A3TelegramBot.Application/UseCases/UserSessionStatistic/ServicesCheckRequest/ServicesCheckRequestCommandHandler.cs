using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate.Specifications;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.ServicesCheckRequest;

/// <inheritdoc />
/// <summary>
///     Обработчик команды сохранение статистики запроса услуг
/// </summary>
internal sealed class ServicesCheckRequestCommandHandler(
    IRepository<UserSessionStatistics> userSessionStatisticsRepository,
    ILogger<ServicesCheckRequestCommandHandler> logger)
    :ICommandHandler<ServicesCheckRequestCommand, Unit>
{
    public async Task<Unit> Handle(
        ServicesCheckRequestCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {command}", nameof(ServicesCheckRequestCommand));

        var userSessionStatistics = await userSessionStatisticsRepository.FirstOrDefaultAsync(new TheOnlyOneUserSessionStatisticsSpecification(), cancellationToken);

        if (userSessionStatistics == null)
        {
            logger.LogCritical("Пользовательская статистика не найдена");
            return Unit.Value;
        }

        userSessionStatistics.RecordServicesRequest();

        await userSessionStatisticsRepository.UpdateAsync(userSessionStatistics, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, статистика обновлена",
                              nameof(ServicesCheckRequestCommand));

        return Unit.Value;
    }
}