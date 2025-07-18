using ErrorOr;

namespace A3TelegramBot.Application.Contracts;

/// <summary>
///     Сервис для запросов к сдай лом рф
///     Фасад над http клиентом
/// </summary>
public interface ISdaiLomRfApiService
{
    /// <summary>
    /// Отправить заявку об 
    /// создании обратного звонка
    /// </summary>
    /// <param name="callBackRequestId">Идентификатор заявки</param>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="cancellationToken">Токен для отмены ассинхронной операции</param>
    public Task<ErrorOr<Success>> SendCallBackRequestCreated(int callBackRequestId, string userName, string phone, CancellationToken cancellationToken);
}