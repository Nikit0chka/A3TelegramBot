namespace A3TelegramBot.Presentation.Security.Authorization.ApiKey;

/// <summary>
/// Настройки авторизации по API-ключу
/// </summary>
public sealed class ApiKeyAuthOptions
{
    /// <summary>
    /// Наименование секции конфигурации
    /// </summary>
    public const string SectionName = "Authorization";
    
    /// <summary>
    /// Секретный API-ключ
    /// </summary>
    public required string ApiKey { get; init; }
}