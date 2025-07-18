using A3TelegramBot.Application.Contracts;
using A3TelegramBot.Infrastructure.HttpClients.Abstractions;
using A3TelegramBot.Infrastructure.HttpClients.SdaiLomRfApi;
using A3TelegramBot.Infrastructure.HttpClients.SdaiLomRfApi.Dtos;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Refit;

internal sealed class SdaiLomRfService(ISdaiLomRfApiClient client, IOptions<SdaiLomRfOptions> options, ILogger<IA3ApiService> logger) : ISdaiLomRfApiService
{
    public async Task<ErrorOr<Success>> SendCallBackRequestCreated(int callBackRequestId, string userName, string phone, CancellationToken cancellationToken)
    {
        try
        {
            var requestModel = new CallBackRequestRequest(callBackRequestId, "Telegram Bot", phone, userName, true);

            var wrapper = new Wrapper(new List<CallBackRequestRequest>  { requestModel });

            await client.SendCallBackRequestCreated(wrapper, options.Value.Token, cancellationToken);

            return Result.Success;
        }
        catch (ApiException ex)
        {
            logger.LogError(ex, "Ошибка API запроса со Статусом: {StatusCode}", ex.StatusCode);
            Console.WriteLine("Response content: " + ex.Content);
            return Error.Failure(description: $"Произошла сетевая ошибка, попробуйте позже");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Непредвиденная Api ошибка");
            return Error.Unexpected(description: "Сервис создания заявки на обратный звонок временно не доступен, попробуйте позже");
        }
    }
}