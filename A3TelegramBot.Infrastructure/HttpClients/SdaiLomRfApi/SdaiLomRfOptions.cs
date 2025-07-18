namespace A3TelegramBot.Infrastructure.HttpClients.SdaiLomRfApi;

/// <summary>
///     Настройки API для интеграции с A3
/// </summary>
internal sealed class SdaiLomRfOptions
{
    /// <summary>
    ///     Наименование секции
    /// </summary>
    public const string SectionName = "Api:SdaiLomRf";

    /// <summary>
    ///     Базовая ссылка API (endpoint)
    /// </summary>
    public required string DefaultConnection { get; init; }

    /// <summary>
    ///     Токен для авторизации в API
    /// </summary>
    public required string Token { get; init; }
}