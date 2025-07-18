using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.ServicesCheckRequest;

/// <inheritdoc />
/// <summary>
///     Обработчик команды сохранение статистики запроса услуг
/// </summary>
internal sealed class ServicesCheckRequestCommandHandler(
    IRepository<ServiceCheckRequestRecord> serviceCheckRequestRecordRepository,
    IReadRepository<UserSession> userSessionRepository,
    ILogger<ServicesCheckRequestCommandHandler> logger)
    :ICommandHandler<ServicesCheckRequestCommand, Unit>
{
    public async Task<Unit> Handle(
        ServicesCheckRequestCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {command}", nameof(ServicesCheckRequestCommand));

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecification(request.ChatId), cancellationToken);

        //TODO:Возможно с этим нужно что-то сделать
        if (userSession is null)
        {
            logger.LogCritical("Сессия по ChatId: {ChatId} не найдена. Статистика не обновлена", request.ChatId);
            return Unit.Value;
        }

        var serviceCheckRequestRecord = new ServiceCheckRequestRecord(userSession.Id);

        await serviceCheckRequestRecordRepository.AddAsync(serviceCheckRequestRecord, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, статистика обновлена",
                              nameof(ServicesCheckRequestCommand));

        return Unit.Value;
    }
}