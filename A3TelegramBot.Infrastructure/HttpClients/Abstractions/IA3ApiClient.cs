using A3TelegramBot.Application.Dto;
using Refit;

namespace A3TelegramBot.Infrastructure.HttpClients.Abstractions;

/// <summary>
///     Http клиент для запросов к А3
/// </summary>
internal interface IA3ApiClient
{
    /// <summary>
    /// Получить ближайшие приемные 
    /// пункты по геолокации
    /// </summary>
    /// <param name="latitude">Широта пользователя</param>
    /// <param name="longitude">Долгота пользователя</param>
    /// <param name="token">Апи токен для авторизации</param>
    /// <param name="cancellationToken">Токен для отмены ассинхронной операции</param>
    /// <returns></returns>
    [Get("/TelegramBotIntegration/nearest-receptions?Latitude={latitude}&Longitude={longitude}")]
    public Task<IReadOnlyCollection<ReceptionInfo>> GetNearestReceptionsAsync(double latitude, double longitude, [Header("X-API-Token")] string token, CancellationToken cancellationToken);
}
