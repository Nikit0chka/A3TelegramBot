namespace A3TelegramBot.Infrastructure.Services.MessageText;

/// <summary>
///     Настройки сообщений телеграм бота
/// </summary>
public sealed class MessageTextOptions
{
    /// <summary>
    ///     Наименование секции
    /// </summary>
    public const string SectionName = "MessageTextOptions";

    /// <summary>
    ///     Ссылка для просмотра цен
    /// </summary>
    public required string PriceLink { get; init; }

    /// <summary>
    ///     Ссылка для просмотра услуг
    /// </summary>
    public required string ServicesLink { get; init; }

    /// <summary>
    ///     Ссылка для просмотра приемных пунктов
    /// </summary>
    public required string ReceptionsLink { get; init; }

    /// <summary>
    ///     Ссылка для просмотра политики обработки персональных данных
    /// </summary>
    public required string PersonalDataPolicyLink { get; init; }
}