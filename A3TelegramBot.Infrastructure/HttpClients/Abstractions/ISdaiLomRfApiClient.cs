using A3TelegramBot.Infrastructure.HttpClients.SdaiLomRfApi.Dtos;
using Refit;

namespace A3TelegramBot.Infrastructure.HttpClients.Abstractions;

/// <summary>
///     Http клиент для запросов 
///     к сдай лом рф
/// </summary>
internal interface ISdaiLomRfApiClient
{
    /// <summary>
    /// Отправить заявку 
    /// на обратный звонок
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <param name="token">Апи токен для авторизации</param>
    /// <param name="cancellationToken">Токен для отмены ассинхронной операции</param>
    /// <returns></returns>
    [Post("/tg_receiver.php")]
    public Task SendCallBackRequestCreated([Body] Wrapper request, [Header("X-API-Token")] string token, CancellationToken cancellationToken);
}
