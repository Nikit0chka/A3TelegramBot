namespace A3TelegramBot.Application.Dto;

/// <summary>
///     Информация о приемном пункте
/// </summary>
/// <param name="Name"> Наименование </param>
/// <param name="Address"> Адрес </param>
/// <param name="Schedule"> Рабочий график </param>
/// <param name="WorkTime"> Рабочее время дня </param>
/// <param name="Phone"> Номера телефонов </param>
public sealed record ReceptionInfo(string Address, string Schedule, string WorkTime, IReadOnlyCollection<string> Phone);