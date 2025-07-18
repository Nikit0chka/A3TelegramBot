namespace A3TelegramBot.Application.UseCases.UserSessionStatistic.GetSummaryStatistic;

/// <inheritdoc />
/// <summary>
///  Объединенная модель для сбора статистики
/// </summary>
/// <param name="PriceCheckRequestsCount">Количество запросов цен</param>
/// <param name="ServicesCheckRequestsCount">Количество запросов услуг</param>
/// <param name="CallbackRequestsCompletedCount">Количество завершенных заявок на обратный звонок</param>
/// <param name="NearestReceptionsSearchesCount">Количество запросов поиска ближайших приемных пунктов</param>
public readonly record struct SummaryStatisticsDto(int PriceCheckRequestsCount, int ServicesCheckRequestsCount, int CallbackRequestsCompletedCount, int NearestReceptionsSearchesCount);