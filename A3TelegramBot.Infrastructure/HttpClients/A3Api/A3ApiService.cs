using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Application.Dto;
using A3TelegramBot.Infrastructure.HttpClients.A3Api;
using A3TelegramBot.Infrastructure.HttpClients.Abstractions;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

internal sealed class A3ApiService(IA3ApiClient client, IOptions<A3ApiOptions> options, ILogger<IA3ApiService> logger) : IA3ApiService
{

    public async Task<ErrorOr<IReadOnlyCollection<ReceptionInfo>>> GetNearestReceptionsAsync(double latitude,
                                                                                             double longitude,
                                                                                             CancellationToken cancellationToken)
    {
        try
        {
            var response = await client.GetNearestReceptionsAsync(
                                                                   latitude,
                                                                   longitude,
                                                                   options.Value.Token,
                                                                   cancellationToken);

            return response.ToList();
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Ошибка API запроса со Статусом: {StatusCode}", ex.StatusCode);
            return Error.Failure(description: $"Сетевая ошибка запроса: {ex.StatusCode}");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Непредвиденная Api ошибка");
            return Error.Unexpected(description: "Сервис поиска ближайших приемных пунктов временно не доступен");
        }
    }
}