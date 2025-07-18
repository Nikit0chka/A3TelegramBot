using Ardalis.SharedKernel;

namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.GetSummaryStatistic;

public readonly record struct GetSummaryStatisticQuery(DateTime Start, DateTime End):IQuery<SummaryStatisticsDto>;