using A3TelegramBot.Application.Dto;

namespace A3TelegramBot.Application.Contracts;

/// <summary>
///     Сервис для отправки ответных сообщений
///     через Telegram Bot API. Объединяет
///     логику формирования текста и клавиатур для бота.
/// </summary>
public interface ITelegramResponseService
{
    /// <summary>
    ///     Отправляет уведомление о том,
    ///     что запрос на обратный звонок
    ///     уже был создан ранее
    /// </summary>
    Task SendCallbackRequestAlreadyCreatedAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет подтверждение успешного
    ///     создания запроса на обратный звонок
    ///     с указанием имени и телефона
    /// </summary>
    Task SendCallbackRequestCompletedAsync(long chatId, string name, string phoneNumber, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет сообщение о том,
    ///     что команда не была распознана
    /// </summary>
    Task SendCommandNotFoundAsync(long chatId, string? providedCommand, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет сообщение об ошибке
    ///     с указанием деталей проблемы
    /// </summary>
    Task SendErrorAsync(long chatId, string errorMessage, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет сообщение со списком
    ///     доступных команд и их описанием
    /// </summary>
    Task SendHelpAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет основное меню бота
    ///     с доступными действиями
    /// </summary>
    Task SendMainMenuAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет запрос на ввод
    ///     имени пользователя
    /// </summary>
    Task SendRequestNameAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет запрос на ввод
    ///     номера телефона пользователя
    /// </summary>
    Task SendRequestPhoneAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет запрос согласия
    ///     с политикой обработки данных
    /// </summary>
    Task SendRequestPersonalDataProcessingPolicyAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет приветственное сообщение
    ///     при первом запуске бота
    /// </summary>
    Task SendStartAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет информацию
    ///     о текущих ценах
    /// </summary>
    Task SendPriceAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет информацию
    ///     о доступных услугах
    /// </summary>
    Task SendServicesAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет подтверждение
    ///     отмены запроса на звонок
    /// </summary>
    Task SendCallBackRequestCancelledAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет запрос
    ///     на предоставление геолокации
    /// </summary>
    Task SendRequestLocationAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет форматированный список
    ///     ближайших пунктов приема
    /// </summary>
    Task SendReceptionsList(long chatId, IEnumerable<ReceptionInfo> receptions, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет уведомление об отмене
    ///     поиска ближайших пунктов
    /// </summary>
    Task SendFindNearestReceptionsCancelledAsync(long chatId, CancellationToken cancellationToken);

    /// <summary>
    ///     Отправляет уведомление о том,
    ///     что запрос на обратный звонок
    ///     уже был создан ранее
    /// </summary>
    Task SendUnhandledExceptionOccured(long chatId, CancellationToken cancellationToken);
}