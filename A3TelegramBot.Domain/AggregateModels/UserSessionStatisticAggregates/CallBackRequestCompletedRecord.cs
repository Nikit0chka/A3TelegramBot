using Ardalis.SharedKernel;

namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;

/// <inheritdoc cref="BaseStatisticRecord" />
/// <summary>
///     Статистика пользовательских сессий.
///     Содержит информацию о количестве заявок на обратный звонок
/// </summary>
public sealed class CallBackRequestCompletedRecord:BaseStatisticRecord, IAggregateRoot
{
    /// <inheritdoc />
    /// <summary>
    ///     Конструктор для EF Core
    /// </summary>
    private CallBackRequestCompletedRecord() { }

    public CallBackRequestCompletedRecord(int userId):base(userId) { }
}