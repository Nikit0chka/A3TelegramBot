namespace A3TelegramBot.Presentation.Endpoints.Statistic.Get;

/// <summary>
///     Запрос для получения статистики
/// </summary>
/// <param name="DateStart"> Начальная дата периода </param>
/// <param name="DateEnd"> Конечная дата периода </param>
public sealed record GetStatisticRequest(
    DateTime DateStart,
    DateTime DateEnd
);