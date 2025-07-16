using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticsAggregate;

//TODO: Пока статистика - одна сущность со всей информацией об действиях пользователей. По тз больше и не нужно было, но выглядит посредственно
/// <summary>
/// Статистика пользовательских сессий.
/// Содержит информацию о количестве заявок на обратный звонок,
/// запросов услуг, запросов ближайших приемных пунктов, запросов цен
/// </summary>
public class UserSessionStatistics:IAggregateRoot
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Дата начала сбора статистики
    /// </summary>
    public DateTime PeriodStart { get; private init; } = DateTime.UtcNow;

    /// <summary>
    /// Количество запросов цен
    /// </summary>
    public int PriceChecks { get; private set; }

    /// <summary>
    /// Количество запросов услуг
    /// </summary>
    public int ServicesRequests { get; private set; }

    /// <summary>
    /// Количество завершенных заявок на обратный звонок
    /// </summary>
    public int CallbackRequestsCompleted { get; private set; }

    /// <summary>
    /// Количество запросов поиска ближайших приемных пунктов
    /// </summary>
    public int NearestReceptionsSearches { get; private set; }

    /// <summary>
    /// Сохранить факт завершения заявки на обратный звонок
    /// </summary>
    public void RecordCallBackRequestCompleted() => CallbackRequestsCompleted++;

    /// <summary>
    /// Сохранить факт завершения поиска ближайших приемных пунктов
    /// </summary>
    public void RecordSearchNearestReceptionsCompleted() => NearestReceptionsSearches++;

    /// <summary>
    /// Сохранить факт запроса цен
    /// </summary>
    public void RecordPriceRequest() => PriceChecks++;

    /// <summary>
    /// Сохранить факт запроса услуг
    /// </summary>
    public void RecordServicesRequest() => ServicesRequests++;
}