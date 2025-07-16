namespace A3TelegramBot.Application.Commands;

/// <summary>
///     Команды телеграм бота
/// </summary>
public static class TelegramBotCommands
{
    // Базовые команды
    internal readonly static TelegramBotCommand Start = new("/start", "Запуск бота", "Запуск бота");
    internal readonly static TelegramBotCommand Help = new("/help", "Помощь", "Помощь");
    internal readonly static TelegramBotCommand MainMenu = new("/main_menu", "Основное меню", "Основное меню");

    // Команды кнопок
    public readonly static TelegramBotCommand FindNearestReceptions = new(
                                                                          "find_receptions",
                                                                          "Найти ближайший приемный пункт",
                                                                          "Поиск ближайших пунктов приема",
                                                                          TelegramCommandType.Button);

    public readonly static TelegramBotCommand RequestCallback = new(
                                                                    "request_callback",
                                                                    "Заказать обратный звонок",
                                                                    "Заказ обратного звонка",
                                                                    TelegramCommandType.Button);

    public readonly static TelegramBotCommand SharePhone = new(
                                                               "share_phone",
                                                               "📱 Поделиться номером",
                                                               "Отправить номер телефона",
                                                               TelegramCommandType.ContactButton);

    public readonly static TelegramBotCommand ShareLocation = new(
                                                                  "share_location",
                                                                  "🗺️ Поделиться локацией",
                                                                  "Отправить местоположение",
                                                                  TelegramCommandType.LocationButton);

    public readonly static TelegramBotCommand Cancel = new(
                                                           "cancel",
                                                           "❌ Отмена",
                                                           "Отмена текущей операции",
                                                           TelegramCommandType.Button);

    public readonly static TelegramBotCommand CheckPrice = new(
                                                               "check_price",
                                                               "Узнать цены",
                                                               "Информация о ценах",
                                                               TelegramCommandType.Button);

    public readonly static TelegramBotCommand Services = new(
                                                             "services",
                                                             "Услуги",
                                                             "Список услуг",
                                                             TelegramCommandType.Button);

    public readonly static TelegramBotCommand Yes = new(
                                                        "yes",
                                                        "Да",
                                                        "Подтверждение действия",
                                                        TelegramCommandType.Button);

    /// <summary>
    ///     Все команды бота
    /// </summary>
    public readonly static IReadOnlyList<TelegramBotCommand> AllCommands =
    [
        Start, Help, MainMenu,
        FindNearestReceptions, RequestCallback,
        SharePhone, ShareLocation, Cancel,
        CheckPrice, Services, Yes
    ];

    private readonly static Dictionary<string, TelegramBotCommand> CommandMap = AllCommands
        .SelectMany(static telegramBotCommand => new[] { telegramBotCommand.Command, telegramBotCommand.DisplayText })
        .Distinct()
        .ToDictionary(static key => key,
                      static key => AllCommands.First(cmd =>
                                                          cmd.Command == key || cmd.DisplayText == key));

    /// <summary>
    ///     Получение команды из списка с локализацией по строке
    /// </summary>
    /// <param name="input"> Строка для получения </param>
    /// <param name="command"> Выходная команда </param>
    internal static bool TryGetCommand(string input, out TelegramBotCommand? command) => CommandMap.TryGetValue(input, out command);
}