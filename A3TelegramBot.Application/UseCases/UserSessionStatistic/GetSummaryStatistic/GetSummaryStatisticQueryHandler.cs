using A3TelegramBot.Application.UseCases.UserSessionStatistic.PriceCheckRequest;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;
using A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates.Specifications;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.GetSummaryStatistic;

internal sealed class GetSummaryStatisticQueryHandler(
    IReadRepository<NearestReceptionSearchCompletedRecord> nearestReceptionSearchCompletedRecordRepository,
    IReadRepository<PriceCheckRequestRecord> priceCheckRequestRecordRepository,
    IReadRepository<CallBackRequestCompletedRecord> callBackRequestCompletedRecordRepository,
    IReadRepository<ServiceCheckRequestRecord> serviceCheckRequestRecordRepository,
    ILogger<PriceCheckRequestCommandHandler> logger)
    :IQueryHandler<GetSummaryStatisticQuery, SummaryStatisticsDto>
{
    public async Task<SummaryStatisticsDto> Handle(
        GetSummaryStatisticQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Обработка запроса {query}, Дата начала: {DateStart}, Дата конца: {DateEnd}", nameof(GetSummaryStatisticQuery), request.Start, request.End);

        var serviceCheckRequestRecordCount = await serviceCheckRequestRecordRepository.CountAsync(new StatisticRecordByDatesSpecification<ServiceCheckRequestRecord>(request.Start, request.End), cancellationToken);
        var nearestReceptionSearchCompletedRecordCount = await nearestReceptionSearchCompletedRecordRepository.CountAsync(new StatisticRecordByDatesSpecification<NearestReceptionSearchCompletedRecord>(request.Start, request.End), cancellationToken);
        var priceCheckRequestRecordCount = await priceCheckRequestRecordRepository.CountAsync(new StatisticRecordByDatesSpecification<PriceCheckRequestRecord>(request.Start, request.End), cancellationToken);
        var callBackRequestCompletedRecordCount = await callBackRequestCompletedRecordRepository.CountAsync(new StatisticRecordByDatesSpecification<CallBackRequestCompletedRecord>(request.Start, request.End), cancellationToken);


        logger.LogInformation("Завершена обработка запроса {Command}, статистика получена",
                              nameof(GetSummaryStatisticQuery));

        return new(priceCheckRequestRecordCount, serviceCheckRequestRecordCount, callBackRequestCompletedRecordCount, nearestReceptionSearchCompletedRecordCount);
    }
}