using A3TelegramBot.Application.Dto;
using Refit;

namespace A3TelegramBot.Infrastructure.Abstractions;

/// <summary>
///     Http клиент для запросов к А3
/// </summary>
public interface IA3ApiClient
{
    [Get("/receptions/nearest?latitude={latitude}&longitude={longitude}")]
    public Task<IReadOnlyCollection<ReceptionInfo>> GetNearestReceptionsAsync(double latitude, double longitude, [Header("Authorization")] string token, CancellationToken cancellationToken);
}