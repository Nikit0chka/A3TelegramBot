using A3TelegramBot.Application.Dto;

namespace A3TelegramBot.Application.Contracts;

/// <summary>
///     Сервис для генерации текстовых сообщений
///     бота в различных сценариях взаимодействия.
///     Содержит готовые тексты ответов для пользователя.
/// </summary>
public interface IMessageTextProvider
{
    /// <summary>
    ///     Возвращает текст для отображения
    ///     главного меню бота
    /// </summary>
    string GetMainMenuText();

    /// <summary>
    ///     Генерирует текст со списком
    ///     всех доступных команд бота
    /// </summary>
    string GetHelpText();

    /// <summary>
    ///     Возвращает приветственное сообщение,
    ///     показываемое при старте бота
    /// </summary>
    string GetStartText();

    /// <summary>
    ///     Возвращает текст запроса
    ///     номера телефона пользователя
    /// </summary>
    string GetRequestPhoneText();

    /// <summary>
    ///     Возвращает текст запроса
    ///     имени пользователя
    /// </summary>
    string GetRequestNameText();

    /// <summary>
    ///     Формирует текст подтверждения
    ///     создания запроса на обратный звонок
    /// </summary>
    string GetCallBackRequestCompletedText(string name, string phoneNumber);

    /// <summary>
    ///     Возвращает текст о том,
    ///     что запрос уже был создан ранее
    /// </summary>
    string GetCallBackRequestAlreadyCreatedText();

    /// <summary>
    ///     Форматирует текст сообщения
    ///     об ошибке для пользователя
    /// </summary>
    string GetErrorText(string errorMessage);

    /// <summary>
    ///     Возвращает стандартное сообщение
    ///     о неизвестной команде
    /// </summary>
    string GetCommandNotFoundText(string? providedCommand);

    /// <summary>
    ///     Возвращает текст с информацией
    ///     о ценах на услуги
    /// </summary>
    string GetPriceText();

    /// <summary>
    ///     Возвращает текст с описанием
    ///     доступных услуг
    /// </summary>
    string GetServicesText();

    /// <summary>
    ///     Возвращает текст
    ///     отмены запроса на звонок
    /// </summary>
    string GetCallBackRequestCancelledText();

    /// <summary>
    ///     Возвращает текст запроса
    ///     геолокации пользователя
    /// </summary>
    string GetRequestLocationText();

    /// <summary>
    ///     Форматирует список пунктов приема
    ///     в читаемый текст с разделителями
    /// </summary>
    string GetReceptionsListText(IEnumerable<ReceptionInfo> receptions);

    /// <summary>
    ///     Возвращает текст отмены
    ///     поиска ближайших пунктов
    /// </summary>
    string GetFindNearestReceptionsCancelledText();

    /// <summary>
    ///     Возвращает текст запроса согласия
    ///     с политикой обработки данных
    /// </summary>
    string GetRequestPersonalDataProcessingPolicyText();

    /// <summary>
    ///     Возвращает текст непредвиденной ошибки
    /// </summary>
    string GetUnhandledExceptionOccuredText();
}