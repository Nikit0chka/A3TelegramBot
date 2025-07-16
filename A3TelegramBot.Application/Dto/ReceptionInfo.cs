namespace A3TelegramBot.Application.Dto;

/// <summary>
///     Информация о приемном пункте
/// </summary>
/// <param name="Name"> Наименование </param>
/// <param name="Address"> Адрес </param>
/// <param name="Phone"> Номер телефона </param>
/// <param name="WorkGraphic"> Рабочий график </param>
public sealed record ReceptionInfo(string Name, string Address, string Phone, string WorkGraphic);