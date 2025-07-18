using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;

/// <inheritdoc cref="BaseStatisticRecord" />
/// <summary>
///     Статистика пользовательских сессий.
///     Содержит информацию о количестве запросов услуг
/// </summary>
public sealed class ServiceCheckRequestRecord:BaseStatisticRecord, IAggregateRoot
{

    /// <inheritdoc />
    /// <summary>
    ///     Конструктор для EF Core
    /// </summary>
    private ServiceCheckRequestRecord() { }

    /// <inheritdoc />
    /// <summary>
    ///     Статистика пользовательских сессий.
    ///     Содержит информацию о количестве запросов услуг
    /// </summary>
    public ServiceCheckRequestRecord(int userId):base(userId) { }
}