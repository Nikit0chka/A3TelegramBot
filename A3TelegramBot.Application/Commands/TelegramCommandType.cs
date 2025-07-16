namespace A3TelegramBot.Application.Commands;

/// <summary>
///     Тип команд бота
/// </summary>
public enum TelegramCommandType
{
    /// <summary>
    ///     Текстовая команда
    /// </summary>
    TextCommand,

    /// <summary>
    ///     Кнопка
    /// </summary>
    Button,

    /// <summary>
    ///     Кнопка с запросом контакта (shared-contact)
    /// </summary>
    ContactButton,

    /// <summary>
    ///     Кнопка с запросом гео-локации (shared-location)
    /// </summary>
    LocationButton
}