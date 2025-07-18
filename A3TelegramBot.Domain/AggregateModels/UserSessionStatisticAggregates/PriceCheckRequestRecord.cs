using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;

/// <inheritdoc cref="BaseStatisticRecord" />
/// <summary>
///     Статистика пользовательских сессий.
///     Содержит информацию о количестве запросов цен
/// </summary>
public sealed class PriceCheckRequestRecord:BaseStatisticRecord, IAggregateRoot
{
    /// <inheritdoc />
    /// <summary>
    /// Конструктор для EF Core
    /// </summary>
    private PriceCheckRequestRecord():base() { }

    public PriceCheckRequestRecord(int userId):base(userId) { }
}