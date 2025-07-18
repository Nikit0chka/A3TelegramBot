namespace A3TelegramBot.Infrastructure.HttpClients.A3Api;

/// <summary>
///     Настройки API для интеграции с A3
/// </summary>
internal sealed class A3ApiOptions
{
    /// <summary>
    ///     Наименование секции
    /// </summary>
    public const string SectionName = "Api:A3";

    /// <summary>
    ///     Базовая ссылка API (endpoint)
    /// </summary>
    public required string DefaultConnection { get; init; }

    /// <summary>
    ///     Токен для авторизации в API
    /// </summary>
    public required string Token { get; init; }
}