using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;

/// <inheritdoc cref="BaseStatisticRecord" />
/// <summary>
///     Статистика пользовательских сессий.
///     Содержит информацию о количестве запросов ближайших приемных пунктов
/// </summary>
public sealed class NearestReceptionSearchCompletedRecord:BaseStatisticRecord, IAggregateRoot
{
    /// <inheritdoc />
    /// <summary>
    /// Конструктор для EF Core
    /// </summary>
    private NearestReceptionSearchCompletedRecord():base() { }

    public NearestReceptionSearchCompletedRecord(int userId):base(userId) { }
}