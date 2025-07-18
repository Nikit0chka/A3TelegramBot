namespace A3TelegramBot.Domain.AggregateModels.UserSessionStatisticAggregates;

/// <summary>
///     Базовый класс для статистики пользовательской сессии
/// </summary>
public abstract class BaseStatisticRecord
{
    /// <summary>
    ///     Конструктор для EF Core
    /// </summary>
    private protected BaseStatisticRecord() { }

    protected BaseStatisticRecord(int userSessionId) { UserSessionId = userSessionId; }

    /// <summary>
    ///     Идентификатора записи
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

    /// <summary>
    ///     Идентификатор пользовательской сессии
    /// </summary>
    public int UserSessionId { get; private init; }
}