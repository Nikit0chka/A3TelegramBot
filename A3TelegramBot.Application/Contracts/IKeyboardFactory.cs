using Telegram.Bot.Types.ReplyMarkups;

namespace A3TelegramBot.Application.Contracts;

/// <summary>
///     Фабрика для создания клавиатур телеграм бота
/// </summary>
public interface IKeyboardFactory
{
    /// <summary>
    ///     Создает основную клавиатуру
    /// </summary>
    ReplyKeyboardMarkup CreateMainMenuKeyboard();

    /// <summary>
    ///     Создает клавиатуру для запроса гео-локации
    /// </summary>
    ReplyKeyboardMarkup CreateLocationKeyboard();

    /// <summary>
    ///     Создает клавиатуру для запроса номера телефона
    /// </summary>
    ReplyKeyboardMarkup CreatePhoneRequestKeyboard();

    /// <summary>
    ///     Создает клавиатуру для запроса имени пользователя
    /// </summary>
    ReplyKeyboardMarkup CreateNameRequestKeyboard();

    /// <summary>
    ///     Создает клавиатуру для согласия с политикой обработки персональных данных
    /// </summary>
    ReplyKeyboardMarkup CreatePersonalDataProcessingPolicyKeyboard();
}