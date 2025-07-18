using Ardalis.Specification;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates.Specifications;

/// <inheritdoc cref="Ardalis.Specification.Specification{T}" />
/// <summary>
///     Спецификация поиска статистики пользователя по дате начала и дате конца
/// </summary>
public sealed class StatisticRecordByDatesSpecification<T>:Specification<T> where T : BaseStatisticRecord
{
    public StatisticRecordByDatesSpecification(DateTime start, DateTime end) { Query.Where(statisticRecord => statisticRecord.CreatedAt >= start && statisticRecord.CreatedAt <= end); }
}