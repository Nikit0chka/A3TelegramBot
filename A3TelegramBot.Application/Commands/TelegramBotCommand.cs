namespace A3TelegramBot.Application.Commands;

/// <summary>
///     Команда телеграм бота
/// </summary>
/// <param name="Command"> Строковая команда </param>
/// <param name="DisplayText"> Текст для отображения </param>
/// <param name="Description"> Текст для описания </param>
/// <param name="Type"> Тип команды </param>
public sealed record TelegramBotCommand(
    string Command,
    string DisplayText,
    string Description,
    TelegramCommandType Type = TelegramCommandType.TextCommand);