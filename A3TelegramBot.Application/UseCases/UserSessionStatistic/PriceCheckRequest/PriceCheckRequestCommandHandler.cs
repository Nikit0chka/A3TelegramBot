using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate;
using A3TelegramBot.Domain.AggregateModels.UserSessionAggregate.Specifications;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;
using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;

/// <inheritdoc />
/// <summary>
///     Обработчик команды сохранение статистики запроса цен
/// </summary>
internal sealed class PriceCheckRequestCommandHandler(
    IRepository<PriceCheckRequestRecord> priceCheckRequestRecordRepository,
    IReadRepository<UserSession> userSessionRepository,
    ILogger<PriceCheckRequestCommandHandler> logger)
    :ICommandHandler<PriceCheckRequestCommand, Unit>
{
    public async Task<Unit> Handle(
        PriceCheckRequestCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка команды {command}", nameof(PriceCheckRequestCommand));

        var userSession = await userSessionRepository.SingleOrDefaultAsync(new UserSessionByChatIdSpecification(request.ChatId), cancellationToken);

        //TODO:Возможно с этим нужно что-то сделать
        if (userSession is null)
        {
            logger.LogCritical("Сессия по ChatId: {ChatId} не найдена. Статистика не обновлена", request.ChatId);
            return Unit.Value;
        }

        var serviceCheckRequestRecord = new PriceCheckRequestRecord(userSession.Id);

        await priceCheckRequestRecordRepository.AddAsync(serviceCheckRequestRecord, cancellationToken);

        logger.LogInformation("Завершена обработка команды {Command}, статистика обновлена",
                              nameof(PriceCheckRequestCommand));

        return Unit.Value;
    }
}