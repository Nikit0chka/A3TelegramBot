// Infrastructure/Services/A3ApiService.cs

using System.Text.Json;
using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Dto;
using A3TelegramBot.Infrastructure.Abstractions;
using ErrorOr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;

public sealed class A3ApiService:IA3ApiService
{
    private readonly IA3ApiClient _client;
    private readonly ILogger<IA3ApiService> _logger;
    private readonly string _apiToken;

    public A3ApiService(IA3ApiClient client, IConfiguration config, ILogger<IA3ApiService> logger)
    {
        _client = client;
        _logger = logger;
        _apiToken = config["ApiTokens:A3"];
    }

    public async Task<ErrorOr<IReadOnlyCollection<ReceptionInfo>>> GetNearestReceptionsAsync(double latitude,
                                                                                             double longitude,
                                                                                             CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.GetNearestReceptionsAsync(
                                                                   latitude,
                                                                   longitude,
                                                                   $"Bearer {_apiToken}",
                                                                   cancellationToken);

            return response.ToList();
        }
        catch (ApiException ex)
        {
            _logger.LogError(ex, "Ошибка API запроса со Статусом: {StatusCode}", ex.StatusCode);
            return Error.Failure(description: $"Сетевая ошибка запроса: {ex.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Непредвиденная Api ошибка");
            return Error.Unexpected(description: "Сервис поиска ближайших приемных пунктов временно не доступен");
        }
    }
}