using A3TelegramBot.Application.Dto;
using ErrorOr;

namespace A3TelegramBot.Application.Contracts;

/// <summary>
///     Сервис для запросов к А3
///     Фасад над http клиентом
/// </summary>
public interface IA3ApiService
{
    /// <summary>
    ///     Получить ближайшие приемные пункты
    /// </summary>
    /// <param name="latitude"> Широта пользователя </param>
    /// <param name="longitude"> Долгота пользователя </param>
    /// <param name="cancellationToken"> Токен отмены для асинхронной операции </param>
    /// <returns> Коллекция приемных пунктов </returns>
    public Task<ErrorOr<IReadOnlyCollection<ReceptionInfo>>> GetNearestReceptionsAsync(double latitude, double longitude, CancellationToken cancellationToken);
}